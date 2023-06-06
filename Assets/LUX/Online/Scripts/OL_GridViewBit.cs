using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OL_GridViewBit : MonoBehaviour
{
    #region Grid Vars

    [Header ("Grid Values")]
    public int gridNumber; 
    public bool gridWalkable;

    #endregion

    #region Click Stuff

    [Header("Click Stuff")]
    public SpriteRenderer spriteRend;
    public int whoTouched;
    public Sprite[] sprites; // 4 sprites, correlate with int state
    public int state; // 1 = not walkable | 2 = walkable | 3 = not walkable (has plus) | 4 = walkable (has minus)


    /// <summary>
    /// 1 = not walkable, 2 = walkable, 3 = not walkable with plus OR walkable with minus when clicked by original clicker, 4 = walkable with minus OR not walkable with plus when clicked by original clicker
    /// </summary>
    /// <param name="switchState"></param>
    public void ToggleTheState(int switchState, bool send)
    {
        // happens on click when send == true
        // happens on network update when send == false

        Debug.Log("Player has selected gridbit number " + gridNumber + ", which is currently at state " + state);

        switch(switchState)
        {
            case 1:
                // case for usage:
                // add plus

                DecisionMade(3, send);

                return;

            case 2:
                // case for usage:
                // add minus

                DecisionMade(4, send);

                return;

            case 3:
                // case for usage:
                // remove plus, is now walkable
                // remove minus, is now not walkable (rescind)

                DecisionMade(2, send);

                return;

            case 4:
                // case for usage:
                // remove plus, remains unwalkable
                // remove minus, remains walkable (rescind)

                DecisionMade(1, send);

                return;
        }
    }
    public void DecisionMade(int choice, bool send)
    {
        spriteRend.sprite = sprites[choice];
        state = choice;
        if(send)
        {
            SendToNetwork(choice);
        }
    }

    private void SendToNetwork(int result)
    {
    }

    #endregion

    #region Generic 
    private void Start()
    {
        spriteRend = gameObject.GetComponent<SpriteRenderer>();
    }
    #endregion
}