using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>This is like a warehouse. By moving variables in here, I am able to reference them in any other script currently in the scene.</summary>

public class SingletonValues : MonoBehaviour
{
    #region Singleton
    public static SingletonValues instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    #endregion


}
