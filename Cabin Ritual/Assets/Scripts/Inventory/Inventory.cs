using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    
    [Tooltip("an array of the inventory slots on the players canvas")]
    [SerializeField]
    InventorySlot[] Slots;
    
    [Tooltip("the item in the players equiped slot")]
    public Item EquipedItem;

    // the amount of items a player can have in there inventory at one time
    [Tooltip("the amount of slots you want in the players inventory")]
    public int InventorySpace = 4;

    [Tooltip("the lost of items within the players inventory")]
    public List<Item> Items = new List<Item>();

    //creating an item delegate so that everytime its called the inventory is updated. this is used for the ui which is why it does nothing now
    public delegate void OnItemChanged();
    public OnItemChanged OnitemChangedCallBack;

    // the UI that displays the inventory
    public GameObject inventoryUI;

    public void Start()
    {
        // this adds a function to the item change call back
        OnitemChangedCallBack += UpdateUI;

        

        
    }

    private void FixedUpdate()
    {
        if(inventoryUI.activeSelf)
        {
            if (Slots.Length == 0)
            {
                //gets all of the inventory slots and puts them in the needed array
                Slots = GameObject.Find("InventoryUI").GetComponentsInChildren<InventorySlot>();
            }
        }

        
    }
    //function to update the UI displays to the screen
    void UpdateUI()
    {
        

        for(int i = 0; i < Slots.Length; ++i)
        {
            if(i < Items.Count)
            {
                Debug.Log("I is less");
                Slots[i].AddItemToSlot(Items[i]);
            }
            
        }
    }

    

    public bool AddItem(Item item)
    {
        if(Items.Count >= InventorySpace)
        {
            Debug.Log(" not enough room");
            return false;
        }

        // items is the list this simple adds it to the list
        Items.Add(item);

        // if the callback has functions attached to it. when this command is done it will call those functions
        if (OnitemChangedCallBack != null)
            OnitemChangedCallBack.Invoke();

        return true;
    }

    public void RemoveItem(Item item)
    {

        // items is the list this simply removes it from the list
        Items.Remove(item);

        // if the callback has functions attached to it. when this command is done it will call those functions
        if (OnitemChangedCallBack != null)
            OnitemChangedCallBack.Invoke();
    }
   

    public void EquipItem(Item item)
    {
        EquipedItem = item;
    }
}
