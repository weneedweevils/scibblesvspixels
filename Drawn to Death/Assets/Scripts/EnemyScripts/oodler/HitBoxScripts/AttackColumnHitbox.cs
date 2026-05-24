using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColumnHitbox : MonoBehaviour    
{

    public PlayerMovement PlayerScript;
    public Oodler oodlerScript;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Column":
                {
                    oodlerScript.DamageStatic(10f);
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

