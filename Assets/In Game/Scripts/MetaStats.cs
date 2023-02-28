using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaStats : MonoBehaviour
{
    // All of the things that the player specifies in the lobby will be passed through to this script.
    // This script is used exclusively by the G.C

    [Header("Grid Generation")]
    [Tooltip("The height of the grid generated.")]
    public int gridGenMaxX;
    [Tooltip("The width of the grid generated.")]
    public int gridGenMaxY;
    [Tooltip("The gridbits that are essential are set to differing 'maps'. Like paths that have to exist or the players will not be able to reach one another.")]
    public int gridMap;
    [Tooltip("The chance of a non-essential gridbit being classed as a type of obstacle. 1 is all non-essentail gridbits will be an obstacle, 0 is no obstacles.")]
    public float gridWalkAbility;

    [Header("Players")]
    [Tooltip("The amount of players in the game")]
    public int playerAmount;
    [Tooltip("The order in which players go in a turn.")]
    public List<int> playerTurnStructure;

    // LUX
}