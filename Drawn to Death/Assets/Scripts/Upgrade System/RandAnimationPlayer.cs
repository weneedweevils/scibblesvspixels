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

        timer = new StateTimer(new float[] { timeBetweenAnimations });

        timer.Start(PlayRandomVariant);
    }

    public void PlayRandomVariant()
    {
        var item = MyUtils.WeightedRandomChoice(variants);
        PlayBackgroundVariant(item.variantID);
        timer.Restart();
    }

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
       