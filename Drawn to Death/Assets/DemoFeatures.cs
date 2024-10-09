using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoFeatures : MonoBehaviour
{
    public DoodleBars bars;
    public MenuManager menu;

    // Update is called once per frame
    void Update()
    {
        if (bars.isDead())
        {
            menu.GotoScene();
        }
    }
}
