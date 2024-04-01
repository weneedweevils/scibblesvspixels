using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubie : EnemyAI
{
    [Header("Cubie Specific References")]
    public GameObject ProjectileObject;

    private bool createdProjectile = true;
    private float windupDuration = 40f / 60f;
    private CooldownTimer windupTimer;

    //Random walk direction
    private Vector2 direction = new Vector2(0, 0);

    override protected void Start()
    {
        //Override variables
        deathDuration = 40f / 60f;
        reviveDuration = 69f / 60f;
        attackDuration = 60f / 60f;
        invincibilityDuration = 16f / 60f;

        //Create a windup timer
        windupTimer = new CooldownTimer(0f, windupDuration);

        //Continue with the base class implementation of Start
        base.Start();
    }

    override protected void FixedUpdate()
    {
        //Update the windup timer
        windupTimer.Update();

        //Continue with the base class implementation of FixedUpdate
        base.FixedUpdate();
    }

    override protected void Attack()
    {
        //Start the attack
        if (target != null && attackTimer.IsUseable())
        {
            createdProjectile = false;
            windupTimer.StartTimer();
            attackTimer.StartTimer();
            animator.SetBool("attacking", true);
            animator.SetBool("chasing", false);
            direction = new Vector2(Random.value - 0.5f, Random.value - 0.5f).normalized;
            rb.velocity = Vector2.zero;
        }

        //End of windup -> Fire Projectile
        if (!createdProjectile && !windupTimer.IsActive())
        {
            createdProjectile = true;

            //Create a projectile
            Instantiate(ProjectileObject, transform);
        }

        if (attackTimer.IsOnCooldown())
        {
            animator.SetBool("attacking", false);
            animator.SetBool("chasing", true);

            //Apply a force in that direction
            Vector2 force = direction * speed / 2 * Time.deltaTime;
            rb.AddForce(force);
        }
    }
}
