using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCrystal : MonoBehaviour
{

    public CooldownTimer invincibilityTimer;
    public float health;
    public float maxHealth;
    public EnemyHealthBarBehaviour healthBar;
    protected float invincibilityDuration = 20f / 60f;
    // Start is called before the first frame update

    private void Start()
    {
        healthBar.SetHealth(health, maxHealth);
        invincibilityTimer = new CooldownTimer(invincibilityDuration * 0.5f, invincibilityDuration * 0.5f);
    }
    // Update is called once per frame
    void Update()
    {
        invincibilityTimer.Update();
    }

    public void CrystalDamage(float damageTaken, bool makeInvincible = true)
    {
        health -= damageTaken;
        healthBar.SetHealth(health, maxHealth);

        if (makeInvincible)
        {
            invincibilityTimer.StartTimer();
    
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }

    }


}
