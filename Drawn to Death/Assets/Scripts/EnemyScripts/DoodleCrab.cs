using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoodleCrab : EnemyAI
{
    [Header("Doodle Crab Specific References")]
    public float lungeForce = 25000f;
    public bool cutscene = false;

    override protected void Start()
    {
        deathDuration = 25f / 60f;
        attackDuration = 12f / 60f;
        invincibilityDuration = 20f / 60f;
        base.Start();
    }

    override protected void FixedUpdate()
    {
        //Continue with the base class implementation of FixedUpdate
        base.FixedUpdate();
        if (!playerMovement.inFreezeDialogue() && !playerMovement.timelinePlaying)
        {
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
    }

    override protected void Attack()
    {
        if (target != null && attackTimer.IsUseable())
        {
            //Play the FMOD event correlating to the attack
            FMODUnity.RuntimeManager.PlayOneShot(attackSfx);
            
            //Lunge at the target
            Vector2 direction = ((Vector2)target.position - rb.position).normalized;
            rb.AddForce(direction * lungeForce * Time.deltaTime);
            
            //Start the Attack Timer
            attackTimer.StartTimer();
            animator.SetBool("attacking", true);
            animator.SetBool("chasing", false);
        }

        if (attackTimer.IsOnCooldown())
        {
            animator.SetBool("attacking", false);
            animator.SetBool("chasing", true);
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
                        if (attackTimer.IsActive() && team == Team.oddle && player.invincibilityTimer.IsUseable())
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


                        if (otherAI != null) {
                            if (attackTimer.IsActive() && otherAI != null && team != otherAI.team && otherAI.team != Team.neutral && otherAI.invincibilityTimer2.IsUseable())
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

                                if (attackTimer.IsActive() && crystal != null && crystal.invincibilityTimer.IsUseable())
                                {
                                    //Damage crystal
                                    crystal.CrystalDamage(damage, true);
                                }
                            }


                        else if (oodler != null)
                        {
                            if (attackTimer.IsActive() && oodler != null && oodler.BossIsDamageable() && !invincibilityTimerOodler.IsActive())//!oodler.invincibilityTimer.IsActive())
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
