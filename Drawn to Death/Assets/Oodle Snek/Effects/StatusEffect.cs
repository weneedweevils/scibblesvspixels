using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomAttributes;

//[CreateAssetMenu(fileName = "Status Effect", menuName = "Status Effect")]
public abstract class StatusEffect : ScriptableObject
{
    public enum ResolutionMethod { Stack, Refresh, Override }

    [Header("Info")]
    public string effectName;
    [Min(0)] public int priority;
    public ResolutionMethod method = ResolutionMethod.Refresh;
    public Color paintColor;
    public bool particles = false;

    [Header("Timers")]
    [Min(0)] public float duration;
    [Min(0)] public float applicationInterval;

    public StateTimer durationTimer { get; private set; }
    public StateTimer applicationTimer { get; private set; }

    public Notifier<StatusEffect> applyEffectNotifier;
    public Notifier<StatusEffect> endEffectNotifier;

    protected StatusEffectController controller;

    // Initialization Function
    public virtual void Init(StatusEffectController controller)
    {
        // Apply the effect
        ApplyEffect(controller);

        // Setup timers
        if (duration > 0)
        {
            // Create Notifiers
            applyEffectNotifier = new Notifier<StatusEffect>();
            endEffectNotifier = new Notifier<StatusEffect>();

            // Create and Start Timers with the appropriate Callback Functions
            applicationTimer = new StateTimer(new float[] { applicationInterval });
            durationTimer = new StateTimer(new float[] { duration });
            
            if (applicationInterval > 0)
                applicationTimer.Start(ApplyEffectCallback);
            durationTimer.Start(EndEffectCallback);
        }
        else
        {
            EndEffect(controller);
            Destroy();
        }
    }
    public virtual void Destroy()
    {
        if (duration > 0)
        {
            // Destroy the timers
            applicationTimer.Destroy();
            durationTimer.Destroy();
        }
    }

    // Notifier Callback Functions
    protected virtual void ApplyEffectCallback()
    {
        // Notify Observers to apply the effect
        applyEffectNotifier.NotifyObservers(this);

        // Restart the application timer
        applicationTimer.Restart();
    }
    protected virtual void EndEffectCallback()
    {
        // Destroy this effect
        Destroy();

        // Notify Observers that the effect has ended
        endEffectNotifier.NotifyObservers(this);
    }

    // Abstract methods to implement specific effects
    public abstract void ApplyEffect(StatusEffectController controller);
    public abstract void EndEffect(StatusEffectController controller);
}