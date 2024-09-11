using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeManager : MonoBehaviour, IDataPersistence
{
    public static UpgradeManager instance { get; private set; }

    public bool loadLevels;

    [Header("Shop")]
    public int currency;
    public TextMeshProUGUI soulCounter;
    public Soul soulBlueprint;
    [Space(10)] public UpgradeMap[] upgrades;
    [Space(10)] [TextArea] public string defaultTextboxContent;
    

    [Header("UI")]
    public TMPro.TextMeshProUGUI currencyCounter;
    public TMPro.TextMeshProUGUI textbox;

    public void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Upgrade Manager in the scene");
        }
        instance = this;
    }

    public void Start()
    {
        if (!loadLevels)
        {
            Init();
        }
    }

    public void Update()
    {
        currencyCounter.text = string.Format("x{0}", currency);
        soulCounter.text = currency.ToString();
    }

    public void Init()
    {
        SetTextbox();

        for (int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i].option.Init(i);
        }
    }
    
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

    public void SetTextbox(string content = null)
    {
        if (content is null)
            textbox.text = defaultTextboxContent;
        else
            textbox.text = content;
    }

    public void LoadData(GameData data)
    {
        //toggle loading upgrade levels from saved game data
        if (loadLevels)
        {
            //Load currency
            currency = data.currency;

            //Ensure the length of the saved 'upgrade levels' list matches the number of upgrades
            if (data.upgradeLevels.Count != upgrades.Length) return;

            //Copy the saved levels from the data to each upgrade
            for (int i = 0; i < data.upgradeLevels.Count; i++)
            {
                upgrades[i].level = data.upgradeLevels[i];
            }
        }

        //Initialize the shop
        Init();

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
        data.currency = currency;
    }

    public void CreateSoul(Vector2 pos, int count, int value)
    {
        for (int i = 0; i < count; i++)
        {
            //Instantiate the soul
            Soul soul = Instantiate<Soul>(soulBlueprint, transform.parent);
            soul.transform.position = pos;
            soul.SetValue(value);
        }
    }
}

[System.Serializable]
public struct UpgradeMap
{
    public Upgrade upgrade;
    public UpgradeOption option;
    public int level;
}