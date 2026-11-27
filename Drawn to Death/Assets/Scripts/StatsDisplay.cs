using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsDisplay : MonoBehaviour
{

    private AchievementManager achievement;
    public TMP_Text DeathStat;
    // Start is called before the first frame update
    void Start()
    {
        achievement = AchievementManager.instance;
        DeathStat.text = "<color=green>Deaths: "+ achievement.stats.deaths+ "</color>";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
