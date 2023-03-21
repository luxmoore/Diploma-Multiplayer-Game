using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>This component is the only way that the player is able to interact with the game aside from buttons.</summary>

public class LUX_GridClicker : MonoBehaviour
{
    private PlayerController pc;
    int playerAffecting;

    public GameController gc;
    [HideInInspector] public GameObject[,] gridbits;

    // RayCasting Vars
    private Camera cam;

    private void Start()
    {
        pc = gameObject.GetComponent<PlayerController>();
        playerAffecting = pc.playerNum;
        gridbits = gc.GetComponent<LUX_Grid>().localGridArray;
        cam = Camera.current;
    }

    private void Update() // the update function is okay, because this component is designed to be turned on and off
    {
        // cast out a ray to check what the player is mousing over and if they have clicked or not.

        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                //hit.collider.gameObject;

                // Do something with the object that was hit by the raycast.
            }
        }
        
    }
}
