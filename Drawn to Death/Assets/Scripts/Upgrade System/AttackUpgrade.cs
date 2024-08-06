using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUpgrade : Upgrade
{
    [Header("Per Level")]
    public float damageIncrease;
    public float knockbackIncrease;

    public override void ApplyUpgrade(int level)
    {
        //Collect the eraser attack reference
        Attack eraser = FindObjectOfType<Attack>();

        //Increase attack damage
        eraser.damage += damageIncrease * level;

        //Increase knockback force
        eraser.knockback += knockbackIncrease * level;
    }
}
