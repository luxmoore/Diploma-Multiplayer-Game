using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_BigMan : MonoBehaviour
{
    #region Variables

    public GameObject camCanvasGObj;
    [SerializeField] Canvas camCanvas;

    public TextMeshProUGUI healthTMP;
    public TextMeshProUGUI atkTMP;
    public TextMeshProUGUI moveTMP;

    #endregion

    #region Functions

    private void Start()
    {
        camCanvas = camCanvas.GetComponent<Canvas>();
    }

    private void ChangeOver(int movementEnergy, int attackEnergy, int health)
    {
        atkTMP.SetText("ATK - " + attackEnergy.ToString()); atkTMP.ForceMeshUpdate();
        moveTMP.SetText("MVM - " + movementEnergy.ToString()); moveTMP.ForceMeshUpdate();
        healthTMP.SetText("HTP - " + health.ToString()); healthTMP.ForceMeshUpdate();
    }
    #endregion
}