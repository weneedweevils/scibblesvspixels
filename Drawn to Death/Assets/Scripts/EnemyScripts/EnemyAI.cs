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
    public GameObject player;
    public Team team = Team.oddle;
    public State state = State.chase;
    public float speed = 200f;
    public float seekDistance = 100f; 
    public float nextWaypointDistance;
    public Transform enemygraphics;
    Transform target;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath;
    Seeker seeker;
    Rigidbody2D rb;
    public float damage;
    public Sprite attacksprite;
    public float cooldown = 2f;
    public float nextAttack;
    public bool isolated = false;

    // Health
    public float health;
    public float maxHealth;
    public EnemyHealthBarBehaviour healthBar;

    //Invincibility Frames
    public CooldownTimer invincibilityTimer;
    public CooldownTimer invincibilityTimer2;
    private float invincibilityDuration = 20f / 60f;

    //Animation and sprites
    public Color hurtCol = Color.red;
    private SpriteRenderer doodleCrab;
    private SpriteRenderer gem;
    private Animator animator;
    private float animationTimer = 0f;
    private float deathDuration = 25f/60f;
    private float reviveDuration = 69f/60f;

    // FMOD Event paths
    public string deathSfx;
    public string attackSfx;

    // Music manager script
    public GameObject musicmanager;
    BasicMusicScript musicscript;

    private void OnDrawGizmosSelected()
    {
        if (path != null)
        {
            Debug.Log(string.Format("Path size: {0}\nPath length: {1}", path.vectorPath.Count, PathLength()));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        doodleCrab = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        gem = gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        //animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
        InvokeRepeating("CheckState", 0f, 0.5f); // this will call the checkstate function to update the path every half second
        if (player == null)
        {
            player = GameObject.Find("Player");
        }
        target = player.transform;
        health = maxHealth;
        healthBar.SetHealth(health, maxHealth);

        invincibilityTimer = new CooldownTimer(invincibilityDuration * 0.5f, invincibilityDuration * 0.5f);
        invincibilityTimer2 = new CooldownTimer(3f, invincibilityDuration);

        musicmanager = GameObject.Find("Music");
        musicscript = musicmanager.GetComponent<BasicMusicScript>();

    }

    void CheckState()
    {
        float inrange = Vector2.Distance(rb.position, target.position);

      
     
        

        // if not travelling to a path and the player is within range calculate new path
        if (seeker.IsDone() && inrange < seekDistance)
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    // Checks if there is a path calculated
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        } else
        {
            path = null;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        invincibilityTimer.Update();
        invincibilityTimer2.Update();

        if (invincibilityTimer.IsOnCooldown())
        {
            doodleCrab.color = Color.white;
        }

        switch (state)
        {
            case State.idle:
                {
                    //idle Behaviour
                    break;
                }
            case State.chase:
                {
                    //chase Behaviour
                    MoveEnemy();
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
        
        if (health <= 0 && state != State.dead && state != State.dying)
        {
            Kill(); // Ded
        }

    }

    // moves enemy and adjusts animation to face player
    void MoveEnemy()
    {
        
        float triggerAttack = Vector2.Distance(rb.position, target.position);

        // if we are in range switch to the attack state
        if (triggerAttack < 10f)
        {
            animator.SetBool("chasing", false);
            animator.SetBool("attacking", true);
            state = State.attack;
            return;
        }

        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

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

    public void Attack()
    {
        float triggerChase = Vector2.Distance(rb.position, target.position);
        nextAttack += Time.deltaTime;


        if (nextAttack >= cooldown)
        {
            // Play the FMOD event correlating to the attack
            FMODUnity.RuntimeManager.PlayOneShot(attackSfx);
       
            Vector2 direction = ((Vector2)target.position - rb.position).normalized;
            //rb.AddForce(direction * 25000f * Time.deltaTime);
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
        
        if (team == Team.oddle) //First Death
        {
            team = Team.neutral;
        }
        health = 0;
        state = State.dying;
        animator.SetBool("attacking", false);
        animator.SetBool("chasing", false);
        animator.SetBool("dying", true);
        rb.velocity = Vector2.zero;
    }

    public bool Revive(float percentMaxHP = 1f, float percentDamage = 1f, float percentSpeed = 1f)
    {
        if (state == State.dead && team == Team.neutral)
        {
            team = Team.player;
            state = State.reviving;
            animator.SetBool("reviving", true);
            gem.enabled = true;
            maxHealth *= percentMaxHP;
            damage *= percentDamage;
            speed *= percentSpeed;
            health = maxHealth;
            
            return true;
        } else
        {
            return false;
        }
    }

    // Function to run when player takes damage
    public void Damage(float damageTaken, bool makeInvincible = true, bool animateHurt = false, Vector2 kockbackDir = default(Vector2), float kockbackPower = 0f)
    {
        health -= damageTaken;
        healthBar.SetHealth(health, maxHealth);
        rb.velocity = kockbackDir.normalized * kockbackPower;
        
        if (makeInvincible)
        {
            invincibilityTimer.StartTimer();
        }
        if (animateHurt)
        {
            doodleCrab.color = hurtCol;
        }
    }

    public void SetTarget(GameObject obj)
    {
        target = obj.transform;
    }

    public void SetTarget(Transform transform)
    {
        target = transform;
    }

    //Estimate the length of the current path
    public float PathLength()
    {
        //No path
        if (path == null)
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
            if (playerAttack != null && playerAttack.attackTimer.IsActive() && team == Team.oddle && !isolated && PathLength() < 13f)
            {
                Vector2 direction = (rb.position - (Vector2)playerAttack.transform.position).normalized;
                Damage(playerAttack.damage, true, true, direction, playerAttack.knockback);

                Debug.Log(string.Format("ouch I have been hit. Health remaining: {0}", health));
                musicscript.setIntensity(20f);
            }
        }
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyAI otherai = collision.gameObject.GetComponent<EnemyAI>();

            if (team == Team.oddle && otherai.team == Team.player && invincibilityTimer2.IsUseable() && health > 0)
            {
                health -= 5;
                invincibilityTimer2.StartTimer();
                Debug.Log(string.Format("ouch I have been hit. Health remaining: {0}", health));
            }
            healthBar.SetHealth(health, maxHealth);
        }

    }
}