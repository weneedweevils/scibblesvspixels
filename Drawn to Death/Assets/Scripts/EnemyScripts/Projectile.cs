﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Projectile : MonoBehaviour
{
    public Team team = Team.neutral;
    public Vector2 target = new Vector2(0, 0);
    
    public float speed = 0f;
    [HideInInspector] public float damage = 0f;
    protected Rigidbody2D rbody;
    protected Vector2 velocity = new Vector2(0, 0);
    protected Color allyCol = Color.green;
    protected SpriteRenderer selfImage;
    protected bool hit = false;

    public bool destroyOnHit = true;
    protected List<GameObject> hitObjects = new List<GameObject>();

    protected virtual void Start()
    {
        // Get Projectile Sprite
        selfImage = gameObject.transform.GetComponent<SpriteRenderer>();

        //Get references
        rbody = GetComponent<Rigidbody2D>();
        EnemyAI parent = transform.GetComponentInParent<EnemyAI>();

        //Assign variables
        if (parent == null) { Debug.LogError("Error - projectile parent not set to instance of EnemyAI"); Destroy(gameObject); }
        team = parent.team;
        try
        {
            target = parent.GetTarget().position;
        }
        catch (NullReferenceException)
        {
            Destroy(gameObject);
            return;
        }
        
        damage = parent.damage.value;

        Transform t = parent.GetTarget();
      
        if (t != null)
        {
            target = t.position;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        //Calculate velocity
        velocity = (target - rbody.position).normalized * speed;

        //Remove from parent but keep position
        transform.parent = parent.transform.parent;
        transform.position = parent.transform.position;

        // Add spread to projectiles if buffed
        if (parent.buffed)
        {
            velocity.x *= UnityEngine.Random.Range(0.5f, 1f);
            velocity.y *= UnityEngine.Random.Range(0.5f, 1f);
        }
    }

    // Update is called once per frame
    protected virtual void Update()
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

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        hit = false;
        if (hitObjects.Contains(collision.gameObject))
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Enemy":
                {
                    EnemyAI enemyai = collision.gameObject.GetComponent<EnemyAI>();
                    HealthCrystal crystal = collision.gameObject.GetComponent<HealthCrystal>();
                    Boss oodler = collision.gameObject.GetComponent<Boss>();

                    if (enemyai != null && team != Team.neutral && enemyai.team != team && !(enemyai.state == State.dead || enemyai.state == State.dying))
                    {
                        enemyai.Damage(damage, true, true, velocity.normalized, 7f);
                        enemyai.Stun();
                        if (destroyOnHit)
                            Destroy(gameObject);
                        hit = true;
                        hitObjects.Add(collision.gameObject);
                    }

                    else if (crystal != null)
                    {

                        //Debug.Log(otherAI.invincibilityTimer2.IsUseable());

                        if (crystal != null && crystal.invincibilityTimer.IsUseable())
                        {
                            //Damage crystal
                            crystal.CrystalDamage(damage, true);
                            if (destroyOnHit)
                                Destroy(gameObject);
                            hit = true;
                            hitObjects.Add(collision.gameObject);
                        }
                    }

                    else if (oodler != null)
                    {
                        if (oodler != null && oodler.BossIsDamageable()) //&& !oodler.invincibilityTimer.IsActive())
                        {
                            //Damage enemy
                            oodler.Damage(damage);
                            //invincibilityTimerOodler.StartTimer();
                            if (destroyOnHit)
                                Destroy(gameObject);
                            hit = true;
                            hitObjects.Add(collision.gameObject);
                        }

                    }
                    else
                    {
                        return;
                    }

                    break;
                }
            case "Player":
                {
                    if (team == Team.oddle && !PlayerMovement.instance.dashTimer.IsActive())
                    {
                        PlayerMovement Player = collision.gameObject.GetComponent<PlayerMovement>();
                        Player.Damage(damage);
                        if (destroyOnHit)
                            Destroy(gameObject);
                        hit = true;
                        hitObjects.Add(collision.gameObject);
                    }
                    else
                    {
                        return;
                    }
                    break;
                }

            case "Obstacle":
                {
                    Destroy(gameObject);
                    return;
                }


            case "Column":
                {
                    float newangle = ProjectileAngle(velocity);

                    if (collision.gameObject.name == "ProjectileDetector")
                    {
                        Debug.Log("we have entered here");
                        if ((0f <= newangle && newangle <= 25f) || (155f <= newangle && newangle <= 180) || (180f <= newangle && newangle <= -155) || (-25 <= newangle && newangle <= 0))
                        {
                            Destroy(gameObject);
                        }
                    }
                    else
                    {
                        Destroy(gameObject);
                        return;
                    }

                    break;
                }

            default:
                {
                    return;
                }
        }
    }

    protected virtual float ProjectileAngle(Vector2 velocity)
    {
        Vector2 dir = velocity.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return angle;
    }
}
