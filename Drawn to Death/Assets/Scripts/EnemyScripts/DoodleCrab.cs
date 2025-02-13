using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoodleCrab : EnemyAI
{
    [Header("Doodle Crab Specific References")]
    public float lungeForce = 30000f;
    public bool cutscene = false;
    private float windupDuration = 35f / 60f;
    private CooldownTimer windupTimer;
    private bool lunged = true;

    override protected void Start()
    {
        deathDuration = 25f / 60f;
        attackDuration = 60f / 60f;
        invincibilityDuration = 20f / 60f;
        type = Type.crab;

        //Create a windup timer
        windupTimer = new CooldownTimer(0f, windupDuration);

        base.Start();
    }

    override protected void FixedUpdate()
    {
        //Continue with the base class implementation of FixedUpdate
        base.FixedUpdate();
        if (!playerMovement.inFreezeDialogue() && !playerMovement.timelinePlaying)
        {
            //Update the windup timer
            windupTimer.Update();

            if (cutscene)
            {
                cutscene = false;
                base.healthBar.Enable();
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
    }

    override protected void Attack()
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
                            player.Damage(damage.value);  
                        }
                        break;
                    }
                case "Enemy":
                    {
                        //Get a reference to the enemy
                        EnemyAI otherAI = collision.gameObject.GetComponent<EnemyAI>();
                        HealthCrystal crystal = collision.gameObject.GetComponent<HealthCrystal>();
                        Boss oodler = collision.gameObject.GetComponent<Boss>();


                        if (otherAI != null) {
                            if (attackTimer.IsActive() && !windupTimer.IsActive() && otherAI != null && team != otherAI.team && otherAI.team != Team.neutral && otherAI.invincibilityTimer2.IsUseable())
                            {
                                //Damage enemy
                                otherAI.Damage(damage.value, false, true);

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
                                    crystal.CrystalDamage(damage.value, true);
                                }
                            }


                        else if (oodler != null)
                        {
                            if (attackTimer.IsActive() && !windupTimer.IsActive() && oodler != null && oodler.BossIsDamageable() && !invincibilityTimerOodler.IsActive())//!oodler.invincibilityTimer.IsActive())
                            {

                                //Damage enemy
                                oodler.Damage(damage.value);
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
