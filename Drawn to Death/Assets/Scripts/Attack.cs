using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    //Player game object
    public GameObject player;

    //Use key
    [Header("Controls")]
    public KeyCode attackButton = KeyCode.Mouse0;
    public KeyCode reviveButton = KeyCode.Mouse1;

        /* ----- Attacking ----- */

    [Header("Attack Info")]
    //Stats
    public float damage = 100;
    public float piercing = 3;
    
    //Timer
    public float attackCooldown = 0f;
    private float attackDuration = 30f/60f;
    private float attackTimer = 0f;

    //Cooldown
    private bool attacking = false;
    private bool attackOnCooldown = false;
    
    //Hitbox
    private BoxCollider2D hitbox;
    private Vector2 initialHitboxOffset;
    private Vector2 initialHitboxSize;

        /* ----- Reviving ----- */

    [Header("Revive Info")]
    //Stats
    public float reviveRadius;
    public int reviveCap;

    //Cooldown
    public float reviveCooldown = 0f;
    private float reviveTimer = 0f;
    private bool reviveOnCooldown = false;

    //Misc
    private List<EnemyAI> allies = new List<EnemyAI>();
    private SpriteRenderer reviveImage;


        /* ----- Misc ----- */

    //Animation
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        //Collect components
        animator = GetComponent<Animator>();
        hitbox = GetComponent<BoxCollider2D>();
        reviveImage = player.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
        reviveImage.transform.localScale *= reviveRadius * 10.45f;

        //Save initial attack hitbox information
        initialHitboxOffset = hitbox.offset;
        initialHitboxSize = hitbox.size;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.GetComponent<PlayerMovement>().inFreezeDialogue() && !player.GetComponent<PlayerMovement>().timelinePlaying)
        {
            CheckAttack();
            CheckRevive();
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
        if (attacking || attackOnCooldown)
        {
            attackTimer += Time.deltaTime;
        }
        //Check if attack is over -> start the cooldown
        if (attacking && attackTimer >= attackDuration)
        {
            attackOnCooldown = true;
            attacking = false;
            animator.SetBool("attacking", false);
            attackTimer -= attackDuration;
        }
        //Check if cooldown is over
        if (attackOnCooldown && attackTimer >= attackCooldown)
        {
            attackOnCooldown = false;
            attackTimer = 0f;
        }
        //Attack
        if (!attacking && !attackOnCooldown && Input.GetKey(attackButton))
        {
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                EnemyAI AI = enemy.GetComponent<EnemyAI>();
                if (AI != null && AI.team == Team.oddle)
                {
                    AI.Kill();
                }
            }
            animator.SetBool("attacking", true);
            attacking = true;
        }
    }

    public void CheckRevive()
    {
        //Revive Timer
        if (reviveOnCooldown)
        {
            reviveTimer += Time.deltaTime;
        }
        //Check if cooldown is over
        if (reviveTimer >= reviveCooldown)
        {
            reviveOnCooldown = false;
            reviveTimer = 0f;
        }
        //Revive
        if (Input.GetKeyDown(reviveButton))
        {
            reviveImage.enabled = true;
        }
        if (Input.GetKeyUp(reviveButton))
        {
            if (!reviveOnCooldown)
            {
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    EnemyAI AI = enemy.GetComponent<EnemyAI>();
                    if (AI == null || allies.Count >= reviveCap)
                    {
                        continue;
                    }

                    if (CustomDist(reviveImage.transform.position, AI.transform.position + 2.5f * Vector3.down) <= reviveRadius)
                    {
                        if (AI.Revive())
                        {
                            allies.Add(AI);
                        }
                    }

                }
                reviveOnCooldown = true;
            }
            reviveImage.enabled = false;
        }
    }

    public float CustomDist(Vector3 a, Vector3 b)
    {
        float xScale = 1f;
        float yScale = 0.5f;
        return Mathf.Sqrt(Mathf.Pow(((a.x - b.x) / xScale), 2) + Mathf.Pow(((a.y - b.y) / yScale), 2));
    }
}
