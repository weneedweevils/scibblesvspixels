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
    public float radius;
    private bool handUp = false;
    private float offset = 400f;
    private UILineRenderer uiLineRenderer;
    public int segments = 50;
    public float step = 100f;
    private float yOffSet = 6f;
    private float xOffSet = -0.5f;
    private float colorFallOff = 25f;
    private float maxRadius = 45f;




    // Start is called before the first frame update
    void Start()
    {
        killCircle = GetComponent<Image>();
        collider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        uiLineRenderer = GetComponent<UILineRenderer>();
        collider.offset= new Vector2(xOffSet,yOffSet);
        collider.enabled = false;




    }

    private void OnEnable()
    {
       
        PlayerMovement.SelfReviveHandUp += ChangeHand;
    }

    private void OnDisable()
    {
        
        PlayerMovement.SelfReviveHandUp -= ChangeHand;
    }


    // Update is called once per frame
    void Update()
    {

        //if (handUp && radius < maxRadius)
        //{
        //    radius = radius + Time.deltaTime * step;
        //    GenerateKillCircle(radius);

        //    if (radius > colorFallOff)
        //    {
        //        var color = uiLineRenderer.color;
        //        Debug.Log(color.a);
        //        color.a = (1-((radius-colorFallOff)/(maxRadius - colorFallOff)));
        //        uiLineRenderer.color = color;
        //    }
        //}
        //else if(handUp && radius >= maxRadius){
        //    uiLineRenderer.points = new Vector2[] { new Vector2(0, 0) };

        //    collider.enabled = false;
        //    uiLineRenderer.SetVerticesDirty();
        //    handUp = false;
        //}
        
     
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

    private IEnumerator StartKillCircle()
    {
        while(radius < maxRadius)
        {
            
            radius = Mathf.Clamp(radius + Time.deltaTime * step,1f,maxRadius);
            Debug.Log("drawing kill circle, current radius is "+radius);
            collider.radius = radius;
            float angleStep = 2f * Mathf.PI / segments;
            var list = new List<Vector2>();
            for (int i = 0; i < segments; i++)
            {
                float xposition = radius * Mathf.Cos(angleStep * i) + xOffSet;
                float yposition = radius * Mathf.Sin(angleStep * i) + yOffSet;
                Vector2 point = new Vector2(xposition, yposition);
                list.Add(point);
                Debug.Log("rendering the point");
            }
           
            uiLineRenderer.points = list.ToArray();
            uiLineRenderer.SetVerticesDirty();
            if (radius > colorFallOff)
            {
                var color = uiLineRenderer.color;
                color.a = (1 - ((radius - colorFallOff) / (maxRadius - colorFallOff)));
                uiLineRenderer.color = color;
            }
            yield return null;
        }

        uiLineRenderer.points = new Vector2[] { new Vector2(0, 0) };
        collider.enabled = false;
        radius = startingRadius;
        collider.radius = radius;
     
        uiLineRenderer.SetVerticesDirty();
        handUp = false;
        yield return null;


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
        Debug.Log("rendering the line: ");
        uiLineRenderer.points = list.ToArray();
        uiLineRenderer.SetVerticesDirty();
    }


    // This is called during a animation event that is triggered when the hand goes up in the self revive animation
    private void ChangeHand()
    {
        Debug.Log("hand went up");
        //if (handUp) return;
        StartCoroutine(StartKillCircle());
        collider.enabled = true;
        handUp = true;
        //PlayerMovement.SelfReviveHandUp -= ChangeHand;
    }
}
