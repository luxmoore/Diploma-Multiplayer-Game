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
    public string givenName;

    [Header("Tracked Stats")]
    public int totalDamage;
    public int totalHealthLost;

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

    [Header("Affectors")]
    [Tooltip("All incoming affector values are multiplied by this.")]
    public float affectorInMultiples;
    [Tooltip("The list of affectors currently on this player.")]
    public List<string> affectorsInflicted;
    [Tooltip("All outgoing affector values are multiplied by this.")]
    public float affectorOutMultiples;
    [Tooltip("The list of affectors currently inflicted by this player on strike.")]
    public List<string> affectorsInflicting;

    // LUX
}