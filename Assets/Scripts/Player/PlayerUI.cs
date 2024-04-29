using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public PlayerController controller;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI healthText;

    // Start is called before the first frame update
    void Start()
    {
        controller = (PlayerController)GetComponentInParent<CapsulePawn>().controller;
    }

    // Update is called once per frame
    void Update()
    {
        livesText.text = "Lives: " + controller.lives;
        healthText.text = "Health: " + controller.pawn.GetComponent<Health>().currentHealth;
    }
}
