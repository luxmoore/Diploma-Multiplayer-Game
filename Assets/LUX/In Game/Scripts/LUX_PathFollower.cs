using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LUX_PathFollower : MonoBehaviour
{
    public GameObject gameControllerGameObj;
    private GameController gameController;

    public Vector2 currentlyAt = Vector2.zero;
    public Vector2 endGoal= Vector2.zero;
    public List<GameObject> pathList = new List<GameObject>();

    private void Start()
    {
        gameController = gameControllerGameObj.GetComponent<GameController>();
    }
}
