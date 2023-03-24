using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LUX_TurnSystem : MonoBehaviour
{
    // --------------------------------------- Variables ---------------------------------------

    private GameController gameController;

    [Header("Miscellanious Variables")]
    public List<GameObject> playerEntities;
    public int playerAmount;
    public int playerAmountAlive;
    public int currentTurn;
    public int whoseGo;

    //[Header("State Machine Variables")]
    public enum STATES {TURN_START, TURN_MEAT, TURN_END, TURN_LIMBO}
    public STATES currentState;

    // --------------------------------------- Functions --------------------------------------- 

    private void Start()
    {
        gameController = gameObject.GetComponent<GameController>();
        playerEntities = gameController.alivePlayers;
    }

    private void ExchangeGoes(int playerNum)
    {
        StartCoroutine("Jimbo Jones");
    }
    
    // --------------------------------------- Turn State System --------------------------------------- 

    public IEnumerator TURN_START()
    {


        yield return new WaitForEndOfFrame();
    }

    private IEnumerator KILL_FUNC()
    {


        yield return new WaitForEndOfFrame();
    }
}