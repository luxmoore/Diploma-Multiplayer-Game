using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>This component is the only way that the player is able to interact with the game aside from buttons.</summary>

public class LUX_GridClicker : MonoBehaviour
{
    // PlayerController Vars
    private PlayerStats ps;

    // GameController Vars
    public GameController gc;
    [HideInInspector] public GameObject[,] gridbits;
    private LUX_Grid gridComp;

    // RayCasting Vars
    Camera cam;
    public GameObject selectedObj, hoveredOverObj;
    public int stepsAway;
    private bool isObjGoodBoy;

    // UI Vars
    private FollowingText followingText;
    public int energy;

    // Debug Vars
    private string debugMsg = "Selection is fucked. DO NOT ALLOW ALGORITHM TO USE OR IT WILL STACK OVERFLOW.";
    public bool debugOut;

    private void Start()
    {
        ps = gameObject.GetComponent<PlayerStats>();

        GameObject gcObj = GameObject.FindWithTag("GC");
        gc = gcObj.GetComponent<GameController>();
        gridComp = gc.GetComponent<LUX_Grid>();

        gridbits = gc.GetComponent<LUX_Grid>().localGridArray;

        cam = Camera.main;
        cam.enabled = true;

        followingText = GameObject.FindWithTag("FollowingText").GetComponent<FollowingText>();

        if(cam != null)
        {
            if (debugOut == true) { Debug.Log("Main camera located"); }
        }
        else { Debug.Log("ERROR - Main camera not located."); }
        
    }

    private void Update() // the update function is okay, because this component is designed to be turned on and off
    {
        // cast out a ray to check what the player is mousing over

        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        energy = ps.moveEnergy;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hoveredOverObj = hit.collider.gameObject;
            if (hoveredOverObj != null && hoveredOverObj.tag == "GridBit" && hoveredOverObj.GetComponent<LUX_GridBit>().visited > 0) // if it there and good
            {
                isObjGoodBoy = true;
            }
            else { isObjGoodBoy = false; }

            //Debug.Log("Goodboy test returned " + isObjGoodBoy);

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
                if (hoveredOverObj.GetComponent<LUX_GridBit>().visited <= energy)
                {
                    GameObject selectedObj = hoveredOverObj;

                    Vector2 selectedGridBitGridPos = selectedObj.GetComponent<LUX_GridBit>().gridPos;
                    if (debugOut == true) { Debug.Log("The gridbit at " + selectedGridBitGridPos); }
                    if(selectedGridBitGridPos == gameObject.GetComponent<PlayerStats>().gridPos)
                    {
                        Debug.Log("Er1 - " + debugMsg);
                        return;
                    }
                    if (selectedObj.GetComponent<LUX_GridBit>().visited <= 0)
                    {
                        Debug.Log("Er2 - " + debugMsg);
                        return;
                    }
                    gridComp.ReceiveSelection(selectedGridBitGridPos, gameObject.GetComponent<PlayerStats>().playerNum);
                }
                else
                {
                    Debug.Log("Selected gridbit is not within possible movement with current amount of energy");
                }
            }
            #endregion
        }
    }

    //LUX
}
