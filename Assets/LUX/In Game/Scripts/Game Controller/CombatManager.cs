using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public bool debug;

    private GameController gameController;

    #region Functions

    private void Start()
    {
        gameController = gameObject.GetComponent<GameController>();
    }

    private void ApplyDamage(Vector2 selection, int damMin, int damMax)
    {
        #region Generate Damage Amount

        float tempDam = Random.Range(damMin, damMax);
        int trueDam = Mathf.RoundToInt(tempDam);
        if (debug) { Debug.Log("Used " + damMin + " and " + damMax + " to generate " + tempDam + " (rounded to " + trueDam + ")"); }

        #endregion

        #region Apply Damage
        #endregion
    }

    #endregion

    #region Input Output

    public void ReceiveSelection(Vector2 selection, int damMin, int damMax)
    {
        ApplyDamage(selection, damMin, damMax);
    }
    #endregion
}