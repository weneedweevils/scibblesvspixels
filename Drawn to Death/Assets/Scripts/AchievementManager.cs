using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Profiling;

public class AchievementManager: Singleton<AchievementManager>, IDataPersistence
{
  public RunStats stats = new RunStats();
  private bool runActive;

    private void Update()
    {
        if (runActive)
        {




            // record stats
        }
    }

    // Start recording the stats
   public void StartNewRun()
    {
        stats.Reset();

    }


    // Stop recording stats
   public void StopRun()
    {
        // do something here
    }


    public void PauseRun()
    {
        
    }

    public void IncrementEnemieskilled(Enum enumtype)
    {
        var enemyType = enumtype.ToString().ToLower();
        stats.enemiesKilled[enemyType] += 1;
        foreach(KeyValuePair<string, int> entry in stats.enemiesKilled)
        {
            if(entry.Key == enemyType){
                Debug.Log(entry);
            }
        }
    }
    

    public void LoadData(GameData data)
    {

        stats.enemiesKilled["crab"] = data.gdCrabDeaths;
        stats.enemiesKilled["knight"] = data.gdKnightDeaths;
        stats.enemiesKilled["cubie"] = data.gdCubieDeaths;
        stats.enemiesKilled["snek"] = data.gdsnekDeaths;
        stats.enemiesKilled["hopper"] = data.gdHoppperDeaths;
        stats.enemiesKilled["bars"] = data.gdBarsDeaths;
        
    }

    public void SaveData(ref GameData data)
    {
        data.gdCrabDeaths=stats.enemiesKilled["crab"];
        data.gdKnightDeaths= stats.enemiesKilled["knight"];
        data.gdCubieDeaths=stats.enemiesKilled["cubie"];
        data.gdsnekDeaths =  stats.enemiesKilled["snek"];
        data.gdHoppperDeaths = stats.enemiesKilled["hopper"];
        data.gdBarsDeaths =  stats.enemiesKilled["bars"];
    }
}



[System.Serializable]
public class RunStats{

    public Dictionary<string, int> enemiesKilled= new Dictionary<String, int>()
    {
      ["crab"] = 0,
      ["cubie"] = 0,
      ["knight"] = 0,
      ["hopper"] = 0,
      ["bars"] = 0,
      ["snek"] = 0,
    };

    public float RunTime;
    public int alliesRevived;
    public float timeSurvived;
    public int upgrades_earned;

    public void Reset()
    {
       RunTime = 0f;
       alliesRevived = 0;
       timeSurvived = 0f;
       upgrades_earned = 0;
       enemiesKilled= new Dictionary<String, int>()
        {
        ["crab"] = 0,
        ["cubie"] = 0,
        ["knight"] = 0,
        ["hopper"] = 0,
        ["bars"] = 0,
        ["snek"] = 0,
        };
        

    }
}
