using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAI : AIBase
{

    

    /// Properties

    [Tooltip("The object this AI should move towards")]
    public Transform FollowObject;

   

    /// Overridables

    private void FixedUpdate()
    {
        Agent.destination = FollowObject.position;
        
    }

    /// Functions




}
