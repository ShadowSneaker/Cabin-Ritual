using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingScript : MonoBehaviour
{
    // notes for next time//
    // the image list needs to find the items automatically in the function (i shoulnd tbe adding them in inspector)
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
    // this needs moving to the ui part
    //public List<Transform> ItemImages = new List<Transform>();

    public void Awake()
    {
  
        CraftingScreen.gameObject.SetActive(false);
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

           // have a function here form the crafting ui that grabs the list and puts all  the images from that list onto the screen

            for (int i = 0; i < CraftingItems.Count; ++i)
            {
               

            }

        }

        CraftingScreen.gameObject.SetActive(true);

    }


    // checks wether the two materials supplied can be crafted make something
    // it will remove the items used from the players inventory if sucessfull
    public void Craft()
    {

    }


}
