using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    //Determines if this object can be interacted with
    public bool Interactable = true;

    // A list of objects that this object activates
    public ActivatableObject[] BoundObjects;

    // a bool to determine if the object deactivates on interaction with this object
    public bool DeactivateOnInteraction;

    // test that shows up on the screen to inform the player how to interact with the object
    public string ScreenText = "Press E to interact";

    // Test that shows up on the screen to inform the player about the object
    public string ObjectInfo;

    public virtual void Interact()
    {
        if(Interactable)
        {
            for(int i = 0; i < BoundObjects.Length; ++i)
            {
                BoundObjects[i].Activate();
            }

            if(DeactivateOnInteraction)
            {
                Interactable = false;
            }
        }
    }

}
