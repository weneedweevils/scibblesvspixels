using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArms : MonoBehaviour
{
    public GameObject weapon;
    public GameObject player;
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
        if (sprite != null && !player.GetComponent<PlayerMovement>().inFreezeDialogue() && !player.GetComponent<PlayerMovement>().timelinePlaying && Time.timeScale != 0f)
        {
            //Get mouse position
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //Flip the sprite according to mouse position relative to the players position
            float flip = mousePosition.x < transform.position.x ? -1 : 1;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * flip, transform.localScale.y, transform.localScale.x);

            // Calculate the angle of the arms
            Vector2 direction = (mousePosition - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y * flip, direction.x * flip) * Mathf.Rad2Deg;

            //Rotate towards mouse position
            transform.rotation = Quaternion.AngleAxis(angle, sprite.flipX ? Vector3.back : Vector3.forward);
        }
        if (weapon.GetComponent<Attack>().lifestealStart)
        {
            transform.rotation = Quaternion.AngleAxis(0f, Vector3.zero);
        }
    }
}
