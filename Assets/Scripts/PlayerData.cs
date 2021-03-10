using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// System.Serializable attribuutti saa luokan tiedot n�kyviin Unityn Inspectorissa.
[System.Serializable]
public class PlayerData
{
    // Konstruktori
    public PlayerData(int money)
    {
        this.money = money;
    }

    public int money;
    public int lives;
}
