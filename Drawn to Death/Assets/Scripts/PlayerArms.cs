using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerArms
{
    private GameObject weapon;
    private GameObject player;
    private GameObject arms;
    protected PlayerMovement playerMovement;
    

    private SpriteRenderer sprite = null;
    private PlayerControlMap controls;
    Vector3 mousePosition = Vector3.zero;


    public PlayerArms(GameObject weapon, GameObject player, GameObject arms, PlayerControlMap controls, PlayerMovement playerMovement)
    {
        this.weapon = weapon;
        this.player = player;
        this.controls = controls;
        this.arms = arms;
        this.playerMovement = playerMovement;

        if (weapon != null)
        {
            sprite = weapon.GetComponent<SpriteRenderer>();
        }
    }

   
    // Update is called once per frame
    public void FrameUpdate(Vector2 aimDirection)
    {
       
        if (sprite != null && !player.GetComponent<PlayerMovement>().inFreezeDialogue() && !player.GetComponent<PlayerMovement>().timelinePlaying && Time.timeScale != 0f)
        {
            //Get mouse position

            if (playerMovement.isGamepad)
            {
                float flip = aimDirection.x < 0f ? -1 : 1;
                arms.transform.localScale = new Vector3(Mathf.Abs(arms.transform.localScale.x) * flip, arms.transform.localScale.y, arms.transform.localScale.x);

                Vector2 direction = (aimDirection).normalized;
                float angle = Mathf.Atan2(direction.y * flip, direction.x * flip) * Mathf.Rad2Deg;
                arms.transform.rotation = Quaternion.AngleAxis(angle, sprite.flipX ? Vector3.back : Vector3.forward);
            }
            else
            {
                mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

                //Flip the sprite according to mouse position relative to the players position
                float flip = mousePosition.x < arms.transform.position.x ? -1 : 1;
                arms.transform.localScale = new Vector3(Mathf.Abs(arms.transform.localScale.x) * flip, arms.transform.localScale.y, arms.transform.localScale.x);

                // Calculate the angle of the arms
                Vector2 direction = (mousePosition - arms.transform.position).normalized;
                float angle = Mathf.Atan2(direction.y * flip, direction.x * flip) * Mathf.Rad2Deg;

                //Rotate towards mouse position
                arms.transform.rotation = Quaternion.AngleAxis(angle, sprite.flipX ? Vector3.back : Vector3.forward);
            }
            
        }
        if (weapon.GetComponent<Attack>().lifestealStart)
        {
            arms.transform.rotation = Quaternion.AngleAxis(0f, Vector3.zero);
        }
    }
}

