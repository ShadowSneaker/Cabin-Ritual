using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour
{
    // the item that is held within the inventory slot
    Item item;
    // the icon that the Item has
    public Image Icon;

    //the images tfor equipslot
    public Image Equipslot;

    // texts needed to display information about the item
    public Text Name;
    public Text Description;

    // buttons to determine what to do with the selected item
    public Image EquipButton;
    public Image CancelButton;

    public void Start()
    {
        EquipButton.gameObject.SetActive(false);
        CancelButton.gameObject.SetActive(false);
    }

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
        //sets the buttons to decide to equip item or not on.
        EquipButton.gameObject.SetActive(true);
        CancelButton.gameObject.SetActive(true);

    }

    public void EquipButtonYes()
    {
        // have this put the equiped item in the equip slot then removes buttons from screen

        Equipslot.sprite = item.Icon;

        EquipButton.gameObject.SetActive(false);
        CancelButton.gameObject.SetActive(false);

    }

    public void EquipButtonNo()
    {
        // have this remove the button prompts to equit the item
        EquipButton.gameObject.SetActive(false);
        CancelButton.gameObject.SetActive(false);
    }



}
