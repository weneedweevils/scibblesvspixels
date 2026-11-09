using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


/// <summary>
/// This is the Main script for the boss battle it handles all physics and game logic
/// </summary>
public class Oodler : MonoBehaviour
{
    // Components //
    public Rigidbody2D Rigidbody { get; set; }
    public Animator animator;
    private SpriteRenderer oodlerSprite;
    public Rigidbody2D oodlerRB;


    [Header("Shadow Reference")]
    public GameObject oodlerShadowObject;
    private Animator shadowAnimator;


    // Player //
    [Header("Player References")]
    public PlayerMovement playerScript;
    public GameObject glich;


    // Public Parameters //
    [Header ("Public float")]
    private float maxHealth = 500f;
    private float currentHealth = 500f;
    public float movementSpeed { get; set; } = 100f;
    public float oodlerAttackDamage = 100f;
    private float invincibilityDuration = 40f / 60f;

    // ENUMS //
    public enum AttackType {Grab,Slam,Run, Default}
    public AttackType attackType;
    public enum Phase {P1,P2,P3}
    public Phase phase = Phase.P1;
  
    // UI //
    [Header ("UI References")]
    public GameObject healthBar;
    public TextMeshProUGUI currentHealthUI;
    public TextMeshProUGUI maxHealthUI;
    public UnityEngine.UI.Image healthBarImage;



    // Collider References
    [Header("HitBox References")]
    public GameObject runHitboxCollider;

    public GameObject attackHitboxCollider;

    public GameObject selfHitboxCollider;

    public GameObject grabHitboxCollider;
    public GameObject attackColumnHitboxCollider;
    public GameObject spriteHitboxCollider;


    // States
    public StateMachine stateMachine { get; set; }
    public ChildStateMachine childStateMachine{ get; set; }


    // Main States
    public OodlerIdle oodlerIdle { get; set; }
    public OodlerSlam oodlerSlam { get; set; } 
    public OodlerQuickSlam oodlerQuickSlam { get; set; } 
    public OodlerGrab oodlerGrab { get; set; } 
    public OodlerDrop oodlerDrop { get; set; }
    public OodlerInitial oodlerInitial{ get; set; }
    public OodlerRun oodlerRun { get; set; } 
    public OodlerIntimidate oodlerIntimidate { get; set; }

    public GameObject prefab;

    // Music and Sound
    public BasicMusicScript musicScript;

    public FMODUnity.EventReference oodlerHurtSFX;
    public FMODUnity.EventReference oodlerSlamSFX;
    public FMODUnity.EventReference oodlerGrabSFX;
    public FMODUnity.EventReference oodlerRunSFX;
    public FMODUnity.EventReference oodlerFloatSFX;
    public FMODUnity.EventReference oodlerChargeupSFX;

    public FMOD.Studio.EventInstance runSFXInstance;
    public FMOD.Studio.EventInstance floatSFXInstance;


    //Vector Initialization's // 
    private Vector3 playerOffSet = Vector3.zero;
    private Vector3 glichLastPosition = Vector3.zero;
    private Vector3 oodlerGroundPosition = Vector3.zero;
    private Vector3 oodlerLandPosition = Vector3.zero;
    private Vector3 oodlerAirPosition = Vector3.zero;
    private Vector3 grabPositionOffset = new Vector3(0, 2f, 0f);
    private Vector3 offScreen = new Vector3(220, 130, 0);

    // Initialization for drop attack 
    public GameObject dropZoneObject;
    private Vector3 dropZone;
    private Vector3 dropZoneCorrected;
    public CooldownTimer invincibilityTimer;

    private float oodlerVelocity;

    //blockers
    public EnemyAI[] blockers;


    // Menu Control //
    public UnityEvent OnDeath = new UnityEvent();
    private float angle = 0f;

    // Bool checks //
    private bool hitHazard = false;
    public bool floating = true;

    private void Awake()
    {
        stateMachine = new StateMachine();

        oodlerIdle = new OodlerIdle(this, stateMachine);
        oodlerSlam = new OodlerSlam(this, stateMachine);
        oodlerGrab = new OodlerGrab(this, stateMachine);
        oodlerDrop = new OodlerDrop(this, stateMachine);
        oodlerInitial = new OodlerInitial(this, stateMachine);
        oodlerRun = new OodlerRun(this, stateMachine);
        oodlerQuickSlam = new OodlerQuickSlam(this, stateMachine);
        oodlerIntimidate = new OodlerIntimidate(this, stateMachine);
    }

    

