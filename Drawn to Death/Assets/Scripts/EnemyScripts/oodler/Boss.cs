using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Build;
using UnityEditorInternal;
using UnityEngine;

public class Boss : MonoBehaviour, IDamagable, Imovable
{
    // references
    [field: SerializeField] public float MaxHealth { get; set; } = 1000f;
    [field: SerializeField] public float CurrentHealth { get; set; }
    public float MovementSpeed { get; set; } = 100f;
    public Rigidbody2D Rigidbody { get; set; }
    public SpriteRenderer BossSprite { get; set; }
  
    [field: SerializeField] public GameObject Glich;

    [field: SerializeField] public SpriteRenderer AttackSprite;

    [field: SerializeField] public SpriteRenderer TestAttackSprite;


    // States
    public OodlerStateMachine StateMachine { get; set; }
    public OodlerChase oodlerChase { get; set; }
    public OodlerIdle oodlerIdle { get; set; }
    public OodlerAttack oodlerAttack { get; set; }
    public OodlerSlam oodlerSlam { get; set; }
    public OodlerRecover oodlerRecover { get; set; }

    //Movment
    private Vector3 playerOffSet = Vector3.zero;
    private Vector3 glichLastPosition = Vector3.zero;
    private Vector3 oodlerAirPosition = Vector3.zero;
    private Vector3 offScreen = new Vector3(220,130,0);

    // Health Crystals
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

    // Attacking
    public PlayerMovement PlayerScript;
    BoxCollider2D DamageCollider;
    CircleCollider2D hitboxCollider;
    public bool oodlerSlamCooldown = false;
    public bool vulnerable = false;
    private float invincibilityDuration = 60f / 60f;
    public CooldownTimer invincibilityTimer;
    public float oodlerAttackDamage = 50f;


    private void Awake()
    {
        StateMachine = new OodlerStateMachine();
        oodlerIdle = new OodlerIdle(this, StateMachine);
        oodlerChase = new OodlerChase(this, StateMachine);
        oodlerAttack = new OodlerAttack(this, StateMachine);
        oodlerSlam = new OodlerSlam(this, StateMachine);
        oodlerRecover = new OodlerRecover(this, StateMachine);
    }


    void Start()
    {
        CurrentHealth = MaxHealth;
        StateMachine.Initialize(oodlerIdle);
        BossSprite = GetComponent<SpriteRenderer>();
        PlayerScript = Glich.GetComponent<PlayerMovement>();
        DamageCollider = GetComponent<BoxCollider2D>(); // trigger hitbox for detecting attack collisions
        hitboxCollider = GetComponent<CircleCollider2D>(); // collider hitbox for detecting physical collisions with object
        invincibilityTimer = new CooldownTimer(invincibilityDuration * 0.5f, invincibilityDuration * 0.5f);
    }


    public void Damage(float damageTaken)
    {
        CurrentHealth = CurrentHealth - damageTaken;

        Debug.Log("Oodler current health is "+ CurrentHealth.ToString());

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

    private void Update()
    {
        
        StateMachine.currentOodlerState.FrameUpdate();
        CheckWinCondition();
        invincibilityTimer.Update();
    }



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







    // BOSS METHODS //



    public void ShowShadow()
    {
        TestAttackSprite.color = new Color(0, 0, 0, 0.25f);
    }

    public void ShowAttack()
    {
        TestAttackSprite.color = new Color(255, 0, 0, 1f);
    }

    public void HideShadow()
    {
        TestAttackSprite.color = new Color(0, 0, 0, 0f);
    }


    // This function will follow the players position with an offset of 10 units above them
    public void Stalk(float speed = 100)
    {
        var step = speed * Time.deltaTime;
        playerOffSet = Glich.transform.position;
        playerOffSet.y = playerOffSet.y + 10f;
        transform.position = Vector3.MoveTowards(transform.position, playerOffSet, step);
        MoveSprite();

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

        MoveSprite();
    }


    // This function will move the oodler off the ground
    public void MoveUp(float speed = 20)
    {
        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, oodlerAirPosition, step);

    }



    // this function will move the sprite
    public void MoveSprite()
    {
        Vector3 spriteOffset = transform.position;
        spriteOffset.y = transform.position.y - 12f;
        TestAttackSprite.transform.position = spriteOffset;

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


    // this function will get the last position of glich
    public void SetLastPosition()
    {
        glichLastPosition = Glich.transform.position;
    }


    // This function will get the last position in the air before slamming down 
    public void SetAirPosition()
    {
        oodlerAirPosition = transform.position;
    }



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


        if (CrystalsRemaining == 0 || CurrentHealth<0)
        {
            if (nextScene != Scene.End)
            {
                GameData data = DataPersistenceManager.instance.GetGameData();
                data.skipCutscene = false;
                DataPersistenceManager.instance.UpdateGame();
            }
            MenuManager.GotoScene(nextScene);
        }
    }

    public bool BossIsDamageable()
    {
        return vulnerable;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided with " + collision.name);

        switch (collision.gameObject.tag)
        {

            case "Player":
                {

                    //if (oodlerSlamCooldown == false && !PlayerScript.dashTimer.IsActive())
                    if (!PlayerScript.dashTimer.IsActive() && oodlerSlamCooldown==false && !PlayerScript.invincibilityTimer.IsActive())
                    {
                        PlayerScript.Damage(oodlerAttackDamage);
                    }
                    


                }
                break;

            case "Enemy":
                {
                    EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();


                    if (enemy != null && !enemy.invincibilityTimer.IsActive() && oodlerSlamCooldown == false)
                    {
                        enemy.Damage(oodlerAttackDamage);
                    }

                    else
                    {
                        HealthCrystal crystal = collision.gameObject.GetComponent<HealthCrystal>();
                        if (crystal != null)
                        {
                            if (crystal != null && crystal.invincibilityTimer.IsUseable() && oodlerSlamCooldown == false)
                            {
                                //Damage enemy
                                crystal.CrystalDamage(oodlerAttackDamage, true);
                            }
                        }
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
