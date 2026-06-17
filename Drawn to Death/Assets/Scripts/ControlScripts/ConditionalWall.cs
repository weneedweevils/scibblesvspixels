using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConditionalWall : MonoBehaviour
{
    [SerializeField][Min(1)] private int requiredKills = 1;
    private int killCount = 0;
    [SerializeField] private EnemyAI[] activateEnemies;

    [Space(20)]
    [SerializeField] private Sprite destroyedSprite;
    [SerializeField] private GameObject explosion;

    [Space(20)]
    public UnityEvent onDestroyed = new UnityEvent();

    private bool destroyed;
    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider2D;

    [Header("FMOD Events")]
    [SerializeField] private FMODUnity.EventReference cellWallBreakSFX;

    public void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        destroyed = false;

        foreach (EnemyAI enemy in FindObjectsOfType<EnemyAI>())
        {
            enemy.onDeath += EnemyKilled;
        }

        foreach (Spawner spawner in FindObjectsOfType<Spawner>())
        {
            spawner.onEnemySpawn += ((enemy) => enemy.onDeath += EnemyKilled);
        }

        foreach (EnemyAI enemy in activateEnemies)
        {
            if (enemy != null)
                enemy.isolated = true;
        }
    }
    public void EnemyKilled(EnemyAI enemy){
        killCount++;
        enemy.onDeath -= EnemyKilled;
        if (!destroyed &&killCount >= requiredKills){
            ConditionReached();
            destroyed = true;
        }
    }

    public void ConditionReached(){
        spriteRenderer.sprite = destroyedSprite;
        polygonCollider2D.enabled = false;
        Instantiate(explosion, transform.position, Quaternion.identity);
        FMODUnity.RuntimeManager.PlayOneShot(cellWallBreakSFX, this.transform.position);

        foreach (EnemyAI enemy in activateEnemies)
        {
            if (enemy != null)
                enemy.isolated = false;
        }
    }
}