using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LUX_PathFollower : MonoBehaviour
{
    public GameObject gameControllerGameObj;
    private GameController gameController;
    private LUX_Grid gridComp;
    private PlayerStats playerRepresentative;
    private LUX_GridClicker gridClicker;
    private UI_BigMan UIman;

    private enum STATE { STATE_MOVE, STATE_IDLE };
    private Transform moveThis;

    public List<GameObject> pathList = new List<GameObject>();

    public bool hitIt = false;

    private void Start()
    {
        gameControllerGameObj = GameObject.FindWithTag("GC");
        gameController = gameControllerGameObj.GetComponent<GameController>();
        gridComp = gameController.GetComponent<LUX_Grid>();
        moveThis = gameObject.GetComponent<Transform>();
        playerRepresentative = gameObject.GetComponentInChildren<PlayerStats>();
        gridClicker = gameObject.GetComponentInChildren<LUX_GridClicker>();
        UIman = gameControllerGameObj.GetComponent<UI_BigMan>();
    }

    private void Update()
    {
        if (hitIt == true)
        {
            hitIt= false;
            StartCoroutine("STATE_MOVE");
        }
    }

    public IEnumerator STATE_MOVE()
    {
        pathList = gridComp.pathList;

        int totalSteps = pathList.Count;
        int stepsTaken = 0;
        Vector2 currentPos = gridComp.debugStartXY;
        Vector2 ultimateGoal = gridComp.debugEndXY;
        Vector2 nextGoal_V2 = Vector2.zero;
        Vector3 nextGoal_V3 = new Vector3(0,0,0);


        if(pathList.Count == 0)
        {
            Debug.Log("Error - no gameobjects in variable pathList");
            Debug.Log("Terminating coroutine");
            yield break;
        }
        else
        {
            moveThis.position = new Vector3(currentPos.x, 1, currentPos.y);
            nextGoal_V2 = pathList[0].GetComponent<LUX_GridBit>().gridPos;
            stepsTaken = 1;
        }

        pathList[0].GetComponent<LUX_GridBit>().playerOnThis = false;
        pathList[0].GetComponent<LUX_GridBit>().playerNumOnThis = -69;

        while ( stepsTaken != totalSteps )
        {
            // set currentPos as last goal's vector2
            // set the next goal to pathList[stepstaken]
            if (stepsTaken != 1)
            {
                currentPos = nextGoal_V2;
            }
            nextGoal_V2 = pathList[stepsTaken].GetComponent<LUX_GridBit>().gridPos;
            nextGoal_V3 = new Vector3(nextGoal_V2.x, 1, nextGoal_V2.y);

            Debug.Log("Currently at " + currentPos + ", moving to " + nextGoal_V2);

            moveThis.position = nextGoal_V3;

            playerRepresentative.moveEnergy = playerRepresentative.moveEnergy - 1;
            playerRepresentative.gridPos = nextGoal_V2;

            gridClicker.energy = playerRepresentative.moveEnergy;
            UIman.ChangeOver(playerRepresentative.moveEnergy, playerRepresentative.atckEnergy, playerRepresentative.currentHealth, playerRepresentative.maxHealth);

            stepsTaken++;
            yield return new WaitForSecondsRealtime(0.5f);
        }

        pathList[pathList.Count - 1].GetComponent<LUX_GridBit>().playerOnThis = true;
        pathList[pathList.Count - 1].GetComponent<LUX_GridBit>().playerNumOnThis = gameObject.GetComponentInChildren<PlayerStats>().playerNum;

        playerRepresentative.gridPos = nextGoal_V2; Debug.Log("Updated recorded position to " + nextGoal_V2);
        gameControllerGameObj.GetComponent<LUX_Grid>().SetAllVisitedNegative(nextGoal_V2);
        gameControllerGameObj.GetComponent<LUX_Grid>().SetDistance(nextGoal_V2);

        gameControllerGameObj.GetComponent<CombatManager>().playerPos = nextGoal_V2;

        yield return new WaitForEndOfFrame();
    }

    // LUX
}