using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    public Image CraftingUIImage;




    // this closes the crafting menu
    // it will also clear all the slots in the crafting menu when done so that nothing is dragged over
    public void CloseCrftingMenu()
    {
        CraftingUIImage.gameObject.SetActive(false);
    }





}
