using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UIElements;

// Used this video for most of the script https://www.youtube.com/watch?v=jvtFUfJ6CP8a
// if you want to use this in FSM inherit from EnemybaseState class
public enum Team {player, neutral, oddle};
public enum State {idle, chase, follow, attack, dying, dead, reviving };
public enum Type { crab, cubie, knight, bars, general };
public abstract class EnemyAI : MonoBehaviour
{

        /* ----- Editor Variables ----- */

    [Header("State")]
    public Team team = Team.oddle;
    public State state = State.chase;
    public Type type = Type.general;
    public bool isolated = false;

    [Header("Stats")]
    public float health;
    public float maxHealth;
    public float speed = 200f;
    public float damage;
    public float attackCooldown;
    public float attackDistance;
    public float slowdownFactor = 3f;
    public float stunCooldownRatio = 0.7f;
    public float knockbackRatio = 1f;

    [Header("Pathfinding")]
    public bool moving = true;
    public Seeker targetSeeker;
    public Seeker playerSeeker;
    public float seekDistance = 100f;
    public float nextWaypointDistance;
    public EnemyAI[] blockers;

    [Header("Music and sound")]
    public string deathSfx;
    public string attackSfx;

    protected FMOD.Studio.EventInstance attackSFXInstance;
    
    [Header("Effects")]
    public bool slowed = false;
    public bool lifestealing = false;
    public CooldownTimer slowedTimer;
    public CooldownTimer buffTimer;
    public float slowDuration;
    public bool buffed = false;

    [Header("References")]
    public Collider2D movementCollider;
    public EnemyHealthBarBehaviour healthBar;
    public Transform enemygraphics;
    public Color hurtCol = Color.red;
    public Color reviveCol = Color.green;
    public Color allyCol = Color.green;
    public SpriteRenderer selfImage;
    public GameObject panel;


    /* ----- Hidden Variables ----- */

    //Invincibility Frames
    public CooldownTimer invincibilityTimer;
    public CooldownTimer invincibilityTimer2;
    public CooldownTimer invincibilityTimerOodler;
    public CooldownTimer attackTimer;
    protected float invincibilityDuration = 20f / 60f;
    private float oodlerInvincibilityDuration = 40f / 60f;

    //Animation and sprites
    protected SpriteRenderer gem;
    protected Animator animator;
    protected float animationTimer = 0f;
    protected float attackDuration = 0f;
    protected float deathDuration = 0f;
    protected float reviveDuration = 138f / 60f;

    //Pathfinding
    protected Transform target;
    protected Path targetPath;
    protected Path playerPath;
    protected bool targetIsPlayer = true;
    protected int currentWaypoint = 0;
    protected Rigidbody2D rb;
    protected Vector2 pathOffset = new Vector2(0, 1.5f);
    protected bool targetIsDropZone = false;

    //Misc
    protected GameObject player;
    protected PlayerMovement playerMovement;
    protected Attack playerAttack;
    protected GameObject musicmanager;
    protected BasicMusicScript musicscript;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //Collect References
        animator = GetComponentInChildren<Animator>();
        selfImage = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        gem = gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        playerAttack = player.GetComponentInChildren<Attack>();
        musicmanager = GameObject.Find("Music");
        musicscript = musicmanager.GetComponent<BasicMusicScript>();

        //Initialize
        target = player.transform;
        health = maxHealth;
        healthBar.SetHealth(health, maxHealth);
        attackSFXInstance = FMODUnity.RuntimeManager.CreateInstance(attackSfx);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(attackSFXInstance, GetComponent<Transform>(), GetComponent<Rigidbody2D>());
      

        //Create Timers
        invincibilityTimer = new CooldownTimer(invincibilityDuration * 0.5f, invincibilityDuration * 0.5f);
        invincibilityTimer2 = new CooldownTimer(0f, invincibilityDuration);
        attackTimer = new CooldownTimer(attackCooldown, attackDuration);
        slowedTimer = new CooldownTimer(0.1f, slowDuration);
        buffTimer = new CooldownTimer(0.1f, playerMovement.allyBuffDuration);
        invincibilityTimerOodler = new CooldownTimer(oodlerInvincibilityDuration * 0.5f, oodlerInvincibilityDuration * 0.5f);


        //Start a repeating functon

        if (moving)
        { 
            InvokeRepeating("CheckState", 0f, 0.5f); //Update the path every half second if not a movable object
        }
        if (blockers.Length != 0)
        {
            isolated = true;
        }

