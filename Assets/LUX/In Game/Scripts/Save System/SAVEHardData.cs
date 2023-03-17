using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class SAVEHardData
{
    public int gridWidth;
    public int gridHeight;

    public int[] gridBitX; // is indexed through a 'for' loop.
    public int[] gridBitY; // when reread by the computer, prints in a line until reaching the specified gridWidth. No need to check for gridHeight.
    public bool[] gridBitWalkable;

    public string[] playerName;
    public int[] playerHealth;

    public int goAmount;
    public int turnAmount;
}
