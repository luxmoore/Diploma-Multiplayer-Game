using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LUX_PathFollower : MonoBehaviour
{
    public GameObject gameControllerGameObj;
    private GameController gameController;
    private LUX_Grid gridComp;
    private enum STATE { STATE_MOVE, STATE_IDLE };
    private Transform moveThis;

    public List<GameObject> pathList = new List<GameObject>();

    public bool hitIt = false;

    private void Start()
    {
        gameController = gameControllerGameObj.GetComponent<GameController>();
        gridComp = gameController.GetComponent<LUX_Grid>();
        moveThis = gameObject.GetComponent<Transform>();
    }

    private void Update()
    {
        if (hitIt == true)
        {
            hitIt= false;
            StartCoroutine("STATE_MOVE");
        }
    }

    IEnumerator STATE_MOVE()
    {
        //pathList.Clear();
        //pathList = gridComp.pathList;

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

        while( stepsTaken != totalSteps )
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

            stepsTaken++;
            yield return new WaitForSecondsRealtime(0.5f);
        }

        StartCoroutine(STATE_IDLE());
        yield return new WaitForEndOfFrame();
    }

    IEnumerator STATE_IDLE()
    {
        yield return new WaitForEndOfFrame();
    }
}