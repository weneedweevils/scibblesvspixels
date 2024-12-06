using UnityEngine;

public class BossTimer{

    private float totalTime;
    private float timer = 0f;
    private bool timerReached = false;

    public BossTimer(float totalTime){
        this.totalTime = totalTime;
        timer = 0f;
    }

    public void ResetTimer(){
        timer = 0f;
        timerReached = false;

    }

    public void ChangeMaxTime(float totalTime){
        this.totalTime = totalTime;
    }

    public bool Update(){

        if(!timerReached){
            timer = timer + Time.deltaTime;
            if(timer>totalTime){
                timerReached = true;
                return true;
            }
            else{
                return false;
            }
        }
        else{
            return true;

        }
    }

}