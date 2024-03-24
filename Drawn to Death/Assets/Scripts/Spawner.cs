using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Limit { None, Time, Unit, Either, Both}
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
    public bool active = true;

    private CooldownTimer attemptTimer;
    private CooldownTimer limitTimer;
    private int spawnCount = 0;
    private float totalChance = 0f;
    private bool triggerActive = true;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    // Start is called before the first frame update
    void Start()
    {
        limitTimer = new CooldownTimer(0f, timeLimit);
        attemptTimer = new CooldownTimer(timeBetweenAttempts, 0f);
        foreach(SpawnData data in spawnData)
        {
            totalChance += data.weight;
        }
        if (active)
        {
            ActivateSpawner();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            //Update Timers
            limitTimer.Update();
            attemptTimer.Update();

            //Make a spawn attempt
            if (attemptTimer.IsUseable())
            {
                AttemptSpawn();
                attemptTimer.StartTimer();
            }

            //Check time limit
            if (limitTimer.IsUseable() && (limit == Limit.Time || limit == Limit.Either))
            {
                active = false;
            }

            //Check unit limit
            if (spawnCount >= unitLimit && (limit == Limit.Unit || limit == Limit.Either))
            {
                active = false;
            }

            //Check time and unit limit
            if (limitTimer.IsUseable() && spawnCount >= unitLimit && limit == Limit.Both)
            {
                active = false;
            }
        }
    }

    public void AttemptSpawn()
    {
        float value = Random.Range(0f, 1f);
        if (spawnChance > 0f && value <= spawnChance)
        {
            GameObject enemy = GetRandomEnemy();
            if (enemy != null)
            {
                GameObject newEnemy = Instantiate(enemy, transform);
                newEnemy.transform.SetParent(enemyObjects.transform);
                newEnemy.transform.position += new Vector3(Random.Range(0f, spawnRadius), Random.Range(0f, spawnRadius), 0);
                ++spawnCount;
            }
        }
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
        if (triggerActive && collision.gameObject.tag == "Player")
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