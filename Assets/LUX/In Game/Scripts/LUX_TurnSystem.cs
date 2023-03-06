using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LUX_TurnSystem : MonoBehaviour
{
    // --------------------------------------- Variables ---------------------------------------

    [Header("Miscellanious Variables")]
    public GameObject[] playerEntities;
    public int playerAmount;
    public int playerAmountAlive;

    //[Header("State Machine Variables")]
    public enum STATES {TURN_START, TURN_MEAT, TURN_END, TURN_LIMBO}
    public STATES currentState;

    // --------------------------------------- Functions --------------------------------------- 

    // --------------------------------------- Turn State System --------------------------------------- 
}
