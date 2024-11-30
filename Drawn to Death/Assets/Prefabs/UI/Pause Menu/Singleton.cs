using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T instance;
    public bool persistent;

    protected virtual void Awake()
    {
        // If an instance already exists and it's not this one, destroy this instance
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = (T)this;
            if (persistent) DontDestroyOnLoad(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        // Reset the instance to null when the object is destroyed
        if (instance == this) instance = null;
    }
}