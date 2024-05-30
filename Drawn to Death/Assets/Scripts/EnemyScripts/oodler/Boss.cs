using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class Boss : MonoBehaviour, Imovable//, IDamagable,
{
    // references
    private float MaxHealth = 1000f;
    private float CurrentHealth = 1000f;
    public float MovementSpeed { get; set; } = 100f;
    public Rigidbody2D Rigidbody { get; set; }
    public SpriteRenderer BossSprite { get; set; }

    [field: SerializeField] public GameObject Glich;

    [field: SerializeField] public SpriteRenderer TestAttackSprite;
   
   




    // States
    public OodlerStateMachine StateMachine { get; set; }
    public OodlerChase oodlerChase { get; set; }
    public OodlerIdle oodlerIdle { get; set; }
    public OodlerAttack oodlerAttack { get; set; }
    public OodlerSlam oodlerSlam { get; set; }
    public OodlerRecover oodlerRecover { get; set; }
    public OodlerGrab oodlerGrab { get; set; }
    public OodlerDrop oodlerDrop { get; set; }

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
   


    private void Awake()
    {
        StateMachine = new OodlerStateMachine();
        oodlerIdle = new OodlerIdle(this, StateMachine);
        oodlerChase = new OodlerChase(this, StateMachine);
        oodlerAttack = new OodlerAttack(this, StateMachine);
        oodlerSlam = new OodlerSlam(this, StateMachine);
        oodlerRecover = new OodlerRecover(this, StateMachine);
        oodlerGrab = new OodlerGrab(this, StateMachine);
        oodlerDrop = new OodlerDrop(this, StateMachine);
    }


    void Start()
    {
        CurrentHealth = MaxHealth;
        currentHealthUI.text = CurrentHealth.ToString();
        maxHealthUI.text = MaxHealth.ToString();

        StateMachine.Initialize(oodlerIdle);

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


    // This function will follow the players position with an offset of 10 units above them
    public void Stalk(float speed = 20)
    {
        var step = speed * Time.deltaTime;
        playerOffSet = Glich.transform.position;
        playerOffSet.y = playerOffSet.y + 10f;
        transform.position = Vector3.MoveTowards(transform.position, playerOffSet, step);
        oodlerRB.MovePosition(transform.position);

        //transform.position = Vector3.Lerp(transform.position, playerOffSet, step);
        //CheckDirection();
        MoveShadowSprite();

        //Vector2 playervel = PlayerScript.GetVelocity();
        //Vector3 backSet = new Vector3(0, 0, 0);
        //float xsign = 1f;
        //float ysign = 1f;
        //if (playervel == Vector2.zero)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, playerOffSet, step);
        //    MoveSprite();
        //}

        //else if (playervel.x >= 0)
        //{
        //    xsign = 0;
        //    ysign = Mathf.Sign(playervel.y) * -1f;
        //    backSet = new Vector3(5, 5, 0); // to be added to the vector
        //}

        //else if (playervel.y >= 0)
        //{
        //    xsign = Mathf.Sign(playervel.x) * -1f;
        //    ysign = 0;
        //    backSet = new Vector3(5, 5, 0); // to be added to the vector
        //}


        //else
        //{

        //    xsign = Mathf.Sign(playervel.x) * -1f;
        //    ysign = Mathf.Sign(playervel.y) * -1f;
        //    backSet = new Vector3(5, 5, 0); // to be added to the vector
        //}

        //Vector3 directionVector = new Vector3(xsign, ysign, 0f);
        //backSet = Vector2.Scale(directionVector, backSet);
        //transform.position = Vector3.MoveTowards(transform.position, playerOffSet + backSet, step);
        //MoveSprite();

    }

    // This function will make the oodler come down and strike the players last known location
    public void Slam(float speed = 100)
    {
       
        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, glichLastPosition, step);

        // Experimental code for adjusting shadowSize
        //float distanceDen = Vector3.Distance(oodlerAirPosition, glichLastPosition);
        //float distanceNum = Vector3.Distance(transform.position, glichLastPosition);
        //float scalingfactor = (1f-distanceNum/distanceDen);
        //var newSize = new Vector3(TestAttackSprite.transform.localScale.x * 2f * scalingfactor, TestAttackSprite.transform.localScale.y * 2f * scalingfactor, TestAttackSprite.transform.localScale.z);
        //TestAttackSprite.transform.root.localScale = newSize;
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
        if (HealthCrystal1 == null && !countedOne)
        {
            CrystalsRemaining -= 1;
            countedOne = true;
        }

        if (HealthCrystal2 == null && !countedTwo)
        {
            CrystalsRemaining -= 1;
            countedTwo = true;
        }

        if (HealthCrystal3 == null && !countedThree)
        {
            CrystalsRemaining -= 1;
            countedThree = true;
        }

        if (HealthCrystal4 == null && !countedFour)
        {
            CrystalsRemaining -= 1;
            countedFour = true;
        }


        if (CrystalsRemaining == 0 || CurrentHealth < 0)
        {
            Debug.Log("we go to end scene");
            if (nextScene != Scene.End)
            {
                GameData data = DataPersistenceManager.instance.GetGameData();
                data.skipCutscene = false;
                DataPersistenceManager.instance.UpdateGame();
            }
            MenuManager.GotoScene(nextScene);
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
        if (CurrentHealth < MaxHealth)
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


    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log("Collided with " + collision.name);

    //    switch (collision.gameObject.tag)
    //    {

    //        case "Player":
    //            {

    //                //if (oodlerSlamCooldown == false && !PlayerScript.dashTimer.IsActive())
    //                if (!PlayerScript.dashTimer.IsActive() && oodlerSlamCooldown==false && !PlayerScript.invincibilityTimer.IsActive() && activateDamage())
    //                {
    //                    PlayerScript.Damage(oodlerAttackDamage);
    //                }



    //            }
    //            break;

    //        case "Enemy":
    //            {
    //                EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();


    //                if (enemy != null && !enemy.invincibilityTimer.IsActive() && oodlerSlamCooldown == false && activateDamage())
    //                {
    //                    enemy.Damage(oodlerAttackDamage);
    //                }

    //                else
    //                {
    //                    HealthCrystal crystal = collision.gameObject.GetComponent<HealthCrystal>();
    //                    if (crystal != null)
    //                    {
    //                        if (crystal != null && crystal.invincibilityTimer.IsUseable() && oodlerSlamCooldown == false && activateDamage())
    //                        {
    //                            //Damage enemy
    //                            crystal.CrystalDamage(oodlerAttackDamage, true);
    //                        }
    //                    }
    //                }
    //            }
    //            break;

    //        case "Column":
    //            {
    //                Destroy(collision.gameObject);

    //            }
    //            break;

    //    default:
    //            {
    //                break;
    //            }
    //    }

    //}




}
