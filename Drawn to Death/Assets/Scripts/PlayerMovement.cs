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
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour, IDataPersistence
{
    //Input options
    [Header("Movement Controls")]
    public KeyCode up = KeyCode.W;
    public KeyCode down = KeyCode.S;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode dash = KeyCode.Space;
    public KeyCode recall = KeyCode.R;
    
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
    public Slider dashBar;
    private CooldownBarBehaviour dashCooldownBar;

    //Animations
    private Animator animator;
    private SpriteRenderer sprite;

    //Recall
    private float recallDuration = 115f/60f;
    [SerializeField] private SpriteRenderer pencil;
    private GameObject[] enemies;
    private CooldownTimer recallTimer;

    //camera 
    private Camera cam;
    private float targetZoom;
    private float zoomFactor = 0.5f;
    private static float noZoom;
    private bool animationDone = true;


    //Physics info
    private Vector2 velocity, acceleration;

    Rigidbody2D rbody;

    // Used to determine if dialogue is happening
    [Header("Cutscene")]
    private GameObject dialogue;
    public bool timelinePlaying = false;

    // Health
    [Header("Player Stats")]
    public float health;
    public float maxHealth;
    public float invincibilityDuration;
    public HealthBarBehaviour healthBar;

    //Invincibility Frames
    public CooldownTimer invincibilityTimer;

    // Start is called before the first frame update
    void Start()
    {
        
        cam = Camera.main;
        noZoom = cam.orthographicSize;
        targetZoom = cam.orthographicSize;
       

        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        health = maxHealth;
        invincibilityTimer = new CooldownTimer(0f, invincibilityDuration);
        recallTimer = new CooldownTimer(0f, recallDuration);
        dashCooldownBar = new CooldownBarBehaviour(dashBar, dashCooldown, Color.red, Color.green);
    }

    // Update is called once per frame
    void Update()
    {


        recallTimer.Update();
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

        // disable movement if player is recalling
        if (!recallTimer.IsActive()) 
        {
            //Determine acceleration
            acceleration.x = ((Input.GetKey(left) ? -1 : 0) + (Input.GetKey(right) ? 1 : 0)) * accelerationCoefficient;
            acceleration.y = ((Input.GetKey(down) ? -1 : 0) + (Input.GetKey(up) ? 1 : 0)) * accelerationCoefficient;

        }
        else
        {
            acceleration.x = 0;
            acceleration.y = 0;
            targetZoom -= zoomFactor * 0.1f;
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime*0.4f);
           
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
               Debug.Log("Dashing");
                dashed = true;
                velocity += velocity.normalized * dashBoost;
            }
            //Dash cooldown
            else if (dashed)
            {
               dashtimer += Time.deltaTime;
                dashCooldownBar.SetBar(dashtimer);
                if (dashtimer >= dashCooldown)
                {
                    dashed = false;
                    dashtimer = 0f;
                }
            }
        }

        // Recall Ability
        if(recallTimer.IsUseable() && Input.GetKey(recall)){
            recallTimer.StartTimer();
            pencil.enabled = false;
            StopMovement();
            animationDone = false;
            animator.SetBool("New Bool", true);
        }

        if (cam.orthographicSize <= noZoom && animationDone == true)
        {

            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, noZoom, Time.deltaTime * 5f);
            targetZoom = noZoom;
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
        if (health < maxHealth)
        {
            health += healthRestored;
        }
        else
        {
            health = maxHealth;
        }
        healthBar.SetHealth(health, maxHealth);
    }

 
    // Teleport function which is called as an animation event in g'liches recall animation
    public void teleport()
    {
        animator.SetBool("New Bool", false);
        animationDone = true;
        pencil.enabled = true;
        enemies = null;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log(enemies.Length);
        foreach( GameObject enemy in enemies) {
            EnemyAI enemyai = enemy.GetComponent<EnemyAI>();
            if (enemyai.team == Team.player)
            {
                enemy.transform.position = transform.position;
            }
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

    //Save Game Stuff
    public void LoadData(GameData data)
    {
        if (data.skipCutscene)
        {
            transform.position = data.playerPosition;
        }
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = transform.position;
    }


}
