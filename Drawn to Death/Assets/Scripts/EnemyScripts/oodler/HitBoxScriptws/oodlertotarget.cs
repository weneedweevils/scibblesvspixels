using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oodlertotarget:MonoBehaviour

    
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

                    if (oodlerScript.grabbing == false)
                    {

                        if (!PlayerScript.dashTimer.IsActive() && oodlerScript.oodlerSlamCooldown == false && !PlayerScript.invincibilityTimer.IsActive() && oodlerScript.activateDamage())
                        {
                            PlayerScript.Damage(oodlerScript.oodlerAttackDamage);
                        }
                    }

                    else
                    {
                        
                        oodlerScript.caught = true;
                        Debug.Log(oodlerScript.caught);
                    }



                }
                break;

            case "Enemy":
                {
                    EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();


                    if (enemy != null && !enemy.invincibilityTimer.IsActive() && oodlerScript.oodlerSlamCooldown == false) //&& oodlerScript.activateDamage())
                    {
                        enemy.Damage(oodlerScript.oodlerAttackDamage);
                    }

                    else
                    {
                        HealthCrystal crystal = collision.gameObject.GetComponent<HealthCrystal>();
                        if (crystal != null)
                        {
                            if (crystal != null && crystal.invincibilityTimer.IsUseable() && oodlerScript.oodlerSlamCooldown == false)// && oodlerScript.activateDamage())
                            {
                                //Damage enemy
                                crystal.CrystalDamage(oodlerScript.oodlerAttackDamage, true);
                            }
                        }
                    }
                }
                break;

            case "Column":
                {
                    Destroy(collision.gameObject);

                }
                break;

            default:
                {
                    break;
                }
        }

    }
}
