using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalWall : MonoBehaviour
{
    private int killCount = 0;
    [Min(1)] public int requiredKills = 1;
    public Sprite destroyedSprite;

    private bool destroyed;
    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider2D;

    public GameObject explosion;

    [Header("FMOD Events")]
    public FMODUnity.EventReference cellWallBreakSFX;

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
    }
}