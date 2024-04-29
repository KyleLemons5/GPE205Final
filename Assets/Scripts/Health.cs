using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Health : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public AudioMixer mixer;
    public AudioClip damageSound;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float amount, Pawn source)
    {
        currentHealth -= amount;
        Debug.Log(source.name + " did " + amount + " damage to " + gameObject.name);

        if(currentHealth <= 0)
        {
            Die(source);
        }
        else{
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
            AudioSource.PlayClipAtPoint(damageSound, gameObject.transform.position, SFXVol);
        }
    }

    public void ReplenishHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0 ,maxHealth);
    }

    public void Die(Pawn source)
    {
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

        //Debug.Log(source.name + " destroyed " + gameObject.name);

        // Can't cast AIController as PlayerController
        PlayerController pCon = new PlayerController();
        if(pCon.GetType().IsAssignableFrom(gameObject.GetComponent<CapsulePawn>().controller.GetType())){
            PlayerController owningPlayerController = (PlayerController) gameObject.GetComponent<CapsulePawn>().controller;
            owningPlayerController.lives -= 1;
        }
        else{
            AIController owningAIController = (AIController) gameObject.GetComponent<CapsulePawn>().controller;
            Debug.Log("Ai: " + owningAIController); // Ai scripts are not AI controllers
            if(owningAIController != null){
                Debug.Log("Ai died");
                GameManager.instance.RemoveAI(owningAIController);
            }
        }

        Destroy(gameObject);
    }
    
}
