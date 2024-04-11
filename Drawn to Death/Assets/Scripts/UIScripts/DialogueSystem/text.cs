using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.TextCore;
using UnityEngine.XR;

public class text : MonoBehaviour
{

    public List<string> Phrases = new List<string>();
    private string input;
    public TextMeshProUGUI textMeshPro;

    
    // Start is called before the first frame update
    void Start()
    {
       Phrases.Add("Rattled yer bones, eh spooky?");
       Phrases.Add("Looks like ya need a hand.Oh wait");
       Phrases.Add("Skill Issue");
       input = Phrases[Random.Range(0,Phrases.Count-1)];
       textMeshPro.text = input;
    }

}
