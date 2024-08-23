using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerTypes {EnemiesKilled, Collision};
public class ConditionalWall : MonoBehaviour
{
    public TriggerTypes triggerType;
}