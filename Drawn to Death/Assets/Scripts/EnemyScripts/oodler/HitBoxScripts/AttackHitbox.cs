using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox:MonoBehaviour

    
{

    public PlayerMovement PlayerScript;
    public Oodler oodlerScript;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided with " + collision.name);

        switch (collision.gameObject.tag)
        {

            case "Player":
                {
                    //if (oodlerSlamCooldown == false && !PlayerScript.dashTimer.IsActive())
                    if (!PlayerScript.dashTimer.IsActive() && !oodlerScript.OnSlamCooldown() && !PlayerScript.invincibilityTimer.IsActive() && oodlerScript.activateDamage())
                    {
                       
                        PlayerScript.Damage(oodlerScript.oodlerAttackDamage);
                        PlayerScript.animator.SetTrigger("Squished");
                    }
                  
                }
                break;

            case "Enemy":
                {
                    EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();


                    if (enemy != null && !enemy.invincibilityTimer.IsActive() && !oodlerScript.OnSlamCooldown()) //&& oodlerScript.activateDamage())
                    {
                        enemy.Damage(oodlerScript.oodlerAttackDamage);
                    }

                    else
                    {
                        HealthCrystal crystal = collision.gameObject.GetComponent<HealthCrystal>();
                        if (crystal != null)
                        {
                            if (crystal != null && crystal.invincibilityTimer.IsUseable() && !oodlerScript.OnSlamCooldown())// && oodlerScript.activateDamage())
                            {
                                //Damage enemy
                                crystal.CrystalDamage(oodlerScript.oodlerAttackDamage, true);
                            }
                        }
                    }
                }
                break;
            default:
                {
                    break;
                }
        }

    }
}
