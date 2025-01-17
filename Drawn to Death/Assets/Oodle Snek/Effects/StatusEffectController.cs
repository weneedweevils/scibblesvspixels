using System;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectController : MonoBehaviour
{
    public List<StatusEffect> effects = new List<StatusEffect>();

    public Observer<StatusEffect> applicationEffectObserver;
    public Observer<StatusEffect> endEffectObserver;

    public List<Attack> attacks { get; private set; }
    public EnemyAI enemyAI { get; private set; }
    public PlayerMovement player { get; private set; }
    public Attack playerAttack { get; private set; }

    public bool isPlayer { get; private set; } = false;

    // Start is called before the first frame update
    private void Awake()
    {
        // Get Component References
        attacks = new List<Attack>(GetComponents<Attack>());
        enemyAI = GetComponent<EnemyAI>();
        if (enemyAI == null)
        {
            player = GetComponent<PlayerMovement>();
            playerAttack = GetComponentInChildren<Attack>();

            if (player != null && playerAttack != null)
            {
                isPlayer = true;
            }
            else
            {
                throw new ArgumentNullException("Error - StatusEffectController could not find a reference to enemyAI nor player");
            }
        }

        // Initialize Observers
        applicationEffectObserver = new Observer<StatusEffect>(OnEffectApplied);
        endEffectObserver = new Observer<StatusEffect>(OnEffectEnded);
    }

    // Apply a status effect
    public void AddStatusEffect(StatusEffect effect)
    {
        if (effect == null) return;
        if (effect.duration == 0)
        {
            StackStatusEffect(effect);
            return;
        }

        // Find all effects of the same type
        List<StatusEffect> sameTypeEffects = effects.FindAll(e => e.GetType() == effect.GetType());

        // Case 1: If no effects of the same type exist, stack the new effect
        if (sameTypeEffects.Count == 0)
        {
            StackStatusEffect(effect);
            return;
        }

        // If effects of the same type already exist, handle conflict based on ResolutionMethod
        switch (effect.method)
        {
            case StatusEffect.ResolutionMethod.Stack:
                // Case 2.1: Stack the effect
                StackStatusEffect(effect);
                break;

            case StatusEffect.ResolutionMethod.Refresh:
                // Case 2.2: Refresh the effect with the highest priority
                StatusEffect highest = sameTypeEffects[0];
                foreach (StatusEffect e in sameTypeEffects)
                {
                    if (e.priority > highest.priority)
                        highest = e;
                }
                RefreshStatusEffect(highest);
                break;

            case StatusEffect.ResolutionMethod.Override:
                // Case 2.3: Override the effect with the lowest priority
                StatusEffect lowest = sameTypeEffects[0];
                foreach (StatusEffect e in sameTypeEffects)
                {
                    if (e.priority < lowest.priority)
                        lowest = e;
                }
                OverrideStatusEffect(lowest, effect);
                break;
        }
    }

    private void StackStatusEffect(StatusEffect effect)
    {
        // Clone and apply the new effect
        StatusEffect effectClone = Instantiate(effect);
        effectClone.Init(this);

        if (effectClone.duration > 0)
        {
            //Track the effect
            effects.Add(effectClone);

            // Start Observing the effect
            applicationEffectObserver.StartObservation(effectClone.applyEffectNotifier);
            endEffectObserver.StartObservation(effectClone.endEffectNotifier);
        }
    }
    private void RefreshStatusEffect(StatusEffect effect)
    {
        // Refresh the duration timer
        effect.durationTimer.Restart();
    }
    private void OverrideStatusEffect(StatusEffect oldEffect, StatusEffect newEeffect)
    {
        // Handle ending of the old effect
        OnEffectEnded(oldEffect);
        oldEffect.Destroy();

        // Apply the new effect
        StackStatusEffect(newEeffect);
    }

    // Observer Callbacks
    private void OnEffectApplied(StatusEffect effect)
    {
        // Apply the effect
        effect.ApplyEffect(this);
    }
    private void OnEffectEnded(StatusEffect effect)
    {
        // Remove the effect
        effect.EndEffect(this);
        effects.Remove(effect);

        // Stop observing the effect
        applicationEffectObserver.StopObservation(effect.applyEffectNotifier);
        endEffectObserver.StopObservation(effect.endEffectNotifier);
    }
}