        // Ensure enemy type is defined
        if (type == Type.general)
        {
            Debug.Log("Enemy type is defined as general. Please define the enemy type in the child script for the enemy.");
        }
    }

    private void CheckState()
    {
        //Dead enemies dont move
        if (state == State.dead || state == State.dying)
        {
            return;
        }

        //Update path to Player
        float inrange = Vector2.Distance(rb.position - pathOffset, player.transform.position - (Vector3)pathOffset);
        if (playerSeeker.IsDone() && inrange < seekDistance)
        {
            playerSeeker.StartPath(rb.position - pathOffset, player.transform.position - (Vector3)pathOffset, OnPlayerPathComplete);
        }

        //Make an attempt at finding a new target
        if (target == null || (PathLength() > seekDistance * 0.1 && team == Team.oddle))
        {
            FindTarget();
        }

        //Check if the target is not the player
        if (!targetIsPlayer)
        {
            //Update path to target
            inrange = Vector2.Distance(rb.position - pathOffset, target.position - (Vector3)pathOffset);
            if (targetSeeker.IsDone() && inrange < seekDistance)
            {
                targetSeeker.StartPath(rb.position - pathOffset, target.position - (Vector3)pathOffset, OnTargetPathComplete);
            }
        }
    }

    // Checks if there is a path calculated
    private void OnTargetPathComplete(Path p)
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

    private void OnPlayerPathComplete(Path p)
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
    protected virtual void FixedUpdate()
    {
        if (!playerMovement.inFreezeDialogue() && !playerMovement.timelinePlaying)
        {
            //Check Blockers
            if (isolated && blockers.Length > 0)
            {
                foreach (EnemyAI blocker in blockers)
                {
                    if (blocker.isDead())
                    {
                        isolated = false;
                        break;
                    }
                }
            }

            //Update Timers
            invincibilityTimer.Update();
            invincibilityTimer2.Update();
            attackTimer.Update();
            slowedTimer.Update();
            buffTimer.Update();
            invincibilityTimerOodler.Update();

            //Fix color after hurt
            if ((invincibilityTimer.IsOnCooldown() && !invincibilityTimer2.IsActive()) ||
                 (invincibilityTimer2.IsUseable() && !invincibilityTimer.IsActive()) ||
                 (!lifestealing && team == Team.player))
            {
                selfImage.color = (team == Team.player ? allyCol : Color.white);
            }

            //Check death conditions
            if (health <= 0 && state != State.dead && state != State.dying)
            {
                Kill(); // Ded
            }

            // Check if buffed
            if (buffed)
            {
                if (buffTimer.IsOnCooldown())
                {
                    buffed = false;
                    if (type == Type.crab)
                    {
                        speed /= playerMovement.crabSpdModifier;
                    }
                    else
                    {
                        speed /= playerMovement.allySpdModifier;
                    }
                    damage /= playerMovement.allyStrModifier;
                    attackTimer.SetCooldown(attackCooldown);
                    selfImage.color = Color.white;
                }
                if (buffTimer.IsUseable())
                {
                    buffTimer.StartTimer();
                }
                selfImage.color = Color.magenta;
            }

            // Check if being lifestolen
            if (lifestealing)
            {
                if (!slowed && team == Team.oddle) // Only slow enemy Oodles
                {
                    speed /= slowdownFactor;
                    attackTimer.SetCooldown(attackCooldown * 1.5f);
                    slowed = true;
                }
                selfImage.color = Color.red;
            }

            // Start timer to end slow if not in lifesteal zone anymore but still slowed
            if (!lifestealing && slowed && slowedTimer.IsUseable())
            {
                slowedTimer.StartTimer();
            }

            // Change color if slowed but not being lifestolen
            if (slowedTimer.IsActive() && !lifestealing)
            {
                selfImage.color = Color.yellow;
            }

            // End slow if timer is done
            if (slowedTimer.IsOnCooldown() && !lifestealing && slowed)
            {
                slowed = false;
                speed *= slowdownFactor;
                attackTimer.SetCooldown(attackCooldown);
                selfImage.color = team == Team.player ? allyCol : Color.white;
            }
        }
        //State Manager
        switch (state)
        {
            case State.idle:
                {
                    //idle Behaviour
                    if (PathLength() < seekDistance && !playerMovement.inFreezeDialogue() && !playerMovement.timelinePlaying)
                    {
                        state = State.chase;
                    }
                    break;
                }
            case State.chase:
                {
                    if (!playerMovement.inFreezeDialogue() && !playerMovement.timelinePlaying)
                    {
                        animator.SetBool("attacking", false);
                        animator.SetBool("chasing", true);
                        //chase Behaviour
                        if (PathLength() > seekDistance && team == Team.oddle)
                        {
                            state = State.idle;
                        }
                        else if (PathLength() <= attackDistance)
                        {
                            state = State.attack;
                        }
                        else
                        {
                            MoveEnemy();
                        }
                    }
                    break;
                }
            case State.attack:
                {
                    if (!playerMovement.inFreezeDialogue() && !playerMovement.timelinePlaying)
                    {
                        //Activate Attack behaviour
                        Attack();

                        if (PathLength() > attackDistance)
                        {
                            animator.SetBool("attacking", false);
                            animator.SetBool("chasing", true);
                            state = State.chase;
                            attackSFXInstance.stop(0);
                            return;
                        }
                    }
                    else
                    {
                        // Prevent sudden attacks after cutscenes
                        Stun();
                    }
                    break;
                }
            case State.dying:
                {
                    // If attack sfx is playing, stop it
                    attackSFXInstance.stop(0);

                    //dying Behaviour
                    animationTimer += Time.deltaTime;
                    selfImage.color = Color.white;
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
                    if (team == Team.player)
                    {
                        Destroy(gameObject);
                    }
                    else if(playerAttack.reviveTimer.IsUseable() && playerAttack.InReviveRange(transform))
                    {
                        selfImage.color = reviveCol;
                    } 
                    else 
                    {
                        selfImage.color = Color.white;
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
                    if (!playerMovement.inFreezeDialogue() && !playerMovement.timelinePlaying)
                    {

                        //follow Behaviour
                        animator.SetBool("attacking", false);
                        animator.SetBool("chasing", true);
                        MoveEnemy();
                    }
                    break;
                }
        }
    }

    //Make an attempt at finding a new target
    virtual protected void FindTarget()
    {
        //Set the minimum target to the player

        if (!targetIsDropZone)
        {
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
    }

    //Moves this entity
    virtual protected void MoveEnemy()
    {
        if (target == null || targetPath == null || currentWaypoint >= targetPath.vectorPath.Count)
        {
            return;
        }

        //Calculate direction to travel to the next waypoint
        Vector2 direction = ((Vector2)targetPath.vectorPath[currentWaypoint] - rb.position + pathOffset).normalized;

        //Apply a force in that direction
        Vector2 force = direction * speed * Time.deltaTime;
        rb.AddForce(force);

        //Check distance to the current waypoint
        float distance = Vector2.Distance(rb.position, targetPath.vectorPath[currentWaypoint]);

        //If close enough to the current waypoint target the next waypoint
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        //Rotate enemy according to its direction of travel
        if (rb.velocity.x >= 0.01f)
        {
            enemygraphics.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (rb.velocity.x <= -0.01f)
        {
            enemygraphics.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    //Attack behaviour for this entity
    abstract protected void Attack();

    //Kill this entity
    virtual public void Kill()
    {
        // Play the death sfx
        FMODUnity.RuntimeManager.PlayOneShot(deathSfx, this.transform.position);
        
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

    //Revive this entity as an ally to the player
    virtual public bool Revive(float percentMaxHP = 1f, float percentDamage = 1f, float percentSpeed = 1f, float percentAttkSpeed = 1f)
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
            attackTimer.SetCooldown(attackTimer.cooldownDuration * percentAttkSpeed);

            //Re-enable collisions
            movementCollider.enabled = true;

            return true;
        } else
        {
            return false;
        }
    }

    // Function to run when enemies/allies takes damage
    virtual public void Damage(float damageTaken, bool makeInvincible = true, bool animateHurt = false, Vector2 knockbackDir = default(Vector2), float knockbackPower = 0f, bool lifeSteal = false)
    {
        // Don't hit dead bodies or buffed knights
        if (state == State.dead || state == State.dying || (team == Team.player && playerAttack.reviveTimer.IsActive() && !lifeSteal) || (type == Type.knight && buffed))
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
            rb.velocity = knockbackDir.normalized * knockbackPower * knockbackRatio;
        }
        
        //Flash hurt color
        if (animateHurt)
        {
            selfImage.color = hurtCol;
        }

        //Start invincibility timer
        if (makeInvincible)
        {
            invincibilityTimer.StartTimer();

         
            Stun();

            attackSFXInstance.stop(0);
        }

        return;
        
    }

    virtual public void Stun()
    {
        // Stop Attack sfx if playing
        attackSFXInstance.stop(0);

        if (!attackTimer.IsOnCooldown())
        {
            attackTimer.StartCooldown(attackCooldown * stunCooldownRatio);
        } else
        {
            attackTimer.StartCooldown(Mathf.Min(attackTimer.timer, attackCooldown * stunCooldownRatio));
        }
        animator.SetBool("attacking", false);
        animator.SetBool("chasing", true);
    }

    // Function to run when enemies/allies heal
    virtual public void Heal(float healthRestored)
    {
        if (health < maxHealth)
        {
            health += healthRestored;
        }
        else
        {
            health = maxHealth;
        }
        healthBar.SetHealth(health, maxHealth);
    }

    //Set a new target using a GameObject
    virtual public void SetTarget(GameObject obj, bool isPlayer = false, bool isDropZone = false)
    {
        target = obj.transform;
        targetIsPlayer = isPlayer;
        if(isDropZone)
        {
            targetIsDropZone = true;
        }
        else
        {
            targetIsDropZone = false;
        }
    }

    //Set a new target using a Transform
    virtual public void SetTarget(Transform transform, bool isPlayer = false, bool isDropZone = false)
    {
        target = transform;
        targetIsPlayer = isPlayer;
        if (isDropZone)
        {
            targetIsDropZone = true;
        }
        else
        {
            targetIsDropZone = false;
        }
    }

    //Get current target
    virtual public Transform GetTarget()
    {
        return target;
    }

    //Estimate the length of the current path
    virtual public float PathLength(bool toPlayer = false)
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

    virtual public bool isDead()
    {
        // Quick getter function that's used in CrabWalkSFX
        return (state == State.dead || state == State.dying);
    }

    [ContextMenu("Path Length")]
    private void ContextPathLength()
    {
        Debug.LogFormat("{0}\n{1}", PathLength(true), PathLength());
    }

    public CooldownTimer GetCooldownTimer()
    {
        return invincibilityTimer;
    }
}