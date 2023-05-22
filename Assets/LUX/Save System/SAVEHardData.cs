using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class SAVEHardData
{
    public int gridWidth;
    public bool[] gridBit; // is indexed through a 'for' loop.

    public string[] playerName;
    public int[] playerHealth;
    public int[] playerDamDealt;
    public int[] playerDamRecieved;
    public int[] playerPosX;
    public int[] playerPosY;

    public int goAmount;
    public int turnAmount;
    public int[] playerEnergy;
}