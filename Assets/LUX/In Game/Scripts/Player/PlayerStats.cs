using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    [Header("Meta")]
    [Tooltip("This signifies which player the following stats specifically affects.")]
    public int playerNum;
    [Tooltip("Dead players do not contribute to the player go structure. Players die when their health reaches zero.")]
    public bool isAlive;
    [Tooltip("For personal flair! For a bit of pizzazz! For the hell of it! For additional entertainment!")]
    public string givenName = "Jimbo Jones"; // default name

    [Header("Tracked Stats")]
    [Tooltip("This is used exclusively by the combat manager and game finisher.")]
    public int totalDamage;
    [Tooltip("This is used exclusively by the combat manager and game finisher.")]
    public int totalHealthLost;
    [Tooltip("This is used  and calculated exclusively at the end of the game.")]
    public int score;

    [Header("Grid")]
    [Tooltip("The location that this player takes up on the grid.")]
    public Vector2 gridPos;

    [Header("Energy")]
    [Tooltip("Both forms of energy convert to eachother, in a 2:1 ratio - 2 move energy = 1 attack energy.")]
    public int moveEnergy;
    [Tooltip("Both forms of energy convert to eachother, in a 2:1 ratio - 2 attack energy = 1 move energy.")]
    public int atckEnergy;

    [Header("Stats")]
    [Tooltip("Maximum health of this player.")]
    public int maxHealth;
    [Tooltip("Current health of this player.")]
    public int currentHealth;
    [Tooltip("The most damage that a strike can inflict.")]
    public int maxDamage;
    [Tooltip("The least amount of damage that the player can inflict.")]
    public int minDamage;

    // LUX
}