using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bouncyBones : MonoBehaviour
{
    Rigidbody2D rb;
    int i;
    float randx;
    float randy;
    //private List<float> xvalues = new List<float>();


    private void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       //xvalues.Add(-1000f);
       //xvalues.Add(1000f);
       i = 5;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.name == "Floor" && i>0)
        {
            randx = Random.Range(-1000f,1000f);
            randy = Random.Range(700f, 1000f);
            rb.AddForce(new Vector3(randx,randy,0));
            i--;
        }
       
    }
}
