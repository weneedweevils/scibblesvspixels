using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public Animator[] transitions;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("BufferAnimators");
    }

    private IEnumerator BufferAnimators()
    {
        yield return null;
        foreach (Animator transition in transitions)
        {
            transition.enabled = false;
        }

        yield return new WaitForSecondsRealtime(0.1f);

        foreach(Animator transition in transitions)
        {
            transition.enabled = true;
        }
    }
}
