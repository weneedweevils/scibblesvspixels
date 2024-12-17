using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OodleSnek : EnemyAI
{
    private float windupDuration = 35f / 60f;
    private StateTimer windupTimer;

    [Header("Oodle Snek Specific References")]
    public SpriteRenderer attackImage;
    private Animator attackAnimator;
    public GameObject projectileObject;
    private SnekProjectile snekProjectile;
    public StatusEffect[] effects;
    private StatusEffect currentEffect;

    //Random walk direction
    private Vector2 direction = new Vector2(0, 0);

    override protected void Start()
    {
        deathDuration = 60f / 60f;
        attackDuration = 60f / 60f;
        invincibilityDuration = 20f / 60f;
        type = Type.snek;

        //Create a windup timer
        windupTimer = new StateTimer(new float[] { windupDuration });
        windupTimer.Initialize();

        base.Start();

        attackAnimator = attackImage.GetComponent<Animator>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        animator.SetBool("moving up", rb.velocity.y > 0);
        attackAnimator.SetBool("attacking", animator.GetBool("attacking"));
    }

    override protected void Attack()
    {
        //Start the attack
        if (target != null && attackTimer.IsUseable())
        {
            // Select an effect
            currentEffect = RandomEffect();
            attackImage.color = currentEffect.paintColor;

            // play the attack sfx
            attackSFXInstance.start();

            //Setup Timers
            windupTimer.Start(EndWindup);
            attackTimer.StartTimer();

            //Handle Animations
            animator.SetBool("attacking", true);
            attackAnimator.SetBool("attacking", true);
            animator.SetBool("chasing", false);

            //Movement
            direction = new Vector2(Random.value - 0.5f, Random.value - 0.5f).normalized;
            rb.velocity = Vector2.zero;
        }

        //Wait for cooldown
        if (attackTimer.IsOnCooldown())
        {
            //Handle Animations
            animator.SetBool("attacking", false);
            animator.SetBool("chasing", true);

            //Apply a force in the random walk direction
            Vector2 force = direction * speed / 2 * Time.deltaTime;
            rb.AddForce(force);
        }
    }

    override public void Stun()
    {
        windupTimer.Stop();
        base.Stun();
        attackAnimator.SetBool("attacking", false);
    }

    public void EndWindup()
    {
        Instantiate(projectileObject, transform);
    }

    protected StatusEffect RandomEffect()
    {
        if (effects.Length > 0)
            return effects[Random.Range(0, effects.Length - 1)];
        else
            return null;
    }

    public StatusEffect GetEffect()
    {
        return currentEffect;
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