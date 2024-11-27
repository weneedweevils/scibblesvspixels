using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OodleHopper : EnemyAI
{
    [Header("Doodle Hopper Specific References")]
    public float lungeForce = 30000f;
    public bool cutscene = false;
    private float hopInterval = 120f / 60f;
    private float hopDuration = 47f / 60f;
    private CooldownTimer hopCooldown;
    [HideInInspector] public SpriteRenderer healImage;
    public float passiveHealAmount = 1f;
    public float abilityHealAmount = 2f;

    override protected void Start()
    {
        deathDuration = 56f / 60f;
        attackDuration = 300f / 60f;
        invincibilityDuration = 20f / 60f;
        type = Type.hopper;

        //Create timers
        hopCooldown = new CooldownTimer(hopInterval, hopDuration);

        healImage = this.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>();
        healImage.transform.localScale *= attackDistance * 11f;

        base.Start();

        // Override target defaults
        target = null;
        targetIsPlayer = false;
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

            // Constant Slow Passive Heals
            if (!lifestealing && state != State.dying && state != State.dead) 
            {
                Heal(passiveHealAmount * Time.deltaTime);
            }
        }
        //State Manager
        switch (state)
        {
            case State.idle:
                {
                    Debug.Log("Idle");
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
                        else if (PathLength() <= attackDistance && attackTimer.IsUseable())
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
                        //Activates healing for whatever team the oodle hopper is on
                        Attack();
                        if (!attackTimer.IsActive() && !hopCooldown.IsActive())
                        {
                            Debug.Log("Attack Over");
                            animator.SetBool("hopping", false);
                            animator.SetBool("idle", true);
                            if (team == Team.player)
                            {
                                state = State.follow;
                                Debug.Log("Follow");
                            }
                            else if (team == Team.oddle)
                            {
                                state = State.chase;
                                Debug.Log("Chase");
                            }
                            attackSFXInstance.stop(0);
                            healImage.enabled = false;
                            return;
                        }
                    }
                    break;
                }
            case State.dying:
                {
                    // If attack sfx is playing, stop it
                    attackSFXInstance.stop(0);
                    healImage.enabled = false;

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
                        animator.SetBool("idle", false);
                        animator.SetBool("hopping", false);
                        animator.SetBool("dying", false);
                    }
                    break;
                }
            case State.follow: 
                {
                    if (!playerMovement.inFreezeDialogue() && !playerMovement.timelinePlaying)
                    {
                        //follow Behaviour
                        //follow player and heal when in range
                        if (PathLength() <= attackDistance && attackTimer.IsUseable())
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
        }

        if (!playerMovement.inFreezeDialogue() && !playerMovement.timelinePlaying)
        {
            //Update timers
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
        // Stop hopper after hop duration is over
        if (hopCooldown.IsOnCooldown() && (rb.velocity != Vector2.zero || rb.angularVelocity != 0f || animator.GetBool("hopping")))
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            animator.SetBool("hopping", false);
            animator.SetBool("idle", true);
        }
    }

    protected override void FindTarget()
    {
        if (!targetIsDropZone)
        {
            //Find closest enemy target in range
            float minDist = float.MaxValue;
            float targetDistance = 10000f;

            //Iterate through all enemies
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                EnemyAI enemy = obj.GetComponent<EnemyAI>();
                HealthCrystal crystal = obj.GetComponent<HealthCrystal>();
                Boss oodler = obj.GetComponent<Boss>();

                if (enemy != null)
                {
                    //Ignore doodleBars and any enemies that are not part of the enemy team
                    if (enemy == null || enemy.team != Team.oddle || enemy is DoodleBars || obj == gameObject)
                    {
                        continue;
                    }

                    float dist = Vector2.Distance(rb.position, enemy.transform.position);
                    if (dist <= targetDistance && dist < minDist)
                    {
                        target = enemy.transform;
                        minDist = dist;
                        targetIsPlayer = false;
                    }
                }


                else if (crystal != null)
                {
                    float dist = Vector2.Distance(rb.position, crystal.transform.position);
                    if (dist <= targetDistance && dist < minDist)
                    {
                        target = crystal.transform;
                        minDist = dist;
                        targetIsPlayer = false;
                    }
                }

                else if (oodler != null && oodler.BossIsDamageable())
                {
                    float dist = Vector2.Distance(rb.position, crystal.transform.position);
                    if (dist <= targetDistance && dist < minDist)
                    {
                        target = oodler.transform;
                        minDist = dist;
                        targetIsPlayer = false;
                    }
                }
                else
                {
                    continue;
                }
            }
        }
    }

    override protected void Attack() // Heal Ring
    {
        if (target != null && attackTimer.IsUseable() && !hopCooldown.IsActive())
        {
            //Start the Attack Timer
            attackTimer.StartTimer();
            animator.SetBool("hopping", false);
            animator.SetBool("idle", true);
            healImage.enabled = true;

            // play the attack sfx
            attackSFXInstance.start();
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            EnemyAI enemy = obj.GetComponent<EnemyAI>();
            //LineRenderer line = enemy.GetComponent<LineRenderer>();
            if (enemy != null)
            {
                if (CustomDist(healImage.transform.position, enemy.transform.position + 4f * Vector3.down) <= attackDistance && enemy.team == team) // Heal oodles on the same team
                {
                    enemy.Heal(abilityHealAmount * Time.deltaTime);
                }
            }
        }

        if (CustomDist(healImage.transform.position, player.transform.position + 4f * Vector3.down) <= attackDistance && team == Team.player) // Heal player if ally
        {
            player.GetComponent<PlayerMovement>().Heal(abilityHealAmount * Time.deltaTime);
        }

        if (attackTimer.IsOnCooldown())
        {
            healImage.enabled = false;
        }
    }

    private float CustomDist(Vector3 a, Vector3 b)
    {
        float xScale = 1f;
        float yScale = 0.5f;
        return Mathf.Sqrt(Mathf.Pow(((a.x - b.x) / xScale), 2) + Mathf.Pow(((a.y - b.y) / yScale), 2));
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
            animator.SetBool("hopping", true);
            animator.SetBool("idle", false);

            // play the attack sfx TODO: REPLACE EVENTUALLY
            attackSFXInstance.start();

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

    public override void Kill()
    {
        //Set Animation variables
        //For Oodle Hopper hopping replaces attacking and idle replaces chasing for more accurate bool names
        animator.SetBool("hopping", false);
        animator.SetBool("idle", false);
        base.Kill();
    }

    public override void Stun()
    {
        //Set Animation variables
        animator.SetBool("hopping", false);
        animator.SetBool("idle", true);
        base.Stun();
    }
}

