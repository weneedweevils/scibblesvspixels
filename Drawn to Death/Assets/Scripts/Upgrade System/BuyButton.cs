using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public enum BuyState { Unavailable, Buyable, Purchased }
    public BuyState state;
    public TMPro.TextMeshProUGUI soulCounter;
    public Button button;
    public Animator animator;
    private int price;
    private UpgradeOption parent;
    private string description;
    public FMODUnity.EventReference upgradeSound;
    public FMODUnity.EventReference hoverSound;

    public void Awake()
    {
        SetBuyState(state);
    }

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

        switch (state)
        {
            case BuyState.Unavailable:
                animator.SetBool("Available", false);
                soulCounter.enabled = true;
                break;
            case BuyState.Buyable:
                animator.SetBool("Available", true);
                soulCounter.enabled = true;
                break;
            case BuyState.Purchased:
                animator.SetBool("Available", true);
                animator.SetBool("Hover", true);
                animator.SetBool("Buy", true);
                soulCounter.enabled = false;
                break;
        }
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
            FMODUnity.RuntimeManager.PlayOneShot(hoverSound);
            animator.SetBool("Hover", true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        parent.displayDescription = true;
        UpgradeManager.instance.SetTextbox(parent.description);
        if (state == BuyState.Buyable)
        {
            animator.SetBool("Hover", false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (state == BuyState.Buyable && UpgradeManager.instance.currency >= price)
        {
            Buy();
            FMODUnity.RuntimeManager.PlayOneShot(upgradeSound);
        }
    }
}
