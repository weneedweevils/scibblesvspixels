using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMusicScript : MonoBehaviour
{
    public static BasicMusicScript instance;
    private FMOD.Studio.EventInstance eventInstance;

    public FMODUnity.EventReference fmodEvent;

    [SerializeField][Range(0f, 30f)]
    private float intensity;

    [Space(10)]
    public bool autoUpdate = true;

    [SerializeField] [Range(0f, 30f)]
    private float intensityPerEnemy;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Basic Music Script in the scene");
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        eventInstance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        eventInstance.start();
        InvokeRepeating("UpdateIntensity", 0f, 2f); //Update the music intensity every 2 seconds
    }

    // Update is called once per frame
    void Update()
    {
        eventInstance.setParameterByName("Intensity", intensity);
    }

    private void OnDestroy() {
        eventInstance.stop(0);
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
            if (enemy != null && !(enemy is DoodleBars) && enemy.team == Team.oddle && (enemy.state == State.chase || enemy.state == State.attack))
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

    public float GetIntensity()
    {
        return intensity;
    }
}
