using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAI : AIBase
{

    

    /// Properties

    [Tooltip("The object this AI should move towards")]
    public Transform FollowObject;

   

    /// Overridables
    

    private void Awake()
    {
        if (!FollowObject)
        {
            GameMode GM = FindObjectOfType<GameMode>();
            FollowObject = GM.GetPlayer(Random.Range(0, GM.GetPlayerCount())).transform;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Agent.destination = FollowObject.position;
        
    }

    /// Functions




}
