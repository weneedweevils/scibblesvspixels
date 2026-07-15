using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Random Rotation")]
    [SerializeField] private float minAngle = 0f;
    [SerializeField] private float maxAngle = 360f;

    [Header("Random Scale")]
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 1.5f;

    [Header("Random Translation")]
    [SerializeField] private Vector2 minTranslation = Vector2.zero;
    [SerializeField] private Vector2 maxTranslation = Vector2.zero;

    [Header("Random Playback Speed")]
    [SerializeField] Animator animator;
    [SerializeField] private float playbackspeed = 1f;
    [SerializeField] private float playbackSpeedVariance = 0.2f;

    // Start is called before the first frame update
    void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();

        RandRot();
        RandScale();
        RandTranslation();
        RandPlaybackSpeed();
    }

    public void RandRot()
    {
        float randomAngle = Random.Range(minAngle, maxAngle);
        transform.rotation = Quaternion.Euler(0f, 0f, randomAngle);
    }

    public void RandScale()
    {
        float randomScale = Random.Range(minScale, maxScale);
        transform.localScale = new Vector3(randomScale, randomScale, 1f);
    }

    public void RandTranslation()
    {
        float randomX = Random.Range(minTranslation.x, maxTranslation.x);
        float randomY = Random.Range(minTranslation.y, maxTranslation.y);
        transform.position += new Vector3(randomX, randomY, 0f);
    }

    public void RandPlaybackSpeed()
    {
        float randomPlaybackSpeed = playbackspeed + Random.Range(-playbackSpeedVariance, playbackSpeedVariance);
        animator.speed = randomPlaybackSpeed;
    }
}
