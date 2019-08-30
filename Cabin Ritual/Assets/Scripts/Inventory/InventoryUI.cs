using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class InventoryUI : MonoBehaviour
{
    public Text FlavourText;
    public AudioMixer SoundEffectMixer;

    // this function is used to open and close the inventory ui panel 
    public bool Close_OpenUI()
    {
        gameObject.SetActive(!gameObject.activeSelf);

        SoundEffectMixer.SetFloat("OpenCloseInv", 20);
        // this is where i will need an audio manager so that i can actually play the sound
        
        return gameObject.activeInHierarchy;
    }

    public void DisableFlavourText()
    {
        FlavourText.enabled = false;
    }

    public void EnableFlavourText()
    {
        FlavourText.enabled = true;
    }

    public void ChangeFlavourText (string text)
    {
        FlavourText.text = text;
    }
}
