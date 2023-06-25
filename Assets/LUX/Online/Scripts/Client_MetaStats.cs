using System.Collections;
using System.Collections.Generic;


/// <summary>
/// This script is what the client knows about itself, the game and the network.
/// </summary>
public static class Client_MetaStats 
{
    // Network
    public static string networkCode;
    public static bool amHost;

    // MetaStats adjacent
    public static bool[] gridGen = new bool[36]; //rewrite as bool[gridGenWidth * gridGenHeight] when not just trying to get over line
    public static bool[] gridGenHoleLocations = new bool[6];
    public static int gridGenWidth = 6;
    public static int gridGenHeight = 6;

    public static string[] allPlayerNames = new string[3];
    public static int[] allPlayerColours = new int[3];

    public static int[] allPlayerHealthCurrent = new int[3];
    public static int[] allPlayerHealthMax = new int[3];

    public static int[] allPlayerDamDealt = new int[3];
    public static int[] allPlayerHealthLost = new int[3];

    // Player
    public static int myPlayerNum;
    public static string myPlayerName;
    public static int myPlayerColour;

    public static int[] myLocation;

    public static int myPlayerHealth;
    public static int myPlayerMaxHealth = 100;

    public static int myPlayerDamageDealt;
    public static int myPlayerHealthLost;
}
