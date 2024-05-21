using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour, IDamagable, Imovable
{
    [field: SerializeField] public float MaxHealth { get; set; } = 100f;
    [field: SerializeField] public float CurrentHealth { get; set; }
    public float MovementSpeed { get; set; } = 100f;
    public Rigidbody2D Rigidbody { get; set; }
    public SpriteRenderer BossSprite { get; set; }
  
    [field: SerializeField] public GameObject Glich;

    [field: SerializeField] public SpriteRenderer AttackSprite;


    // States
    public OodlerStateMachine StateMachine { get; set; }
    public OodlerChase oodlerChase { get; set; }
    public OodlerIdle oodlerIdle { get; set; }
    public OodlerAttack oodlerAttack { get; set; }
    public OodlerSlam oodlerSlam { get; set; }



    //Movment
    Vector3 offSet;
    private Vector3 glichLastPosition = Vector3.zero;


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

    // Start is called before the first frame update

    private void Awake()
    {
        StateMachine = new OodlerStateMachine();
        oodlerIdle = new OodlerIdle(this, StateMachine);
        oodlerChase = new OodlerChase(this, StateMachine);
        oodlerAttack = new OodlerAttack(this, StateMachine);
        oodlerSlam = new OodlerSlam(this, StateMachine);
    }


    void Start()
    {
        CurrentHealth = MaxHealth;
        StateMachine.Initialize(oodlerIdle);
        BossSprite = GetComponent<SpriteRenderer>();
    }


    public void Damage(float damageTaken)
    {
        CurrentHealth = damageTaken;

        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        throw new System.NotImplementedException();
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

        if(HealthCrystal1 == null && !countedOne)
        {
            CrystalsRemaining -= 1 ;
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


        if (CrystalsRemaining==0)
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



    // This function will follow the players position with an offset of 10 units above them
    public void Stalk()
    {
        var step = MovementSpeed * Time.deltaTime;
        offSet = Glich.transform.position;
        offSet.y = offSet.y + 10f;
        transform.position = Vector3.MoveTowards(transform.position, offSet, step);
    }

    // This function will make the oodler come down and strike the player with their hand
    public void Slam()
    {
        var step = MovementSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, glichLastPosition, step);
    }


    // this function will return a bool if the oodler has reached the glich
    public bool ReachedPlayer()
    {
        if (transform.position == offSet)
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

    public void SetLastPosition()
    {
        glichLastPosition = Glich.transform.position;
    }

    // this function will increase the alpha value slowly and reveal the outline of where the hand will slam
    public bool RevealAttack()
    {
        if (AttackSprite.color.a < 1)
        {
            var temp = AttackSprite.color;
            temp.a += 0.01f;
            AttackSprite.color = temp;
            return false;
        }
        else
        {
            return true;
        }
      
    }

}
