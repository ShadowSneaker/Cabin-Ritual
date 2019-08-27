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
    
    public List<GameObject> ItemObject = new List<GameObject>();

    //creating an item delegate so that everytime its called the inventory is updated. this is used for the ui which is why it does nothing now
    public delegate void OnItemChanged();
    public OnItemChanged OnitemChangedCallBack;

    // the UI that displays the inventory
    public GameObject inventoryUI;

    // to determine wether the inventory is open or not
    private bool InventoryOpen;

    // this is the gameobject this inventory is equiped to 
    private GameObject ThisInventory;

   // private GameObject ItemPoolObject;

    public void Start()
    {
        // this adds a function to the item change call back
        OnitemChangedCallBack += UpdateUI;

        InventoryOpen = false;

        ThisInventory = this.gameObject;


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
       

        if (Slots.Length != 0)
        {


            if (Items.Count >= InventorySpace)
            {
                Debug.Log(" not enough room");
                return false;
            }

            // items is the list this simple adds it to the list
            Items.Add(item);

            if(item.name == "Tool Box")
            {
                Debug.Log("Tool box added correctly");

                ItemObject.Add(GameObject.Find("ToolBox(ItemPool)"));
            }


            

            

            // if the callback has functions attached to it. when this command is done it will call those functions
            if (OnitemChangedCallBack != null)
                OnitemChangedCallBack.Invoke();

            return true;
        }
        return false;

      
        
    }

    public void RemoveItem(Item item)
    {
        Debug.Log("Removed");
        // items is the list this simply removes it from the list


        // this works for only the tool box for now so what i will do is make a switch of all of the items there can be 
        if (item.name == "Tool Box")
        {

            Instantiate(ItemObject[0], new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
            Debug.Log("instatiate happened");
        }





        Items.Remove(item);

       

        // if the callback has functions attached to it. when this command is done it will call those functions
        if (OnitemChangedCallBack != null)
            OnitemChangedCallBack.Invoke();
    }
   

    public void EquipItem(Item item)
    {
        EquipedItem = item;
    }

    public bool IsInventoryOpen()
    {
        return InventoryOpen;
    }

    public void ChangeInventoryOpen()
    {
        InventoryOpen = !InventoryOpen;
    }


    // returns the game object that this inventory is attached to 
    public GameObject GetPlayer()
    {
        return ThisInventory;
    }
}
