using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAI : AIBase
{

    

    /// Properties

    [Tooltip("The object this AI should move towards")]
    public Transform FollowObject;


    public string Key;
    private GameMode GM = null;

    public Animator Anim;




    /// Overridables


    private void OnEnable()
    {
        GM = FindObjectOfType<GameMode>();
        if (!FollowObject)
        {
            FollowObject = GM.GetPlayer(Random.Range(0, GM.GetPlayerCount())).transform;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Agent.destination = FollowObject.position;
        
    }

    /// Functions


    public void Return()
    {
        GM.GetPlayer(0).GetComponent<PlayersPoints>().AddPoints(100);
        //Anim.SetBool("Dying", true);
        GM.Despawn(Key, this.gameObject);
        
    }

}
