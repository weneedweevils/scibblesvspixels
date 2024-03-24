using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

// Used this video for most of the script https://www.youtube.com/watch?v=jvtFUfJ6CP8a
// if you want to use this in FSM inherit from EnemybaseState class
public enum Team {player, neutral, oddle};
public enum State {idle, chase, follow, attack, dying, dead, reviving };
public class EnemyAI : MonoBehaviour
{

        /* ----- Editor Variables ----- */

    [Header("State")]
    public Team team = Team.oddle;
    public State state = State.chase;
    public bool isolated = false;

    [Header("Stats")]
    public float health;
    public float maxHealth;
    public float speed = 200f;
    public float damage;
    public float cooldown = 2f;
    public float nextAttack;

    [Header("Pathfinding")]
    public Seeker targetSeeker;
    public Seeker playerSeeker;
    public float seekDistance = 100f;
    public float nextWaypointDistance;

    [Header("Music and sound")]
    public string deathSfx;
    public string attackSfx;
    

    [Header("References")]
    public Collider2D movementCollider;
    public EnemyHealthBarBehaviour healthBar;
    public Transform enemygraphics;
    public Sprite attacksprite;
    public Color hurtCol = Color.red;

        /* ----- Hidden Variables ----- */

    //Invincibility Frames
    public CooldownTimer invincibilityTimer;
    public CooldownTimer invincibilityTimer2;
    private float invincibilityDuration = 20f / 60f;

    //Animation and sprites
    public SpriteRenderer doodleCrab;
    private SpriteRenderer gem;
    private Animator animator;
    private float animationTimer = 0f;
    private float deathDuration = 25f/60f;
    private float reviveDuration = 69f/60f;

    //Pathfinding
    private Transform target;
    private Path targetPath;
    private Path playerPath;
    private bool targetIsPlayer = true;
    private int currentWaypoint = 0;
    private Rigidbody2D rb;

    //Misc
    private GameObject player;
    private Attack playerAttack;
    private GameObject musicmanager;
    private BasicMusicScript musicscript;
    public bool slowed = false;
    public bool lifestealing = false;
    public CooldownTimer slowedTimer;
    public float slowDuration;

    // Start is called before the first frame update
    void Start()
    {
        //Collect References
        animator = GetComponentInChildren<Animator>();
        doodleCrab = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        gem = gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        playerAttack = player.GetComponentInChildren<Attack>();
        musicmanager = GameObject.Find("Music");
        musicscript = musicmanager.GetComponent<BasicMusicScript>();

        //Initialize
        target = player.transform;
        health = maxHealth;
        healthBar.SetHealth(health, maxHealth);

        //Create Timers
        invincibilityTimer = new CooldownTimer(invincibilityDuration * 0.5f, invincibilityDuration * 0.5f);
        invincibilityTimer2 = new CooldownTimer(3f, invincibilityDuration);
        slowedTimer = new CooldownTimer(0.1f, slowDuration);

        //Start a repeating functon
        InvokeRepeating("CheckState", 0f, 0.5f); //Update the path every half second
    }

    void CheckState()
    {
        //Dead enemies dont move
        if (state == State.dead || state == State.dying)
        {
            return;
        }

        //Update path to Player
        float inrange = Vector2.Distance(rb.position, player.transform.position);
        if (playerSeeker.IsDone() && inrange < seekDistance)
        {
            playerSeeker.StartPath(rb.position, player.transform.position, OnPlayerPathComplete);
        }

        //Make an attempt at finding a new target
        if (target == null || (PathLength() > seekDistance * 0.5 && team == Team.oddle))
        {
            FindTarget();
        }

        //Check if the target is not the player
        if (!targetIsPlayer)
        {
            //Update path to target
            inrange = Vector2.Distance(rb.position, target.position);
            if (targetSeeker.IsDone() && inrange < seekDistance)
            {
                targetSeeker.StartPath(rb.position, target.position, OnTargetPathComplete);
            }
        }
        
    }

    // Checks if there is a path calculated
    void OnTargetPathComplete(Path p)
    {
        if (targetIsPlayer)
        {
            targetPath = playerPath;
            currentWaypoint = 0;
        } 
        else
        {
            if (!p.error)
            {
                targetPath = p;
                currentWaypoint = 0;
            } else
            {
                targetPath = null;
            }
        }
    }

