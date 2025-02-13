using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniBoss : OodleKnight
{
    [Header("Mini-Boss")]
    public GameObject bossHPBar;
    public TMPro.TextMeshProUGUI hpInfo;
    public SpeedBoost boostEffect;
    public SpeedBoost allyBoostEffect;
    public float boostCooldown;
    [HideInInspector] public float baseSpeed;
    public CooldownTimer boostTimer;
    public HealthBar secondHpBar;

    override protected void Start()
    {
        base.Start();
        baseSpeed = speed.baseValue;
        boostTimer = new CooldownTimer(boostCooldown, boostEffect.duration);
    }

    override protected void FixedUpdate()
    {
        base.FixedUpdate();

        if (!playerMovement.inFreezeDialogue() && !playerMovement.timelinePlaying)
        {
            //Enable/Disable bossHPBar
            if (bossHPBar != null)
            {
                if (state == State.chase || state == State.attack)
                {
                    bossHPBar.SetActive(true);
                    hpInfo.text = string.Format("{0} / {1}", Mathf.CeilToInt(health), Mathf.CeilToInt(maxHealth));
                }
                else
                {
                    bossHPBar.SetActive(false);
                }
            }

            //Boost Ability
            boostTimer.Update();
            if (boostTimer.IsUseable() && (state == State.chase || state == State.attack))
            {
                boostTimer.StartTimer();
                effectController.AddStatusEffect(team == Team.oddle ? boostEffect : allyBoostEffect);
            }
        }
    }

    override public void Stun()
    {
        //Mini boss is immune to stun
        return;
    }

    public override bool Revive(float percentMaxHP = 1, float percentDamage = 1, float percentSpeed = 1, float percentAttkSpeed = 1)
    {
        if (base.Revive(1f, 1f, 1f, 1f))
        {
            //Override base.Revive Set Stats
            maxHealth = 150;
            health = maxHealth;
            damage.baseValue = 30;
            speed.baseValue = 12000;
            baseSpeed = 12000;
            attackCooldown.baseValue = 2f;
            attackTimer.SetCooldown(attackCooldown.value);

            //Fix HP bar
            healthBar = secondHpBar;
            secondHpBar.gameObject.SetActive(true);
            healthBar.SetHealth(health, maxHealth);

            //Adjust the scale
            StartCoroutine(AdjustScale(new Vector3(1.4f, 1.4f, 1f), reviveDuration));
            
            //Adjust attack distance to account for the new scale
            attackDistance = 16f;

            //Nerf boost ability
            boostCooldown = 5f;
            boostTimer.SetCooldown(boostCooldown);
            boostTimer.SetDuration(allyBoostEffect.duration);

            return true;
        }

        return false;
    }

    public override void Kill()
    {
        base.Kill();
        if (bossHPBar != null) Destroy(bossHPBar);
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