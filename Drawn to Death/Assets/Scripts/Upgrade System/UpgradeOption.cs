using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int id;
    public TMPro.TextMeshProUGUI titleRef;
    public Image iconRef;
    public string title;
    public Sprite icon;
    public BuyButton[] upgradeTier;
    public bool displayDescription = true;
    public string description { get; private set; }
    private Image background;

    public void Init(int _id)
    {
        background = GetComponent<Image>();

        //Set id to know which upgrade to reference in the UpgradeManager upgrade map
        id = _id;
        UpgradeMap upgradeMap = UpgradeManager.instance.upgrades[id];

        //Set Upgrade title and icon
        titleRef.text = title;
        if (icon != null) iconRef.sprite = icon;
        description = upgradeMap.upgrade.upgradeDescription;

        //Initialize all Buy Buttons
        for (int i = 0; i < upgradeTier.Length; i++)
        {
            //Select the correct buy button state
            BuyButton.BuyState state = BuyButton.BuyState.Unavailable;
            if (upgradeMap.level == i)
                state = BuyButton.BuyState.Buyable;
            if (upgradeMap.level > i)
                state = BuyButton.BuyState.Purchased;

            //Initialize Buy Button
            upgradeTier[i].Init(state, this, upgradeMap.upgrade.upgradeCosts[i], upgradeMap.upgrade.upgradeDescriptions[i]);
        }
    }

    public void UpgradeTier()
    {
        UpgradeManager.instance.upgrades[id].level++;
        UpgradeMap upgrade = UpgradeManager.instance.upgrades[id];

        for (int i = 0; i < upgradeTier.Length; i++)
        {
            BuyButton.BuyState state = BuyButton.BuyState.Unavailable;
            if (upgrade.level == i)
                state = BuyButton.BuyState.Buyable;
            if (upgrade.level > i)
                state = BuyButton.BuyState.Purchased;

            upgradeTier[i].SetBuyState(state);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MyUtils.SetAlpha(background, 0.2f);

        if (displayDescription)
            UpgradeManager.instance.SetTextbox(description);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MyUtils.SetAlpha(background, 0f);
        UpgradeManager.instance.SetTextbox();
    }
}
