using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoodleCrab : EnemyAI
{
    override protected void Attack()
    {
        // Play the FMOD event correlating to the attack
        FMODUnity.RuntimeManager.PlayOneShot(attackSfx);

        //Lunge at the target
        Vector2 direction = ((Vector2)target.position - rb.position).normalized;
        rb.AddForce(direction * 25000f * Time.deltaTime);
    }
}
