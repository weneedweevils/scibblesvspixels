using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test5 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Enemy enemyA = new Enemy(health: 100);
        

        Enemy enemyB = enemyA;

        enemyB.health = 50;

        Debug.Log(enemyB.health);   
        Debug.Log(enemyA.health);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public struct Enemy
{
    public int health;
    public Enemy(int health)
    {
        this.health = health;
    }
}
