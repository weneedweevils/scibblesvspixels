using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OodleSnek : EnemyAI
{
    private float windupDuration = 35f / 60f;
    private CooldownTimer windupTimer;

    [Header("Oodle Snek Specific References")]
    public StatusEffect[] effects;

    override protected void Start()
    {
        deathDuration = 60f / 60f;
        attackDuration = 60f / 60f;
        invincibilityDuration = 20f / 60f;
        type = Type.crab;

        //Create a windup timer
        windupTimer = new CooldownTimer(0f, windupDuration);

        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        animator.SetBool("moving up", rb.velocity.y > 0);
    }

    override protected void Attack()
    {
        return;
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

    protected StatusEffect RandomEffect()
    {
        if (effects.Length > 0)
            return effects[Random.Range(0, effects.Length - 1)];
        else
            return null;
    }

    [ContextMenu("ApplyRandomEffect")]
    public void ApplyRandomEffect()
    {
        Debug.Log("Getting StatusEffectController");
        StatusEffectController controller = player.GetComponent<StatusEffectController>();
        Debug.Log("Getting StatusEffect");
        StatusEffect effect = RandomEffect();
        Debug.LogFormat("Applying {0} to {1}", effect.effectName, controller.name);
        controller?.AddStatusEffect(effect);
    }
}