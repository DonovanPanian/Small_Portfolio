using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    //need to set the volume to the same value of the slider, but also need to convert to using a log10, because that's what mixers use 
    //multiplying by 20 makes sure the decemal value stays true to the information being given by the slider 
    //for more info on this, go to: https://www.youtube.com/watch?v=xNHSGMKtlv4 

    [Header("Audio Settings Mixers")]
    //Don't need an overall mixer, because that slider affects all other sliders/volumes/mixers 
    [SerializeField] AudioMixer musicAudioMixer = null;
    [SerializeField] AudioMixer sfxAudioMixer = null;
    [SerializeField] AudioMixer dialogueAudioMixer = null;

    [Header("Audio Settings Sliders")]
    //[SerializeField] Slider overallSlider = null;
    [SerializeField] Slider musicSlider = null;
    [SerializeField] Slider sfxSlider = null;
    [SerializeField] Slider dialogueSlider = null;

    Slider[] sliders;


    //DEVELOPEMENT NOTES:
    //
    //      Try to find a way to set audio mixers using code? 
    //      Awake and Start methods did NOT work when Donovan tried them on 09/28/2020
    //
    //      Try to find a way to set sliders using code? 
    //      This has not been attempted as of 09/28/2020

    private void Start()
    {
        sliders = new Slider[] {musicSlider, sfxSlider, dialogueSlider};
    }


    /*public void SetOverallAudioVolume(float sliderValue)
    {
        //needs to change the volume of all mixers
        //none of the other sliders should be able to override the settings put forward by this slider

        //make a factor 
        //if over all = 20%, set float to .2f

        foreach (Slider s in sliders) 
        {
            s.maxValue = sliderValue;
        }

        musicAudioMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        sfxAudioMixer.SetFloat("SFX_Volume", Mathf.Log10(sliderValue) * 20);
        dialogueAudioMixer.SetFloat("DialogueVolume", Mathf.Log10(sliderValue) * 20);

    }*/

    public void SetMusicVolume(float sliderValue)
    {
        musicAudioMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void SetSFXVolume(float sliderValue)
    {
        sfxAudioMixer.SetFloat("SFX_Volume", Mathf.Log10(sliderValue) * 20);
    }

    public void SetDialogueVolume(float sliderValue)
    {
        dialogueAudioMixer.SetFloat("DialogueVolume", Mathf.Log10(sliderValue) * 20);
    }


}
