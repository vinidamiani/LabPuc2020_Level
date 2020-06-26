using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guidedBomb : MonoBehaviour
{
    public GameObject target;
    Rigidbody rdb;
    public float bombForce = 1000;
    public GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        rdb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (target)
        {
            transform.LookAt(target.transform);

        }
        rdb.AddForce(transform.forward*50);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            Explode();
    }


        void Explode()
    {
        GameObject explo = Instantiate(explosion, transform.position, transform.rotation);
        Destroy(explo, 4);
        Destroy(gameObject);
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, 5, Vector3.up, 10);

        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                if (hit.rigidbody)
                {
                    hit.rigidbody.isKinematic = false;
                    hit.rigidbody.AddExplosionForce(bombForce, transform.position, 10);
                    hit.collider.gameObject.SendMessage("GetDamage", SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }
}
