using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Text FlavourText;

    // this function is used to open and close the inventory ui panel 
    public bool Close_OpenUI()
    {
        gameObject.SetActive(!gameObject.activeSelf);
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
