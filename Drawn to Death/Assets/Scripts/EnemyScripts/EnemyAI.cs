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

    // Health
    public float health;
    public float maxHealth;
    public EnemyHealthBarBehaviour healthBar;




    //Animation
    private Animator animator;
    private float animationTimer = 0f;
    private float deathDuration = 28f/60f;
    private float reviveDuration = 69f/60f;

    // Start is called before the first frame update
    void Start()
    {

        animator = GetComponentInChildren<Animator>();
        //spriterenderer = GetComponent<SpriteRenderer>();
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
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
        float triggerAttack = Vector2.Distance(rb.position, target.position);
        animator.SetBool("chasing", true);

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
            animator.SetBool("attacking", true);
            Vector2 direction = ((Vector2)target.position - rb.position).normalized;
            rb.AddForce(direction * 25000f * Time.deltaTime);
            nextAttack = 0;
        }

        if (triggerChase > 10f)
        {
            animator.SetBool("attacking", false);
            animator.SetBool("chasing", true);
            state = State.chase;
            return;
          
        }
    }

    public void Kill()
    {
        //First Death
        if (team == Team.oddle)
        {
            team = Team.neutral;
        }
        state = State.dying;
        animator.SetBool("dying", true);
        rb.velocity = Vector2.zero;
    }

    public bool Revive()
    {
        if (state == State.dead && team == Team.neutral)
        {
            team = Team.player;
            state = State.reviving;
            animator.SetBool("reviving", true);
            return true;
        } else
        {
            return false;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("ouch I have collided with the player");
            health -= 10f; // CAN AND SHOULD BE CHANGED LATER TO REFERNCE PLAYER DAMAGE
            healthBar.SetHealth(health, maxHealth);
            if (health <= 0)
            {
                Kill(); // Ded
            }
        }
    }



}
