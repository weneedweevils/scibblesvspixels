﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHitbox:MonoBehaviour

    
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
                    oodlerScript.SetBossCaught(true);
                    Debug.Log(oodlerScript.IsCaught());
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