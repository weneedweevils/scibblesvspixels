using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalWall : MonoBehaviour
{
    public EnemyAI[] enemies;
    public Sprite destroyedSprite;
    
    private bool allDead;
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
    }
    public void Update(){
        allDead = true;

        if (!destroyed){
            // Check if all enemies are dead
            foreach (EnemyAI enemy in enemies){
                if (!(enemy.isDead() || enemy.team == 0)){
                    allDead = false;
                    break;
                }
            }

            if (allDead){
                ConditionReached();
                destroyed = true;
            }
        }
    }

    public void ConditionReached(){
        spriteRenderer.sprite = destroyedSprite;
        polygonCollider2D.enabled = false;
        Instantiate(explosion, transform.position, Quaternion.identity);
        FMODUnity.RuntimeManager.PlayOneShot(cellWallBreakSFX, this.transform.position);
    }
}