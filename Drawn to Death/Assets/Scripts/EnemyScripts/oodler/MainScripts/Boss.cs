using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Schema;
using JetBrains.Annotations;
using Pathfinding;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Random = UnityEngine.Random;


public class Boss : MonoBehaviour
{
    // Private variables
    public Rigidbody2D Rigidbody { get; set; }
    public Animator animator;
    private SpriteRenderer oodlerSprite;
 

    // Public Parameters
    [Header ("Public float")]
    private float MaxHealth = 500f;
    private float CurrentHealth = 500f;
    public float MovementSpeed { get; set; } = 100f;
    public float oodlerAttackDamage = 50f;
    private float invincibilityDuration = 40f / 60f;

     // timings and speeds for boss battle
    public float bossVulnerabilityTime = 10f; // Time the oodler is vulnerable
    public float slamWarningTime = 1f; // Time the oodler stops before Slamming player
    public float grabWarningTime = 1.25f; // Time the oodler stops before grabbing player
    public float airTime = 5f;
    public int allowedSlams = 1;
    public int SlamNum = 0;

    
    [Header("Public bool Parameters")]
    public bool grabbing = false;
    public bool caught = false;

    // ENUMS
    public enum AttackType {Grab,Slam}
    public enum Phase {P1,P2,P3}
    public Phase phase = Phase.P1;
    public AttackType attackType = AttackType.Slam;

    

    [Header ("Shadow Reference")]
    public GameObject oodlerShadowObject;
    public SpriteRenderer oodlerShadow;
    public Animator shadowAnimator;
   


    [Header ("Player References")]
    public PlayerMovement PlayerScript;
    public GameObject Glich;


    // Health Crystals and health

    [Header ("Health Crystals")]
    public GameObject HealthCrystal1;
    bool countedOne = false;
    public GameObject HealthCrystal2;
    bool countedTwo = false;
    public GameObject HealthCrystal3;
    bool countedThree = false;
    public GameObject HealthCrystal4;
    bool countedFour = false;
    private int CrystalsRemaining = 4;


    // UI
    [Header ("UI References")]
    public GameObject healthBarParent;
    public GameObject healthBar;
    public TextMeshProUGUI currentHealthUI;
    public TextMeshProUGUI maxHealthUI;
    private UnityEngine.UI.Image healthBarImage;


    [Header ("HitBox References")]
    // Collider References
    public GameObject runHitboxCollider;

    public GameObject attackHitboxCollider;

    public GameObject selfHitboxCollider;


    // States
    public OodlerStateMachine StateMachine { get; set; }
    public OodlerChase oodlerChase { get; set; }
    public OodlerIdle oodlerIdle { get; set; }
    public OodlerChargeAttack oodlerChargeAttack { get; set; }
    public OodlerSlam oodlerSlam { get; set; }
    public OodlerRecover oodlerRecover { get; set; }
    public OodlerGrab oodlerGrab { get; set; }
    public OodlerDrop oodlerDrop { get; set; }
    public OodlerInitial oodlerInitial{ get; set; }
    public OodlerRun oodlerRun { get; set; }


    //Movment
    private Vector3 playerOffSet = Vector3.zero;
    private Vector3 glichLastPosition = Vector3.zero;
    private Vector3 oodlerAirPosition = Vector3.zero;
    private Vector3 oodlerRunDirection = Vector3.zero;
    private Vector3 runPosition = Vector3.zero; 
    private Vector3 offScreen = new Vector3(220, 130, 0);
    public bool oodlerSlamCooldown = false;
    public bool vulnerable = false;
   
    public CooldownTimer invincibilityTimer;
    
    Rigidbody2D oodlerRB;


    // for controlling enemies for Drop attack
    public GameObject dropZoneObject;
    private Vector3 dropZone;
    private Vector3 dropZoneCorrected;


    // Phases
    private bool enteredPhase2=false;
    private bool enteredPhase3=false;

    
    //blockers
    public EnemyAI[] blockers;

    
    public Scene nextScene = Scene.End;

    private float angle = 0f;
    private List<Vector3> points;

    private void Awake()
    {
        StateMachine = new OodlerStateMachine();
        oodlerIdle = new OodlerIdle(this, StateMachine);
        oodlerChase = new OodlerChase(this, StateMachine);
        oodlerChargeAttack = new OodlerChargeAttack(this, StateMachine);
        oodlerSlam = new OodlerSlam(this, StateMachine);
        oodlerRecover = new OodlerRecover(this, StateMachine);
        oodlerGrab = new OodlerGrab(this, StateMachine);
        oodlerDrop = new OodlerDrop(this, StateMachine);
        oodlerInitial = new OodlerInitial(this, StateMachine);
        oodlerRun = new OodlerRun(this, StateMachine);
        
    }


