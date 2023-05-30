using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WibbleWobblah : MonoBehaviour
{
    public IEnumerator OnHit()
    {
        bool repeatA = true;
        float repeatATimer = 0;
        bool repeatB = true;
        float repeatBTimer = 0;

        float sineValue;

        while(repeatA)
        {
            // sinusoidal changing of the gameObject.transform scale

            Vector3 oldScale = gameObject.transform.localScale;

            sineValue = Mathf.Sin(repeatATimer) * 0.001f;

            gameObject.transform.localScale = new Vector3(oldScale.x - sineValue, oldScale.y - sineValue, oldScale.z - sineValue);

            yield return new WaitForEndOfFrame();

            repeatATimer = repeatATimer + Time.deltaTime;
            if (repeatATimer >= 2)
            {
                repeatA = false;
            }
        }

        Vector3 transitionVar = gameObject.transform.localScale;

        while(repeatB)
        {
            // interpolate back to original size over span of a second 

            gameObject.transform.localScale = Vector3.Lerp(transitionVar, new Vector3(1, 1, 1), repeatBTimer);

            yield return new WaitForEndOfFrame();

            repeatBTimer = repeatBTimer + Time.deltaTime;
            if (repeatBTimer >= 1)
            {
                repeatB = false;
            }
        }

        yield return null;
    }
}
