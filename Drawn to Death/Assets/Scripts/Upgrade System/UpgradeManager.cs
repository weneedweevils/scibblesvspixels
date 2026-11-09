using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using EasyButtons;

public class UpgradeManager : Singleton<UpgradeManager>, IDataPersistence
{
    public bool loadLevels;

    [Header("Shop")]
    public int currency;
    public TextMeshProUGUI soulCounter;
    public Soul soulBlueprint;
    [Space(10)] public UpgradeMap[] upgrades;
    [Space(10)][TextArea] public string defaultTextboxContent;


    [Header("UI")]
    public TMPro.TextMeshProUGUI currencyCounter;
    public TMPro.TextMeshProUGUI textbox;
    public TMP_FontAsset basicFont;
    public TMP_FontAsset fancyFont;
    public GameObject ShopUI;
    public Image screenFade;
    public float fadeDuration = 1f;
    public float fontSize;

    public void Update()
    {
        currencyCounter.text = currency.ToString();
        soulCounter.text = currency.ToString();
    }

    public void Init()
    {
        Debug.Log("Initializing UpgradeManager...");
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

        textbox.font = (DialogueManager.fancyFont ? fancyFont : basicFont);
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

    public void CreateSoul(Vector3 pos, int count, int value)
    {
        for (int i = 0; i < count; i++)
        {
            //Instantiate the soul
            Soul soul = Instantiate<Soul>(soulBlueprint, pos, Quaternion.identity );
            soul.SetValue(value);
        }
    }

    [Button]
    public static void OpenShop()
    {
        //Freeze movement
        PlayerMovement.instance.timelinePlaying = true;

        // Stop existing coroutines
        instance.StopAllCoroutines();

        // Change action map to UI
        CustomInput.instance.playerInput.SwitchCurrentActionMap("UI");

        // Start opening sequence
        instance.StartCoroutine(instance.FadeController
        (
            instance.fadeDuration,
            new Color(0, 0, 0, 1),
            () =>
            {
                // On complete -> activeate shop UI and revert the fade effect
                instance.ShopUI.SetActive(true);
                instance.StartCoroutine((instance.FadeController(instance.fadeDuration, new Color(0, 0, 0, 0))));
                
            }
         ));
    }

    /// <summary>
    /// Closes the shop UI with a smooth fade effect
    /// </summary>
    [Button]
    public static void CloseShop()
    {
        // Stop existing coroutines
        instance.StopAllCoroutines();

        // Change action map to player
        CustomInput.instance.playerInput.SwitchCurrentActionMap("Player");

        // Start closing sequence
        instance.StartCoroutine(instance.FadeController
        (
            instance.fadeDuration,
            new Color(0, 0, 0, 1),
            () =>
            {
                // On complete -> close shop UI and revert the fade effect and unfreeze movement
                instance.ShopUI.SetActive(false);
                instance.StartCoroutine((instance.FadeController
                (
                    instance.fadeDuration, 
                    new Color(0, 0, 0, 0), 
                    () => PlayerMovement.instance.timelinePlaying = false
                )));
                
            }
         ));
    }

    /// <summary>
    /// Control fade effect related to the shop
    /// </summary>
    private IEnumerator FadeController(float fadeDuration, Color endColor, Action callback = null)
    {
        // Save start color
        Color startColor = screenFade.color;
        float elapsedTime = 0f;

        // For the duration of the fade
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            // Interpolate between start->end colors proportional to ratio of elapsed time to total duration
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            screenFade.color = Color.Lerp(startColor, endColor, t);

            yield return null;
        }

        // Effect ended -> invoke the callback (optional)
        screenFade.color = endColor;
        callback?.Invoke();
    }
}

    [System.Serializable]
public struct UpgradeMap
{
    public Upgrade upgrade;
    public UpgradeOption option;
    public int level;
}