using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemy: MonoBehaviour
{
    public float level, health, moveSpeed;
    public int money;
    // Pidetään yllä tietoa, _mistä_ suunnasta ollaan kävelemässä
    [HideInInspector] public Direction from;
    // Pidetään yllä tietoa, _mihin_ suuntaan ollaan kävelemässä
    [HideInInspector] public Vector3 destination;
}

public enum Direction
{
    NORTH,
    EAST,
    SOUTH,
    WEST
}
