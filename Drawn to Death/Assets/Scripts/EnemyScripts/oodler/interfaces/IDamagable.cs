using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public interface IDamagable 
{
    // Start is called before the first frame update
    void Damage(float damageTaken);
    void Die();

    float MaxHealth { get; set; }
    float CurrentHealth { get; set; }
    
}
