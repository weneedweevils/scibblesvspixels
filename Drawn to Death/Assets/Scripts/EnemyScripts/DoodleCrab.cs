using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoodleCrab : EnemyAI
{
    [Header("Doodle Crab Specific References")]
    public float lungeForce = 25000f;

    override protected void Start()
    {
        deathDuration = 25f / 60f;
        reviveDuration = 69f / 60f;
        attackDuration = 4f / 60f;
        base.Start();
    }

    override protected void FixedUpdate()
    {
        //Continue with the base class implementation of FixedUpdate
        base.FixedUpdate();
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
}
