using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MiniBoss : OodleKnight
{
    public GameObject bossHPBar;
    public TMPro.TextMeshProUGUI hpInfo;

    override protected void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (state == State.chase || state == State.attack)
        {
            bossHPBar.SetActive(true);
            hpInfo.text = string.Format("{0} / {1}", Mathf.CeilToInt(health), Mathf.CeilToInt(maxHealth));
        }
        else
        {
            bossHPBar.SetActive(false);
        }
    }

    override public void Stun()
    {
        //Mini boss is imune to stun
        return;
    }
}
