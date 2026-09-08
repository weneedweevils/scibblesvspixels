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

    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        destroyed = false;

        // Start listening to all enemies onDeath event
        foreach (EnemyAI enemy in FindObjectsOfType<EnemyAI>())
        {
            enemy.onDeath += EnemyKilled;
        }

        // Subscribe to all spawners to start listening for newly created enemies' onDeath event
        foreach (Spawner spawner in FindObjectsOfType<Spawner>())
        {
            spawner.onEnemySpawn += ((enemy) => enemy.onDeath += EnemyKilled);
        }

        // Ensure all associated enemies are marked as isolated
        foreach (EnemyAI enemy in activateEnemies)
        {
            if (enemy != null)
                enemy.isolated = true;
        }
    }
    
    /// <summary>
    /// Observer function for when an enemy is killed
    /// </summary>
    public void EnemyKilled(EnemyAI enemy)
    {
        // increment kill counter
        killCount++;

        // Remove listener
        enemy.onDeath -= EnemyKilled;

        // Check for activation condition
        if (!destroyed && killCount >= requiredKills)
        {
            ConditionReached();
            destroyed = true;
        }
    }

    /// <summary>
    /// Activation function for when the condition is reached
    /// </summary>
    public void ConditionReached()
    {
        // Change sprite and disable collider
        spriteRenderer.sprite = destroyedSprite;
        polygonCollider2D.enabled = false;

        // Spawn explosion
        Instantiate(explosion, transform.position, Quaternion.identity);

        // Play sfx
        FMODUnity.RuntimeManager.PlayOneShot(cellWallBreakSFX, this.transform.position);

        // Release isolated enemies
        foreach (EnemyAI enemy in activateEnemies)
        {
            if (enemy != null)
                enemy.isolated = false;
        }
    }
}