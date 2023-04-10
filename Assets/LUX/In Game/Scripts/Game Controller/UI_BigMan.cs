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

    private TextMeshProUGUI healthTMP;
    private TextMeshProUGUI atkTMP;
    private TextMeshProUGUI moveTMP;

    #endregion

    #region Functions

    private void Start()
    {
        camCanvas = camCanvas.GetComponent<Canvas>();

        moveTMP = GameObject.FindGameObjectWithTag("MovementText").GetComponent<TextMeshProUGUI>();
        atkTMP = GameObject.FindGameObjectWithTag("AttackText").GetComponent<TextMeshProUGUI>();
        healthTMP = GameObject.FindGameObjectWithTag("HealthText").GetComponent<TextMeshProUGUI>();
        //turnDisplay = GameObject.FindGameObjectWithTag("TurnText").GetComponent<TextMeshProUGUI>();
        //goDisplay = GameObject.FindGameObjectWithTag("GoText").GetComponent<TextMeshProUGUI>();
    }

    public void ChangeOver(int movementEnergy, int attackEnergy, int health, int maxHealth)
    {
        atkTMP.SetText("ATK - " + attackEnergy.ToString()); atkTMP.ForceMeshUpdate();
        moveTMP.SetText("MVM - " + movementEnergy.ToString()); moveTMP.ForceMeshUpdate();
        healthTMP.SetText("HTP - " + health.ToString() + "/" + maxHealth.ToString()); healthTMP.ForceMeshUpdate();
    }
    #endregion
}