    void OnPlayerPathComplete(Path p)
    {
        if (!p.error)
        {
            playerPath = p;
            if (targetIsPlayer)
            {
                targetPath = playerPath;
                currentWaypoint = 0;
            }
        }
        else
        {
            playerPath = null;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Update Timers
        invincibilityTimer.Update();
        invincibilityTimer2.Update();
        slowedTimer.Update();

        //Fix color after hurt
        if ( (invincibilityTimer.IsOnCooldown() && !invincibilityTimer2.IsActive()) || 
             (invincibilityTimer2.IsOnCooldown() && !invincibilityTimer.IsActive()) ||
             (!lifestealing && team == Team.player) )
        {
            doodleCrab.color = Color.white;
        }
        
        //Check death conditions
        if (health <= 0 && state != State.dead && state != State.dying)
        {
            Kill(); // Ded
        }

        // Check if being lifestolen
        if (lifestealing)
        {
            if (!slowed && team == Team.oddle) // Only slow enemy Oodles
            {
                speed /= 2;
                slowed = true;
            }
            doodleCrab.color = Color.red;
        }

        // Start timer to end slow if not in lifesteal zone anymore but still slowed
        if (!lifestealing && slowed && slowedTimer.IsUseable())
        {
            slowedTimer.StartTimer();
        }

        // Change color if slowed but not being lifestolen
        if (slowedTimer.IsActive() && !lifestealing)
        {
            doodleCrab.color = Color.yellow;
        }

        // End slow if timer is done
        if (slowedTimer.IsOnCooldown() && !lifestealing && slowed)
        {
            slowed = false;
            speed *= 2;
            doodleCrab.color = Color.white;
        }

        //State Manager
        switch (state)
        {
            case State.idle:
                {
                    //idle Behaviour
                    if (PathLength() < seekDistance)
                    {
                        state = State.chase;
                    }
                    break;
                }
            case State.chase:
                {
                    //chase Behaviour
                    if (PathLength() > seekDistance && team == Team.oddle)
                    {
                        state = State.idle;
                    } else
                    {
                        MoveEnemy();
                    }
                    break;
                }
            case State.attack:
                {
                    if (Time.time > nextAttack)
                    {
                        Attack();
                    }
                    break;
                }
            case State.dying:
                {
                    //dying Behaviour
                    animationTimer += Time.deltaTime;
                    doodleCrab.color = Color.white;
                    if (animationTimer >= deathDuration)
                    {
                        animationTimer = 0f;
                        state = State.dead;
                        animator.SetBool("dying", false);
                    }
                    break;
                }
            case State.dead:
                {
                    //dead Behaviour
                    doodleCrab.color = Color.white;
                    if (team == Team.player)
                    {
                        Destroy(gameObject);
                    }
                    break;
                }
            case State.reviving:
                {
                    //reviving Behaviour
                    animationTimer += Time.deltaTime;
                    if (animationTimer >= reviveDuration)
                    {
                        animationTimer = 0f;
                        state = State.follow;
                        animator.SetBool("reviving", false);
                        animator.SetBool("chasing", false);
                        animator.SetBool("attacking", false);
                        animator.SetBool("dying", false);
                    }
                    break;
                }
            case State.follow:
                {
                    //follow Behaviour
                    MoveEnemy();
                    break;
                }
        }
    }

    // moves enemy and adjusts animation to face player
    void MoveEnemy()
    {
        if (target == null)
        {
            return;
        }

        float triggerAttack = Vector2.Distance(rb.position, target.position);

        // if we are in range switch to the attack state
        if (triggerAttack < 10f)
        {
            animator.SetBool("chasing", false);
            animator.SetBool("attacking", true);
            state = State.attack;
            return;
        }

        if (targetPath == null || currentWaypoint >= targetPath.vectorPath.Count)
        {
            return;
        }

        Vector2 direction = ((Vector2)targetPath.vectorPath[currentWaypoint] - rb.position + 2 * Vector2.down).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, targetPath.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
            
        }

        if (rb.velocity.x >= 0.01f)
        {
            enemygraphics.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (rb.velocity.x <= -0.01f)
        {
            enemygraphics.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void FindTarget()
    {
        //Set the minimum target to the player
        float dist = Vector2.Distance(rb.position, player.transform.position);
        target = player.transform;
        targetIsPlayer = true;

        //Compare against player allies
        foreach (EnemyAI enemy in playerAttack.GetAllies())
        {
            //Check if the ally is a better target
            float newDist = Vector2.Distance(rb.position, enemy.transform.position);
            if (newDist <= dist)
            {
                dist = newDist;
                target = enemy.transform;
                targetIsPlayer = false;
            }
        }
    }

    public void Attack()
    {
        if (target == null)
        {
            return;
        }

        float triggerChase = Vector2.Distance(rb.position, target.position);
        nextAttack += Time.deltaTime;


        if (nextAttack >= cooldown)
        {
            // Play the FMOD event correlating to the attack
            FMODUnity.RuntimeManager.PlayOneShot(attackSfx);
       
            Vector2 direction = ((Vector2)target.position - rb.position).normalized;
            rb.AddForce(direction * 25000f * Time.deltaTime);
            nextAttack = 0;
        }

        if (triggerChase > 10f)
        {
            animator.SetBool("attacking", false);
            animator.SetBool("chasing", true);
            if (team == Team.player)
            {
                state = State.chase;
            }
            if (team == Team.oddle)
            {
                state = State.chase;
            }
            return;
          
        }
    }

    public void Kill()
    {
        // Play the FMOD event correlating to the death
        FMODUnity.RuntimeManager.PlayOneShot(deathSfx);
        
        //Set State
        if (team == Team.oddle) //First Death
        {
            team = Team.neutral;
        }
        health = 0;
        state = State.dying;
        target = null;
        targetIsPlayer = false;

        //Set Movement
        rb.velocity = Vector2.zero;
        movementCollider.enabled = false;

        //Set Animation variables
        animator.SetBool("attacking", false);
        animator.SetBool("chasing", false);
        animator.SetBool("dying", true);

        // Remove slow if on at death
        if (slowed)
        {
            slowed = false;
            speed *= 2;
        }
    }

    public bool Revive(float percentMaxHP = 1f, float percentDamage = 1f, float percentSpeed = 1f)
    {
        if (state == State.dead && team == Team.neutral)
        {
            //Set State
            team = Team.player;
            state = State.reviving;

            //Animation
            animator.SetBool("reviving", true);
            gem.enabled = true;

            //Set Stats
            maxHealth *= percentMaxHP;
            damage *= percentDamage;
            speed *= percentSpeed;
            health = maxHealth;

            //Re-enable collisions
            movementCollider.enabled = true;

            return true;
        } else
        {
            return false;
        }
    }

    // Function to run when player takes damage
    public void Damage(float damageTaken, bool makeInvincible = true, bool animateHurt = false, Vector2 knockbackDir = default(Vector2), float knockbackPower = 0f)
    {
        //Dont hit dead bodies
        if (state == State.dead || state == State.dying)
        {
            return;
        }

        //Inflict damage
        health -= damageTaken;
        healthBar.SetHealth(health, maxHealth);

        //Check death conditions
        if (health <= 0)
        {
            Kill(); // Ded
            return;
        }

        //Apply Knockback
        if (knockbackPower > 0f)
        {
            rb.velocity = knockbackDir.normalized * knockbackPower;
        }
        
        //Flash hurt color
        if (animateHurt)
        {
            doodleCrab.color = hurtCol;
        }

        //Start invincibility timer
        if (makeInvincible)
        {
            invincibilityTimer.StartTimer();
        }
        
    }

    public void SetTarget(GameObject obj, bool isPlayer = false)
    {
        target = obj.transform;
        targetIsPlayer = isPlayer;
    }

    public void SetTarget(Transform transform, bool isPlayer = false)
    {
        target = transform;
        targetIsPlayer = isPlayer;
    }

    //Estimate the length of the current path
    public float PathLength(bool toPlayer = false)
    {
        //Path to calculate
        Path path = toPlayer ? playerPath : targetPath;

        //No path
        if (isolated || path == null)
        {
            return float.MaxValue;
        }

        //Need at least 2 values to estimate distance -> otherwise distance is practically 0
        int size = path.vectorPath.Count;
        if (size < 2)
        {
            return 0f;
        }

        //Distance estimate
        return Vector3.Distance(path.vectorPath[0], path.vectorPath[1]) * size;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Attack" && invincibilityTimer.IsUseable() && health > 0)
        {
            Attack playerAttack = collision.gameObject.GetComponent<Attack>();
            if (playerAttack != null && playerAttack.attackTimer.IsActive() && team == Team.oddle && PathLength(true) < 13f)
            {
                Vector2 direction = (rb.position - (Vector2)playerAttack.transform.position).normalized;
                Damage(playerAttack.damage, true, true, direction, playerAttack.knockback);
                musicscript.setIntensity(20f);
            }
        }
        if (collision.gameObject.tag == "Enemy" && invincibilityTimer2.IsUseable() && health > 0)
        {
            EnemyAI otherai = collision.gameObject.GetComponent<EnemyAI>();

            if (team != otherai.team && otherai.team != Team.neutral)
            {
                Vector2 direction = (rb.position - (Vector2)otherai.transform.position).normalized;
                Damage(otherai.damage, false, true, direction, playerAttack.knockback);
                invincibilityTimer2.StartTimer();
            }
            healthBar.SetHealth(health, maxHealth);
        }

    }
}