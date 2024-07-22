using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    //Player game object
    public GameObject player;
    private PlayerMovement playerMovement;

    //Use key
    [Header("Controls")]
    //public KeyCode attackButton = KeyCode.Mouse0;
    //public KeyCode lifestealButton = KeyCode.Mouse1;
    //public KeyCode reviveButton = KeyCode.R;
    

        /* ----- Attacking ----- */

    [Header("Attack Info")]
    //Stats
    public float damage = 100;
    public float knockback = 3;
    public float attackCooldown = 0f;
    public CooldownTimer attackTimer;
    private float attackDuration = 30f/60f;
    private bool attacking;
    
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
    private float reviveDuration = 126f / 60f; // Revive and Recall share timer
    private UnityEngine.UI.Image reviveNotifier;
    public bool activatedReviveNotifier = false;

         /* ----- Lifesteal ----- */

    [Header("Lifesteal Info")]
    //Stats
    public float lifestealRadius;
    public float lifestealCooldown = 10f;
    public float targetDistanceLifesteal = 150f;
    public float lifestealDamage = 10f;
    public float lifestealDuration = 5f;
    public Slider lifestealBar;
    public CooldownTimer lifestealTimer;
    public CooldownTimer lifestealStartTimer;
    private CooldownBarBehaviour lifestealCooldownBar;
    private UnityEngine.UI.Image lifeStealNotifier;
    private bool activatedLsNotifier = false;
    private float lifestealRatio;

    //Misc
    private List<EnemyAI> allies = new List<EnemyAI>();
    private SpriteRenderer lifestealImage;
    public bool lifestealStart;
    public TextMeshProUGUI uiCounter;


    /* ----- Misc ----- */

    //Animation
    [HideInInspector] public Animator animator;
    private SpriteRenderer sprite;

    // FMOD sound event paths
    public string eraserSfx;
    public string reviveSfx;
    public FMODUnity.EventReference lifestealSfx;
    private FMOD.Studio.EventInstance lifestealFmod;
    // Condition for playing hit version of eraserSfx
    public int isHit;


    // herearaer

    private PlayerInput playerInput;



    // Start is called before the first frame update
    void Awake()
    {
        //Collect components
        playerMovement = player.GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        hitbox = GetComponent<PolygonCollider2D>();
        lifestealImage = player.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>();
        lifestealImage.transform.localScale *= lifestealRadius * 10.45f;

        //Setup Timers and cooldown bars
        reviveTimer = new CooldownTimer(reviveCooldown, reviveDuration);
        attackTimer = new CooldownTimer(attackDuration*0.35f, attackDuration*0.65f);
        lifestealTimer = new CooldownTimer(lifestealCooldown, lifestealDuration);
        lifestealStartTimer = new CooldownTimer(0.65f, 0.35f);

        lifestealRatio = lifestealCooldown / lifestealDuration;
        lifestealStart = false;
        attacking = false;

        reviveCooldownBar = new CooldownBarBehaviour(reviveBar, reviveCooldown);
        lifestealCooldownBar = new CooldownBarBehaviour(lifestealBar, lifestealCooldown);

        reviveTimer.Connect(reviveCooldownBar);
        lifestealTimer.Connect(lifestealCooldownBar);

        lifeStealNotifier = lifestealBar.transform.parent.GetChild(1).GetComponent<UnityEngine.UI.Image>();
        reviveNotifier = reviveBar.transform.parent.GetChild(1).GetComponent<UnityEngine.UI.Image>();

        // Get a reference to the script that controls the lifestealFMOD event
        lifestealFmod = FMODUnity.RuntimeManager.CreateInstance(lifestealSfx);
        isHit = 0;


        playerInput = playerMovement.GetComponent<PlayerInput>();
        


    }

    // Update is called once per frame
    void Update()
    {
        if (allies.Count > 0)
        {
            ControlAllies();
        }
        if (uiCounter != null)
        {
            uiCounter.text = allies.Count.ToString();
        }
        if (!player.GetComponent<PlayerMovement>().inFreezeDialogue() && !player.GetComponent<PlayerMovement>().timelinePlaying && Time.timeScale!=0f)
        {
            CheckAttack();
            CheckRevive();
        }
        CheckLifesteal();
    }

    public void CheckAttack()
    {
        if (!lifestealStart)
        {
            //Attack Timer
            attackTimer.Update();
            if (attackTimer.IsUseable())
            {
                animator.SetBool("attacking", false);
                attacking = false;
            }

            //Attack
            if (attackTimer.IsUseable() && playerMovement.CanUseAbility() && playerInput.actions["Attack"].IsPressed() && !playerMovement.pauseInput)
            {

                // FMODUnity.RuntimeManager.PlayOneShot(eraserSfx, isHit);
                var instance = FMODUnity.RuntimeManager.CreateInstance(FMODUnity.RuntimeManager.PathToGUID(eraserSfx));
                instance.setParameterByName("IsHit", isHit);
                instance.start();
                instance.release();
                isHit = 0;

                animator.SetBool("attacking", true);
                attacking = true;
                attackTimer.StartTimer();
            }
        }
    }

    public void CheckRevive()
    {
        //Revive Timer
        reviveTimer.Update();
        
        // if revive is at max cooldown, flash the notifier
         if(reviveTimer.IsUseable() && !activatedReviveNotifier){
            var temp1 = reviveNotifier.color;
            temp1.a = 1f;
            reviveNotifier.color = temp1;
            activatedReviveNotifier = true;
        }

        // bring revive notifier alpha back to zero after it flashes
        if (reviveNotifier.color.a > 0 )
        {
            var temp = reviveNotifier.color;
            temp.a -= 0.01f;
            reviveNotifier.color = temp;

        }

        //Freeze Movement while reviving
        if (reviveTimer.IsActive())
        {
            playerMovement.speedModifier = 0f;
            playerMovement.StopMovement();
            playerMovement.ZoomCamera(0.35f); // Revive and Recall share timer
        } else
        {
            playerMovement.speedModifier = 1f;
        }

        //Revive
        if (playerMovement.CanUseAbility())
        {
            if (playerInput.actions["Revive"].triggered && reviveTimer.IsUseable() && !playerMovement.pauseInput)
            {
                Debug.Log("Attempting to revive enemies");

                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    EnemyAI enemy = obj.GetComponent<EnemyAI>();
                    if (enemy == null || allies.Count >= reviveCap)
                    {
                        continue;
                    }

                    if (InReviveRange(enemy.transform))
                    {
                        activatedReviveNotifier = false; // used for flashing icon on cooldown bar
                        if (enemy.Revive(0.8f, 0.8f, 1f, 0.75f))
                        {
                            allies.Add(enemy);
                            reviveTimer.StartTimer();
                            playerMovement.animator.SetBool("reviving", true);
                            sprite.enabled = false;
                            playerMovement.animationDone = false;
                            FMODUnity.RuntimeManager.PlayOneShot(reviveSfx);
                        }
                    }
                }
            }
        }

        if (reviveTimer.IsOnCooldown())
        {
            if (playerMovement.animator.GetBool("reviving"))
            {
                sprite.enabled = true;
                playerMovement.animator.SetBool("reviving", false);
                playerMovement.animationDone = true;
            }
        }
    }

    public void CheckLifesteal()
    {
        if (!player.GetComponent<PlayerMovement>().inFreezeDialogue() && !player.GetComponent<PlayerMovement>().timelinePlaying && Time.timeScale != 0f)
        {
            //Lifesteal Timer
            lifestealTimer.Update();
        }
        lifestealStartTimer.Update();

        // check if if cooldown is at max
        if (lifestealTimer.IsUseable() && !activatedLsNotifier)
        {
            var temp1 = lifeStealNotifier.color;
            temp1.a = 1f;
            lifeStealNotifier.color = temp1;
            activatedLsNotifier = true;
        }

        // bring life steal notifier alpha back to zero after it flashes
        if (lifeStealNotifier.color.a > 0)
        {
            var temp = lifeStealNotifier.color;
            temp.a -= 0.01f;
            lifeStealNotifier.color = temp;
        }

        if (playerMovement.CanUseAbility() && lifestealTimer.IsUseable() && playerInput.actions["LifeSteal"].triggered && !playerMovement.pauseInput && !lifestealStart)
        {
            // End melee attack if active (lifesteal takes priority)
            if (attacking)
            {
                animator.SetBool("attacking", false);
                attackTimer.ResetTimer();
                attacking = false;
            }
            lifestealStartTimer.StartTimer();
            animator.SetBool("lifestealstart", true);
            lifestealFmod.start();
            lifestealStart = true;
        }
        if (lifestealStartTimer.IsUseable() && lifestealStart)
        {
            lifestealStart = false;
            animator.SetBool("lifestealstart", false);
        }
        //if we use life steal ability set the notifier to false
        if (lifestealTimer.IsUseable() && lifestealStartTimer.IsOnCooldown())
        {
            lifestealImage.enabled = true;
            lifestealTimer.StartTimer();
            activatedLsNotifier = false;

            // Bar moves down
            lifestealCooldownBar.SetBar((lifestealDuration * lifestealRatio) - (lifestealTimer.timer * lifestealRatio));
        }
        if (lifestealTimer.IsActive() && lifestealImage.enabled && !player.GetComponent<PlayerMovement>().inFreezeDialogue() && !player.GetComponent<PlayerMovement>().timelinePlaying && Time.timeScale != 0f)
        {

            if (playerInput.actions["LifeSteal"].triggered && lifestealStartTimer.IsUseable())
            {
                lifestealTimer.StartCooldown(lifestealCooldown - (lifestealTimer.timer * lifestealRatio));
            }

            float dmg = lifestealDamage / lifestealDuration * Time.deltaTime;

            // Bar moves down
            lifestealCooldownBar.SetBar((lifestealDuration * lifestealRatio) - (lifestealTimer.timer * lifestealRatio));

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                EnemyAI enemy = obj.GetComponent<EnemyAI>();
                //LineRenderer line = enemy.GetComponent<LineRenderer>();
                if (enemy != null)
                {
                    if (CustomDist(lifestealImage.transform.position, enemy.transform.position + 2.5f * Vector3.down) <= lifestealRadius)
                    {
                        if (enemy.team == Team.oddle)
                        {
                            enemy.Damage(dmg, false, lifeSteal: true);

                            player.GetComponent<PlayerMovement>().Heal(dmg / 2); // HEALS
                            enemy.lifestealing = true;
                            //line.SetPosition(0, new Vector3(player.transform.position.x, player.transform.position.y, -1));
                            //line.SetPosition(1, new Vector3(enemy.transform.position.x, enemy.transform.position.y, -1));

                        }
                        else if (enemy.team == Team.player && playerMovement.health < playerMovement.maxHealth) // Won't lifesteal from allies if full health
                        {

                            enemy.Damage(dmg, false, lifeSteal: true);
                            player.GetComponent<PlayerMovement>().Heal(dmg); // HEALS
                            enemy.lifestealing = true;
                            //line.SetPosition(0, new Vector3(player.transform.position.x, player.transform.position.y, -1));
                            //line.SetPosition(1, new Vector3(enemy.transform.position.x, enemy.transform.position.y, -1));

                        }
                        else
                        {
                            enemy.lifestealing = false;
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
        }
        if (lifestealTimer.IsOnCooldown())
        {
            lifestealFmod.stop(0);
            lifestealImage.enabled = false;
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                EnemyAI enemy = obj.GetComponent<EnemyAI>();
                if (enemy != null)
                {
                    enemy.lifestealing = false;
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

    public bool InReviveRange(Transform other)
    {
        return (CustomDist(transform.position, other.position) <= reviveRadius);
    }

    public void ControlAllies()
    {
        //Find closest enemy target in range
        //EnemyAI target = null;
        GameObject target = null;
        float minDist = float.MaxValue;

        //Iterate through all enemies
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            EnemyAI enemy = obj.GetComponent<EnemyAI>();
            HealthCrystal crystal = obj.GetComponent<HealthCrystal>();
            Boss oodler = obj.GetComponent<Boss>();

            if (enemy != null) { 
            //Ignore doodleBars and any enemies that are not part of the enemy team
                if (enemy == null || enemy.team != Team.oddle || enemy is DoodleBars)
                {
                    continue;
                }

                float dist = enemy.PathLength(true);
                if (dist <= targetDistance && dist < minDist)
                {
                    target = obj;
                    minDist = dist;
                }
            }

            
            else if (crystal != null)
            {
                float dist = Vector3.Distance(obj.transform.position, player.transform.position);
                if (dist <= targetDistance && dist < minDist)
                {
                    target = obj;
                    minDist = dist;
                }
            }

            else if (oodler != null && oodler.BossIsDamageable())
            {
                float dist = Vector3.Distance(obj.transform.position, player.transform.position);
                if(dist<=targetDistance && dist < minDist)
                {
                    target = obj;
                    minDist = dist;
                }
            }


            else
            {
                continue;
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

    protected void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                {
                    //Get a reference to the enemy
                    EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
                    HealthCrystal crystal = collision.gameObject.GetComponent<HealthCrystal>();
                    Boss oodler = collision.gameObject.GetComponentInParent<Boss>();

                    if (enemy != null)
                    {
                        if (attackTimer.IsActive() && enemy != null && enemy.team == Team.oddle && enemy.invincibilityTimer.IsUseable() && enemy.PathLength(true) <= 15f)
                        {
                            //Calculate knockback
                            Vector2 direction = ((Vector2)enemy.transform.position - (Vector2)transform.position).normalized;
                            //Damage enemy
                            enemy.Damage(damage, true, true, direction, knockback);
                        }
                    }
                  
                        
                    else if (crystal != null)
                    {
                        if (attackTimer.IsActive() && crystal != null && crystal.invincibilityTimer.IsUseable())
                        {
                            //Damage enemy
                            crystal.CrystalDamage(damage, true);
                        }
                    }
                    

                    else if(oodler != null)
                    {
                        if (attackTimer.IsActive() && oodler != null && oodler.BossIsDamageable() && !oodler.invincibilityTimer.IsActive())
                        {
                            //Damage enemy
                            oodler.Damage(damage);

                        }


                    }

                    break;
                    
                }
            default:
                {
                    break;
                }
        }
    }

    private void OnDestroy()
    {
        lifestealFmod.stop(0);
    }

}