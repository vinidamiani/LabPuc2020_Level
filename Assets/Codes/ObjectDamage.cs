using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDamage : MonoBehaviour
{

    public MeshRenderer render;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Damage()
    {
        //Destroy(gameObject);
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        int blinks = 6;
        while (blinks > 0)
        {
            render.enabled = !render.enabled;
            yield return new WaitForSeconds(0.1f);
            blinks--;
        }
    }
}

