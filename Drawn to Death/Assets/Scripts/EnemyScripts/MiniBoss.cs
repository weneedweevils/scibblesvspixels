using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniBoss : OodleKnight
{
    public GameObject bossHPBar;
    public TMPro.TextMeshProUGUI hpInfo;
    public float boostSpeedModifier;
    public float boostDuration;
    public float boostCooldown;
    [HideInInspector] public float baseSpeed;
    public CooldownTimer boostTimer;

    override protected void Start()
    {
        base.Start();
        baseSpeed = speed;
        boostTimer = new CooldownTimer(boostCooldown, boostDuration);
    }

    override protected void FixedUpdate()
    {
        base.FixedUpdate();

        if (!playerMovement.inFreezeDialogue() && !playerMovement.timelinePlaying)
        {
            //Enable/Disable bossHPBar
            if (state == State.chase || state == State.attack || team == Team.player)
            {
                bossHPBar.SetActive(true);
                hpInfo.text = string.Format("{0} / {1}", Mathf.CeilToInt(health), Mathf.CeilToInt(maxHealth));
            }
            else
            {
                bossHPBar.SetActive(false);
            }

            //Boost Ability
            boostTimer.Update();
            if (boostTimer.IsUseable() && (state == State.chase || state == State.attack))
            {
                boostTimer.StartTimer();
            }
            if (boostTimer.IsActive())
            {
                speed = baseSpeed * boostSpeedModifier * (buffed ? playerMovement.allySpdModifier : 1f);
            }
            else if (boostTimer.IsOnCooldown())
            {
                speed = baseSpeed * (buffed ? playerMovement.allySpdModifier : 1f);
            }
        }
    }

    override public void Stun()
    {
        //Mini boss is imune to stun
        return;
    }

    public override bool Revive(float percentMaxHP = 1, float percentDamage = 1, float percentSpeed = 1, float percentAttkSpeed = 1)
    {
        if (base.Revive(1f, 1f, 1f, 1f))
        {
            //Override base.Revive Set Stats
            maxHealth = 150;
            health = maxHealth;
            damage = 30;
            speed = 12000;
            baseSpeed = 12000;
            attackCooldown = 2f;
            attackTimer.SetCooldown(attackCooldown);

            //Fix HP bar
            healthBar.SetHealth(health, maxHealth);

            //Adjust the scale
            StartCoroutine(AdjustScale(new Vector3(1.4f, 1.4f, 1f), reviveDuration));
            
            //Adjust attack distance to account for the new scale
            attackDistance = 16f;

            //Nerf boost ability
            boostCooldown = 5f;
            boostDuration = 0.4f;
            boostSpeedModifier = 6f;

            boostTimer.SetCooldown(boostCooldown);
            boostTimer.SetDuration(boostDuration);

            return true;
        }

        return false;
    }

    public override void Kill()
    {
        base.Kill();
        if (team == Team.player)
        {
            Destroy(bossHPBar);
        }
    }

    public IEnumerator AdjustScale(Vector3 newScale, float duration)
    {
        Vector3 initialScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Interpolate between the initial and new scale based on time
            transform.localScale = Vector3.Lerp(initialScale, newScale, elapsedTime / duration);

            // Increase elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the final scale is exactly the target scale
        transform.localScale = newScale;
    }
}