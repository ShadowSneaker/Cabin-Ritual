using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    
    [Tooltip("the image of the players equip slot")]
    public Image EquipSlot;

    
    [Tooltip("the images for thre equip slot when its empty")]
    public Sprite EmptyEquipSlot;

    // the inventory slots
    private InventorySlot[] Slots;

    private void Start()
    {
        Slots = FindObjectsOfType<InventorySlot>();
    }

    //Unequip button
    public void Unequip()
    {
        //for now just changes the image back to null
        EquipSlot.sprite = EmptyEquipSlot;

        foreach(InventorySlot i in Slots)
        {
            if(i.Equiped)
            {
                i.Equiped = false;
            }
        }
    }

}
