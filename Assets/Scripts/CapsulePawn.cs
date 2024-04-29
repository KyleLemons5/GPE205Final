using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CapsulePawn : Pawn
{
    private float nextShootTime;
    private float shotDelay;
    private WeaponScript weapon;
    public AudioClip shootSound;
    public AudioMixer mixer;

    // Start is called before the first frame update
    public override void Start()
    {
        float secondsPerShot;
        if(fireRate >= 0)
            secondsPerShot = 1/fireRate;
        else{
            secondsPerShot = Mathf.Infinity;
        }
        nextShootTime = Time.time + secondsPerShot;
        shotDelay = secondsPerShot;
        weapon = GetComponentInChildren<WeaponScript>();
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void MoveForward()
    {
        mover.Move(transform.forward, moveSpeed);
    }

    public override void MoveBackward()
    {
        mover.Move(transform.forward, -moveSpeed);
    }
    public override void MoveLeft()
    {
        mover.Move(transform.right, -moveSpeed);
    }
    public override void MoveRight()
    {
        mover.Move(transform.right, moveSpeed);
    }

    public override void Shoot()
    {
        if(Time.time >= nextShootTime)
        {
            if(weapon.ammo > 0)
            {
                if(!weapon.isReloading)
                {
                    shooter.Shoot(bulletPrefab, inaccuracy, damage);
                    weapon.Shoot();
                    float MainVol;
                    float SFXVol;
                    mixer.GetFloat("MasterVolume", out MainVol);
                    mixer.GetFloat("SFXVolume", out SFXVol);
                    // Calculates the Db of the volumes as a percentage to put in PlayAtClipPoint
                    SFXVol = SFXVol / 20;
                    SFXVol = Mathf.Pow(10, SFXVol);
                    MainVol = MainVol / 20;
                    MainVol = Mathf.Pow(10, MainVol);
                    SFXVol = SFXVol * MainVol;
                    AudioSource.PlayClipAtPoint(shootSound, gameObject.transform.position, SFXVol);
                    nextShootTime = Time.time + shotDelay;
                }
            }
            else
            {
                weapon.Reload();
            }
        }
    }

    public override void Reload()
    {
        weapon.Reload();
    }

    public override void Jump()
    {
        mover.Jump(jumpForce);
    }

    public override void RotateTowards(Vector3 targetPosition)
    {
        //rotate toward target
        Vector3 vectorToTarget = targetPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    public override void MakeNoise()
    {
        if(noiseMaker != null)
            noiseMaker.volumeDistance = noiseMakerVolume;
    }

    public override void StopNoise()
    {
        if(noiseMaker != null)
            noiseMaker.volumeDistance = 0;
    }
}
