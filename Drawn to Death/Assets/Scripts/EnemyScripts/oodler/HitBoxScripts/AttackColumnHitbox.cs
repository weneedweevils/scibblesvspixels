using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColumnHitbox : MonoBehaviour    
{

    public PlayerMovement PlayerScript;
    public Oodler oodlerScript;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided with " + collision.name);
        switch (collision.gameObject.tag)
        {
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

