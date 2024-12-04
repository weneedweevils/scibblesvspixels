using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class StateTimer
{
    public bool unscaledTime = false;
    public float[] durations;
    public float timer { get; private set; } = 0f;
    public int state { get; private set; } = 0;
    public bool active { get; private set; } = false;

    public Notifier<StateTimer> stateNotifier;

    // Delegate to hold the callback function
    private Action onTimerEndCallback;

    // Constructor that initializes the timer with durations and registers it with the TimerManager
    public StateTimer(float[] _durations)
    {
        timer = 0;
        durations = _durations;
        Initialize();
    }

    // Initialize the timer and add it to the TimerManager
    public void Initialize()
    {
        stateNotifier = new Notifier<StateTimer>();
        TimerManager.Add(this);
    }

    public void Update()
    {
        // Exit if the timer is not active
        if (!active) { return; }

        // Increment the timer by the time passed since the last frame
        timer += (unscaledTime ? Time.unscaledDeltaTime :Time.deltaTime);

        // Check if the timer has exceeded the duration for the current state
        if (timer >= durations[state])
        {
            // Subtract the duration from the timer and move to the next state
            timer -= durations[state];
            state = (state + 1) % durations.Length;

            // If we return to the first state, deactivate the timer and call the callback
            if (state == 0)
            {
                timer = 0;
                active = false;

                // Invoke the callback function if it's set
                onTimerEndCallback?.Invoke();
            }
            stateNotifier.NotifyObservers(this);
        }
    }

    // Restart the timer from the beginning
    public void Restart()
    {
        state = 0;
        timer = 0;
        active = true;
    }

    // Start the timer from the beginning
    public void Start(Action callback = null)
    {
        state = 0;
        timer = 0;
        active = true;
        onTimerEndCallback = callback;
    }

    // Stop the timer and reset to the first state
    public void Stop()
    {
        active = false;
        state = 0;
        timer = 0;
    }

    // Start the timer at specific state
    public void StartAtState(int startState, Action callback = null)
    {
        if (startState >= 0 && startState < durations.Length)
        {
            state = startState;
            timer = 0;
            active = true;
            onTimerEndCallback = callback;
        }
        else
        {
            //Log an error if the new state index is invalid
            Debug.LogError("Invalid StateTimer state index.");
        }
    }

    public void Destroy()
    {
        // Remove the timer from TimerManager when the object is destroyed
        TimerManager.Remove(this);
    }

    // Destructor that removes the timer from the TimerManager when the object is destroyed
    ~StateTimer()
    {
        Destroy();
    }
}