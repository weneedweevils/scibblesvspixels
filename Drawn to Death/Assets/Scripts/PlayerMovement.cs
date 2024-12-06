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
using UnityEngine.UIElements;
using static System.Net.Mime.MediaTypeNames;
using System.Threading;
using UnityEngine.InputSystem;
using System.Xml.Serialization;

public class PlayerMovement : Singleton<PlayerMovement>, IDataPersistence
{
    //Movement Checks
    [Header("Physics")]
    public float accelerationCoefficient;   //how quickly it speeds up
    public float maxVelocity;               //how fast it can go horizontally
    public float friction;                  //how quickly it slows down
    public float speedModifier;             //modifiers applied to the player (affects maxVelocity)

    //Dash
    [Header("Dash")]
    public bool dashEnabled;
    public float dashBoost;
    public float dashCooldown;
    public CooldownTimer dashTimer;
    public UnityEngine.UI.Slider dashBar;
    private CooldownBarBehaviour dashCooldownBar;
    private UnityEngine.UI.Image dashNotifier;
    private bool activatedDashNotifier = false;

    //Animations
    [HideInInspector] public Animator animator;
    private SpriteRenderer sprite;
    public Attack weapon { get; private set; }

    //Recall
    [Header("Recall")]
    private float recallDuration = 115f/60f; 
    public float recallCooldown;
    [Range(0, 1)] public float allyHealPercentage;
    public float allyStrModifier;
    public float allySpdModifier;
    public float allyAtkSpdModifier;
    public float allyBuffDuration;
    public float crabSpdModifier;
    public float crabAtkSpdModifier;
    public float hopperStrModifier;

    [SerializeField] private SpriteRenderer pencil;
    private GameObject[] enemies;
    public CooldownTimer recallTimer;
    public UnityEngine.UI.Slider recallBar;
    private CooldownBarBehaviour recallCooldownBar;
    
    
    private UnityEngine.UI.Image recallNotifier;
    private bool activatedRecallNotifier = false;

    //camera 
    private Camera cam;
    private float targetZoom;
    //private float zoomFactor = 0.5f;
    private static float noZoom;
    [HideInInspector] public bool animationDone = true;

    //Physics info
    private Vector2 velocity, acceleration, accelerationCorrected;
   

    Rigidbody2D rbody;
    BoxCollider2D boxCollider;
    private bool boxColliding = false;

    // Used to determine if timeline is active
    [Header("Cutscene")]
    public bool timelinePlaying = false;

    // Health
    [Header("Player Stats")]
    public float health;
    public float maxHealth;
    public float invincibilityDuration;
    public HealthBarBehaviour healthBar;
    [Range(0, 1)]
    public float abilityDamageReduction = 0.1f;

    [Header("Other")]
    [SerializeField] private GameObject hud;
    public GameObject pauseUi;
    private GameObject panel; // This is the panel that contains in image whose color can be changed to simulate a damage effect
    private UnityEngine.UI.Image restricted;
    private GameObject lifestealOrb;
    private bool orb = false;
    private CooldownTimer lifestealEndTimer;
    public GameObject volumeControllerObject;
    private VolumeController volumeController;

    public Animator CameraReference;
    public Animator HealthBarReference;

    //Invincibility Frames
    public CooldownTimer invincibilityTimer;
    private UnityEngine.UI.Image damageScreen;
    private bool hit = false;
    private SpriteRenderer eraser;

    // Oodler Invincibility 
    public bool oodlerCooldown = false;

    // Pause all input besides escape
    public bool pauseInput = false;

    //additional scripts
    [Header("New input system")]
    public GameObject inputHandler;
    private PlayerArms playerarms;
    public GameObject eraserObject;
    public GameObject armsObject;
    public bool isGamepad = false;
    private Vector2 aimDirection;

    [HideInInspector]
    public PlayerInput playerInput;
    
    // Start is called before the first frame update
    void Start()
    {
        playerInput = CustomInput.instance.playerInput;

        cam = Camera.main;
        noZoom = cam.orthographicSize;
        targetZoom = cam.orthographicSize;

        boxCollider = GetComponent<BoxCollider2D>();
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        weapon = GetComponentInChildren<Attack>();
        eraser = transform.GetChild(0).GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        lifestealOrb = transform.GetChild(4).gameObject;
        volumeController = volumeControllerObject.GetComponent<VolumeController>();

        health = maxHealth;
        dashTimer = new CooldownTimer(dashCooldown, dashBoost / friction);
        invincibilityTimer = new CooldownTimer(0f, invincibilityDuration);
        recallTimer = new CooldownTimer(recallCooldown, recallDuration);
        lifestealEndTimer = new CooldownTimer(0.1f, 0.532f);

        dashCooldownBar = new CooldownBarBehaviour(dashBar, dashCooldown);
        recallCooldownBar = new CooldownBarBehaviour(recallBar, recallCooldown);

        dashTimer.Connect(dashCooldownBar);
        recallTimer.Connect(recallCooldownBar);

        dashNotifier = dashBar.transform.parent.GetChild(1).GetComponent<UnityEngine.UI.Image>();
        recallNotifier = recallBar.transform.parent.GetChild(1).GetComponent<UnityEngine.UI.Image>();

        panel = GameObject.FindGameObjectWithTag("DamageFlash");
        damageScreen = panel.GetComponent<UnityEngine.UI.Image>();

        restricted = GameObject.Find("RestrictRally").GetComponent<UnityEngine.UI.Image>();

        //playerInput = GetComponent<PlayerInput>();

        playerarms = new PlayerArms(eraserObject, gameObject, armsObject, playerInput, this);
    }

