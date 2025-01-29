using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomAttributes;

public class SnekProjectile : Projectile
{
    [Header("Advanced")]
    public AnimationCurve damageFalloff;
    public StatusEffect effect;
    [Min(0f)] public float lifespan = 1f;
    private StateTimer lifetimer;
    private float baseDamage = 0f;

    [Header("Effects")]
    public bool grow = true;
    [ShowIf("grow", Relation.Equal, true)] public Vector3 maxScale; // Target scale at the end of lifespan

    public Color paintColor
    {
        get
        {
            return effect.paintColor;
        }
        set
        {
            throw new AccessViolationException("You are not allowed to change this value");
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        OodleSnek parent = transform.GetComponentInParent<OodleSnek>();
        effect = parent?.GetEffect();
        
        base.Start();

        baseDamage = damage;
        
        lifetimer = new StateTimer(new float[] { lifespan });
        lifetimer.Initialize();
        lifetimer.Start(EndOfLife);

        ApplyRotationWithFlip(MyUtils.LookAt2D(transform.position, target));

        StartCoroutine(Grow());

        allyCol = paintColor;
        selfImage.material.SetColor("_Color", paintColor);
        //selfImage.color = paintColor;

        GetComponent<Animator>().SetFloat("playbackSpeed", 9f / (lifespan * 60f));
    }

    private void EndOfLife()
    {
        Destroy(gameObject);
    }

    public void ApplyRotationWithFlip(Quaternion rotation)
    {
        // Extract the angle from the Quaternion
        Vector3 eulerAngles = rotation.eulerAngles;
        float angle = eulerAngles.z; // Z-axis rotation for 2D

        // Normalize the angle to the range [0, 360]
        angle = Mathf.Repeat(angle, 360f);

        // Check if the angle is between 90 and 270 degrees
        if (angle > 90f && angle < 270f)
        {
            // Flip the x scale
            Vector3 localScale = transform.localScale;
            if (localScale.x > 0)
            {
                // Ensure the scale is flipped
                localScale.x *= -1; 
                maxScale.x *= -1;
            }
                
            transform.localScale = localScale;

            // Adjust the angle to maintain upright appearance
            angle = Mathf.Repeat(angle + 180f, 360f);
        }
        else
        {
            // Ensure the x scale is positive if not flipping
            Vector3 localScale = transform.localScale;
            if (localScale.x < 0)
                localScale.x *= -1; // Reset the scale if previously flipped
            transform.localScale = localScale;
        }

        // Apply the adjusted rotation
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        damage = damage * damageFalloff.Evaluate(Mathf.Clamp01(lifetimer.timer / lifespan));

        base.OnTriggerEnter2D(collision);
        
        if (hit)
        {
            StatusEffectController effectController = collision.gameObject.GetComponent<StatusEffectController>();
            effectController?.AddStatusEffect(effect);
        }
        
    }

    private void OnDestroy()
    {
        lifetimer.Stop();
        lifetimer.Destroy();
    }

    private IEnumerator Grow()
    {
        if (grow)
        {
            Vector3 initialScale = transform.localScale; // Record the initial scale
            float elapsedTime = 0f;

            while (elapsedTime < lifespan)
            {
                elapsedTime += Time.deltaTime;

                // Calculate the progress as a percentage (0 to 1)
                float progress = elapsedTime / lifespan;

                // Interpolate the scale based on the progress
                transform.localScale = Vector3.Lerp(initialScale, maxScale, progress);

                yield return null; // Wait until the next frame
            }

            // Ensure final scale is set at the end
            transform.localScale = maxScale;
        }
    }
}