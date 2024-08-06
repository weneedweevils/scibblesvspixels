using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour, IDataPersistence
{
    public bool loadLevels;
    [Space(10)]
    [SerializeField] private UpgradeMap[] upgrades;

    public void ApplyUpgrades()
    {
        //Apply the effect for each upgrade that exists
        foreach (UpgradeMap map in upgrades)
        {
            //Check to see if the upgrade is not null
            if (map.upgrade is null) continue;

            //Apply this upgrade's effect at the appropriate level
            map.upgrade.ApplyUpgrade(map.level);
        }
    }

    public void LoadData(GameData data)
    {
        //DEBUG OPTION: toggle loading upgrade levels from saved game data
        if (loadLevels)
        {
            //Ensure the length of the saved 'upgrade levels' list matches the number of upgrades
            if (data.upgradeLevels.Count != upgrades.Length) return;

            //Copy the saved levels from the data to each upgrade
            for (int i = 0; i < data.upgradeLevels.Count; i++)
            {
                upgrades[i].level = data.upgradeLevels[i];
            }
        }

        //Apply the upgrades
        ApplyUpgrades();
    }

    public void SaveData(ref GameData data)
    {
        List<int> upgradeLevels = new List<int>();
        foreach (UpgradeMap map in upgrades)
        {
            upgradeLevels.Add(map.level);
        }

        data.upgradeLevels = upgradeLevels;
    }
}

[System.Serializable]
public struct UpgradeMap
{
    public Upgrade upgrade;
    public int level;
}