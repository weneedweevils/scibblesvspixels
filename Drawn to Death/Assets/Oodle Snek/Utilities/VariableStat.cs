using System;
using UnityEngine;

[Serializable]
public class VariableStat
{
    public enum Sign { Neutral, Positive, Zero, Negative }

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

    [Tooltip("What sign the output value should be. Note: Final output is still subjected to the min-max clamp.\n" +
        "Neutral: Do not fix the Output\n" +
        "Positive: Fix the Output to Positive\n" +
        "Zero: Fix the Output to zero\n" +
        "Negative: Fix the Output to Negative")]
    public Sign setSign = Sign.Neutral;

    [Tooltip("Maximum value for the final result after modifiers.")]
    public float maxValue = float.MaxValue;

    /// <summary>
    /// The final value after applying all modifiers.
    /// </summary>
    public float value
    {
        get
        {
            return Calculate(baseValue);
        }
    }

    /// <summary>
    /// Define all variables for this object
    /// </summary>
    public void Set(float baseValue, float multiplier, float baseIncrease, float flatIncrease, float minValue, float maxValue)
    {
        this.baseValue = baseValue;
        this.multiplier = multiplier;
        this.baseIncrease = baseIncrease;
        this.flatIncrease = flatIncrease;
        this.minValue = minValue;
        this.maxValue = maxValue;
    }

    /// <summary>
    /// Calculate the result of applying all modifiers to the baseValue
    /// </summary>
    /// <param name="baseValue">The base value the modifiers will be applied to</param>
    public float Calculate(float baseValue)
    {
        // Step 1: Add pre-multiplication increase
        float modifiedValue = baseValue + baseIncrease;

        // Step 2: Apply multiplier
        modifiedValue *= multiplier;

        // Step 3: Add post-multiplication increase
        modifiedValue += flatIncrease;

        // Step 4: Apply setSign
        switch (setSign)
        {
            case Sign.Neutral:
                break;
            case Sign.Positive:
                modifiedValue = Mathf.Abs(modifiedValue);
                break;
            case Sign.Zero:
                modifiedValue = 0;
                break;
            case Sign.Negative:
                modifiedValue = -Mathf.Abs(modifiedValue);
                break;
            default:
                throw new ArgumentException(string.Format("Error - Unexpected setSign value of {0}", setSign));
        }

        // Step 5: Return clamped output
        return Mathf.Clamp(modifiedValue, minValue, maxValue);
    }
}