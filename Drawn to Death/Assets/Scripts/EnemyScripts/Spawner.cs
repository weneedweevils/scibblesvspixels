using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Limit { None, Time, Unit, Either, Both}
public enum ActivationTrigger { None, Collider, Blocker}
public class Spawner : MonoBehaviour
{
    [Header("References")]
    public GameObject enemyObjects;
    public SpawnData[] spawnData;

    [Header("Spawn Stats")]
    [Range(0, 1)]
    public float spawnChance;
    [Min(0f)]
    public float timeBetweenAttempts;
    [Min(0f)]
    public float spawnRadius;

    [Header("Limits")]
    public Limit limit;
    public float timeLimit;
    public int unitLimit;
    public ActivationTrigger activationTrigger = ActivationTrigger.None;
    public EnemyAI[] blockers;
    [HideInInspector] public bool active = true;

    private CooldownTimer attemptTimer;
    private CooldownTimer limitTimer;
    private CooldownTimer animationTimer;
    private int spawnCount = 0;
    private float totalChance = 0f;
    private bool triggerActive = true;
    private PlayerMovement playerMovement;
    private GameObject spawnAnimation;
    private Vector3 spawnPosition;
    private GameObject enemy;
    private bool spawnTime;

    [Header("FMOD Events")]
    public FMODUnity.EventReference spawnerSFX;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        limitTimer = new CooldownTimer(0f, timeLimit);
        attemptTimer = new CooldownTimer(timeBetweenAttempts, 0f);
        animationTimer = new CooldownTimer(0f, 1.6f);
        spawnAnimation = transform.GetChild(0).gameObject;
        spawnAnimation.transform.position = transform.position;
        foreach(SpawnData data in spawnData)
        {
            totalChance += data.weight;
        }
        active = (  (activationTrigger == ActivationTrigger.None) || 
                    (activationTrigger == ActivationTrigger.Blocker && blockers.Length == 0));
        if (active)
        {
            ActivateSpawner();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Check Blockers
        if (triggerActive && activationTrigger == ActivationTrigger.Blocker)
        {
            foreach (EnemyAI blocker in blockers)
            {
                if (blocker.isDead())
                {
                    ActivateSpawner();
                    break;
                }
            }
        }

        if (active && !playerMovement.inFreezeDialogue() && !playerMovement.timelinePlaying)
        {
            //Update Timers
            limitTimer.Update();
            attemptTimer.Update();
            animationTimer.Update();

            //Make a spawn attempt
            if (attemptTimer.IsUseable() && AttemptSpawn() && !spawnTime)
            {
                enemy = GetRandomEnemy();
                if (enemy != null)
                {
                    spawnAnimation.transform.position = transform.position;
                    spawnPosition = new Vector3(Random.Range(0f, spawnRadius), Random.Range(0f, spawnRadius), 0);
                    Vector3 animationPosition = spawnPosition;
                    animationPosition.x -= 3.25f;
                    animationPosition.y += 2.3f;
                    spawnAnimation.transform.position += animationPosition;
                    spawnAnimation.SetActive(true);
                    FMODUnity.RuntimeManager.PlayOneShot(spawnerSFX, this.transform.position);
                    animationTimer.StartTimer();
                    spawnTime = true;
                }
                else
                {
                    attemptTimer.StartTimer();
                }
            }

            //Check time limit
            if (attemptTimer.IsUseable() && limitTimer.IsUseable() && (limit == Limit.Time || limit == Limit.Either))
            {
                active = false;
            }

            //Check unit limit
            if (spawnCount >= unitLimit && (limit == Limit.Unit || limit == Limit.Either))
            {
                active = false;
            }

            //Check time and unit limit
            if (attemptTimer.IsUseable() && limitTimer.IsUseable() && spawnCount >= unitLimit && limit == Limit.Both)
            {
                active = false;
            }

            // Disable animation and spawn enemy
            if (animationTimer.IsUseable() && spawnTime)
            {
                spawnAnimation.SetActive(false);
                GameObject newEnemy = Instantiate(enemy, transform);
                newEnemy.transform.SetParent(enemyObjects.transform);
                newEnemy.transform.position += spawnPosition;
                ++spawnCount;
                spawnTime = false;
                attemptTimer.StartTimer();
            }

            if (!active && !triggerActive)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public bool AttemptSpawn()
    {
        float value = Random.Range(0f, 1f);
        return spawnChance > 0f && value <= spawnChance;
    }

    public GameObject GetRandomEnemy()
    {
        float value = Random.Range(0f, totalChance);
        float total = 0f;
        foreach (SpawnData data in spawnData)
        {
            total += data.weight;
            if (value <= total)
            {
                return data.enemy;
            }
        }
        return null;
    }

    public void ActivateSpawner()
    {
        active = true;
        triggerActive = false;
        spawnCount = 0;
        limitTimer.StartTimer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggerActive && activationTrigger == ActivationTrigger.Collider && collision.gameObject.tag == "Player")
        {
            ActivateSpawner();
        }
    }
}

[System.Serializable]
public class SpawnData
{
    public GameObject enemy;
    public float weight;
}