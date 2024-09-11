using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade : MonoBehaviour
{
    public int[] upgradeCosts;
    [TextArea] public string upgradeDescription;
    [TextArea] public string[] upgradeDescriptions;

    public abstract void ApplyUpgrade(int level);
}
