using Radishmouse;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script will make a circle radiate from glich upon a self revive killing enemies
/// </summary>
public class Killcircle : MonoBehaviour
{
    private Image killCircle;
    private CircleCollider2D collider;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float startingRadius = 1f;
    private float radius;
    private bool handUp = false;
    private float offset = 400f;
    private UILineRenderer uiLineRenderer;
    public int segments = 100;
    public float step = 100f;
    private float yOffSet = 6f;
    private float xOffSet = -0.5f;
    private float colorFallOff = 25f;
    private float maxRadius = 45f;




    // Start is called before the first frame update
    void Start()
    {
        radius = startingRadius;
        killCircle = GetComponent<Image>();
        collider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        uiLineRenderer = GetComponent<UILineRenderer>();
        PlayerMovement.OnSelfRevive += ChangeHand;
        collider.enabled = false;
        collider.offset= new Vector2(xOffSet,yOffSet);
        



    }


    // Update is called once per frame
    void Update()
    {

        if (handUp && radius < maxRadius)
        {
            radius = radius + Time.deltaTime * step;
            GenerateKillCircle(radius);

            if (radius > colorFallOff)
            {
                var color = uiLineRenderer.color;
                Debug.Log(color.a);
                color.a = (1-((radius-colorFallOff)/(maxRadius - colorFallOff)));
                uiLineRenderer.color = color;
            }
        }
        else if(handUp && radius >= maxRadius){
            uiLineRenderer.points = new Vector2[] { new Vector2(0, 0) };
            collider.enabled = false;
            uiLineRenderer.SetVerticesDirty();
            handUp = false;
        }
        
     
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Debug.Log("enemy encountered");
            EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
            if (enemy.GetComponent<DoodleBars>() == null)
            {
                enemy.Destroy();
            }
        }
    }

    // parts of code borrowed from this video https://www.youtube.com/watch?v=HU8UZFaUImQ
    private void GenerateKillCircle(float radius)
    {
        
        collider.radius = radius;
        float angleStep = 2f * Mathf.PI / segments;
        var list = new List<Vector2>();

        for (int i = 0; i < segments; i++)
        {
            float xposition = radius * Mathf.Cos(angleStep * i) + xOffSet;
            float yposition = radius * Mathf.Sin(angleStep * i) + yOffSet;
            Vector2 point = new Vector2(xposition, yposition);
            list.Add(point);
        }

        uiLineRenderer.points = list.ToArray();
        uiLineRenderer.SetVerticesDirty();
    }


    // This is called during a animation event that is triggered when the hand goes up in the self revive animation
    private void ChangeHand()
    {
        collider.enabled = true;
        handUp = true;
        PlayerMovement.OnSelfRevive -= ChangeHand;
    }
}
