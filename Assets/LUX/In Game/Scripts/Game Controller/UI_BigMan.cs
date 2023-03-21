using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BigMan : MonoBehaviour
{
    #region Variables

    public GameObject camCanvasGObj;
    Canvas camCanvas;

    public Text mouseText;
    public int stepCount;

    #endregion

    #region Functions

    private void Start()
    {
        camCanvas = camCanvas.GetComponent<Canvas>();
    }

    private void Update()
    {
        
    }
    #endregion
}
