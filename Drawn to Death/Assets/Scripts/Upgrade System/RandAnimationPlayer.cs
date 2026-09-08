using EasyButtons;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandAnimationPlayer : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float timeBetweenAnimations = 5f;

    [Space(10)][SerializeField] private List<VariantWeights> variants;

    private StateTimer timer;

    public void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();

        // Start a timer for when to play a random animation
        timer = new StateTimer(new float[] { timeBetweenAnimations });
        timer.Start(PlayRandomVariant);
    }

    // Activate a random animation variant
    public void PlayRandomVariant()
    {
        // Pick a random variant from the weighted list
        var item = MyUtils.WeightedRandomChoice(variants);
        PlayBackgroundVariant(item.variantID);

        // Restart the timer
        timer.Restart();
    }

    /// <summary>
    /// Play a specific background animation variant
    /// </summary>
    /// <param name="variantID"></param>
    [Button]
    public void PlayBackgroundVariant(int variantID)
    {
        animator?.SetInteger("Variant", variantID);
        animator?.SetTrigger("Play");
    }
}

[System.Serializable]
public class VariantWeights : MyUtils.IWeightedOption
{
    public int variantID;
    public float weight;
    
    float MyUtils.IWeightedOption.weight => weight;
}
       