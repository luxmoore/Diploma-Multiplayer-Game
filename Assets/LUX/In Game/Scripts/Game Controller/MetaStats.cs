using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MetaStats
{
    // All of the things that the player specifies in the lobby will be passed through to this script.
    // This script is used exclusively by the G.C for startup

    [Header("The Important Quesiton")]
    public static bool isLoadedFromSave;

    [Header("Grid Generation")]
    [Tooltip("This contains all of the information needed to generate a grid. false = hole, true = walkable gridbit.")]
    public static bool[] gridbitGen;
    [Tooltip("The height of the grid generated.")]
    public static int gridGenMaxX = 6; // NONCHANGING CURRENTLY
    [Tooltip("The width of the grid generated.")]
    public static int gridGenMaxY = 6; // NONCHANGING CURRENTLY
    [Tooltip("The gridbits that are essential are set to differing 'maps'. Like paths that have to exist or the players will not be able to reach one another.")]
    public static int gridMap; // INOPERATIONAL
    [Tooltip("The amount of holes present in a generated grid. They are chosen at random.")]
    public static int gridHolesAmount = 10;

    [Header("Players")]
    [Tooltip("The amount of players in the game")]
    public static int playerAmount; // INOPERATIONAL
    [Tooltip("The order in which players go in a turn.")]
    public static int[] playerTurnStructure; // INOPERATIONAL
    [Tooltip("The names given by players.")]
    public static string[] playerNames = new string[3];
    [Tooltip("The life amount of players.")]
    public static int[] playerHealth;
    [Tooltip("The class of each player.")]
    public static int[] playerClass; // INOPERATIONAL
    [Tooltip("The amount of damage that a player has dealt.")]
    public static int[] playerDamageDealt;
    [Tooltip("The amount of damage that a player has taken.")]
    public static int[] playerHealthLost;
    [Tooltip("The X of the player's location on the grid.")]
    public static int[] playerGridPosX;
    [Tooltip("The Y of the player's location on the grid.")]
    public static int[] playerGridPosY;

    [Header("Turn")]
    [Tooltip("The current turn")]
    public static int turnAmount;
    [Tooltip("The current go")]
    public static int turnGoAmount;
    [Tooltip("The current amount of energy of player whose turn it is. 0 = movement, 1 = attack.")]
    public static int[] playerEnergy;

    // LUX
}