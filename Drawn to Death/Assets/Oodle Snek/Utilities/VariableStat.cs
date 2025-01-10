using System;
using UnityEngine;

[Serializable]
public class VariableStat
{
    [Tooltip("The base value before any modifications.")]
    public float baseValue = 0f;

    [Tooltip("Multiplier applied to the base value (e.g., 2 for double).")]
    public float multiplier = 1f;

    [Tooltip("Increase to the base value before applying the multiplier.")]
    public float baseIncrease = 0f;

    [Tooltip("Increase to the final value after applying the multiplier.")]
    public float flatIncrease = 0f;

    [Tooltip("Minimum value for the final result after modifiers.")]
    public float minValue = float.MinValue;

    [Tooltip("Maximum value for the final result after modifiers.")]
    public float maxValue = float.MaxValue;

    /// <summary>
    /// The final value after applying all modifiers.
    /// </summary>
    public float value
    {
        get
        {
            // Step 1: Add pre-multiplication increase
            float modifiedValue = baseValue + baseIncrease;

            // Step 2: Apply multiplier
            modifiedValue *= multiplier;

            // Step 3: Add post-multiplication increase
            modifiedValue += flatIncrease;

            return Mathf.Clamp(modifiedValue, minValue, maxValue);
        }
    }
}