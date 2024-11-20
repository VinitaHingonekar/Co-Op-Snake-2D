using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerUp : MonoBehaviour
{
public enum PowerUpType {Shield, ScoreBoost, SpeedUp}
    public PowerUpType powerUpType;
    public float duration;
}