    public void OnDeviceChanged(PlayerInput pi)
    {
        Debug.Log(pi.currentControlScheme.ToString());
        if(pi.currentControlScheme.Equals("Gamepad") || pi.currentControlScheme.Equals("Playstation") || pi.currentControlScheme.Equals("Xbox"))
        {
            isGamepad = true;
        }
        else
        {
            isGamepad = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        acceleration = playerInput.actions["Move"].ReadValue<Vector2>()*accelerationCoefficient;
        aimDirection = playerInput.actions["Aim"].ReadValue<Vector2>();

        playerarms.FrameUpdate(aimDirection);
        dashTimer.Update();
        recallTimer.Update();
        invincibilityTimer.Update();
        lifestealEndTimer.Update();

        // Handles lifesteal animation
        if (weapon.lifestealTimer.IsActive() && weapon.lifestealStartTimer.IsOnCooldown() && !orb)
        {
            lifestealOrb.SetActive(true);
            lifestealOrb.GetComponent<Animator>().SetTrigger("lifestealstart");
            orb = true;
        }
        else if (weapon.lifestealTimer.IsOnCooldown() && orb)
        {
            lifestealOrb.GetComponent<Animator>().SetTrigger("lifestealend");
            lifestealEndTimer.StartTimer();
            orb = false;
        }
        else if (lifestealEndTimer.IsOnCooldown())
        {
            lifestealOrb.SetActive(false);
        }

        if (invincibilityTimer.IsActive() && hit) // Illustrates Iframes
        {
            sprite.color = new Color(255, 255, 255, 0.70f);
            eraser.color = new Color(255, 255, 255, 0.70f);
        }
        else if (invincibilityTimer.IsUseable() && hit)
        {
            sprite.color = new Color(255, 255, 255, 1f);
            eraser.color = new Color(255, 255, 255, 1f);
            hit = false;
        }
        
        // Disable movement if in dialogue/cutscene where we don't want movement
        if (!inFreezeDialogue() && !timelinePlaying) //&& pauseUi.active == false)
        {
            hud.SetActive(true);
            //Determine acceleration

             // Decrease SFX volume
            volumeController.inCutscene = false;
        }
        else 
        {
            hud.SetActive(false);
            weapon.animator.SetBool("attacking", false);
            acceleration.x = 0;
            acceleration.y = 0;

            // Return SFX volume to original setting
            volumeController.inCutscene = true;
        }

        // disable movement if player is recalling
        if (weapon.reviveTimer.IsActive() || recallTimer.IsActive())
        {
            acceleration.x = 0;
            acceleration.y = 0;
            //ZoomCamera(zoomFactor);
        }

        //Calculate velocity
        velocity.x = VelocityCalc(acceleration.x, velocity.x, speedModifier);
        velocity.y = VelocityCalc(acceleration.y, velocity.y, speedModifier);

        
        
        //Dash ability

        // if dash is useable flash the dash notifier
        if(dashTimer.IsUseable() && !activatedDashNotifier){
            var temp1 = dashNotifier.color;
            temp1.a = 1f;
            dashNotifier.color = temp1;
            activatedDashNotifier = true;
        }

        // if dash notifier is visible, decrease the alpha value
        if (dashNotifier.color.a > 0 )
        {
            var temp = dashNotifier.color;
            temp.a -= 0.01f;
            dashNotifier.color = temp;

        }


        if (dashEnabled && dashTimer.IsUseable() && CanUseAbility() && playerInput.actions["Dash"].triggered && Mathf.Abs(velocity.magnitude) > 0f && !pauseInput)
        {
            activatedDashNotifier = false;
            velocity += acceleration.normalized * dashBoost;
            animator.SetBool("dashing", true);
            pencil.enabled = false;
            sprite.color = new Color(255, 255, 255, 0.50f);
            dashTimer.StartTimer();
            FMODUnity.RuntimeManager.PlayOneShot("event:/DashAbility");
        }
        else if (dashTimer.IsOnCooldown())
        {
            if (animator.GetBool("dashing"))
            {
                pencil.enabled = true;
                animator.SetBool("dashing", false);
                sprite.color = new Color(255, 255, 255, 1f);
            }
        }

        // Recall Ability

        // Update X over Recall UI
        if (restricted != null)
        {
            // if we have no allies show x over the icon otherwise flash the recall notifier if off cooldown
            if (weapon.GetAllies().Count == 0)
            {
                var temp = restricted.color;
                temp.a = 1f;
                restricted.color = temp;
            }
            else
            {
                var temp = restricted.color;
                temp.a = 0f;
                restricted.color = temp;
                
                if(recallTimer.IsUseable() && !activatedRecallNotifier){
                    var temp1 = recallNotifier.color;
                    temp1.a = 1f;
                    recallNotifier.color = temp1;
                    activatedRecallNotifier = true;
                }
            }
        }

        //reset notifier if we have allies or have pressed revive or recall
        if ((playerInput.actions["Rally"].triggered || playerInput.actions["Revive"].triggered) && weapon.GetAllies().Count > 0)
        {
            activatedRecallNotifier = false;
        }

        // if recall notifier is visible, decrease the alpha value
        if (recallNotifier.color.a > 0)
        {
            var temp = recallNotifier.color;
            temp.a -= 0.01f;
            recallNotifier.color = temp;
        }

        //If player pressed recall and they are not on cooldown and they have allies, do recall
        if (recallTimer.IsUseable() && CanUseAbility() && playerInput.actions["Rally"].triggered && weapon.GetAllies().Count > 0)
        {
            recallTimer.StartTimer();
            pencil.enabled = false;
            StopMovement();
            FMODUnity.RuntimeManager.PlayOneShot("event:/RallyAbility");
            animationDone = false;
            animator.SetBool("New Bool", true);
        }

        if (cam.orthographicSize != noZoom && animationDone == true)
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

        //change screen flash back to normal 
        ChangeScreenColor(false);
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

        if (!inFreezeDialogue() && !timelinePlaying && !weapon.reviveTimer.IsActive() && !recallTimer.IsActive())
        {
            if (dashTimer.IsActive() && velocity.x != 0f)
            {
                //Flip the sprite according to movement
                sprite.flipX = velocity.x < 0f;
            }
            else
            {
                //Flip the sprite according to mouse position relative to the players position

                if(isGamepad)
                {
                    sprite.flipX = armsObject.transform.localScale.x < 0;
                    
                }
                else
                {
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                    sprite.flipX = mousePosition.x < transform.position.x;
                }
            }
        }

        //Account for backwards movement
        animator.SetBool("backwards", velocity.x != 0f && (velocity.x < 0f != sprite.flipX));

        // Check low health which triggers health bar to rotate back and forth
        CheckLowHealth();

    }

    public void StopMovement()
    {
        velocity = Vector2.zero;
        acceleration = Vector2.zero;
    }

    // Ensures movement is disabled if dialogue wants it to be
    public bool inFreezeDialogue()
    {
        if (DialogueManager.instance == null)
        {
            return false; 
        }
        else
        {
            return DialogueManager.instance.dialogueActive || Pause.PauseManager.paused;
        }
    }

    // Function to run when player takes damage
    public void Damage(float damageTaken, Vector2 knockbackDir = default(Vector2), float knockbackPower = 0f)
    {
        if (dashTimer.IsActive() || inFreezeDialogue() || timelinePlaying)
        {
            return;
        }

        HealthBarReference.SetTrigger("HealthBarShake");
        CameraReference.SetTrigger("Shake");

        Debug.Log(health);

        Debug.Log(maxHealth);

       

        if (UsingAbility())
        {
            health -= (damageTaken * (1 - abilityDamageReduction));
        }
        else
        {
            health -= damageTaken;
        }
        invincibilityTimer.StartTimer();
        healthBar.SetHealth(health, maxHealth);
        hit = true;

        //Apply Knockback
        if (knockbackPower > 0f)
        {
            velocity = knockbackDir.normalized * knockbackPower * 3;
        }

        // function will make the health bar move around when low on health
        

        // flashes damage indicator around health bar
        ChangeScreenColor(true);

        if (health <= 0)
        {
            StartCoroutine(MenuManager.LoadScene(Scene.Ded));
        }
    }

    // function to handle changing the color of the screen when damaged
    public void ChangeScreenColor(bool damaged)
    {
        if (damageScreen != null)
        {
            if (damageScreen.color.a > 0 && !damaged)
            {
                var temp = damageScreen.color;
                temp.a -= 0.005f;
                damageScreen.color = temp;

            }
            // change screen to red by adjusting alpha value
            if (damaged)
            {
                var temp = damageScreen.color;
                temp.a = 0.5f;
                damageScreen.color = temp;
            }
        }
    }

    public void CheckLowHealth()
    {
        if (health < maxHealth/3f)
        {
            HealthBarReference.SetBool("LowHealth", true);
        }
        else
        {
            HealthBarReference.SetBool("LowHealth", false);
        }
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
 
    //Some abilities can not be used simultaneously - Check to see if any of those are not active
    public bool CanUseAbility()
    {
        //Debug.Log(dashTimer.IsActive());
        return !(weapon.reviveTimer.IsActive() || dashTimer.IsActive() || recallTimer.IsActive()) &&
               !(inFreezeDialogue() || timelinePlaying);
    }

    public bool UsingAbility()
    {
        return weapon.reviveTimer.IsActive() || recallTimer.IsActive() || weapon.lifestealTimer.IsActive() || dashTimer.IsActive() || weapon.lifestealStartTimer.IsActive();
    }

    //Animate the camera zoom
    public void ZoomCamera(float zoom)
    {
        targetZoom -= zoom * 0.1f;
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * 0.4f);
    }

    // Teleport function which is called as an animation event in g'liches recall animation
    public void teleport()
    {
        RaycastHit hit;
        animator.SetBool("New Bool", false);
        animationDone = true;
        pencil.enabled = true;
        enemies = null;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        var Safespot = findOpenSpace();
        Vector3 teleportRadius = new Vector3(3f, 3f, 0f);
        Vector3 total = Vector3.Scale(Safespot.Item1, teleportRadius);

        foreach ( GameObject enemy in enemies) {
            EnemyAI enemyai = enemy.GetComponent<EnemyAI>();
            if (enemyai != null)
            {
                if (enemyai.team == Team.player)
                {
                    enemy.transform.position = transform.position + total;
                    enemyai.Heal(enemyai.maxHealth * allyHealPercentage);
                    enemyai.buffed = true;
                    if (enemyai.type == Type.hopper)
                    {
                        enemyai.damage *= hopperStrModifier;
                    }
                    else
                    {
                        enemyai.damage *= allyStrModifier;
                    }
                    if (enemyai.type == Type.crab)
                    {
                        enemyai.speed *= crabSpdModifier;
                        enemyai.attackTimer.SetCooldown(enemyai.attackCooldown * crabAtkSpdModifier);
                    }
                    else
                    {
                        enemyai.speed *= allySpdModifier;
                        enemyai.attackTimer.SetCooldown(enemyai.attackCooldown * allyAtkSpdModifier);
                    }
                }
            }
        }
    }

    // This function will find a direction to teleport allies into that is not next to a wall
    // returns a vector3 with the viable direction and a float with the distance of a ray in that direction
    public (Vector3, float) findOpenSpace()
    {


        // offset used to make sure that the players position is not overlapping with wall
        var directions = new List<Vector3> { Vector3.down, Vector3.up, Vector3.left, Vector3.right, Vector3.left + Vector3.down, Vector3.right + Vector3.up, Vector3.left + Vector3.up, Vector3.right + Vector3.down };
        Vector3 offSet = new Vector3(0f, -4f, 0f);
        Vector3 PlayerPosition = transform.position + offSet;
        int layerMask = 1 << 8;
        float best_distance = 0;
        Vector3 bestDirection = Vector3.zero;
        Vector2 point = new Vector2(0,0);


        // This for loop will go through all directions and teleport the enemies in the direction where there is the most available space
        foreach (Vector3 direction in directions)
        {
            RaycastHit2D hit = Physics2D.Raycast(PlayerPosition, direction, Mathf.Infinity, layerMask);
            //Debug.DrawRay(PlayerPosition, (Vector3.up) * 100f, Color.red, 5f);
            
            if (hit)
            {
                float distance = hit.distance;
               
                if (distance > best_distance)
                {
                    point = hit.point;
                    best_distance = distance;
                    bestDirection = direction;
                    
                }
            }
        }
        var bestDirDis = (bestDirection, best_distance);
        return bestDirDis;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {   
        return;
    }

    public void SetTimelineActive(bool isActive)
    {
        timelinePlaying = isActive;
    }

    //Save Game Stuff
    public void LoadData(GameData data)
    {
        /*
        if (data.skipCutscene)
        {
            transform.position = data.playerPosition;
        }*/
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = transform.position;
    }

    public Vector2 GetVelocity()
    {
        return velocity;
    }

    // function that pauses player input
    public void PausePlayerInput(bool pause)
    {
        if (pause)
        {
            pauseInput = true;
        }
        else
        {
            pauseInput = false;
        }
    }

    public PlayerInput getInputSystem()
    {
        return playerInput;
    }

     public void ChangeSpriteSortingOrder(int sortingLayer){
        sprite.sortingOrder = sortingLayer;
    }

    public SpriteRenderer GetPencil()
    {
        return pencil;
    }
}
