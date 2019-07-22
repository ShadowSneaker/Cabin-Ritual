using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivatableObject : MonoBehaviour
{

    public enum ActivationBehaviour
    {
        // will toggle this object on/off when interacted with
        Toggle,

        // will only allow activation when this object is deactivated
        ActivateOnly,

        // will only alllow activation when this object is activated
        DeactivateOnly,

        //Runs the activation script every time the object is interacted with
        Reset
    }

    // the enum to say how this object will be activated
    public ActivationBehaviour Behaviour;

    //has this object already been activated?
    public bool Activated;

    // what events need to be activated when interacted with
    public UnityEvent[] Events;


    public virtual void Activate()
    {
        switch(Behaviour)
        {
            case ActivationBehaviour.Toggle:
                {
                    Activated = !Activated;
                    for(int i = 0; i < Events.Length; ++i)
                    {
                        if(Events[i] != null)
                        {
                            Events[i].Invoke();
                        }
                    }
                    break;
                }

            case ActivationBehaviour.ActivateOnly:
                {
                    if(!Activated)
                    {
                        Activated = true;
                        for(int i = 0; i < Events.Length; ++i)
                        {
                            if (Events[i] != null)
                            {
                                Events[i].Invoke();
                            }
                        }
                        
                    }
                    break;
                }

            case ActivationBehaviour.DeactivateOnly:
                {
                    if (Activated)
                    {
                        Activated = false;
                        for (int i = 0; i < Events.Length; ++i)
                        {
                            if (Events[i] != null)
                            {
                                Events[i].Invoke();
                            }
                        }

                    }
                    break;
                }

            case ActivationBehaviour.Reset:
                {
                    
                    
                       Activated = true;
                       for (int i = 0; i < Events.Length; ++i)
                       {
                            if (Events[i] != null)
                            {
                                Events[i].Invoke();
                            }
                       }

                    
                    break;
                }
        }
    }
}