    void Start()
    {
        StateMachine.Initialize(oodlerRun);
        CurrentHealth = MaxHealth;
        currentHealthUI.text = CurrentHealth.ToString();
        maxHealthUI.text = MaxHealth.ToString();


        //shadowAnimator = oodlerShadowObject.GetComponentInChildren<Animator>();
        //oodlerShadow = oodlerShadowObject.GetComponentInChildren<SpriteRenderer>();


        animator = GetComponentInChildren<Animator>();
        oodlerSprite = GetComponentInChildren<SpriteRenderer>();

        PlayerScript = Glich.GetComponent<PlayerMovement>();

        invincibilityTimer = new CooldownTimer(invincibilityDuration * 0.5f, invincibilityDuration * 0.5f);
        healthBarImage = healthBar.GetComponent<UnityEngine.UI.Image>();
        oodlerRB = GetComponent<Rigidbody2D>();
      

        dropZoneCorrected = new Vector3(dropZoneObject.transform.position.x, dropZoneObject.transform.position.y + 10f, 0);
        dropZone = new Vector3(dropZoneObject.transform.position.x, dropZoneObject.transform.position.y, 0);

        Debug.Log("My Rigidbody is" + oodlerRB);
        Debug.Log("my shadow is" + shadowAnimator);
        Debug.Log("My shadow sprite is" + oodlerShadow);

        points = new List<Vector3>();
    }

    // Damage Function will damage the oodler and check if they are dead
    public void Damage(float damageTaken)
    {
        CurrentHealth = CurrentHealth - damageTaken;
        
        Debug.Log(CurrentHealth);
        invincibilityTimer.StartTimer();
        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    // Die function will be called when the oodler dies
    public void Die()
    {
        Debug.Log("oodler is dead :/");
    }



    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.currentOodlerState.AnimationTriggerEvent(triggerType);

    }

    public enum AnimationTriggerType
    {
        BossIdle,
        BossFollow,
        BossDamage
    }


    #region Update
    private void Update(){
        CheckWinCondition();
        currentHealthUI.text = Mathf.Ceil(CurrentHealth).ToString();
        healthBarImage.fillAmount = CurrentHealth / MaxHealth;
        CheckPhase();
        
        //maxHealthUI.text = MaxHealth.ToString();
    }

    // FixedUpdate to update physics
    private void FixedUpdate()
    {
        StateMachine.currentOodlerState.FrameUpdate();
        invincibilityTimer.Update();
    }
    #endregion


    #region Animation
    private void CheckSpriteDirection(){
        if(transform.position.x - Glich.transform.position.x >= 0){
            oodlerSprite.flipX = true;
            oodlerShadow.flipX = true; // might need to change this so I only have to reference it
        }
        else{
             oodlerSprite.flipX = false;
               oodlerShadow.flipX = false;
        }
    }

     // This function shows the oodlers shadows
    public void ShowShadow()
    {
        oodlerShadow.color = new Color(0, 0, 0, 0.25f);
    }
     
    // This function shows the attack
    public void ShowAttack()
    {
        oodlerShadow.color = new Color(255, 0, 0, 1f);
    }

    // This function hides the oodlers shadow
    public void HideShadow()
    {
        oodlerShadow.color = new Color(0, 0, 0, 0f);
    }

     public void ChangeSpriteSortingOrder(int sortingLayer){
        oodlerSprite.sortingOrder = sortingLayer;
    }
    

     // this function will increase the alpha value slowly and reveal the outline of where the hand will slam
    public bool RevealAttack()
    {
        if (oodlerShadow.color.a < 1)
        {
            var temp = oodlerShadow.color;
            temp.a += 0.5f * Time.deltaTime;
            oodlerShadow.color = temp;
            return false;
        }
        else
        {
            return true;
        }
      
    }

    public Animator GetShadow(){
        return shadowAnimator;
    }



    # endregion


    // --BOSS METHODS-- //


    // ENABLERS AND DISABLERS //


    // Enabling/Disabling Hitboxes


    #region Hitboxes
    public void EnableAttackHitbox(bool enable)
    {
        if(enable)
        {
            attackHitboxCollider.SetActive(true);
        }
        else
        {
            attackHitboxCollider.SetActive(false);
        }
    }



    // Enabling/Disabling Areabox
    public void EnableAreaHitbox(bool enable)
    {
        if (enable)
        {
            selfHitboxCollider.SetActive(true);
        }
        else
        {
            selfHitboxCollider.SetActive(false);
        }
    }


