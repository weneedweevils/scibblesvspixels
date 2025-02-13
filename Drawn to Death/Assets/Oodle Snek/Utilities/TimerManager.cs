using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : Singleton<TimerManager>
{
    private static List<StateTimer> timers;
    public int count = 0;

    protected override void Awake()
    {
        base.Awake();
        timers = new List<StateTimer>();
    }

    void Update()
    {
        // Create a copy of the timers list to iterate through
        List<StateTimer> timersCopy = new List<StateTimer>(timers);

        foreach (StateTimer timer in timersCopy)
        {
            // Check if the timer is null or has been destroyed
            if (timer == null)
            {
                timers.Remove(timer);
                continue;
            }

            timer.Update();
        }
        count = timers.Count;
    }

    public static void Add(StateTimer timer)
    {
        if (!timers.Contains(timer))
        {
            timers.Add(timer);
        }
    }

    public static void Remove(StateTimer timer)
    {
        if (timers.Contains(timer))
        {
            timers.Remove(timer);
        }
    }
}