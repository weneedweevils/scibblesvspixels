using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class Boss : MonoBehaviour, Imovable//, IDamagable,
{
    // references
    private float MaxHealth = 500f;
    private float CurrentHealth = 500f;
    public float MovementSpeed { get; set; } = 100f;
    public Rigidbody2D Rigidbody { get; set; }
    public SpriteRenderer BossSprite { get; set; }

    [field: SerializeField] public GameObject Glich;

    [field: SerializeField] public SpriteRenderer TestAttackSprite;
   
   




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

    //Movment
    private Vector3 playerOffSet = Vector3.zero;
    private Vector3 glichLastPosition = Vector3.zero;
    private Vector3 oodlerAirPosition = Vector3.zero;
    private Vector3 offScreen = new Vector3(220, 130, 0);

    // Health Crystals and health
    public GameObject HealthCrystal1;
    bool countedOne = false;
    public GameObject HealthCrystal2;
    bool countedTwo = false;
    public GameObject HealthCrystal3;
    bool countedThree = false;
    public GameObject HealthCrystal4;
    bool countedFour = false;
    public Scene nextScene = Scene.End;
    int CrystalsRemaining = 4;


    // UI
    public GameObject healthBarParent;
    public GameObject healthBar;
    public TextMeshProUGUI currentHealthUI;
    public TextMeshProUGUI maxHealthUI;
    private UnityEngine.UI.Image healthBarImage;


    // Attacking
    public PlayerMovement PlayerScript;


    //BoxCollider2D DamageCollider;
    PolygonCollider2D DamageCollider;
    CircleCollider2D hitboxCollider;
    public bool oodlerSlamCooldown = false;
    public bool vulnerable = false;
    private float invincibilityDuration = 40f / 60f;
    public CooldownTimer invincibilityTimer;
    public float oodlerAttackDamage = 50f;
    Rigidbody2D oodlerRB;


    // for controlling enemies for Drop attack
    public GameObject dropZoneObject;
    public bool grabbing = false;
    public bool caught = false;
    private Vector3 dropZone;
    private Vector3 dropZoneCorrected;


    // timings and speeds for boss battle
    public float bossVulnerabilityTime; // Time the oodler is vulnerable
    public float slamWarningTime; // Time the oodler stops before Slamming player
    public float grabWarningTime; // Time the oodler stops before grabbing player
    public float airTime;
   


    // ENUMS
    public enum AttackType {Grab,Slam}
    public enum Phase {P1,P2,P3}
    public Phase phase = Phase.P1;
    public AttackType attackType = AttackType.Slam;


    // Phases
    private bool enteredPhase2=false;
    private bool enteredPhase3=false;

    public int allowedSlams = 1;
    public int SlamNum = 0;

    //blockers

    public EnemyAI[] blockers;

    //animation

    public Animator animator;



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
        
    }


    void Start()
    {

        animator = GetComponent<Animator>();    
        CurrentHealth = MaxHealth;
        currentHealthUI.text = CurrentHealth.ToString();
        maxHealthUI.text = MaxHealth.ToString();

        StateMachine.Initialize(oodlerChase);

        BossSprite = GetComponent<SpriteRenderer>();
        PlayerScript = Glich.GetComponent<PlayerMovement>();
        DamageCollider = GetComponentInChildren<PolygonCollider2D>();
        //DamageCollider = GetComponent<BoxCollider2D>(); // trigger hitbox for detecting attack collisions
        hitboxCollider = GetComponent<CircleCollider2D>(); // collider hitbox for detecting physical collisions with object
        invincibilityTimer = new CooldownTimer(invincibilityDuration * 0.5f, invincibilityDuration * 0.5f);
        healthBarImage = healthBar.GetComponent<UnityEngine.UI.Image>();
        oodlerRB = GetComponent<Rigidbody2D>();

        

        dropZoneCorrected = new Vector3(dropZoneObject.transform.position.x, dropZoneObject.transform.position.y + 10f, 0);
        dropZone = new Vector3(dropZoneObject.transform.position.x, dropZoneObject.transform.position.y, 0);


    }


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

    public void Die()
    {
        Debug.Log("oodler is dead :/");
    }


    public void MoveEnemy(Vector2 velocity)
    {
        Rigidbody.velocity = velocity;
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


    private void Update(){
        CheckWinCondition();
        currentHealthUI.text = Mathf.Ceil(CurrentHealth).ToString();
        healthBarImage.fillAmount = CurrentHealth / MaxHealth;
        CheckPhase();
        
        //maxHealthUI.text = MaxHealth.ToString();
    }

    private void FixedUpdate()
    {
        
        StateMachine.currentOodlerState.FrameUpdate();
        
        invincibilityTimer.Update();
    }




    // BOSS METHODS //


    // ENABLERS AND DISABLERS //



    // Enabling/Disabling Hitboxes
    public void EnableAttackHitbox(bool enable)
    {
        if(enable)
        {
            DamageCollider.enabled = true;  
        }
        else
        {
            DamageCollider.enabled = false;
        }
    }



    // Enabling/Disabling Areabox
    public void EnableAreaHitbox(bool enable)
    {
        if (enable)
        {
            hitboxCollider.enabled = true;
        }
        else
        {
            hitboxCollider.enabled = false;
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

    // This function shows the oodlers shadows
    public void ShowShadow()
    {
        TestAttackSprite.color = new Color(0, 0, 0, 0.25f);
    }
     
    // This function shows the attack
    public void ShowAttack()
    {
        TestAttackSprite.color = new Color(255, 0, 0, 1f);
    }

    // This function hides the oodlers shadow
    public void HideShadow()
    {
        TestAttackSprite.color = new Color(0, 0, 0, 0f);
    }





    // MOVING METHODS //


    // This function moves the oodler to the drop zone where they drop glich
    public void MoveToDropZone(float speed = 20)
    {
        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, dropZoneCorrected, step);
        MoveShadowSprite();
    }


    // This function drops glich to the drop zone 
    public void DropGlich(float speed = 10)
    {
        var step = speed * Time.deltaTime;
        Glich.transform.position = Vector3.MoveTowards(Glich.transform.position, dropZone, step);

        
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
        transform.position = Vector3.MoveTowards(transform.localPosition, playerOffSet, step);
        oodlerRB.MovePosition(transform.position);
        MoveShadowSprite();

       

    }

    // This function will make the oodler come down and strike the players last known location
    public void Slam(float speed = 100)
    {
        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, glichLastPosition, step);
    }

    // This function will move the oodler to a location offscreen
    public void MoveOffScreen(float speed = 100)
    {
        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, offScreen, step);

        MoveShadowSprite();
    }


    // This function will move the oodler off the ground
    public void MoveUp(float speed = 20)
    {
        playerOffSet = Glich.transform.localPosition;
        playerOffSet.y = playerOffSet.y + 10f;
        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, oodlerAirPosition, step);

    }



    // this function will move the Shadow Sprite
    public void MoveShadowSprite()
    {
        Vector3 spriteOffset = transform.position;
        spriteOffset.y = transform.position.y - 12f;
        TestAttackSprite.transform.position = spriteOffset;

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




    // CHECKS //

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

    // This fucntion returns a bool if glich has reached their drop zone location 
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
        if (transform.position == playerOffSet)
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
        if (transform.position == glichLastPosition)
        {
           
            return true;
        }
        else
        {
            return false;
        }
    }

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


    public void CheckDirection()
    {
        if (transform.position.x >= Glich.transform.position.x)
        {
            if (BossSprite.flipX == false)
            {
                BossSprite.flipX = true;
                TestAttackSprite.flipX = true;
            }
        }
        else
        {
            if (BossSprite.flipX == true)
            {
                BossSprite.flipX = false;
                TestAttackSprite.flipX = false;
            }
        }
    }




    // SETTERS AND GETTERS //

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




    // OTHER //

    // this function will increase the alpha value slowly and reveal the outline of where the hand will slam
    public bool RevealAttack()
    {
        if (TestAttackSprite.color.a < 1)
        {
            var temp = TestAttackSprite.color;
            temp.a += 0.01f;
            TestAttackSprite.color = temp;
            return false;
        }
        else
        {
            return true;
        }
      
    }

    // this function will check to see if all the crystals are still active or if the oodler dies, cutscene plays if any one of these conditions are met
   

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
}
