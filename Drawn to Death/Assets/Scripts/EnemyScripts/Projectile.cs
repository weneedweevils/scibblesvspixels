using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Team team = Team.neutral;
    public Vector2 target = new Vector2(0, 0);
    public float speed = 0f;
    public float damage = 0f;
    private Rigidbody2D rbody;
    private Vector2 velocity = new Vector2(0, 0);
    private Color allyCol = Color.green;
    private SpriteRenderer selfImage;

    private void Start()
    {
        // Get Projectile Sprite
        selfImage = gameObject.transform.GetComponent<SpriteRenderer>();

        //Get references
        rbody = GetComponent<Rigidbody2D>();
        EnemyAI parent = transform.GetComponentInParent<EnemyAI>();

        //Assign variables
        if (parent == null) { Debug.LogError("Error - projectile parent not set to instance of EnemyAI"); Destroy(gameObject); }
        team = parent.team;
        target = parent.GetTarget().position;
        damage = parent.damage;

        //Calculate velocity
        velocity = (target - rbody.position).normalized * speed;

        //Remove from parent but keep position
        transform.parent = parent.transform.parent;
        transform.position = parent.transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (team != Team.neutral)
        {
            // Hue shift projectile green if ally
            if (team == Team.player)
            {
                selfImage.color = allyCol;
            }

            //Predict new position
            Vector2 currentPos = rbody.position;
            Vector2 newPos = currentPos + velocity * Time.fixedDeltaTime;

            //Move to new position
            //Debug.Log("Moving: " + velocity.ToString());
            rbody.MovePosition(newPos);
        }

        if (velocity.magnitude == 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                {
                    EnemyAI enemyai = collision.gameObject.GetComponent<EnemyAI>();
                    if (team != Team.neutral && enemyai.team != team && !(enemyai.state == State.dead || enemyai.state == State.dying))
                    {
                        enemyai.Damage(damage, true, true, velocity.normalized, 7f);
                        enemyai.Stun();
                        Destroy(gameObject);
                    }
                    break;
                }
            case "Player":
                {
                    if (team == Team.oddle)
                    {
                        PlayerMovement Player = collision.gameObject.GetComponent<PlayerMovement>();
                        Player.Damage(damage);
                        Destroy(gameObject);
                    }
                    break;
                }
            case "Obstacle":
                {
                    Destroy(gameObject);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
