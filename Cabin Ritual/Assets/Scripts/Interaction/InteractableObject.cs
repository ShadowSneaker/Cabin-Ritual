using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    
    [Tooltip("is this object able to be interacted with")]
    public bool Interactable = true;

    
    [Tooltip("a list of objects that this object activates")]
    public ActivatableObject[] BoundObjects;

    
    [Tooltip("does this object deactivate on interaction")]
    public bool DeactivateOnInteraction;

    
    [Tooltip("the text you want appearing on the screen when the player goes to interact with something")]
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


    public virtual string GetFlavourText(Controller Player)
    {
        return ScreenText;
    }
}
