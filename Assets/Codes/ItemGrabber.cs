using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrabber : MonoBehaviour
{
    public GameObject weaponOnHand; 
    public Transform handposition;
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("WeaponDroped"))
        {
            if (weaponOnHand)
            {
                weaponOnHand.transform.parent = null;
                weaponOnHand.GetComponent<Rigidbody>().isKinematic = false;
                weaponOnHand.transform.Translate(-transform.up);
                weaponOnHand.layer = 0;
            }

            weaponOnHand = other.gameObject;
            other.transform.parent = handposition; //coloca como filho da mao
            other.transform.localPosition = Vector3.zero;//vai pra posicao da mao
            weaponOnHand.GetComponent<Rigidbody>().isKinematic = true;//desativa o rigidbody
            other.transform.localRotation = Quaternion.identity;//reseta a rotacao
            other.transform.gameObject.layer = transform.gameObject.layer;

        }
    }
}
