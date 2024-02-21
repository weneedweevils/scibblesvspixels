/*  References:
 *      - Youtube: Creating 2D Player Movement for Isometric Games with Unity 2018.3! (Tutorial)
 *          - By: Unity
 *          - Link: https://www.youtube.com/watch?v=tywt9tOubEY
 *          
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Input options
    public KeyCode up = KeyCode.W;
    public KeyCode down = KeyCode.S;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode dash = KeyCode.LeftShift;

    //Movement Checks
    public float accelerationCoefficient;   //how quickly it speeds up
    public float maxVelocity;               //how fast it can go horizontally
    public float friction;                  //how quickly it slows down
    public float speedModifier;             //modifiers applied to the player (affects maxVelocity)

    //Dash
    public float dashBoost;
    public float dashCooldown;
    private float dashtimer = 0;
    private bool dashed = false;
    
    //Animations
    private Animator animator;
    private SpriteRenderer sprite;
    
    //Physics info
    private Vector2 velocity, acceleration;
    
    Rigidbody2D rbody;

    // Used to determine if dialogue is happening
    private GameObject dialogue;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!inFreezeDialogue()) // Disable movement if in dialogue/cutscene where we don't want movement
        {
            //Determine acceleration
            acceleration.x = ((Input.GetKey(left) ? -1 : 0) + (Input.GetKey(right) ? 1 : 0)) * accelerationCoefficient;
            acceleration.y = ((Input.GetKey(down) ? -1 : 0) + (Input.GetKey(up) ? 1 : 0)) * accelerationCoefficient;

            //Calculate velocity
            velocity.x = VelocityCalc(acceleration.x, velocity.x);
            velocity.y = VelocityCalc(acceleration.y, velocity.y);

            if (!dashed && Input.GetKey(dash))
            {
                dashed = true;
                velocity += velocity.normalized * dashBoost;
            }
            else if (dashed)
            {
                dashtimer += Time.deltaTime;
                if (dashtimer >= dashCooldown)
                {
                    dashed = false;
                    dashtimer = 0f;
                }
            }
            //Predict new position
            Vector2 currentPos = rbody.position;
            Vector2 newPos = currentPos + velocity * Time.fixedDeltaTime;

            //Move to new position
            rbody.MovePosition(newPos);
        }

        //Animate
        ManageAnimations();
    }

    private float VelocityCalc(float a, float v)
    {
        //  a = Acceleration
        //  v = Velocity

        //Accelerate
        if (Mathf.Abs(a) > 0f && Mathf.Abs(v) <= maxVelocity * speedModifier)
        {
            v += a * Time.deltaTime;
            v = Mathf.Clamp(v, -maxVelocity * speedModifier, maxVelocity * speedModifier);
        }
        //Account for friction
        else if (Mathf.Abs(v) > 0f)
        {
            //Reduce our absolute velocity
            v = Mathf.Sign(v) * Mathf.Max(Mathf.Abs(v) - friction * Time.deltaTime, 0f);
        }

        //Return velocity bound by maxVelocity
        return v;
    }

    private void ManageAnimations()
    {
        //Set the speed parameter in the animator
        animator.SetFloat("speed", velocity.magnitude);

        //Flip the sprite according to mouse position relative to the players position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        sprite.flipX = mousePosition.x < transform.position.x;

        //Account for backwards movement
        animator.SetBool("backwards", velocity.x != 0f && (velocity.x < 0f != sprite.flipX));
    }

    // Ensures movement is disabled if dialogue wants it to be
    private bool inFreezeDialogue()
    {
        if (dialogue != null)
        {
            if (!dialogue.GetComponent<DialogueController>().DialogueActive()) // Ensures dialogue object is destroyed if movement freeze is on
            {
                dialogue.SetActive(false); // Deactivates dialogue after end, can be changed if we ever want repeatable dialogue
                dialogue = null;
                return false;
            }
            return dialogue.GetComponent<DialogueController>().DialogueActive() && dialogue.GetComponent<DialogueController>().stopMovement;
        }
        else
        {
            return false;
        }
    }

    // Dialogue trigger
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Dialogue")
        {
            dialogue = collision.gameObject;
            dialogue.GetComponent<DialogueController>().ActivateDialogue();
        }
    }

    // Dialogue exit
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (dialogue != null) {
            dialogue.SetActive(false); // Deactivates dialogue after trigger, can be changed if we ever want repeatable dialogue
            dialogue = null;
        }
    }
}
