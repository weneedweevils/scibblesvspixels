using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHitbox : MonoBehaviour
{
    public GameObject Glich;
    public Oodler oodlerScript;

    private Rigidbody2D glichRb;

    private PlayerMovement PlayerScript;

    public void Start()
    {
        PlayerScript = Glich.GetComponent<PlayerMovement>();
        glichRb = Glich.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log("Hit something");

    }
    
}
