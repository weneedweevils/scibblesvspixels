using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notifier<T>
{
    protected List<Observer<T>> observers = new List<Observer<T>>();

    public void AddObserver(Observer<T> observer)
    {
        if (observer == null)
            throw new System.ArgumentNullException(nameof(observer));

        if (observers.Contains(observer))
            return;

        observers.Add(observer);
    }
    public void RemoveObserver(Observer<T> observer)
    {
        if (observer == null)
            throw new System.ArgumentNullException(nameof(observer));

        observers.Remove(observer);
    }

    public void NotifyObservers(T data)
    {
        foreach (Observer<T> observer in new List<Observer<T>>(observers))
        {
            observer.OnNotified(data);
        }
    }

    public void Destroy()
    {
        foreach (Observer<T> observer in new List<Observer<T>>(observers))
        {
            observer.StopObservation(this);
        }

        observers.Clear();  // Clean up the observer list
    }

    ~Notifier()
    {
        Destroy();
    }
}