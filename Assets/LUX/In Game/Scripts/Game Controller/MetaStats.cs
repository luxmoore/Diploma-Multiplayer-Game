using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaStats : MonoBehaviour
{
    // All of the things that the player specifies in the lobby will be passed through to this script.
    // This script is used exclusively by the G.C for startup

    [Header("The Important Quesiton")]
    public bool isLoadedFromSave;

    [Header("Grid Generation")]
    [Tooltip("This contains all of the information needed to generate a grid from existing data.")]
    public LUX_GridBit[] gridbits;
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
    [Tooltip("The amount of damage that a player has dealt.")]
    public int[] playerDamageDealt;
    [Tooltip("The amount of damage that a player has taken.")]
    public int[] playerHealthLost;

    [Header("Turn")]
    [Tooltip("The current turn")]
    public int[] turnAmount;
    [Tooltip("The current go")]
    public int[] turnGoAmount;
    [Tooltip("The current amount of energy of player whose turn it is. 0 = movement, 1 = attack.")]
    public int[] playerEnergy;

    // LUX
}