using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHitbox : MonoBehaviour
{
    public GameObject Glich;
    public Oodler oodlerScript;

    private Rigidbody2D glichRb;

    private PlayerMovement PlayerScript;

    public void Start()
    {
        PlayerScript = Glich.GetComponent<PlayerMovement>();
        glichRb = Glich.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided with something in sprite hitbox" + collision.name);

        switch (collision.gameObject.tag)
        {

            case "BottomBorder":
                {
                    oodlerScript.ChangeSpriteSortingOrder(8);
                    oodlerScript.EnableSpriteHitbox(false);
                    Debug.Log("Hit the border !!!");
           
                }
                break;

            case "Player":
                {
                        Debug.Log("hit glich inside sprite hitbox");
                }
                break;



            default:
                {
                    break;
                }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Collided with something in sprite hitbox" + collision.name);

        switch (collision.gameObject.tag)
        {

            case "BottomBorder":
                {
                    oodlerScript.ChangeSpriteSortingOrder(8);
                    oodlerScript.EnableSpriteHitbox(false);
                    Debug.Log("Hit the border !!!");

                }
                break;

            case "Player":
                {
                    Debug.Log("hit glich inside sprite hitbox");
                }
                break;



            default:
                {
                    break;
                }
        }

    }
}
