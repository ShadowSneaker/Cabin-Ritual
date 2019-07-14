using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
  
    //stores the information of the players four inventory slots
    InventorySlot[] Slots;
    //Transoform that is the parent to all of the inventory slots
    public Transform SlotsPrefab;

    public void Start()
    {
        // this adds a function to the item change call back
        OnitemChangedCallBack += UpdateUI;

        //gets all of the inventory slots and puts them in the needed array
        Slots = SlotsPrefab.GetComponentsInChildren<InventorySlot>();
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

    // this is the singlton used to so you dont have to use find object in item pick up script
    public static Inventory Instance;

    private void Awake()
    {
        // when the game starts sets the inventory for the player to this inventory 
        if(Instance != null)
        {
            Debug.LogWarning(" more then one inventory active");
            return;
        }

        Instance = this;
    }

    // the amount of items a player can have in there inventory at one time
    public int InventorySpace = 4;

    //creating an item delegate so that everytime its called the inventory is updated. this is used for the ui which is why it does nothing now
    public delegate void OnItemChanged();
    public OnItemChanged OnitemChangedCallBack;

    // the list of items within the players inventory
    public List<Item> Items = new List<Item>();

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
   
}
