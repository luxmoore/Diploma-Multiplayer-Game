using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Meta")]
    public int playerNum;

    [Header("Energy")]
    public int moveEnergy;
    public int atckEnergy;

    [Header("Stats")]
    public int health;
    public int damage;
}