using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour
{
    public TrdWalk walkscript;
    private void OnTriggerEnter(Collider other)
    {
        walkscript.objectToLook = other.gameObject;
    }
}
