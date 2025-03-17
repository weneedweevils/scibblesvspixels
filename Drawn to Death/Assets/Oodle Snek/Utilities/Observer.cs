using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Observer<T>
{
    private readonly Action<T> callbackFunction;
    private readonly List<Notifier<T>> notifiers = new List<Notifier<T>>();

    public Observer(Action<T> callback)
    {
        callbackFunction = callback ?? throw new ArgumentNullException(nameof(callback));
    }

    public void StartObservation(Notifier<T> notifier)
    {
        if (notifier == null) 
            throw new ArgumentNullException(nameof(notifier));

        if (notifiers.Contains(notifier)) 
            return;

        notifiers.Add(notifier);
        notifier.AddObserver(this);
    }
    public void StopObservation(Notifier<T> notifier)
    {
        if (notifier == null)
            throw new ArgumentNullException(nameof(notifier));

        if (notifiers.Remove(notifier))
        {
            notifier.RemoveObserver(this);
        }
    }

    public void OnNotified(T data)
    {
        callbackFunction?.Invoke(data);
    }

    public void Destroy()
    {
        foreach (Notifier<T> notifier in notifiers)
        {
            notifier.RemoveObserver(this);
        }
        notifiers.Clear();
    }

    ~Observer()
    {
        Destroy();
    }
}