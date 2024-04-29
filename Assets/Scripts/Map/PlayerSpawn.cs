using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerSpawn : MonoBehaviour
{
    public Transform playerSpawnTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerSpawnTransform = transform;
    }
}
