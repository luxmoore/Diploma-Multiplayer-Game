using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>This component is the only way that the player is able to interact with the game aside from buttons.</summary>

public class LUX_GridClicker : MonoBehaviour
{
    private Camera cam;
    private Ray ray;
    private GameController gc;
    [HideInInspector] public GameObject[,] gridbits;

    public int playerAffecting;

    private void Start()
    {
        gc = gameObject.GetComponent<GameController>();
    }

    void Update()
    {
        VariableChangesAndRayDrawing();
        if (Input.GetMouseButtonDown(0))
        {
            SelectGridbit();
        }
    }

    private void SelectGridbit()
    {

    }

    private void VariableChangesAndRayDrawing()
    {
        cam = Camera.current;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 100;

    }
}
