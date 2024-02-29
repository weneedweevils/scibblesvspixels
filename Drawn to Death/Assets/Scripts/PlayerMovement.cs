/*  References:
*      - Youtube: Creating 2D Player Movement for Isometric Games with Unity 2018.3! (Tutorial)
*          - By: Unity
*          - Link: https://www.youtube.com/watch?v=tywt9tOubEY
*          
*/

using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayerMovement : MonoBehaviour
{
    //Input options
    [Header("Movement Controls")]
    public KeyCode up = KeyCode.W;
    public KeyCode down = KeyCode.S;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode dash = KeyCode.Space;

    //Movement Checks
    [Header("Physics")]
    public float accelerationCoefficient;   //how quickly it speeds up
    public float maxVelocity;               //how fast it can go horizontally
    public float friction;                  //how quickly it slows down
    public float speedModifier;             //modifiers applied to the player (affects maxVelocity)

    //Dash
    [Header("Dash Options")]
    public bool dashEnabled;
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
    public bool timelinePlaying = false;

    // Health
    public float health;
    public float maxHealth;
    public HealthBarBehaviour healthBar;

    //Invincibility Frames
    public CooldownTimer invincibilityTimer;
    private float invincibilityDuration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        health = maxHealth;
        invincibilityTimer = new CooldownTimer(0f, invincibilityDuration);
    }

    // Update is called once per frame
    void Update()
    {
        invincibilityTimer.Update();
        if (!inFreezeDialogue() && !timelinePlaying) // Disable movement if in dialogue/cutscene where we don't want movement
        {
            //Determine acceleration
            acceleration.x = ((Input.GetKey(left) ? -1 : 0) + (Input.GetKey(right) ? 1 : 0)) * accelerationCoefficient;
            acceleration.y = ((Input.GetKey(down) ? -1 : 0) + (Input.GetKey(up) ? 1 : 0)) * accelerationCoefficient;
        }
        else
        {
            acceleration.x = 0;
            acceleration.y = 0;
        }

        //Calculate velocity
        velocity.x = VelocityCalc(acceleration.x, velocity.x, speedModifier);
        velocity.y = VelocityCalc(acceleration.y, velocity.y, speedModifier);
        
        //Dash ability
        if (dashEnabled)
        {
            //Check if dash was activated
            if (!dashed && Input.GetKey(dash))
            {
                dashed = true;
                velocity += velocity.normalized * dashBoost;
            }
            //Dash cooldown
            else if (dashed)
            {
                dashtimer += Time.deltaTime;
                if (dashtimer >= dashCooldown)
                {
                    dashed = false;
                    dashtimer = 0f;
                }
            }
        }
        
        //Predict new position
        Vector2 currentPos = rbody.position;
        Vector2 newPos = currentPos + velocity * Time.fixedDeltaTime;

        //Move to new position
        rbody.MovePosition(newPos);

        //Animate
        ManageAnimations();

        healthBar.SetHealth(health, maxHealth);
    }

    private float VelocityCalc(float a, float v, float modifier = 1f)
    {
        //  a = Acceleration
        //  v = Velocity

        //Accelerate
        if (Mathf.Abs(a) > 0f && Mathf.Abs(v) <= maxVelocity * modifier)
        {
            v += a * modifier * Time.deltaTime;
            v = Mathf.Clamp(v, -maxVelocity * modifier, maxVelocity * modifier);
        }
        //Account for friction
        else if (Mathf.Abs(v) > 0f)
        {
            //Reduce our absolute velocity
            v = Mathf.Sign(v) * Mathf.Max(Mathf.Abs(v) - friction * modifier * Time.deltaTime, 0f);
        }

        //Return velocity bound by maxVelocity
        return v;
    }

    private void ManageAnimations()
    {
        //Set the speed parameter in the animator
        animator.SetFloat("speed", velocity.magnitude);

        if (!inFreezeDialogue() && !timelinePlaying)
        {
            //Flip the sprite according to mouse position relative to the players position
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            sprite.flipX = mousePosition.x < transform.position.x;
        }

        //Account for backwards movement
        animator.SetBool("backwards", velocity.x != 0f && (velocity.x < 0f != sprite.flipX));
    }

    public void StopMovement()
    {
        velocity = Vector2.zero;
        acceleration = Vector2.zero;
    }

    // Ensures movement is disabled if dialogue wants it to be
    public bool inFreezeDialogue()
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

    // Function to run when player takes damage
    public void Damage(float damageTaken)
    {
        health -= damageTaken;
        invincibilityTimer.StartTimer();
        healthBar.SetHealth(health, maxHealth);
    }

    // Function to run when player heals
    public void Heal(float healthRestored)
    {
        health += healthRestored;
        invincibilityTimer.StartTimer();
        healthBar.SetHealth(health, maxHealth);
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

    public void SetTimelineActive(bool isActive)
    {
        timelinePlaying = isActive;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && invincibilityTimer.IsUseable())
        {
            EnemyAI enemyai = collision.gameObject.GetComponent<EnemyAI>();
            if (enemyai.team == Team.oddle)
            {
                Debug.Log("oooof I have collided with an enemy");
                Damage(collision.gameObject.GetComponent<EnemyAI>().damage);
                if (health <= 0)
                {
                    Debug.Log("oooof I am ded RIP :(");
                    MenuManager.GotoScene(Scene.Ded);
                }
            }
        }
    }
}