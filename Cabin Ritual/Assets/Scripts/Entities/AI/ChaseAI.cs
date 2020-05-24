using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAI : AIBase
{

    [Header("Zombie attack")]

    [Tooltip("The amount of damage that is applied to the hit object.")]
    [SerializeField]
    public int Damage = 10;

    [Tooltip("The time before can attack again.")]
    [SerializeField]
    public int AttackCoolDown = 10;

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

    private bool Attacking = false;

    private bool AttackAgain = false;
    private bool PlayerInRange = false;

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
        float Distance = Vector3.Distance(FollowObject.position, transform.position);

        if (Distance <= Agent.stoppingDistance)
        {
            base.FixedUpdate();
            Agent.destination = FollowObject.transform.position;

            Anim.SetBool("Walking", false);
            Anim.SetTrigger("Attacking");

            PlayerInRange = true;

            Attacking = true;

            

            //StartCoroutine(ZombieAttack());
            //ZombieAttack();
        }

        else
        {
            Attacking = false;
            Anim.SetBool("Walking", true);
            Agent.destination = FollowObject.transform.position;
            PlayerInRange = false;
        }
    }

    /// Functions


    public void Return()
    {
        GM.GetPlayer(0).GetComponent<PlayersPoints>().AddPoints(100);
        GM.GetPlayer(0).GetComponent<PlayersPoints>().AddKill();
        GM.Despawn(Key, this.gameObject);
    }

    void Attack()
    {
        Debug.Log ("here");
        float Distance = Vector3.Distance(FollowObject.position, transform.position);
        if (Distance <= Agent.stoppingDistance)
        {
            if (PlayerInRange)
            {
                FollowObject.transform.GetComponent<Entity>().TakeDamage(Damage);
            }
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    //StartCoroutine(Invunrable());
    //
    //    if ((other.gameObject.tag == "Player"))
    //    {
    //        other.gameObject.GetComponent<Entity>().TakeDamage(Damage);
    //        //other.gameObject.GetComponent<Entity>().Health -= Damage;            
    //    }
    //}

    //IEnumerator Invunrable()
    //{
    //    yield return new WaitForSeconds(2);
    //
    //    AttackAgain = true;
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
