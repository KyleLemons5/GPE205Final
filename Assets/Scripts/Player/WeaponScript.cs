using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public int maxAmmo;
    public int ammo;
    public float reloadTime;
    public bool isReloading;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        ammo = maxAmmo;
        isReloading = false;
    }

    void Update()
    {
        if(isReloading)
        {
            if(Time.time >= timer)
            {
                CompleteReload();
            }
        }
    }

    public void Shoot()
    {
        ammo -= 1;
    }

    public void Reload()
    {
        if(!isReloading)
        {
            isReloading = true;
            timer = Time.time + reloadTime;
        }
    }

    void CompleteReload()
    {
        ammo = maxAmmo;
        isReloading = false;
    }
}
