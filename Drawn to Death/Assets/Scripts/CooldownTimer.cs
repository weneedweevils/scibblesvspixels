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
    public bool active = false;
    public bool onCooldown = false;

    //Misc
    private CooldownBarBehaviour cooldownBar = null;
    private CooldownTimer other = null;

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

    public void Update(float timeModifier = 1f)
    {
        //Update the timer if it is not usable 
        if (!IsUseable())
        {
            timer += Time.deltaTime * timeModifier;

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

        UpdateCooldownBar();

        if (other != null)
        {
            other.timer = timer;
            other.active = active;
            other.onCooldown = onCooldown;
            other.UpdateCooldownBar();
        }
    }

    public void StartTimer(float startTime = 0f)
    {
        active = true;
        onCooldown = false;
        timer = startTime;
    }

    public void StartCooldown(float startTime = 0f)
    {
        active = false;
        onCooldown = true;
        timer = startTime;
    }

    public void ResetTimer()
    {
        active = false;
        onCooldown = false;
        timer = 0f;
    }

    public void SetCooldown(float cooldownValue)
    {
        cooldownDuration = cooldownValue;
    }

    public void SetDuration(float durationValue)
    {
        activeduration = durationValue;
    }

    public void Connect(CooldownBarBehaviour bar)
    {
        cooldownBar = bar;
    }

    public void Couple(CooldownTimer timer)
    {
        other = timer;
    }

    public void UpdateCooldownBar()
    {
        if (cooldownBar != null)
        {
            if (active)
            {
                cooldownBar.SetBar(0f);
            }
            else if (onCooldown)
            {
                cooldownBar.SetBar(timer);
            } 
            else
            {
                cooldownBar.SetBar(cooldownBar.maxValue);
            }
        }
    }
}
