using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this script is attached to the item you want picking up

public class ItemPickUp : MonoBehaviour
{
    // once interactble ibject script made change the monobehaviour to that

    // item that the player is picking up
    // the item that is attached to the object this script is on
    [Tooltip("the item that you want this script to work with")]
    public Item item;

   

    public void Pickup()
    {
        // adds the item that has been picked up to the inventory
        // if statement check if the item is added. if so it is destroyed.
        // if it isnt then its left within the world
        Controller temp = FindObjectOfType<Controller>();
        if(temp.ReturnLookingAt())
        {
            if (temp.GetPlayerInv().AddItem(item))
            {
               
                Debug.Log("Entered destroy");

                gameObject.GetComponent<MeshRenderer>().enabled = false;
                // this moves the gameobjects position inorder to the illusion of picking it up
                gameObject.transform.position = new Vector3(0, 1000, 0);

            }
        }
        
        
    }


}
