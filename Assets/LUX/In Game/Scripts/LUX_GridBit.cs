using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> This class does not contain any functions, only information about the specific gridbit. </summary>

public class LUX_GridBit : MonoBehaviour
{
    public Vector2 gridPos = Vector2.zero;
    public int visited = -1;
    public bool playerOnThis = false;

    // image variables
    public Image image = null;
    public Sprite[] spriteList = new Sprite[3];

    private void Start()
    {
        image = gameObject.GetComponentInChildren<Image>();
    }
}