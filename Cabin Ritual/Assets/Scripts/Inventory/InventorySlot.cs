using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    // the item that is held within the inventory slot
    Item item;
    // the icon that the Item has
    public Image Icon;

    //the images that the 

    // texts needed to display information about the item
    public Text Name;
    public Text Description;
    

    //this will need to take an item and adds it to the inventory this inventory slot
    public void AddItemToSlot(Item NewItem)
    {
        item = NewItem;
        Icon.sprite = item.Icon;
        Icon.enabled = true;


        Name.text = item.name;
        Description.text = item.Description;
        
    }

    //removes the item held within this slot
    public void RemoveItemFromSlot()
    {
        // this will be changed to display an actual image of just a clear slot
        item = null;
        Icon.sprite = null;
        Icon.enabled = false;

        Name.text = null;
        Description.text = null;

        Inventory.Instance.RemoveItem(item);
    }

    //uses the item currently held in the slot. and removes it   
    public void UseItem()
    {
        //player equips the item so...
        Debug.Log("Button Working");
        // when the player clicks on the item a image pops up asking do they want to equip item
        //two buttons appear saying yes or no
        //if the player presses no the button prompts go away
        // if yes the item is then move to the equiped section
        // so these next functions should be easy for now

    }

    public void EquipButtonYes()
    {
        // have this put the equiped item in the equip slot
    }

    public void EquipButtonNo()
    {
        // have this remove the button prompts to equit the item
    }



}