    private void Start()
    {
        InstantiateVariables();

        if (attackType == AttackType.Grab)
        {
            stateMachine.Initialize(oodlerGrab);
            Debug.Log("going into grab state");
        }
        else if (attackType == AttackType.Slam)
        {
            stateMachine.Initialize(oodlerSlam);
        }
        else if (attackType == AttackType.Run)
        {
            stateMachine.Initialize(oodlerRun);
        }
        else if (attackType == AttackType.Default)
        {
            stateMachine.Initialize(oodlerInitial);
        }

        if (musicScript.currentTrack != 1)
        {
            musicScript.currentTrack = 1;
            musicScript.switchTrack();
        }
        musicScript.setIntensity(0f);
        musicScript.autoUpdate = false;


        // run sfx
        runSFXInstance = FMODUnity.RuntimeManager.CreateInstance(oodlerRunSFX);
       
        // float sfx
        floatSFXInstance = FMODUnity.RuntimeManager.CreateInstance(oodlerFloatSFX);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(floatSFXInstance, GetComponent<Transform>(), GetComponent<Rigidbody2D>());
        floatSFXInstance.start();
    }

        
   
    // Instantiate private references //
    private void InstantiateVariables()
    {
        // Health
        maxHealthUI.text = maxHealth.ToString();
        currentHealthUI.text = currentHealth.ToString();


        // Sprite components for oolder and oodler shadow
        animator = GetComponentInChildren<Animator>();
        oodlerSprite = GetComponentInChildren<SpriteRenderer>();
        shadowAnimator = oodlerShadowObject.GetComponentInChildren<Animator>();
        BringSpriteToForeground();


        playerScript = glich.GetComponent<PlayerMovement>();

        invincibilityTimer = new CooldownTimer(invincibilityDuration * 0.5f, invincibilityDuration * 0.5f);
        //healthBarImage = healthBar.<UnityEngine.UI.Image>();
        oodlerRB = GetComponent<Rigidbody2D>();

        // values for drop zone in grab attack
        dropZoneCorrected = new Vector3(dropZoneObject.transform.position.x, dropZoneObject.transform.position.y + 10f, 0);
        dropZone = new Vector3(dropZoneObject.transform.position.x, dropZoneObject.transform.position.y, 0);
    }

   

    #region Update


    private void Update(){
        invincibilityTimer.Update();

        // var velocity = (current - previous) / Time.deltaTime;
        

        if (floating)
        {
            //Debug.Log("Current Velocity: " + oodlerVelocity);
            floatSFXInstance.setParameterByName("Velocity", 0.5f);

        }
        else
        {
            floatSFXInstance.setParameterByName("Velocity", 0f);
        }

    }
    // FixedUpdate to update physics
    private void FixedUpdate()
    {
        stateMachine.currentState.ParentFrameUpdate();
    }
    #endregion



    #region Health

    // Damage function for damage from player //
    public void Damage(float damageTaken)
    {
        FMODUnity.RuntimeManager.PlayOneShot(oodlerHurtSFX, transform.position);
        currentHealth = currentHealth - damageTaken;
        Debug.Log(currentHealth);
        invincibilityTimer.StartTimer();
        if(currentHealth < maxHealth / 2f)
        {
            phase = Phase.P2;
        }
        if (currentHealth <= 0f)
        {
            Die();
        }
        UpdateUIHealthBar();
    }

    // Damage function for damage from other objects //
    public void DamageStatic(float damageTaken)
    {

        setHazard(true);
        Debug.Log(currentHealth);
        if (currentHealth <= 0f)
        {
            Die();
        }
        UpdateUIHealthBar();
    }


    // Updates Health UI
    public void UpdateUIHealthBar()
    {
        currentHealthUI.text = currentHealth.ToString();
        healthBarImage.fillAmount = currentHealth / maxHealth;

    }

