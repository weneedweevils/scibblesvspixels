using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAttack : MonoBehaviour
{
    private OodleKnight knight;

    // Start is called before the first frame update
    void Start()
    {
        knight = GetComponentInParent<OodleKnight>();
        if (knight == null) Debug.LogError("Error - " + gameObject.name + " is missing reference to an OodleKnight Object");
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        knight.DeferredOnTriggerStay2D(collision);
    }
}
