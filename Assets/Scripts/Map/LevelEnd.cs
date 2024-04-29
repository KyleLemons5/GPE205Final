using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        PlayerController player = (PlayerController) other.gameObject.GetComponent<CapsulePawn>().controller;
        if(player != null)
        {
            GameManager.instance.ActivateGameOver();
        }
    }
}
