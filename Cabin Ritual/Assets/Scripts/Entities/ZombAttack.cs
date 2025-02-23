﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombAttack : MonoBehaviour
{
    [Header("Zombie attack")]

    [Tooltip("The amount of damage that is applied to the hit object.")]
    [SerializeField]
    public int Damage = 10;

    [Tooltip("The time before can attack again.")]
    [SerializeField]
    public int AttackCoolDown = 10;

    [Tooltip("Force applied on the hit object.")]
    [SerializeField]
    public float ImpactForce = 30.0f;

    public GameObject Zombies;    

    

    public void ZombieAttack()
    {
        RaycastHit Hit;
        if (Physics.Raycast(Zombies.transform.position, Zombies.transform.forward, out Hit))
        {
            Debug.DrawRay(Zombies.transform.position, Zombies.transform.forward, Color.red);
       
            Entity Target = Hit.transform.GetComponent<Entity>();
            if (Target != null)
            {
                Target.TakeDamage(Damage);
            }
       
            if (Hit.rigidbody != null)
            {
                Hit.rigidbody.AddForce(-Hit.normal * ImpactForce);
            }            
        }
    }
}
