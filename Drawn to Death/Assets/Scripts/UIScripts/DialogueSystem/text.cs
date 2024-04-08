using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.TextCore;

public class text : MonoBehaviour
{

    public List<string> Phrases = new List<string>();
    private string input;
    public TextMeshProUGUI textMeshPro;

    
    // Start is called before the first frame update
    void Start()
    {
       Phrases.Add("BAHAHAHAHAHAHAHAHHAHAHAHAH");
       input = Phrases[Random.Range(0,Phrases.Count-1)];
       textMeshPro.text = input;
    }

    // got from here https://gamedev.stackexchange.com/questions/85807/how-to-read-a-data-from-text-file-in-unity
    void readTextFile(string file_path)
    {
        StreamReader inp_stm = new StreamReader(file_path);

        while (!inp_stm.EndOfStream)
        {
            string inp_ln = inp_stm.ReadLine();
            // Do Something with the input. 
        }

        inp_stm.Close();
    }

}
