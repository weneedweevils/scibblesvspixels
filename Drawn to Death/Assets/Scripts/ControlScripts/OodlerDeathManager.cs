using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Dedicated class to handle oodlers death sequence
/// </summary>
public class OodlerDeathManager : Singleton<OodlerDeathManager>
{
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] float explosionSpawnInterval = 0.5f;
    [SerializeField] private Oodler oodler;
    [SerializeField] private Image screenFade;
    [SerializeField] private float fadeDuration;

    private void Start()
    {
        // Start listening for the oodler death event
        oodler.OnDeath.AddListener(HandleOodlerDeath);
    }

    /// <summary>
    /// Handle oodlers death event
    /// </summary>
    private void HandleOodlerDeath()
    {
        // Disable movement and the oodler script
        PlayerMovement.instance.SetTimelineActive(true);
        oodler.enabled = false;

        // Start the death sequence
        StartCoroutine(OodlerDeathCoroutine());
    }

    /// <summary>
    /// The oodler death sequence
    /// </summary>
    public IEnumerator OodlerDeathCoroutine()
    {
        int count = 0;
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            // Begin screen fade
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            MyUtils.SetAlpha(screenFade, alpha);

            // Spawn explosions according to the explosionSpawnInterval
            if (elapsedTime >= count * explosionSpawnInterval)
            {
                Instantiate(explosionPrefab, oodler.transform.position, Quaternion.identity);
                count++;
            }
            yield return null;
        }

        // Complete the screen fade
        MyUtils.SetAlpha(screenFade, 1f);

        // Hold fade for 0.5s
        yield return new WaitForSeconds(0.5f);

        // Goto ending scene
        menuManager.GotoScene();
    }
}
