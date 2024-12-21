using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleManager : MonoBehaviour
{

    GameObject[] gameObjects;
    public Scene nextScene = Scene.End;
    public GameObject Oodler;
    public GameObject OodlerOutline;
    public GameObject Glich;
    Vector3 offScreen;
    Vector3 offSet;
    public float speed = 100f;
    public bool following = false;
    bool reached = false;
    private SpriteRenderer AttackColour;
    Color basecolor;
    Color pinkColor;
    private float timer = 0f;


    // Start is called before the first frame update
    void Start()
    {
        offScreen = new Vector3(81f, 21f, 0f);
        //InvokeRepeating("TeleportToGlich", 0.5f, 10f);
        Color basecolor = new Color(255,100,200,1);
        Color pinkColor = new Color(50, 255, 0, 0);
        AttackColour = OodlerOutline.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        gameObjects = GameObject.FindGameObjectsWithTag("HealthPillar");
        Debug.Log(gameObjects.Length);


        // If all health crystals are destroyed go to end cutscene
        if (gameObjects.Length == 0)
        {
            if (nextScene != Scene.End)
            {
                GameData data = DataPersistenceManager.instance.GetGameData();
                data.skipCutscene = false;
                DataPersistenceManager.instance.UpdateGame();
            }
            StartCoroutine(MenuManager.LoadScene(nextScene));
        }

        Debug.Log(Glich.transform.position);


        if (timer > 10.0f)
        {
            // set state to follow if not
            if (!following)
            {
                following = true;
                AttackColour.color = pinkColor;
            }

            // set state to attack if not
            else
            {
                following = false;
                reached = false;
                AttackColour.color = basecolor;
            }
            timer = 0f;
        }




        // if the state of the oodler is set to follow
        if (following)
        {
            var step = speed * Time.deltaTime;
            offSet = Glich.transform.position;
            offSet.y = offSet.y + 10f;
            Oodler.transform.position = Vector3.MoveTowards(Oodler.transform.position, offSet, step);
            
            if(Oodler.transform.position == offSet)
            {
                reached = true;
            }
            


            if (AttackColour.color.a < 1 && reached)
            {
                var temp = AttackColour.color;
                temp.a += 0.01f;
                AttackColour.color = temp;

            }
        }


        // if state of oodler is set to idle go off screen
        if(!following && Oodler.transform.position != offScreen) {
         var step2 = speed * Time.deltaTime;
         Oodler.transform.position = Vector3.MoveTowards(Oodler.transform.position, offScreen, step2);
        }
    }


    // function called every 10 seconds to determine if the oodler should back away or follow glich
    void TeleportToGlich()
    {
        if (!following)
        {
            following = true;
            AttackColour.color = pinkColor;
        }
        else
        {
            following = false;
            reached = false;
            AttackColour.color = basecolor;
        }



    }
}