     public void EnableRunHitbox(bool enable)
    {
        if (enable)
        {
            runHitboxCollider.SetActive(true);
        }
        else
        {
            runHitboxCollider.SetActive(false);
        }
    }


    // This function enables/disables gliches hitboxes
    public void EnableGlichColliders(bool enable)
    {
        if (enable)
        {
            Glich.GetComponent<CapsuleCollider2D>().enabled = true;
            Glich.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            Glich.GetComponent<CapsuleCollider2D>().enabled = false;
            Glich.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    #endregion

   


    #region Moving Methods
    // MOVING METHODS //

    public void CircleGlich( float speed, float radius){


        // if(angle>360f){
        //     angle = 0f;
        // }

        var step = speed * Time.deltaTime;
       
        Debug.Log("My angle is "+ angle);

        playerOffSet = Glich.transform.localPosition;
        playerOffSet.y = playerOffSet.y + 10f;

        float x = playerOffSet.x + (Mathf.Cos(angle)*radius);
        float y = playerOffSet.y + (Mathf.Sin(angle)*radius);

        angle = angle + speed*Time.deltaTime;
        Debug.Log("angle is "+ angle);
        MoveShadowSprite();
        Vector3 circlePosition = new Vector3(x,y,0);


        //transform.position = circlePosition;
        //points.Add(circlePosition);

        oodlerRB.MovePosition(circlePosition);
    }

    // void OnDrawGizmos(){
    //     Gizmos.color = Color.magenta;
    //      if(points.Count > 1){
    //         Gizmos.DrawLine(points[points.Count-1],points[points.Count-2]);
    //     }

    // }

    public void SelectRunPosition(){
        //RaycastHit hit;
        float starting_angle = 0;
        float radius = 30f;
        var Positions = new List<Vector3>();
        int layerMask = 1 << 8;
     
        
        for(float i = starting_angle; i<360f; i = i + 1f){

            
            float x = Glich.transform.position.x + (Mathf.Cos(i)*radius);
            float y = Glich.transform.position.y + (Mathf.Sin(i)*radius);
            Vector3 landingSpot = new Vector3(x,y,0);
            Vector3 direction = (Glich.transform.position - landingSpot).normalized;

            RaycastHit2D hit = Physics2D.Raycast(landingSpot, direction, Mathf.Infinity, layerMask);
         

            Debug.Log("I hit a wall at a distance of " + hit.distance + " from the point");
            
            if(hit.distance > radius){
                Positions.Add(landingSpot);
            //     Debug.DrawLine(landingSpot, Glich.transform.position, Color.magenta, 5f);
            //     Debug.Log(i + " is a valid angle and there are not obstacles in the way");
            }
        }

        var rnd = new Random();
        int index = Random.Range(0, Positions.Count);
        runPosition = Positions[index];



    }

    // This method will move the ooodler to the position where it will try to run glich over
    public bool MoveToRunPosition(float speed = 50){
        var step = speed * Time.deltaTime;
        oodlerRB.MovePosition(Vector3.MoveTowards(transform.position, runPosition, step));
        MoveShadowSprite();
        if(Vector3.Distance(transform.position,runPosition)<0.3f){
            return true;
        }else{
            return false;
        }
    }       


    public bool ReachedCirclePosition(float speed = 50){
        var step = speed * Time.deltaTime;
        oodlerRB.MovePosition(Vector3.MoveTowards(transform.position, runPosition, step));
        MoveShadowSprite();
        if(Vector3.Distance(transform.position,runPosition)<0.3f){
            return true;
        }else{
            return false;
        }

    }


    // This method will "Land" the oodler on the ground
    public bool LandForRun(float speed = 15){
        SetAirPosition();
        var step = speed * Time.deltaTime;
        var runGroundPosition = runPosition + new Vector3(0,-12f,0);
        oodlerRB.MovePosition(Vector3.MoveTowards(transform.position,runGroundPosition,step));
        if(Vector3.Distance(transform.position,runGroundPosition)<0.3f){
            animator.SetTrigger("Walk");
            shadowAnimator.SetTrigger("Walk");
            oodlerRunDirection = (Glich.transform.position-transform.position).normalized;
            HideShadow();
            return true;

        }else{
            return false;
        }
    }

    public bool Run(float speed = 100){
        var step = speed * Time.deltaTime;
        oodlerRB.MovePosition(transform.position + oodlerRunDirection * step);
        //oodlerRB.MovePosition(Vector3.MoveTowards(transform.position,oodlerRunDirection*Mathf.Infinity,step));
        CheckSpriteDirection();


        return false;
        //  if(Vector3.Distance(transform.position,Glich.transform.position+oodlerRunDirection*20f)<0.3f){
        //     animator.SetTrigger("Idle");
        //     shadowAnimator.SetTrigger("Idle");
        //     return true;
        // }else{
        //     return false;
        // }
    }


    // This function moves the oodler to the drop zone where they drop glich
    public void MoveToDropZone(float speed = 20)
    {
        var step = speed * Time.deltaTime;
        oodlerRB.MovePosition(Vector3.MoveTowards(transform.position, dropZoneCorrected, step));
        MoveShadowSprite();
    }


    // This function drops glich to the drop zone 
    public void DropGlich(float speed = 10)
    {
        var step = speed * Time.deltaTime;
        Glich.transform.position = Vector3.MoveTowards(Glich.transform.position, dropZone, step); // CHANGE THIS LATER TO RIGIDBODY

        
    }


    //void OnDrawGizmos()
    //{

    //    Gizmos.color = new Color(1, 0, 0, 0.5f);
    //    Gizmos.DrawCube(playerOffSet, new Vector3(1, 1, 1));
    //    Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
    //}


    // This function will follow the players position with an offset of 10 units above them
    public void Stalk(float speed = 20f)
    {
        var step = speed * Time.deltaTime;
        playerOffSet = Glich.transform.localPosition;
        playerOffSet.y = playerOffSet.y + 10f;
        //transform.position = Vector3.MoveTowards(transform.position, playerOffSet, step);
        oodlerRB.MovePosition(Vector3.MoveTowards(transform.position, playerOffSet, step));
        MoveShadowSprite();

       

    }

    // This function will make the oodler come down and strike the players last known location
    public void Slam(float speed = 100)
    {
        var step = speed * Time.deltaTime;
        oodlerRB.MovePosition(Vector3.MoveTowards(transform.position, glichLastPosition, step));
        //oodlerSprite.transform.position = Vector3.MoveTowards(transform.position, oodlerAirPosition, step);

        //oodlerRB.MovePosition(Vector3.MoveTowards(transform.position, oodlerShadow.transform.position, step));
    }

    public void Land(float speed = 100)
    {
        var step = speed * Time.deltaTime;
        oodlerRB.MovePosition(Vector3.MoveTowards(transform.position, playerOffSet, step));
    }

    // This function will move the oodler to a location offscreen
    public void MoveOffScreen(float speed = 100)
    {
        var step = speed * Time.deltaTime;
        oodlerRB.MovePosition(Vector3.MoveTowards(transform.position, offScreen, step));
        MoveShadowSprite();
    }


    // This function will move the oodler off the ground
    public void MoveUp(float speed = 20)
    {
        playerOffSet = Glich.transform.localPosition;
        playerOffSet.y = playerOffSet.y + 10f;
        var step = speed * Time.deltaTime;
        oodlerRB.MovePosition(Vector3.MoveTowards(transform.position, oodlerAirPosition, step));

    }



    // this function will move the Shadow Sprite
    public void MoveShadowSprite()
    {
        Vector3 spriteOffset = transform.position;
        spriteOffset.y = transform.position.y - 12f;
        //oodlerShadow.transform.position = spriteOffset;
        oodlerShadowObject.GetComponent<Rigidbody2D>().MovePosition(spriteOffset);

    }


    // This function will move the glich with the oodler if they are caught
    public void MoveGlichWithOodler()
    {
        if (caught == true)
        {
            Glich.transform.position = transform.position;
        }

        PlayerScript.StopMovement();
    }


    #endregion

    // CHECKS //

    #region Bool Checks

    // this function will return a bool if the oodler has reached offscreen
    public bool ReachedOffScreen()
    {
        if (transform.position == offScreen)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // This function will check if the oodler reached the last position it was in the air
    public bool ReachedAirPosition()
    {
        if (transform.position == oodlerAirPosition)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // This function returns a bool if the oodler reached the correct drop zone which is a few positions up from the actual dropzone
    public bool ReachedDropZone()
    {
        if (transform.position == dropZoneCorrected)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // This fucntion returns a bool if glich has reached their drop zone location when the oodler drops them
    public bool GlichReachedDropZone()
    {

        var glichCurrentRoundedPosition = new Vector3(Mathf.Round(Glich.transform.position.x),Mathf.Round(Glich.transform.position.y),Mathf.Round(Glich.transform.position.z));

        if (Glich.transform.position == dropZone ||Vector3.Distance(Glich.transform.position,dropZone)<0.3)
        {
            return true;
        }
        else
        {
            PlayerScript.StopMovement();
            Debug.Log("Rounded Glich Position is "+ glichCurrentRoundedPosition);
            Debug.Log("Actual Glich Position is " + Glich.transform.position);
            Debug.Log("Drop Zone Position is" + dropZone);

            return false;
        }
    }

    // this function will return a bool if the oodler has reached the glichs offset position
    public bool ReachedPlayer()
    {
        if (Vector3.Distance(transform.position,playerOffSet)<0.3f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // this function will return a bool if the oodler has reached gliches actual position
    public bool ReachedPlayerReal()
    {
        if (Vector3.Distance(transform.position,glichLastPosition)<0.2f)
        {
           
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool ActivateSlamHitbox(){
         if (Vector3.Distance(transform.position,glichLastPosition)<1.5f)
        {
            return true;
        }
        else{
            return false;
        }
    }

    #endregion

    // this function will check to see if all the crystals are still active or if the oodler dies, cutscene plays if any one of these conditions are met
    public void CheckWinCondition()
    {
        //if (HealthCrystal1 == null && !countedOne)
        //{
        //    CrystalsRemaining -= 1;
        //    countedOne = true;
        //}

        //if (HealthCrystal2 == null && !countedTwo)
        //{
        //    CrystalsRemaining -= 1;
        //    countedTwo = true;
        //}

        //if (HealthCrystal3 == null && !countedThree)
        //{
        //    CrystalsRemaining -= 1;
        //    countedThree = true;
        //}

        //if (HealthCrystal4 == null && !countedFour)
        //{
        //    CrystalsRemaining -= 1;
        //    countedFour = true;
        //}


        if (CurrentHealth < 0)
        {
            Debug.Log("we go to end scene");
            if (nextScene != Scene.End)
            {
                GameData data = DataPersistenceManager.instance.GetGameData();
                data.skipCutscene = false;
                DataPersistenceManager.instance.UpdateGame();
            }
            StartCoroutine(MenuManager.LoadScene(nextScene));
        }
    }


    // This function will check if the boss is vulnerable
    public bool BossIsDamageable()
    {
        return vulnerable;
    }


    // This function will return a bool whether 
    public bool activateDamage()
    {
        float distance = Vector3.Distance(transform.position, GetLastPosition());

        if (distance < 5f)
        {
            return true;

        }
        else
        {
            return false;

        }
    }


    
    // SETTERS AND GETTERS //
    #region Setters

    // this function will save a position glich was at
    public void SetLastPosition()
    {
        glichLastPosition = Glich.transform.position;
    }

    // this function will get the saved position glich was at
    public Vector3 GetLastPosition()
    {
        return glichLastPosition;
    }


    // This function will get the last position of the oodler before they slam their hand down
    public void SetAirPosition()
    {
        oodlerAirPosition = transform.position;
    }

    public void SetSlamCooldown(bool onCooldown){
        if(onCooldown){
            oodlerSlamCooldown = true;
        }
        else{
             oodlerSlamCooldown = false;
        }
    }

    public void SetBossVulnerability(bool isVulnerable){
        if(isVulnerable){
            vulnerable = true;
        }
        else{
            vulnerable = false;
        }
    }

    public void SetBossCaught(bool isCaught){
        if(isCaught){
            caught = true;
        }
        else{
            caught = false;
        }

    }


    #endregion



    // OTHER //
    #region Other
   

    public bool OnSlamCooldown(){
        return oodlerSlamCooldown;
    }

    public bool IsCaught(){
        return caught;
    }


    public void heal(float heal_amount)
    {
        if (CurrentHealth <= MaxHealth)
        {
            if (CurrentHealth + heal_amount > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
            else
            {
                CurrentHealth += heal_amount;
            }
        }
    }

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
    
    public void CheckPhase()
    {
        if (!enteredPhase2 && MaxHealth / 2f > CurrentHealth)
        {
            enteredPhase2 = true;
            phase = Phase.P2;
            allowedSlams = 2;
            bossVulnerabilityTime = 6f;
            slamWarningTime = 0.75f;
            grabWarningTime = 1.00f;
            airTime = 2f;
            SlamNum = 0;

        }

        if (!enteredPhase3 && MaxHealth / 4f > CurrentHealth)
        {
            enteredPhase3 = true;
            phase = Phase.P3;
            allowedSlams = 3;
            bossVulnerabilityTime = 4f;
            slamWarningTime = 0.5f;
            grabWarningTime = 0.75f;
            airTime = 0.5f;
            SlamNum = 0;


        }
    }

    #endregion 
}
