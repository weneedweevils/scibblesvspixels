using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownTimer
{
    //Timer
    public float cooldownDuration;
    public float activeduration;
    public float timer = 0f;

    //Cooldown
    private bool active = false;
    private bool onCooldown = false;

    public CooldownTimer(float _cooldown, float _duration)
    {
        cooldownDuration = _cooldown;
        activeduration = _duration;
    }

    public bool IsActive()
    {
        return active;
    }

    public bool IsOnCooldown()
    {
        return onCooldown;
    }

    public bool IsUseable()
    {
        return !(active || onCooldown);
    }

    public void Update()
    {
        //Update the timer if it is not usable 
        if (!IsUseable())
        {
            timer += Time.deltaTime;

            //Check if active state is over -> start the cooldown
            if (active && timer >= activeduration)
            {
                onCooldown = true;
                active = false;
                timer -= activeduration;
            }
            //Check if cooldown is over
            if (onCooldown && timer >= cooldownDuration)
            {
                onCooldown = false;
                timer = 0f;
            }
        }
    }

    public void StartTimer()
    {
        active = true;
    }
}
