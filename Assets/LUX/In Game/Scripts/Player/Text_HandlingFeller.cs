using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Text_HandlingFeller : MonoBehaviour
{
    public TMP_Text playerName;
    public TMP_Text playerDam;

    public Vector3 playerDamDefaultPos;

    private RectTransform rect;

    private float playerDamDefaultFontSize = 0.4f;

    private void Start()
    {
        playerDam.text = "";
        playerDam.ForceMeshUpdate();

        rect = playerDam.GetComponent<RectTransform>();
    }

    public IEnumerator TookDam(string damTook)
    {
        float uni = 0;
        bool repeatThis = true;

        playerDamDefaultPos = rect.position;

        playerDam.text = damTook;
        playerDam.fontSize = playerDamDefaultFontSize;
        playerDam.ForceMeshUpdate();

        while (uni <= 3)
        {
            rect.position = new Vector3(rect.position.x, rect.position.y + 0.0001f, rect.position.z);
            uni = uni + Time.deltaTime;

            if (playerDam.fontSize > 0 && repeatThis == true)
            {
                playerDam.fontSize = playerDam.fontSize - 0.001f;
                playerDam.ForceMeshUpdate();
            }
            else
            {
                //playerDam.text = "";
                playerDam.ForceMeshUpdate();

                repeatThis = false;
            }
            
            yield return new WaitForEndOfFrame();
        }

        yield break;
    }

}