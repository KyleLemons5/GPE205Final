using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public AudioClip hoverSound;
    public AudioClip clickSound;
    private Transform locTransform;
    private AudioSource aSource;
    public AudioMixer mixer;
    public Transform camTransform;

    public void Start()
    {
        aSource = this.GetComponentInParent<AudioSource>();
        locTransform = this.GetComponentInParent<Transform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        aSource.PlayOneShot(hoverSound);
        Camera cam = FindObjectOfType<Camera>();
        camTransform = cam.transform;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(aSource.isActiveAndEnabled){
            aSource.PlayOneShot(clickSound);
        }
        else{
            float MainVol;
            float SFXVol;
            mixer.GetFloat("MasterVolume", out MainVol);
            mixer.GetFloat("SFXVolume", out SFXVol);
            // Calculates the Db of the volumes as a percentage to put in PlayAtClipPoint
            SFXVol = SFXVol / 20;
            SFXVol = MathF.Pow(10, SFXVol);
            MainVol = MainVol / 20;
            MainVol = MathF.Pow(10, MainVol);
            SFXVol = SFXVol * MainVol;
            AudioSource.PlayClipAtPoint(clickSound, camTransform.position, SFXVol);
        }
    }
}
