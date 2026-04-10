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
    private bool faded = false;

    UnityEngine.UI.Image image;
    public static event Action OnDeathUiActive;
    private float fadeInDuration = 2f;
    private float fadeOutDuration = 5.7f;


    private Light2D[] sceneLights;
    private float[] initialIntensyArray;



    // Start is called before the first frame update
    void OnEnable()
    {
        PlayerMovement.OnPlayerDeath += StartDeathScreen;
        PlayerMovement.OnSelfReviveComplete += EndDeathScreen;

        image = panel.GetComponent<UnityEngine.UI.Image>();

    }

    private void OnDisable()
    {
        PlayerMovement.OnPlayerDeath -= StartDeathScreen;
        PlayerMovement.OnSelfReviveComplete -= EndDeathScreen;
    }

    // Update is called once per frame
    // This function is run when OnPlayer Death event activates from player movement
    private void StartDeathScreen()
    {

        Debug.Log("we started the deathscreen");
        //PlayerMovement.OnPlayerDeath -= StartDeathScreen;
        
        sceneLights = GetVisibleLights();
        dead = true;
        faded = false;
        StartCoroutine(DeathFade());
    }


    // This function is run when the OnSelfReviveComplete event activates when the players revive animation is finished
    private void EndDeathScreen()
    {
        //PlayerMovement.OnSelfReviveComplete -= EndDeathScreen;
    }


    // This function is run when the players selects the self-revive button
    public void OnSelfReviveClick()
    {
        reviving = true;
        dead = false;
        deathUIAnimator.SetBool("slide", true);
        PlayerMovement.instance.StartSelfRevive();
        StartCoroutine(ReviveFade());


    }



    


    private IEnumerator DeathFade()
    {
        yield return null;
        faded = true;
        float elapsed = 0f;

        while (elapsed < fadeInDuration)
        {
            elapsed += Time.unscaledDeltaTime;
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

        
        deathUIAnimator.SetBool("slide", false);
        OnDeathUiActive?.Invoke();
        deathUI.SetActive(true);
        yield return null;
    }

    private IEnumerator ReviveFade()
    {
        float elapsed = 0f;
        reviving = false;

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float x = Mathf.Clamp01(elapsed / fadeOutDuration);


            // Fade ind lights and screen
            for (int i = 0; i < sceneLights.Length; i++)
                sceneLights[i].intensity = Mathf.Lerp(0f, initialIntensyArray[i], x);

            var temp = image.color;
            temp.a = Mathf.Lerp(1f, 0f, x);
            image.color = temp;
            yield return null;

        }

       
        var temp2 = image.color;
        temp2.a = 0f;
        image.color = temp2;
        deathUI.SetActive(false);
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
