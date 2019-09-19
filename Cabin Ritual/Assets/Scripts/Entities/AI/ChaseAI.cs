using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAI : AIBase
{

    [Header("Zombie attack")]

    [Tooltip("The amount of damage that is applied to the hit object.")]
    [SerializeField]
    public int Damage = 10;

    //[Tooltip("Force applied on the hit object.")]
    //[SerializeField]
    //public float ImpactForce = 30.0f;

    public GameObject Zombie;

    // Properties

    [Tooltip("The object this AI should move towards")]
    public Transform FollowObject;

    public string Key;

    private GameMode GM = null;

    //private ZombAttack Attacking;

    public Animator Anim;

    private bool Attacking;

    //Entity Dying;


    void start()
    {
        Anim = GetComponent<Animator>();
    }

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


        if (Agent.remainingDistance <= Agent.stoppingDistance)
        {
            base.FixedUpdate();
            Agent.destination = FollowObject.transform.position;

            Anim.SetBool("Walking", false);
            Anim.SetBool("Attacking", true);

            //Attacking = true;

            //StartCoroutine(ZombieAttack());
            //ZombieAttack();
        }

        else
        {
            Attacking = false;
            Anim.SetBool("Walking", true);
            Anim.SetBool("Attacking", false);
            Agent.destination = FollowObject.transform.position;

        }


    }

    /// Functions


    public void Return()
    {
        GM.GetPlayer(0).GetComponent<PlayersPoints>().AddPoints(100);
        //Anim.SetBool("Dying", true);
        GM.Despawn(Key, this.gameObject);

    }


    //void OnTriggerEnter(Collider Other)
    //{
    //    if (Other.gameObject.CompareTag("Player"))
    //    {
    //        if (Attacking == true)
    //        {
    //            Anim.SetBool("Attacking", true);
    //            Entity Health = transform.GetComponent<Entity>();
    //            Health.Health -= Damage;
    //        }
    //    }
    //}


        // Touch insta kill?

        // IEnumerator ZombieAttack()
        // {
        //     Anim.SetBool("Attacking", true);
        //
        //     yield return new WaitForSeconds(1);
        //
        //    if (Attacking == true)
        //    {
        //
        //
        //        
        //
        //        //RaycastHit Hit;
        //        //if (Physics.Raycast(Zombie.transform.position, Zombie.transform.forward, out Hit))
        //        //{
        //        //    Debug.DrawRay(Zombie.transform.position, Zombie.transform.forward, Color.red);
        //        //
        //        //    Entity Target = transform.GetComponent<Entity>();
        //        //    if (Target != null)
        //        //    {
        //        //        Target.TakeDamage(Damage);
        //        //    }
        //        //}
        //    }
        //      
        // }
        //
        //public void ZombieAttack()
        //{
        //    RaycastHit Hit;
        //    if (Physics.Raycast(Zombies.transform.position, Zombies.transform.forward, out Hit))
        //    {
        //        Debug.DrawRay(Zombies.transform.position, Zombies.transform.forward, Color.red);
        //
        //        Entity Target = Hit.transform.GetComponent<Entity>();
        //        if (Target != null)
        //        {
        //            Target.TakeDamage(Damage);
        //        }
        //
        //        if (Hit.rigidbody != null)
        //        {
        //            Hit.rigidbody.AddForce(-Hit.normal * ImpactForce);
        //        }
        //    }
        //}


    
}
