using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMusicScript : MonoBehaviour
{
    private FMOD.Studio.EventInstance instance;

    public FMODUnity.EventReference fmodEvent;

    [SerializeField][Range(0f, 30f)]
    private float intensity;

    [Space(10)]
    public bool autoUpdate = true;

    [SerializeField] [Range(0f, 30f)]
    private float intensityPerEnemy;

    // Start is called before the first frame update
    void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        instance.start();
        InvokeRepeating("UpdateIntensity", 0f, 2f); //Update the music intensity every 2 seconds
    }

    // Update is called once per frame
    void Update()
    {
        instance.setParameterByName("Intensity", intensity);
    }

    private void OnDestroy() {
        instance.stop(0);
    }

    public void setIntensity(float value)
    {
        intensity = Mathf.Clamp(value, 0f, 30f);
    }

    public float CalculateIntensity()
    {
        float total = 0f;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            EnemyAI enemy = obj.GetComponent<EnemyAI>();
            //Is this enemy on the oddle team and trying to attack
            if (enemy != null && enemy.team == Team.oddle && (enemy.state == State.chase || enemy.state == State.attack))
            {
                //Increment the intensity 
                total += intensityPerEnemy;
            }
        }

        //Clamp the total
        return Mathf.Clamp(total, 0f, 30f);
    }

    public void UpdateIntensity()
    {
        if (autoUpdate) { setIntensity(CalculateIntensity()); }
    }
}
