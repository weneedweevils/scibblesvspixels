using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OodleSnek : EnemyAI
{
    private float windupDuration = 35f / 60f;
    private StateTimer windupTimer;

    [Header("Oodle Snek Specific References")]
    public GameObject paintObject;
    private SpriteRenderer paintRenderer;
    private Animator paintAnimator;
    public GameObject projectileObject;
    private SnekProjectile snekProjectile;
    public StatusEffect defaultEffect;
    public StatusEffect rallyEffect;
    public StatusEffect currentEffect { get; private set; }

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

        paintRenderer = paintObject.GetComponent<SpriteRenderer>();
        paintAnimator = paintObject.GetComponent<Animator>();

        SetEffect(defaultEffect);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        animator.SetBool("moving up", rb.velocity.y > 0);

        paintAnimator.SetBool("attacking", animator.GetBool("attacking"));
        paintAnimator.SetBool("chasing", animator.GetBool("chasing"));
        paintAnimator.SetBool("dying", animator.GetBool("dying"));
        paintAnimator.SetBool("reviving", animator.GetBool("reviving"));
        paintAnimator.SetBool("idle", animator.GetBool("idle"));
        paintAnimator.SetBool("moving up", animator.GetBool("moving up"));
    }

    override protected void Attack()
    {
        //Start the attack
        if (target != null && attackTimer.IsUseable())
        {
            // play the attack sfx
            attackSFXInstance.start();

            //Setup Timers
            windupTimer.Start(EndWindup);
            attackTimer.StartTimer();

            //Handle Animations
            animator.SetBool("attacking", true);
            paintAnimator.SetBool("attacking", true);
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
            Vector2 force = direction * speed.value / 2 * Time.deltaTime;
            rb.AddForce(force);
        }
    }

    override public void Stun()
    {
        windupTimer.Stop();
        base.Stun();
        paintAnimator.SetBool("attacking", false);
    }

    public void EndWindup()
    {
        Instantiate(projectileObject, transform);
    }

    public void SetEffect(StatusEffect newEffect)
    {
        currentEffect = newEffect;
        paintRenderer.color = currentEffect.paintColor;
    }
}