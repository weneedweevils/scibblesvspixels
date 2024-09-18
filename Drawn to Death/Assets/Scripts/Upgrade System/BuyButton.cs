using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public enum BuyState { Unavailable, Buyable, Purchased }
    public BuyState state;
    public Text soulCounter;
    public Button button;
    public Image image;
    public Color[] colors;
    private int price;
    private UpgradeOption parent;
    private string description;

    public void Init(BuyState _state, UpgradeOption _parent, int _price, string _description)
    {
        price = _price;
        parent = _parent;
        description = _description;
        soulCounter.text = string.Format("{0} Souls", price);
        SetBuyState(_state);
    }

    public void SetBuyState(BuyState _state)
    {
        state = _state;
        button.interactable = (state == BuyState.Buyable);
        image.color = colors[(int)state];
        if (state == BuyState.Purchased)
            soulCounter.text = "PURCHASED";
    }

    public void Buy()
    {
        UpgradeManager.instance.currency -= price;
        parent.UpgradeTier();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (state == BuyState.Buyable)
        {
            parent.displayDescription = false;
            UpgradeManager.instance.SetTextbox(description);
            FMODUnity.RuntimeManager.PlayOneShot("event:/UIHover");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        parent.displayDescription = true;
        UpgradeManager.instance.SetTextbox(parent.description);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (state == BuyState.Buyable && UpgradeManager.instance.currency >= price)
        {
            Buy();
            FMODUnity.RuntimeManager.PlayOneShot("event:/UIAccept");
        }
    }
}
