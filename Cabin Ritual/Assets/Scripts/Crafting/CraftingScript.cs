using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingScript : MonoBehaviour
{
    // notes for next time//
    // need to make it so the creen appears on opening the crafting
    // need to put the function open craft menu on to the object in game
    //the image list needs to find the items automatically in the function (i shoulnd tbe adding them in inspector)
    // then need to do combining
    // then need to do actual removing of items
    // have the new combined item put in players inventory
    ///////////////////////////////////////////////////
    

    // the two crafting slot items the player has added
    Item SlotOneItem;
    Item SlotTwoItem;

    public Image CraftingScreen;

    [Tooltip("the items within the players inventory")]
    public List<Item> CraftingItems = new List<Item>();

    // list of item images for the differnt items
    public List<Image> ItemImages = new List<Image>();

    public void Awake()
    {
        CraftingScreen.enabled = false;
    }

    // a function that makes the crafting menu open 
    // this function will need to get when the player has in thier inventory
    public void OpenCraftingMenu()
    {
        //finds the controller of the player
        Controller temp = FindObjectOfType<Controller>();
        //checks to make sure its the one that is bieng looked at
        if (temp.ReturnLookingAt())
        {
            CraftingItems = temp.GetPlayerInv().Items;

            for(int i = 0; i < CraftingItems.Count; ++i)
            {
                ItemImages[i].sprite = CraftingItems[i].Icon;
            }

        }

        CraftingScreen.enabled = true;

    }


    // checks wether the two materials supplied can be crafted make something
    // it will remove the items used from the players inventory if sucessfull
    public void Craft()
    {

    }


    // this closes the crafting menu
    // it will also clear all the slots in the crafting menu when done so that nothing is dragged over
    public void CloseCrftingMenu()
    {

    }


}
