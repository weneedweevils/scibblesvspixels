using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Attack : MonoBehaviour
{
    //Player game object
    public GameObject player;
    private PlayerMovement playerMovement;

    //Use key
    [Header("Controls")]
    public KeyCode attackButton = KeyCode.Mouse0;
    public KeyCode reviveButton = KeyCode.Mouse1;
    public KeyCode lifestealButton = KeyCode.T;

        /* ----- Attacking ----- */

    [Header("Attack Info")]
    //Stats
    public float damage = 100;
    public float piercing = 3;
    public float attackCooldown = 0f;
    public CooldownTimer attackTimer;
    private float attackDuration = 30f/60f;
    
    //Hitbox
    private BoxCollider2D hitbox;
    private Vector2 initialHitboxOffset;
    private Vector2 initialHitboxSize;

        /* ----- Reviving ----- */

    [Header("Revive Info")]
    //Stats
    public float reviveRadius;
    public int reviveCap;
    public float reviveCooldown = 0f;
    public float targetDistance = 100f;
    public CooldownTimer reviveTimer;
    private float reviveDuration = 69f / 60f;

         /* ----- Lifesteal ----- */

    [Header("Lifesteal Info")]
    //Stats
    public float lifestealRadius;
    public float lifestealCooldown = 10f;
    public float targetDistanceLifesteal = 150f;
    public float lifestealDamagePerFrame = 0.01f;
    public CooldownTimer lifestealTimer;
    private float lifestealDuration = 5f;

    //Misc
    private List<EnemyAI> allies = new List<EnemyAI>();
    private SpriteRenderer reviveImage;
    private SpriteRenderer lifestealImage;


    /* ----- Misc ----- */

    //Animation
    private Animator animator;

    // FMOD sound event path
    public string sfx;

    // Music manager script
    public GameObject musicmanager;
    BasicMusicScript musicscript;


    // Start is called before the first frame update
    void Start()
    {
        //Collect components
        playerMovement = player.GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        hitbox = GetComponent<BoxCollider2D>();
        reviveImage = player.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
        reviveImage.transform.localScale *= reviveRadius * 10.45f;
        lifestealImage = player.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>();
        lifestealImage.transform.localScale *= lifestealRadius * 10.45f;

        //Save initial attack hitbox information
        initialHitboxOffset = hitbox.offset;
        initialHitboxSize = hitbox.size;

        //Setup Timers
        reviveTimer = new CooldownTimer(reviveCooldown, reviveDuration);
        attackTimer = new CooldownTimer(attackCooldown, attackDuration);
        lifestealTimer = new CooldownTimer(lifestealCooldown, lifestealDuration);

        // Get a reference to the script that controls the FMOD event
        //eraserSFX = GetComponent<eraserSFX>;

        musicmanager = GameObject.Find("Music");
        musicscript = musicmanager.GetComponent<BasicMusicScript>();

    }

    // Update is called once per frame
    void Update()
    {
        if (allies.Count > 0)
        {
            ControlAllies();
        }

        if(allies.Count > 3) {
            musicscript.setIntensity(30f);
        }
        
        if (!player.GetComponent<PlayerMovement>().inFreezeDialogue() && !player.GetComponent<PlayerMovement>().timelinePlaying)
        {
            CheckAttack();
            CheckRevive();
            CheckLifesteal();
        }
        
    }

    //Used to flip the attack hitbox as needed when rotating
    public void FlipHitbox(bool flip)
    {
        if (flip)
        {
            hitbox.offset = -initialHitboxOffset;
        } else
        {
            hitbox.offset = initialHitboxOffset;
        }
    }

    public void CheckAttack()
    {
        //Attack Timer
        attackTimer.Update();
        if (!attackTimer.IsActive())
        {
            animator.SetBool("attacking", false);
        }

        //Attack
        if (attackTimer.IsUseable() && Input.GetKey(attackButton))
        {
            // Play the FMOD event correlating to the attack
            FMODUnity.RuntimeManager.PlayOneShot(sfx);

            animator.SetBool("attacking", true);
            attackTimer.StartTimer();
        }
    }

    public void CheckRevive()
    {
        //Revive Timer
        reviveTimer.Update();

        //Freeze Movement while reviving
        if (reviveTimer.IsActive())
        {
            playerMovement.speedModifier = 0f;
            playerMovement.StopMovement();
        } else
        {
            playerMovement.speedModifier = 1f;
        }

        //Revive
        if (Input.GetKey(reviveButton) && reviveTimer.IsUseable())
        {
            reviveImage.enabled = true;
        }
        if (Input.GetKeyUp(reviveButton) && reviveTimer.IsUseable())
        {
            Debug.Log("Attempting to revive enemies");
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                EnemyAI enemy = obj.GetComponent<EnemyAI>();
                if (enemy == null || allies.Count >= reviveCap)
                {
                    continue;
                }

                if (CustomDist(reviveImage.transform.position, enemy.transform.position + 2.5f * Vector3.down) <= reviveRadius)
                {
                    if (enemy.Revive(0.5f, 0.5f, 0.9f))
                    {
                        allies.Add(enemy);
                        reviveTimer.StartTimer();
                    }
                }
            }
            reviveImage.enabled = false;
        }
    }

    public void CheckLifesteal()
    {
        //Lifesteal Timer
        lifestealTimer.Update();

        //Freeze Movement while reviving
        if (lifestealTimer.IsUseable() && Input.GetKeyDown(lifestealButton))
        {
            lifestealImage.enabled = true;
            lifestealTimer.StartTimer();
        }
        else if (lifestealTimer.IsOnCooldown())
        {
            lifestealImage.enabled = false;
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                EnemyAI enemy = obj.GetComponent<EnemyAI>();
                LineRenderer line = enemy.GetComponent<LineRenderer>();
                //line.SetPosition(0, Vector3.zero);
                //line.SetPosition(1, Vector3.zero);
            }

        }
        if (lifestealTimer.IsActive()) {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                EnemyAI enemy = obj.GetComponent<EnemyAI>();
                //LineRenderer line = enemy.GetComponent<LineRenderer>();
                if (CustomDist(lifestealImage.transform.position, enemy.transform.position + 2.5f * Vector3.down) <= lifestealRadius)
                {
                    if (enemy.team == Team.oddle)
                    {
                        enemy.Damage(lifestealDamagePerFrame);
                        player.GetComponent<PlayerMovement>().Heal(lifestealDamagePerFrame / 2); // HEALS
                        //line.SetPosition(0, new Vector3(player.transform.position.x, player.transform.position.y, -1));
                        //line.SetPosition(1, new Vector3(enemy.transform.position.x, enemy.transform.position.y, -1));
                    }
                    else if (enemy.team == Team.player)
                    {
                        enemy.Damage(lifestealDamagePerFrame);
                        player.GetComponent<PlayerMovement>().Heal(lifestealDamagePerFrame); // HEALS
                        //line.SetPosition(0, new Vector3(player.transform.position.x, player.transform.position.y, -1));
                        //line.SetPosition(1, new Vector3(enemy.transform.position.x, enemy.transform.position.y, -1));
                    }
                }
                else
                {
                    //line.SetPosition(0, Vector3.zero);
                    //line.SetPosition(1, Vector3.zero);
                }
            }
        }
    }

    public float CustomDist(Vector3 a, Vector3 b)
    {
        float xScale = 1f;
        float yScale = 0.5f;
        return Mathf.Sqrt(Mathf.Pow(((a.x - b.x) / xScale), 2) + Mathf.Pow(((a.y - b.y) / yScale), 2));
    }

    public void ControlAllies()
    {
        //Find closest enemy target in range
        EnemyAI target = null;
        float minDist = float.MaxValue;

        //Iterate through all enemies
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            EnemyAI enemy = obj.GetComponent<EnemyAI>();
            //Ignore any enemies that are not part of the enemy team
            if (enemy == null || enemy.team != Team.oddle)
            {
                continue;
            }

            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist <= targetDistance && dist < minDist)
            {
                target = enemy;
                minDist = dist;
            }
        }

        //Set Allies target & remove dead allies
        List<EnemyAI> temp = new List<EnemyAI>(allies);
        foreach (EnemyAI ally in temp)
        {
            //Ignore allies currently being revived
            if (ally.state == State.reviving)
            {
                continue;
            }
            //Remove Dead Allies
            if (ally.state == State.dead || ally.state == State.dying)
            {
                allies.Remove(ally);
            } else if (target != null)  //Found  a target -> go attack target
            {
                if (ally.state == State.follow)
                {
                    ally.state = State.chase;
                }
                ally.SetTarget(target.transform);
                print("Attacking");

            } else  //No target and ally is not dead -> follow player
            {
                ally.state = State.follow;
                ally.SetTarget(player);
                print("Following");
            }
        }
    }
}