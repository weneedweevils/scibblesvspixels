using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OodleHopper : EnemyAI
{
    [Header("Doodle Hopper Specific References")]
    public float lungeForce = 30000f;
    public bool cutscene = false;
    private float windupDuration = 35f / 60f;
    private float hopInterval = 120f / 60f;
    private float hopDuration = 47f / 60f;
    private CooldownTimer windupTimer;
    private CooldownTimer hopCooldown;
    private bool lunged = true;

    override protected void Start()
    {
        deathDuration = 56f / 60f;
        attackDuration = 47f / 60f;
        invincibilityDuration = 20f / 60f;
        type = Type.hopper;

        //Create timers
        windupTimer = new CooldownTimer(0f, windupDuration);
        hopCooldown = new CooldownTimer(hopInterval, hopDuration);

        base.Start();
    }

    override protected void FixedUpdate()
    {
        //Continue with the base class implementation of FixedUpdate with slight modifications
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
                    else if (playerAttack.reviveTimer.IsUseable() && playerAttack.InReviveRange(transform))
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

        if (!playerMovement.inFreezeDialogue() && !playerMovement.timelinePlaying)
        {
            //Update timers
            windupTimer.Update();
            hopCooldown.Update();

            if (cutscene)
            {
                cutscene = false;
                base.healthBar.SetHealth(base.health, base.maxHealth);
            }
        }
        else if (!cutscene)
        {
            cutscene = true;
            base.healthBar.Disable();
        }
        else
        {
            // Prevents lunge from happening if cutscene interupted attack
            lunged = true;
        }
        // Stop hopper after hop duration is over
        if (hopCooldown.IsOnCooldown() && (rb.velocity != Vector2.zero || rb.angularVelocity != 0f))
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            animator.SetBool("attacking", false);
            animator.SetBool("chasing", true);
        }
    }

    override protected void Attack() // Heal Ring
    {
        if (target != null && attackTimer.IsUseable())
        {
            //Start the Attack and Windup Timers
            lunged = false;
            attackTimer.StartTimer();
            windupTimer.StartTimer();
            animator.SetBool("attacking", true);
            animator.SetBool("chasing", false);
        }

        if (PathLength() > attackDistance)
        {
            // Prevents lunge with no animation if player goes out of range
            lunged = true;
        }

        if (!lunged && !windupTimer.IsActive())
        {
            // play the attack sfx
            attackSFXInstance.start();

            //Lunge at the target
            Vector2 direction = ((Vector2)target.position - rb.position).normalized;
            rb.AddForce(direction * lungeForce * Time.deltaTime);
            lunged = true;
        }

        if (attackTimer.IsOnCooldown())
        {
            animator.SetBool("attacking", false);
            animator.SetBool("chasing", true);
        }
    }

    override public void Stun()
    {
        windupTimer.ResetTimer();
        base.Stun();
    }

    protected override void MoveEnemy()
    {
        if (target == null || targetPath == null || currentWaypoint >= targetPath.vectorPath.Count)
        {
            return;
        }

        //Calculate direction to travel to the next waypoint
        Vector2 direction = ((Vector2)targetPath.vectorPath[currentWaypoint] - rb.position + pathOffset).normalized;

        if (hopCooldown.IsUseable())
        {
            hopCooldown.StartTimer();
            animator.SetBool("attacking", true);
            animator.SetBool("chasing", false);

            //Apply a force in that direction
            Vector2 force = direction * speed * Time.deltaTime;
            rb.AddForce(force);
        }

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

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (health > 0)
        {
            switch (collision.gameObject.tag)
            {
                case "Player":
                    {

                        //Get a reference to the player
                        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
                        if (attackTimer.IsActive() && !windupTimer.IsActive() && team == Team.oddle && player.invincibilityTimer.IsUseable())
                        {
                            //Damage player
                            player.Damage(damage);
                        }
                        break;
                    }
                case "Enemy":
                    {
                        //Get a reference to the enemy
                        EnemyAI otherAI = collision.gameObject.GetComponent<EnemyAI>();
                        HealthCrystal crystal = collision.gameObject.GetComponent<HealthCrystal>();
                        Boss oodler = collision.gameObject.GetComponent<Boss>();


                        if (otherAI != null)
                        {
                            if (attackTimer.IsActive() && !windupTimer.IsActive() && otherAI != null && team != otherAI.team && otherAI.team != Team.neutral && otherAI.invincibilityTimer2.IsUseable())
                            {
                                Debug.Log(string.Format("{0} Hut {1} for {2} damage", name, otherAI.name, damage));
                                //Damage enemy
                                otherAI.Damage(damage, false, true);

                                //Start enemies secondary invincibility timer
                                otherAI.invincibilityTimer2.StartTimer();
                                otherAI.Stun();
                            }
                        }

                        else if (crystal != null)
                        {

                            //Debug.Log(otherAI.invincibilityTimer2.IsUseable());

                            if (attackTimer.IsActive() && !windupTimer.IsActive() && crystal != null && crystal.invincibilityTimer.IsUseable())
                            {
                                //Damage crystal
                                crystal.CrystalDamage(damage, true);
                            }
                        }


                        else if (oodler != null)
                        {
                            if (attackTimer.IsActive() && !windupTimer.IsActive() && oodler != null && oodler.BossIsDamageable() && !invincibilityTimerOodler.IsActive())//!oodler.invincibilityTimer.IsActive())
                            {

                                //Damage enemy
                                oodler.Damage(damage);
                                invincibilityTimerOodler.StartTimer();

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
    }
}

