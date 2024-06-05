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
   
    public GameObject oodler;
    
    public LineRenderer lineRenderer;

    private float timer = 0f;

    private bool inRange = false;

    private Oodler bossScript;
    // Start is called before the first frame update

    private void Start()
    {
        healthBar.SetHealth(health, maxHealth);
        invincibilityTimer = new CooldownTimer(invincibilityDuration * 0.5f, invincibilityDuration * 0.5f);
        bossScript = oodler.GetComponent<Oodler>();
        
    }
    // Update is called once per frame
    void Update()
    {
        invincibilityTimer.Update();
        DrawLine();

        if (inRange)
        {
            timer = timer + Time.deltaTime;
            if (timer>2f)
            {
                bossScript.heal(5f);
                timer = 0f;
            }

        }

        else
        {
            timer = 0f;
        }
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

    public void DrawLine()
    {
        if (Vector3.Distance(oodler.transform.position, transform.position)<100f)
        {
            inRange = true;
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, oodler.transform.position);
        }
        else
        {
            inRange = false;
            lineRenderer.enabled = false;
        }
    }


}
