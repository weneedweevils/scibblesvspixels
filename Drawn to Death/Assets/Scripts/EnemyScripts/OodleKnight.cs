using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OodleKnight : EnemyAI
{
    [Header("Oodle Knight Specific References")]
    public float knockback = 0f;

    protected Vector2 targetOffset = new Vector2(0, 1.5f);
    private bool cutscene = false;
    private float windupDuration = 28f / 60f;
    private CooldownTimer windupTimer;
    private KnightAttack attackObject;

    override protected void Start()
    {
        //Override variables
        deathDuration = 88f / 60f;
        attackDuration = 44f / 60f;
        invincibilityDuration = 20f / 60f;
        type = Type.knight;

        //Create a windup timer
        windupTimer = new CooldownTimer(attackDuration - windupDuration, windupDuration);

        //Coolect Object references
        attackObject = GetComponentInChildren<KnightAttack>();
        if (attackObject == null) Debug.LogError("Error - " + gameObject.name + " is missing reference to an Attack Object");

        //Continue with the base class implementation of Start
        base.Start();
    }

    override protected void FixedUpdate()
    {
        //Flip attack object
        attackObject.gameObject.transform.localRotation = enemygraphics.localRotation;

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
    }

    override protected void Attack()
    {
        
        //Face the target
        if (target != null && (target.position.x > transform.position.x))
        {
            enemygraphics.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            enemygraphics.localRotation = Quaternion.Euler(0, 0, 0);
        }

        //Try to get into position to attack the target
        if (!attackTimer.IsActive())
        {
            animator.SetBool("attacking", false);
            animator.SetBool("chasing", true);

            //Update target offset
            if (target != null)
            {
                targetOffset.Set(((target.position.x > transform.position.x) ? -1 : 1) * 3f, -1f);

                //Calculate direction to travel to the next waypoint
                Vector2 direction = ((Vector2)target.position + targetOffset - rb.position).normalized;

                //Apply a force in that direction
                Vector2 force = direction * speed.value * 0.8f * Time.deltaTime;
                rb.AddForce(force);
            }
        }
    }

    override public void Stun()
    {
        windupTimer.ResetTimer();
        base.Stun();
    }

    override public float PathLength(bool toPlayer = false)
    {
        if (state == State.attack && !(toPlayer && !targetIsPlayer) && target != null)
        {
            return Vector2.Distance(rb.position, target.position);
        }
        else
        {
            return base.PathLength(toPlayer);
        }
    }

    public void DeferredOnTriggerStay2D(Collider2D collision)
    {
        if (health > 0)
        {
            //Start the attack
            if (target != null && state == State.attack && attackTimer.IsUseable() && collision.gameObject.transform == target)
            {
                // start the attack sfx
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(attackSFXInstance, GetComponent<Transform>(), GetComponent<Rigidbody2D>());
                attackSFXInstance.start();

                windupTimer.StartTimer();
                attackTimer.StartTimer();
                animator.SetBool("attacking", true);
                animator.SetBool("chasing", false);
            }

            //Try to damage the target
            if (attackTimer.IsActive() && windupTimer.IsOnCooldown())
            {   
                switch (collision.gameObject.tag)
                {
                    case "Player":
                        {
                            //Get a reference to the player
                            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
                            if (team == Team.oddle && player.invincibilityTimer.IsUseable())
                            {
                                //Calculate knockback
                                Vector2 direction = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;

                                //Damage player
                                player.Damage(damage.value, direction, knockback);
                            }
                            break;
                        }
                    case "Enemy":
                        {
                            //Get a reference to the enemy
                            EnemyAI otherAI = collision.gameObject.GetComponent<EnemyAI>();
                            if (otherAI != null && team != otherAI.team && otherAI.team != Team.neutral && otherAI.invincibilityTimer2.IsUseable())
                            {
                                //Calculate knockback
                                Vector2 direction = ((Vector2)otherAI.transform.position - (Vector2)transform.position).normalized;

                                //Damage enemy
                                otherAI.Damage(damage.value, false, true, direction, knockback);
                                Debug.LogFormat("{0} Hit {1} for {2} Damage", name, otherAI.name, damage);

                                //Start enemies secondary invincibility timer
                                otherAI.invincibilityTimer2.StartTimer();
                                otherAI.Stun();
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
}
