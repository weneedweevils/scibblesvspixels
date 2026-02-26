using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleKill : MonoBehaviour { 

    CircleCollider2D circleCollider;
    RectTransform rectTransform;

    void  OnEnable()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        rectTransform = GetComponent<RectTransform>();
        circleCollider.enabled = false;
    }
    void FixedUpdate()
    {
        if (circleCollider.enabled)
        {
            circleCollider.radius = rectTransform.rect.width / 2f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            //collision.gameObject.GetComponent<EnemyAI>().InstantKill();
        }
    }
}
