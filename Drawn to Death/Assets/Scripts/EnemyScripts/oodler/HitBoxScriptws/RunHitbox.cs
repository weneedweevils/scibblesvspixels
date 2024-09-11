using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunHitbox : MonoBehaviour{


    public GameObject Glich;
    public Oodler oodlerScript;

    private Rigidbody2D glichRb;

    private PlayerMovement PlayerScript;

    public void Start(){
        PlayerScript = Glich.GetComponent<PlayerMovement>();
        glichRb = Glich.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided with " + collision.name);

        switch (collision.gameObject.tag)
        {

            case "Player":
                {

                    //if (oodlerSlamCooldown == false && !PlayerScript.dashTimer.IsActive())

                    if (!PlayerScript.dashTimer.IsActive() &&  !PlayerScript.invincibilityTimer.IsActive() && oodlerScript.activateDamage())
                    {
                        PlayerScript.Damage(oodlerScript.oodlerAttackDamage);
                        glichRb.AddForce(new Vector2(100f,100f));
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