    // Heal Function //
    public void heal(float heal_amount)
    {
        if (currentHealth <= maxHealth)
        {
            if (currentHealth + heal_amount > maxHealth)
            {
                currentHealth = maxHealth;
            }
            else
            {
                currentHealth += heal_amount;
            }
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }


    // When Health Reaches Zero
    [ContextMenu("Die")]
    public void Die()
    {
        OnDeath.Invoke();
    }

    #endregion

    #region Animation

    ///<summary>
    /// flips sprite direction of shadow and oodler relative to glich
    ///</summary> 
    public void CheckSpriteDirection(){
        if(transform.position.x - glich.transform.position.x >= 0){
            oodlerSprite.flipX = true;
        }
        else{
             oodlerSprite.flipX = false;
        }
    }

    ///<summary>
    /// changes sprite ordering of oodler
    ///</summary> 
    public void ChangeSpriteSortingOrder(int sortingLayer)
    {
        oodlerSprite.sortingOrder = sortingLayer;
    }

    ///<summary>
    /// changes sprite ordering of oodler to be in the foreground
    ///</summary> 
    public void BringSpriteToForeground()
    {
        ChangeSpriteSortingOrder(8);
    }

    ///<summary>
    /// changes sprite ordering of oodler to be in the background
    ///</summary> 
    public void BringSpriteToBackground()
    {
        ChangeSpriteSortingOrder(5);
    }

    ///<summary>
    /// This function will display the oodlers shadow
    ///</summary> 
    public void ShowShadow()
    {
        shadowAnimator.SetTrigger("Idle");
    }

    ///<summary>
    /// This function hides the oodlers shadow
    ///</summary> 
    public void HideShadow()
    {
        shadowAnimator.SetTrigger("Hidden");
    }

    # endregion


    // --BOSS METHODS-- //


    // ENABLERS AND DISABLERS //


    // Enabling/Disabling Hitboxes


    #region Hitboxes

    public void EnableAttackHitbox(bool enable)
    {
       
         attackHitboxCollider.SetActive(enable);
        
      
    }

    public void EnableColumnHitbox(bool enable)
    {
    
        attackColumnHitboxCollider.SetActive(enable);
        
        
    }

    public void EnableAreaHitbox(bool enable)
    {
   
        selfHitboxCollider.SetActive(enable);
      
    }

    public void EnableRunHitbox(bool enable)
    {
        runHitboxCollider.SetActive(enable);
        
    }

    public void EnableGrabHitbox(bool enable)
    {
       
        grabHitboxCollider.SetActive(enable);
       
    }

    public void EnableSpriteHitbox(bool enable)
    {
        
        spriteHitboxCollider.SetActive(enable);

        
    }


    ///<summary>
    /// Enables/Disables gliches hitbox specifically for the grab attack
    ///</summary> 
    public void EnableGlichColliders(bool enable)
    {
        if (enable)
        {
            glich.GetComponent<CapsuleCollider2D>().enabled = true;
            glich.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            glich.GetComponent<CapsuleCollider2D>().enabled = false;
            glich.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    #endregion




    #region Moving Methods
    // MOVING METHODS //


    //This function will make the oodler go to the closest position along a radius around the player 
    public bool GoToCircle(float speed, float radius)
    {
        var step = speed * Time.deltaTime;
        var a = glich.transform.localPosition;
        var b = transform.position;
        a.y = a.y + 10f;

        Vector2 circlePos = new Vector2();
        circlePos.x = a.x + (radius * ((b.x - a.x) / Mathf.Sqrt(Mathf.Pow((b.x - a.x),2f) + Mathf.Pow((b.y - a.y),2f))));
        circlePos.y = a.y + (radius * ((b.y - a.y) / Mathf.Sqrt(Mathf.Pow((b.x - a.x), 2f) + Mathf.Pow((b.y - a.y), 2f))));


        oodlerVelocity = step;
        oodlerRB.MovePosition(Vector3.MoveTowards(transform.position, circlePos, step));
        MoveShadowSprite();

        if (Vector3.Distance(transform.position, circlePos) < 0.1f)
        {
            oodlerRB.MovePosition(playerOffSet);

            Vector2 startAngle = new Vector2();
            startAngle.x = b.x - a.x;
            startAngle.y = b.y - a.y;
            angle = Mathf.Atan2(startAngle.y, startAngle.x);
            return true;
        }
        else
        {

            return false;
        }
    }


    // this function will make the oodler circle the player at a constant speed
    public void Circleglich( float speed, float radius)
    {
        var step = speed * Time.deltaTime;
       
        playerOffSet = glich.transform.localPosition;
        playerOffSet.y = playerOffSet.y + 10f;

        float x = playerOffSet.x + (Mathf.Cos(angle)*radius);
        float y = playerOffSet.y + (Mathf.Sin(angle)*radius);

        angle = angle + speed*Time.deltaTime;
        MoveShadowSprite();
        Vector3 circlePosition = new Vector3(x,y,0);
        if(angle > 2* Mathf.PI)
        {
            angle = 0f;
        }
        oodlerVelocity = step;
        oodlerRB.MovePosition(circlePosition);
    }

 



    // This function will move the oodler and the player to the drop area after a successful grab
    public bool MoveToDropZone(float speed = 20)
    {
        var step = speed * Time.deltaTime;
        oodlerRB.MovePosition(Vector3.MoveTowards(transform.position, dropZoneCorrected, step));
        MoveShadowSprite();
        MoveGlichWithOodler();
        oodlerVelocity = step;
        if (Vector3.Distance(transform.position, dropZoneCorrected) < 0.3f)
        {
            oodlerRB.MovePosition(dropZoneCorrected);
            return true;
        }
        else
        {
            return false;
        }
    }



    // This function drops the player inside the drop area
    public bool DropGlich(float speed = 10)
    {
        var step = speed * Time.deltaTime;
        glich.transform.position = Vector3.MoveTowards(glich.transform.position, dropZone, step); // CHANGE THIS LATER TO RIGIDBODY
        if (Vector3.Distance(glich.transform.position, dropZone) < 0.3f)
        {
            glich.transform.position = dropZone;
            return true;
        }
        else
        {
            return false;
        }
    }




    // This function will make the oodler come down and strike the players last known location
    public bool Slam(float speed = 200f)
    {
       
        var step = speed * Time.deltaTime;
        oodlerRB.MovePosition(Vector3.MoveTowards(transform.position, glichLastPosition, step));
        if (Vector3.Distance(transform.position, glichLastPosition) < 0.1f)
        {
            return true;
        }
        else
        {

            return false;
        }

    }

    // This function wil This function will make the oodler come down and strike the players last known location I created a function for just grab so the same sfx doesn't play as slam
    public bool Grab(float speed = 200f)
    {
        
        var step = speed * Time.deltaTime;
        oodlerRB.MovePosition(Vector3.MoveTowards(transform.position, glichLastPosition, step));
        if (Vector3.Distance(transform.position, glichLastPosition) < 0.1f)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    // This function will follow the players position with an offset of 10 units above them if we reached the target in anyway then reached target then it will always return true
    public bool MoveToGlich(float speed)
    {
        
        var step = speed * Time.deltaTime;
        oodlerVelocity = step;
        playerOffSet = glich.transform.localPosition;
        playerOffSet.y = playerOffSet.y + 10f;
        oodlerRB.MovePosition(Vector3.MoveTowards(transform.position, playerOffSet, step));
        MoveShadowSprite();

        if (Vector3.Distance(transform.position, playerOffSet) < 1f)
        {
            oodlerRB.MovePosition(playerOffSet);
            return true;
        }
        else
        {
           
            return false;
        }
    }

    // This method will "Land" the oodler on the ground
    public bool LandOodler(float  landSpeed)
    {
        var step = landSpeed * Time.deltaTime;
        oodlerVelocity = step;
        oodlerRB.MovePosition(Vector3.MoveTowards(transform.position, oodlerLandPosition, step));
        if (Vector3.Distance(transform.position, oodlerLandPosition) < 0.3f)
        {
            oodlerRB.MovePosition(oodlerLandPosition);
            HideShadow();
            return true;
        }
        else
        {
            return false;
        }
    }

    // This function will rise the oodler from its current position
    public bool RiseOodler(float speed = 10f)
    {

        var step = speed * Time.deltaTime;
        oodlerVelocity = step;
        oodlerRB.MovePosition(Vector3.MoveTowards(transform.position, oodlerAirPosition, step));

        if (Vector3.Distance(transform.position, oodlerAirPosition) < 0.3f)
        {
            oodlerRB.MovePosition(oodlerAirPosition);
            return true;
        }
        else
        {
            return false;
        }
    }

    // This function will make the oodler run towards glich
    public void OodlerRun(float runSpeed, Vector3 oodlerRunDirection)
    {
        var step = runSpeed * Time.deltaTime;
        oodlerRB.MovePosition(transform.position + oodlerRunDirection * step);
    }


    // this function will move the Shadow Sprite
    public void MoveShadowSprite()
    {
        Vector3 spriteOffset = transform.position;
        spriteOffset.y = transform.position.y - 12f;
        oodlerShadowObject.GetComponent<Rigidbody2D>().MovePosition(spriteOffset);

    }

    public void ResetShadow()
    {
        oodlerShadowObject.GetComponent<Rigidbody2D>().MovePosition(transform.position);
    }


    // This function will move the glich with the oodler if they are caught
    public void MoveGlichWithOodler() { 
        
        var step = 10f * Time.deltaTime;
        glich.transform.position = transform.position - grabPositionOffset;
    }


    #endregion

    // CHECKS //

    #region Bool Checks


    // This function will check if the boss is vulnerable
    public bool BossIsDamageable()
    {
        if (stateMachine.GetCurrentState().GetCurrentChildState() is Vulnerable)
        {
            return true;

        }
        else
        {
            return false;
        }
    }

    // Bool for setting and getting if the boss hit an on level hazard 
    public void setHazard(bool hazard)
    {
        hitHazard = hazard;
    }

    public bool checkHazard()
    {
        return hitHazard;
    }

    #endregion



    // SETTERS AND GETTERS //
    #region Setters

    // this function will save a position glich was at
    public void SetLastPosition()
    {
        glichLastPosition = glich.transform.position;
    }

    // this function will get the saved position glich was at
    public Vector3 GetLastPosition()
    {
        return glichLastPosition;
    }

    // this function will get the landing position of oodler
    public void SetLandPosition()
    {
         oodlerLandPosition = transform.position + new Vector3(0, -12f, 0);
    }

    // This function will get the last position of the oodler before they slam their hand down
    public void SetGroundPosition()
    {
        oodlerGroundPosition = transform.position;
    }

    public void SetAirPosition()
    {
        oodlerAirPosition = transform.position;
        oodlerAirPosition.y = oodlerAirPosition.y + 12f;
    }

    #endregion



    // OTHER //
    #region Other
   
    // This Function will control enemies to go to the drop zone location
    public void ControlAllies(GameObject target, bool toDropZone = false)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            EnemyAI enemy = obj.GetComponent<EnemyAI>();

            if (enemy != null && enemy.team == Team.oddle && (enemy.state != State.dead || enemy.state != State.dying))
            {
                Debug.Log("set new target");
                enemy.SetTarget(target, false, toDropZone);
            }
        }
    }
    


    // This function checks if the player is close to a wall
    public bool GlichInOpen()
    {

        float theta = 1.0472f;
        // offset used to make sure that the players position is not overlapping with wall
        var directions = new List<Vector3> { Vector3.up,Vector3.right, Vector3.down, Vector3.left};
        Vector3 offSet = new Vector3(0f, -4f, 0f);
        Vector3 PlayerPosition = glich.transform.position + offSet;
        int layerMask = 1 << 8;
        float closestWall = 1000f;

        Vector2 point = new Vector2(0, 0);

       
        // This for loop will go through all directions and teleport the enemies in the direction where there is the most available space
        foreach (Vector3 direction in directions)
        {
            
            // corrects direction close to isometric grid
            var trueDirection = new Vector3(direction.y*Mathf.Sin(theta) + direction.x*Mathf.Cos(theta), direction.y * Mathf.Cos(theta) + direction.x * -Mathf.Sin(theta), 0);


            RaycastHit2D hit = Physics2D.Raycast(PlayerPosition, trueDirection, Mathf.Infinity, layerMask);

            if (hit)
            {
                float distance = hit.distance;

                if (distance < closestWall)
                {
                    point = hit.point;
                    closestWall = distance;

                }
            }
        }

        if (closestWall > 20f)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    // Gets player's health 
    public float GetGlichHealth()
    {
        if (playerScript != null)
        {

            return playerScript.health;
        }
        else
        {
            return 1f;
        }
    }


    // Gets player's max health 
    public float GetGlichMaxHealth()
    {
        if (playerScript != null)
        {

            return playerScript.maxHealth;
        }
        else
        {
            return 2f;
        }
    }
    #endregion

    // End of file
}
