using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAVEMaster : MonoBehaviour
{
    public MetaStats metaStats;

    private void Awake()
    {
        if(metaStats.isLoadedFromSave != true)
        {
            SAVEHardData saveData = new SAVEHardData();
        }
    }

}
