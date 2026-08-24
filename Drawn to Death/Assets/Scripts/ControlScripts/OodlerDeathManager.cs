using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        oodler.OnDeath.AddListener(HandleOodlerDeath);
    }

    private void HandleOodlerDeath()
    {
        PlayerMovement.instance.SetTimelineActive(true);
        oodler.enabled = false;

        StartCoroutine(OodlerDeathCoroutine());
    }

    public IEnumerator OodlerDeathCoroutine()
    {
        int count = 0;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            MyUtils.SetAlpha(screenFade, alpha);

            if (elapsedTime >= count * explosionSpawnInterval)
            {
                Debug.Log("Spawning explosion at: " + oodler.transform.position + " at time: " + elapsedTime);
                Instantiate(explosionPrefab, oodler.transform.position, Quaternion.identity);
                count++;
            }
            yield return null;
        }

        MyUtils.SetAlpha(screenFade, 1f);
        yield return new WaitForSeconds(0.5f);
        menuManager.GotoScene();
    }
}
