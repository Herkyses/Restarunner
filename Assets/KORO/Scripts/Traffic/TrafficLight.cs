using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public enum LightState 
    { 
        Red, 
        Yellow,
        Green 
    }
    public LightState currentState;
    public float redDuration = 5f;
    public float yellowDuration = 2f;
    public float greenDuration = 5f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        switch (currentState)
        {
            case LightState.Red:
                if (timer >= redDuration)
                {
                    ChangeLight(LightState.Green);
                }
                break;
            case LightState.Yellow:
                if (timer >= yellowDuration)
                {
                    ChangeLight(LightState.Red);
                }
                break;
            case LightState.Green:
                if (timer >= greenDuration)
                {
                    ChangeLight(LightState.Yellow);
                }
                break;
        }
    }

    void ChangeLight(LightState newState)
    {
        currentState = newState;
        timer = 0f;
    }
}
