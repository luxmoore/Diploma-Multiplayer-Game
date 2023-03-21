using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>This component is the only way that the player is able to interact with the game aside from buttons.</summary>

public class LUX_GridClicker : MonoBehaviour
{
    // PlayerController Vars
    private PlayerController pc;
    int playerAffecting;

    // GameController Vars
    public GameController gc;
    [HideInInspector] public GameObject[,] gridbits;

    // RayCasting Vars
    public Camera cam;
    public GameObject selectedObj, hoveredOverObj;
    public int stepsAway;
    private bool isObjGoodBoy;

    // UI Vars
    public FollowingText followingText;
    public int energy;

    private void Start()
    {
        pc = gameObject.GetComponent<PlayerController>();
        playerAffecting = pc.playerNum;
        gridbits = gc.GetComponent<LUX_Grid>().localGridArray;
    }

    private void Update() // the update function is okay, because this component is designed to be turned on and off
    {
        // cast out a ray to check what the player is mousing over

        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            hoveredOverObj = hit.collider.gameObject;
            if (hoveredOverObj != null && hoveredOverObj.tag == "GridBit" && hoveredOverObj.GetComponent<LUX_GridBit>().visited > 0) // if it there and good
            {
                isObjGoodBoy = true;
            }
            else { isObjGoodBoy = false; }

            #region Update The Cursor TMP
            if (isObjGoodBoy == true) // if it there and good
            {
                
                stepsAway = hoveredOverObj.GetComponent<LUX_GridBit>().visited;
                if(stepsAway <= energy)
                {
                    followingText.UpdateText(stepsAway.ToString());
                }
                else { followingText.UpdateText(""); }
            }
            else { followingText.UpdateText(""); }
            #endregion

            #region Do Things On Click
            if (Input.GetMouseButtonUp(0))
            {
                selectedObj = hoveredOverObj;

                // do shit whit the selected object
            }
            #endregion
        }
    }
}
