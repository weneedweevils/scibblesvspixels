using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    public float attractDistance = 5f; // Distance at which the soul starts moving towards the player
    public float collectDistance = 1f; // Distance at which the soul is collected

    public int value;
    private Vector2 initialDirection;
    private float initialMagnitude;
    private float duration = 1f; //Time for the soul to reach its target position
    private float elapsedTime = 0.0f;
    private PlayerMovement player;
    private Animator animator;

    public void Start()
    {
        //Generate a random direction with a positive y-component
        initialDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f)).normalized;

        //Generate a random magnitude
        initialMagnitude = Random.Range(5f, 10f);

        //Find player reference
        player = FindObjectOfType<PlayerMovement>();

        // Get animator component
        animator = GetComponent<Animator>();
    }

    //Update is called once per frame
    void Update()
    {
        if (player == null) return;

        // Calculate the distance to the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer < collectDistance && !animator.GetCurrentAnimatorStateInfo(0).IsName("Soul Spawn") && !player.inFreezeDialogue() && !player.timelinePlaying)
        {
            // Collect the soul and destroy the object
            CollectSoul();
        }
        else if (distanceToPlayer < attractDistance && !animator.GetCurrentAnimatorStateInfo(0).IsName("Soul Spawn") && !player.inFreezeDialogue() && !player.timelinePlaying)
        {
            // Move towards the player
            MoveTowardsPlayer(distanceToPlayer);
        }
        else if (elapsedTime < duration)
        {
            // Increment elapsed time
            elapsedTime += Time.deltaTime;

            // Calculate the lerp factor
            float lerpFactor = elapsedTime / duration;

            // Lerp the velocity
            Vector2 direction = Vector2.Lerp(initialDirection, new Vector2(initialDirection.x, 0), lerpFactor);
            float magnitude = Mathf.Lerp(initialMagnitude, 0f, lerpFactor);

            // Apply the velocity to move the soul
            transform.position += (Vector3)direction * magnitude * Time.deltaTime;
        }
    }
    
    //Set the value of the soul
    public void SetValue(int _value)
    {
        value = _value;
    }

    // Move the soul towards the player
    private void MoveTowardsPlayer(float distance)
    {
        // Calculate an exponential speed factor based on the distance to the player
        float speed = Mathf.Exp(-(distance/10)) * 25f; // Adjust the multiplier to control the maximum speed

        // Calculate the direction to the player
        Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;

        // Move the soul towards the player
        transform.position += (Vector3)directionToPlayer * speed * Time.deltaTime;
    }

    // Handle soul collection
    private void CollectSoul()
    {
        UpgradeManager.instance.currency += value;
        Destroy(gameObject);
    }
}
