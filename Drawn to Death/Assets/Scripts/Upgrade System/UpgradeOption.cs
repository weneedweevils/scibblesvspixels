using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeOption : MonoBehaviour
{
    public int id;
    public TMPro.TextMeshProUGUI titleRef;
    public Image iconRef;
    public string title;
    public Sprite icon;
    public BuyButton[] upgradeTier;

    public void Init(int _id)
    {
        //Set id to know which upgrade to reference in the UpgradeManager upgrade map
        id = _id;
        UpgradeMap upgrade = UpgradeManager.instance.upgrades[id];

        //Set Upgrade title and icon
        titleRef.text = title;
        if (icon != null) iconRef.sprite = icon;

        //Initialize all Buy Buttons
        for (int i = 0; i < upgradeTier.Length; i++)
        {
            //Select the correct buy button state
            BuyButton.BuyState state = BuyButton.BuyState.Unavailable;
            if (upgrade.level == i)
                state = BuyButton.BuyState.Buyable;
            if (upgrade.level > i)
                state = BuyButton.BuyState.Purchased;

            //Initialize Buy Button
            upgradeTier[i].Init(state, this, upgrade.upgrade.upgradeCosts[i], upgrade.upgrade.upgradeDescriptions[i]);
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
}
