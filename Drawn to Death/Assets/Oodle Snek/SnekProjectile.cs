using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnekProjectile : Projectile
{
    [Header("Advanced")]
    public AnimationCurve damageFalloff;
    public StatusEffect effect;
    [Min(0f)] public float lifespan = 1f;
    private StateTimer lifetimer;
    private float baseDamage = 0f;

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
    }

    private void EndOfLife()
    {
        Destroy(gameObject);
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
}
