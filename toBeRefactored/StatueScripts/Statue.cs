using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : MonoBehaviour
{
    //basically the identifier script
    public StatueValues StatueValues;
    public bool IsGrabbable;

    void Start()
    {
        if (GetComponent<OVRGrabbable>() != null)
            IsGrabbable = false;
    }
}
