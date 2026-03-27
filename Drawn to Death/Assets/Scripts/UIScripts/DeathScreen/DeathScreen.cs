using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DeathScreen : MonoBehaviour
{

    public GameObject panel;
    public GameObject lights;
    public GameObject deathUI;
    public Animator deathUIAnimator;
    private bool dead = false;
    private bool reviving = false;
    UnityEngine.UI.Image image;
    private float timer = 0f;
    public static event Action OnDeathUiActive;
    private float fadeInDuration = 2f;
    private float fadeOutDuration = 5.7f;
    private bool startedDeath = false;
    private bool startedRevive = false;


    private Light2D[] sceneLights;
    private float[] initialIntensyArray;

    //private UnityEngine.UI.Image image = panel.GetComponent<UnityEngine.UI.Image>();


    // Start is called before the first frame update
    void OnEnable()
    {
        PlayerMovement.OnPlayerDeath += StartDeathScreen;
        
        image = panel.GetComponent<UnityEngine.UI.Image>();
        //deathUIAnimator = deathUI.GetComponent<Animator>();

    }

    private void OnDisable()
    {
        PlayerMovement.OnPlayerDeath -= StartDeathScreen;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            if (!startedDeath) {
                StartCoroutine(DeathFade());
            }
        }
        else if(reviving && !dead)
        {
            StartCoroutine(ReviveFade());
        }

        
    }


    // This function is run when OnPlayer Death event activates from player movement
    private void StartDeathScreen()
    {

        Debug.Log("we started the deathscreen");
        //PlayerMovement.OnPlayerDeath -= StartDeathScreen;
        PlayerMovement.OnSelfReviveComplete += EndDeathScreen;
        sceneLights = GetVisibleLights();
        dead = true;
        startedDeath = false;
    }


    // This function is run when the OnSelfReviveComplete event activates when the players revive animation is finished
    private void EndDeathScreen()
    {
        PlayerMovement.OnPlayerDeath += StartDeathScreen;
        //PlayerMovement.OnSelfReviveComplete -= EndDeathScreen;
    }


    // This function is run when the players selects the self-revive button
    public void OnSelfReviveClick()
    {
        reviving = true;
        deathUIAnimator.SetBool("slide", true);
        PlayerMovement.instance.StartSelfRevive();
        dead = false;

    }



    


    private IEnumerator DeathFade()
    {
        startedDeath = true;
        float elapsed = 0f;

        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            float x = Mathf.Clamp01(elapsed / fadeInDuration);

            
            // Fade out lights and fade in black screen
            for (int i = 0; i < sceneLights.Length; i++)
                sceneLights[i].intensity = Mathf.Lerp(initialIntensyArray[i], 0f, x);

            var temp = image.color;
            temp.a = Mathf.Lerp(0f, 1f, x);
            image.color = temp;

            yield return null;
        }

        
        foreach (var light in sceneLights)
            light.intensity = 0f;

        var temp2 = image.color;
        temp2.a = 1f;
        image.color = temp2;

        deathUI.SetActive(true);
        deathUIAnimator.SetBool("slide", false);
        OnDeathUiActive?.Invoke();
        yield return null;
    }

    private IEnumerator ReviveFade()
    {
        float elapsed = 0f;

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float x = Mathf.Clamp01(elapsed / fadeOutDuration);


            // Fade ind lights and screen
            for (int i = 0; i < sceneLights.Length; i++)
                sceneLights[i].intensity = Mathf.Lerp(0f, initialIntensyArray[i], x);

            var temp = image.color;
            temp.a = Mathf.Lerp(1f, 0f, x);
            image.color = temp;

            yield return null;
        }

        deathUI.SetActive(false);
        var temp2 = image.color;
        temp2.a = 0f;
        image.color = temp2;
        reviving = false;
        yield return null;
    }

    private Light2D[] GetVisibleLights()
    {
        Camera cam = Camera.main;
        Light2D[] lightsInLevel = FindObjectsOfType<Light2D>();

        List<Light2D> visibleLights = new List<Light2D>();
        List<float> visibleIntensities = new List<float>();

        foreach (Light2D light in lightsInLevel)
        {
            Vector2 viewportPos = cam.WorldToViewportPoint(light.transform.position);

            bool inView = viewportPos.x >= -0.2f && viewportPos.x <= 1.2f &&
                          viewportPos.y >= -0.2f && viewportPos.y <= 1.2f;

            if (inView)
            {
                visibleLights.Add(light);
                visibleIntensities.Add(light.intensity);
            }
        }
        initialIntensyArray = visibleIntensities.ToArray();

        return visibleLights.ToArray();
    }
}
