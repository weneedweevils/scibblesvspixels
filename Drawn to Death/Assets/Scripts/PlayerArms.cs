using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArms : MonoBehaviour
{
    public GameObject weapon;
    private SpriteRenderer sprite = null;

    // Start is called before the first frame update
    void Start()
    {
        if (weapon != null)
        {
            sprite = weapon.GetComponent<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (sprite != null)
        {
            //Get mouse position
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //Flip the sprite according to mouse position relative to the players position
            sprite.flipX = mousePosition.x < transform.position.x;

            // Calculate the angle of the arms
            Vector2 direction = (mousePosition - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, Mathf.Abs(direction.x)) * Mathf.Rad2Deg;

            //Rotate towards mouse position
            transform.rotation = Quaternion.AngleAxis(angle, sprite.flipX ? Vector3.back : Vector3.forward);
        }
    }
}
