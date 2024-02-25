using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

// Used this video for most of the script https://www.youtube.com/watch?v=jvtFUfJ6CP8a
// if you want to use this in FSM inherit from EnemybaseState class
public class EnemyAI : MonoBehaviour
{

    public Transform target;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public Transform enemygraphics;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath;
    Seeker seeker;
    Rigidbody2D rb;
    public float maxhealth;
    public float damage;


    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("CheckState", 0f, 0.5f); // this will call the checkstate function to update the path every half second


    }
    void CheckState()
    {
        float inrange = Vector2.Distance(rb.position, target.position);

        // if not travelling to a path and the player is within range calculate new path
        if (seeker.IsDone() && inrange < 6f)
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    // Checks if there is a path calculated
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveEnemy();

    }

    // moves enemy and adjusts animation to face player
    void MoveEnemy()
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (rb.velocity.x >= 0.01f)
        {
            enemygraphics.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (rb.velocity.x <= -0.01f)
        {
            enemygraphics.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
