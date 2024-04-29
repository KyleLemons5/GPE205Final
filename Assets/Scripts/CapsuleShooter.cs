using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleShooter : Shooter
{
    public Transform bulletSpawnTransform;
    public float bulletForce;

    // Start is called before the first frame update
    public override void Start()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }

    public override void Shoot(GameObject bulletPrefab, float inaccuracy, float damage)
    {
        GameObject newBullet = Instantiate(bulletPrefab, bulletSpawnTransform.position, bulletSpawnTransform.rotation) as GameObject;
        
        newBullet.GetComponent<BulletOnHit>().inaccuracy = inaccuracy;
        newBullet.GetComponent<BulletOnHit>().damage = damage;
        newBullet.GetComponent<BulletOnHit>().owner = GetComponentInParent<Pawn>();
        if (newBullet.GetComponent<BulletOnHit>().owner == null)
        {
            newBullet.GetComponent<BulletOnHit>().owner = GetComponent<Pawn>();
        }
        CapsulePawn player = GetComponentInParent<CapsulePawn>();
        if(player != null)
        {
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if(rb.velocity.y > 0)
            {
                //Debug.Log("launch");
                Vector3 launch = rb.transform.forward;
                Vector3 yDirection = rb.GetComponentInChildren<Camera>().transform.forward;
                launch = (launch + yDirection).normalized * bulletForce;
                rb.AddForce(-launch, ForceMode.Force);
            }
        }
    }
}
