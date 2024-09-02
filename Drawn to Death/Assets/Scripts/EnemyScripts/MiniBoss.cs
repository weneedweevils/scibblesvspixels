using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
            if (state == State.chase || state == State.attack)
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
            if (boostTimer.IsUseable())
            {
                boostTimer.StartTimer();
            }
            else if (boostTimer.IsActive())
            {
                speed = baseSpeed * boostSpeedModifier;
            }
            else if (boostTimer.IsOnCooldown())
            {
                speed = baseSpeed;
            }
        }
    }

    override public void Stun()
    {
        //Mini boss is imune to stun
        return;
    }
}
