using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class SAVEHardData
{
    public int gridWidth;
    public bool[] gridBit; // is indexed through a 'for' loop.

    public string[] playerName = new string[3];
    public int[] playerHealth = new int[3];
    public int[] playerDamDealt = new int[3];
    public int[] playerDamRecieved = new int[3];
    public int[] playerPosX = new int[3];
    public int[] playerPosY = new int[3];

    public int goAmount;
    public int turnAmount;
    public int[] playerEnergy = new int[3];
}