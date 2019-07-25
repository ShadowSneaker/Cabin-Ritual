using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour
{
    // the item that is held within the inventory slot
    Item item;

    [Tooltip("the icon the item displays")]
    public Image Icon;
    
  
    [Tooltip("the sprite that displays when the slot is empty")]
    public Sprite Emptyslot;

   
    [Tooltip("the equip slot images")]
    public Image Equipslot;

 
    [Tooltip("the text that displays the name of the item of this slot")]
    public Text Name;

    [Tooltip("the text that displays the discription of the item in this slot")]
    public Text Description;

    
    [Tooltip("the equip button image")]
    public Image EquipButton;

    [Tooltip("the cancel button image")]
    public Image CancelButton;

    // a bool to determine wether this slot is linked to the equipment slot
    [Tooltip("is this item equiped to the player")]
    public bool Equiped;

    //Enum Used to determine what inventory slot it is (may be needed for equipment later)
    // this commment is here to say it hasnt been used yet (delete below code if never used)
    public enum InventoryslotEnum { SlotOne, SlotTwo, SlotThree, SlotFour};
    public InventoryslotEnum Slot;

    //this is the players inventory
    private Inventory PlayerInventory;

    public void Start()
    {
        EquipButton.gameObject.SetActive(false);
        CancelButton.gameObject.SetActive(false);
        PlayerInventory = FindObjectOfType<Inventory>();
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

        PlayerInventory.RemoveItem(item);

        item = null;
        Icon.sprite = Emptyslot;
        

        Name.text = "Name";
        Description.text = "Description";

        if(Equiped)
        {
            Equipslot.sprite = null;
            Equiped = false;
        }
        
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
        Equiped = true;
        PlayerInventory.EquipItem(item);
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
