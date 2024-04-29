using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;
    public float jumpForce;
    public Mover mover;
    public Shooter shooter;
    public GameObject bulletPrefab;
    public float damage;
    public float fireRate;
    public float inaccuracy;
    public Noisemaker noiseMaker;
    public float noiseMakerVolume;
    public Controller controller;

    // Start is called before the first frame update
    public virtual void Start()
    {
        mover = GetComponent<Mover>();
        if (mover == null)
        {
            mover = GetComponentInChildren<Mover>();
        }
        shooter = GetComponent<Shooter>();
        if (shooter == null)
        {
            shooter = GetComponentInChildren<Shooter>();
        }
        noiseMaker = GetComponent<Noisemaker>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public abstract void MoveForward();
    public abstract void MoveBackward();
    public abstract void MoveLeft();
    public abstract void MoveRight();

    public abstract void Shoot();
    public abstract void Reload();
    public abstract void Jump();
    public abstract void RotateTowards(Vector3 targetPostition);
    public abstract void MakeNoise();
    public abstract void StopNoise();
}
