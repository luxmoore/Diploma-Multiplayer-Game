using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFader : MonoBehaviour
{
    public Vector2 defaultPos;

    public IEnumerator FADE()
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition = defaultPos;

        bool continueDown = true;
        while(continueDown == true)
        {
            // did not allow me to move the thinger without doing this.
            Vector2 togetherPos = gameObject.GetComponent<RectTransform>().anchoredPosition;
            togetherPos.y = togetherPos.y -= 0.03f;
            gameObject.GetComponent<RectTransform>().anchoredPosition = togetherPos;

            if (gameObject.GetComponent<RectTransform>().anchoredPosition.y == -1)
            {
                continueDown = false;
            }
        }

        yield return new WaitForEndOfFrame();
    }
}
