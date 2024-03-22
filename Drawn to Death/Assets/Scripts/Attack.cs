using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public float knockback = 3;
    public float attackCooldown = 0f;
    public CooldownTimer attackTimer;
    private float attackDuration = 30f/60f;
    
    //Hitbox
    private PolygonCollider2D hitbox;

        /* ----- Reviving ----- */

    [Header("Revive Info")]
    //Stats
    public float reviveRadius;
    public int reviveCap;
    public float reviveCooldown = 0f;
    public float targetDistance = 100f;
    public CooldownTimer reviveTimer;
    public Slider reviveBar;
    private CooldownBarBehaviour reviveCooldownBar;
    private float reviveDuration = 69f / 60f;

         /* ----- Lifesteal ----- */

    [Header("Lifesteal Info")]
    //Stats
    public float lifestealRadius;
    public float lifestealCooldown = 10f;
    public float targetDistanceLifesteal = 150f;
    public float lifestealDamage = 10f;
    public float lifestealDuration = 5f;
    public Slider lifestealBar;
    private CooldownTimer lifestealTimer;
    private CooldownBarBehaviour lifestealCooldownBar;

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
        hitbox = GetComponent<PolygonCollider2D>();
        reviveImage = player.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
        reviveImage.transform.localScale *= reviveRadius * 10.45f;
        lifestealImage = player.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>();
        lifestealImage.transform.localScale *= lifestealRadius * 10.45f;

        //Setup Timers and cooldown bars
        reviveTimer = new CooldownTimer(reviveCooldown, reviveDuration);
        attackTimer = new CooldownTimer(attackDuration*0.35f, attackDuration*0.65f);
        lifestealTimer = new CooldownTimer(lifestealCooldown, lifestealDuration);
        reviveCooldownBar = new CooldownBarBehaviour(reviveBar, reviveCooldown, Color.red, Color.green);
        lifestealCooldownBar = new CooldownBarBehaviour(lifestealBar, lifestealCooldown, Color.red, Color.green);

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

    public void CheckAttack()
    {
        //Attack Timer
        attackTimer.Update();
        if (attackTimer.IsUseable())
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
        if (reviveTimer.IsOnCooldown())
        {
            reviveCooldownBar.SetBar(reviveTimer.timer);
        }
    }

    public void CheckLifesteal()
    {
        //Lifesteal Timer
        lifestealTimer.Update();

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
                //LineRenderer line = enemy.GetComponent<LineRenderer>();
                //line.SetPosition(0, Vector3.zero);
                //line.SetPosition(1, Vector3.zero);
                enemy.lifestealing = false;
            }

        }
        if (lifestealTimer.IsActive()) {
            float dmg = lifestealDamage / lifestealDuration * Time.deltaTime;

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                EnemyAI enemy = obj.GetComponent<EnemyAI>();
                //LineRenderer line = enemy.GetComponent<LineRenderer>();
                if (CustomDist(lifestealImage.transform.position, enemy.transform.position + 2.5f * Vector3.down) <= lifestealRadius)
                {
                    if (enemy.team == Team.oddle)
                    {
                        enemy.Damage(dmg, false);
                        player.GetComponent<PlayerMovement>().Heal(dmg / 2); // HEALS
                        enemy.lifestealing = true;
                        //line.SetPosition(0, new Vector3(player.transform.position.x, player.transform.position.y, -1));
                        //line.SetPosition(1, new Vector3(enemy.transform.position.x, enemy.transform.position.y, -1));
                    }
                    else if (enemy.team == Team.player)
                    {
                        enemy.Damage(dmg, false);
                        player.GetComponent<PlayerMovement>().Heal(dmg); // HEALS
                        enemy.lifestealing = true;
                        //line.SetPosition(0, new Vector3(player.transform.position.x, player.transform.position.y, -1));
                        //line.SetPosition(1, new Vector3(enemy.transform.position.x, enemy.transform.position.y, -1));
                    }
                }
                else
                {
                    enemy.lifestealing = false;
                    //line.SetPosition(0, Vector3.zero);
                    //line.SetPosition(1, Vector3.zero);
                }
            }
        }
        if (lifestealTimer.IsOnCooldown())
        {
            lifestealCooldownBar.SetBar(lifestealTimer.timer);
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

            float dist = enemy.PathLength();
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
            } 
            else if (target != null)  //Found  a target -> go attack target
            {
                if (ally.state == State.follow)
                {
                    ally.state = State.chase;
                }
                ally.SetTarget(target.transform);

            } 
            else  //No target and ally is not dead -> follow player
            {
                ally.state = State.follow;
                ally.SetTarget(player, true);
            }
        }
    }

    public List<EnemyAI> GetAllies()
    {
        return allies;
    }
}