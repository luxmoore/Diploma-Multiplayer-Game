using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaStats : MonoBehaviour
{
    // All of the things that the player specifies in the lobby will be passed through to this script.
    // This script is used exclusively by the G.C for startup

    [Header("Grid Generation")]
    [Tooltip("The height of the grid generated.")]
    public int gridGenMaxX;
    [Tooltip("The width of the grid generated.")]
    public int gridGenMaxY;
    [Tooltip("The gridbits that are essential are set to differing 'maps'. Like paths that have to exist or the players will not be able to reach one another.")]
    public int gridMap;
    [Tooltip("The amount of holes present in a generated grid. They are chosen at random.")]
    public int gridHolesAmount;

    [Header("Players")]
    [Tooltip("The amount of players in the game")]
    public int playerAmount; // INOPERATIONAL
    [Tooltip("The order in which players go in a turn.")]
    public int[] playerTurnStructure; // INOPERATIONAL
    [Tooltip("The names given by players.")]
    public int[] playerNames;
    [Tooltip("The life amount of players.")]
    public int[] playerHealth;
    [Tooltip("The class of each player.")]
    public int[] playerClass; // INOPERATIONAL


    // LUX
}