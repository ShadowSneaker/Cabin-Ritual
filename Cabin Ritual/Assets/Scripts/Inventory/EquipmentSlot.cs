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

    [Tooltip("the panel that appears when a letter is used")]
    public Image LetterPanel;

    [Tooltip("the text that appears on the letter")]
    public Text LetterPanelText;

    private void Start()
    {
        Slots = FindObjectsOfType<InventorySlot>();
        LetterPanel.gameObject.SetActive(false);
        
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

    public void Inspect()
    {
        foreach(InventorySlot i in Slots)
        {
            if(i.Equiped)
            {
                Debug.Log("equipped registered");
                if(i.GetItem().LetterItem)
                {

                    LetterPanelText.text = i.GetItem().LetterText;
                    LetterPanel.gameObject.SetActive(true);
                    
                }
            
            }
        }
    
    }

    public void Close()
    {
        LetterPanel.gameObject.SetActive(false);
        
    }

}
