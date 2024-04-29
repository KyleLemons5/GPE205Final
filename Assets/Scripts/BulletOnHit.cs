using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletOnHit : MonoBehaviour
{
    public float inaccuracy;
    public float maxDistance;
    public float damage;
    public Pawn owner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit impact;
        Vector3 shotForward = transform.forward;
        shotForward.x += Random.Range(-inaccuracy, inaccuracy);
        shotForward.y += Random.Range(-inaccuracy, inaccuracy);
        shotForward.z += Random.Range(-inaccuracy, inaccuracy);
        Physics.Raycast(transform.position, transform.forward, out impact, maxDistance);

        Debug.Log("Shoot!");

        if(impact.collider != null)
        {
            Debug.Log("Hit!");
            Health otherHealth = impact.collider.GetComponent<Health>();
            if(otherHealth != null)
            {
                otherHealth.TakeDamage(damage, owner);
            }

            Destroy(gameObject);
        }
        Destroy(gameObject, 0.02f);
    }
}
