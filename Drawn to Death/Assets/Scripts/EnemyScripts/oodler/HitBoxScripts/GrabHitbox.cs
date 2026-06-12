using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHitbox:MonoBehaviour

    
{

    public PlayerMovement playerScript;
    public Oodler oodlerScript;
    public static event Action grabbedGlich;

    private void OnTriggerEnter2D(Collider2D collision)
    {
      

        switch (collision.gameObject.tag)
        {

            case "Player":
                {
                    if (!playerScript.dashTimer.IsActive() && !playerScript.invincibilityTimer.IsActive())
                    {
                        grabbedGlich?.Invoke();
                    }

                    
                }
                break;

            default:
                {
                    break;
                }
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {


        switch (collision.gameObject.tag)
        {

            case "Player":
                {
                    if (!playerScript.dashTimer.IsActive() && !playerScript.invincibilityTimer.IsActive())
                    {
                        grabbedGlich?.Invoke();
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
