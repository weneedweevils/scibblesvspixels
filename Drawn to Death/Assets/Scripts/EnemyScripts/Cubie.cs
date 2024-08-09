using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cubie : EnemyAI
{
    [Header("Cubie Specific References")]
    public GameObject ProjectileObject;
    public GameObject DarkProjectileObject;

    private bool createdProjectile = true;
    private float windupDuration = 40f / 60f;
    private CooldownTimer windupTimer;
    public bool cutscene = false;

    //Random walk direction
    private Vector2 direction = new Vector2(0, 0);
    private UnityEngine.SceneManagement.Scene currentScene;
    private string sceneName;

    override protected void Start()
    {
        //Override variables
        deathDuration = 40f / 60f;
        attackDuration = 60f / 60f;
        invincibilityDuration = 20f / 60f;

        //Create a windup timer
        windupTimer = new CooldownTimer(0f, windupDuration);

        // find scene
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        //Continue with the base class implementation of Start
        base.Start();
    }

    override protected void FixedUpdate()
    {
        //Continue with the base class implementation of FixedUpdate
        base.FixedUpdate();
        if (!playerMovement.inFreezeDialogue() && !playerMovement.timelinePlaying)
        {
            //Update the windup timer
            windupTimer.Update();

            if (cutscene)
            {
                cutscene = false;
                base.healthBar.SetHealth(base.health, base.maxHealth);
            }
        }
        else if (!cutscene)
        {
            cutscene = true;
            base.healthBar.Disable();
        }
        else
        {
            // Prevent projectiles from being fired if cutscene interupted attack
            createdProjectile = true;
        }
    }

    override protected void Attack()
    {
        //Start the attack
        if (target != null && attackTimer.IsUseable())
        {
            // play the attack sfx
            attackSFXInstance.start();
            
            createdProjectile = false;
            windupTimer.StartTimer();
            attackTimer.StartTimer();
            animator.SetBool("attacking", true);
            animator.SetBool("chasing", false);
            direction = new Vector2(Random.value - 0.5f, Random.value - 0.5f).normalized;
            rb.velocity = Vector2.zero;
        }

        if (PathLength() > attackDistance)
        {
            // Prevent projectiles from being fired if player goes out of range
            createdProjectile = true;
        }

        //End of windup -> Fire Projectile
        if (!createdProjectile && !windupTimer.IsActive())
        {
            createdProjectile = true;

            //Create a projectile

            if (sceneName == "Level 3" || sceneName == "Level 4"){
                Instantiate(DarkProjectileObject, transform);
                Debug.Log("dark projectile");
            }
            else{
                Debug.Log("light projectile");
                Instantiate(ProjectileObject, transform);
            }
        }

        if (attackTimer.IsOnCooldown())
        {
            animator.SetBool("attacking", false);
            animator.SetBool("chasing", true);

            //Apply a force in that direction
            Vector2 force = direction * speed / 2 * Time.deltaTime;
            rb.AddForce(force);
        }
    }

    override public void Stun()
    {
        windupTimer.ResetTimer();
        base.Stun();
    }
}
