using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionGenerator : MonoBehaviour
{
    [Header("Explosion")]
    [SerializeField] private Explosion explosionPrefab;
    [SerializeField] float explosionSpawnInterval = 0.5f;
    private List<Explosion> activeExplosions = new List<Explosion>();
    private bool isExploding = false;

    [Header("Camera Shake")]
    [SerializeField] private Camera camera;
    [SerializeField] private float shakeAmount = 0.5f;
    [SerializeField] private float damping = 0.5f;

    //TODO: add SFX event for explosions

    public void Update()
    {
        if (isExploding)
        {
            Vector2 shakeOffset = Random.insideUnitCircle * shakeAmount;
            camera.transform.localPosition = Vector3.Lerp(
                camera.transform.localPosition,
                new Vector3(shakeOffset.x, shakeOffset.y, camera.transform.localPosition.z),
                damping
             );
        }
        else
        {
            camera.transform.localPosition = new Vector3(0f, 0f, camera.transform.localPosition.z);
        }
    }

    public void StartSpawnExplosions(int count)
    {
        isExploding = true;
        StartCoroutine(SpawnExplosions(count));
    }

    public IEnumerator SpawnExplosions(int total)
    {
        int count = 0;
        float elapsedTime = 0f;

        while (count < total)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= count * explosionSpawnInterval)
            {
                Explosion explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity, transform);
                activeExplosions.Add(explosion);
                count++;
            }
            yield return null;
        }
    }

    public void KillExplosions()
    {
        StopAllCoroutines();

        foreach (var explosion in activeExplosions)
        {
            if (explosion != null)
            {
                Destroy(explosion.gameObject);
            }
        }
        isExploding = false;
    }
}
