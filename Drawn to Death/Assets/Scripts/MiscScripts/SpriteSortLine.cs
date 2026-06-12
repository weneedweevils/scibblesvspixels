using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///This script instantiates objects along a set of lines which act as a trigger to change the sprite ordering of the oodler
///</summary>
public class SpriteSortLine : MonoBehaviour
{
    

    //private GameObject pointOne;
    //private GameObject pointTwo;
    private float slope;
    private float intercept;
    [SerializeField] private GameObject prefab;

    public List<Vector2> listOfNodes = new List<Vector2>();


    // Start is called before the first frame update
    private void Start()
    {
        //pointOne = GameObject.Find("Cross1");
        //pointTwo = GameObject.Find("Cross2");
        listOfNodes.Add(new Vector2(-136.483f, -41.788f));
        listOfNodes.Add(new Vector2(-69.878f, -8.405f));
        listOfNodes.Add(new Vector2(52.97f, -69.93f));
        listOfNodes.Add(new Vector2(-5.899f, 23.508f));
        listOfNodes.Add(new Vector2(116.974f, -37.93f));

        InstantiateBetweenPoints(listOfNodes[0], listOfNodes[1]);
        InstantiateBetweenPoints(listOfNodes[1], listOfNodes[2]);
        InstantiateBetweenPoints(listOfNodes[1], listOfNodes[3]);
        InstantiateBetweenPoints(listOfNodes[3], listOfNodes[4]);
        
    }
    void Update()
    {
        //Debug.Log("The location of the first point is: " + pointOne.transform.position);
        //Debug.Log("The location of the second point is: " + pointTwo.transform.position);
        //Debug.Log("The slope between the two points is: "+ slope);
    }


    ///<summary>
    ///This script instantiates objects between two points given two Vector2 positions
    ///</summary>
    private void InstantiateBetweenPoints(Vector2 pointOne, Vector2 pointTwo)
    {
        slope = (pointTwo.y - pointOne.y) / (pointTwo.x - pointOne.x);
        intercept = (pointOne.y - (slope * pointOne.x));

        // print an object down the length of the walls on the bottom
        for (int i = Mathf.CeilToInt(pointOne.x); i < Mathf.FloorToInt(pointTwo.x); i = i + 2)
        {
            int y = (Mathf.CeilToInt((slope * i) + intercept));
            Vector3 loc = new Vector3(i, y, 30f);
            GameObject border = Instantiate(prefab, loc, transform.rotation);
            border.transform.SetParent(transform);
        }
    }
}
