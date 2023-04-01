using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticClassManager : MonoBehaviour
{
    void Awake()
    {
        // clean up of static instances / listeners to avoid errors.
        CuttingCounter.ResetStaticData();
        BaseCounter.ResetStaticData();
        TrashCounter.ResetStaticData();
        Player.ResetStaticData();
    }
}

