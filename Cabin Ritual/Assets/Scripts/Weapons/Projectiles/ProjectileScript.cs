using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [Tooltip("Determiens how fast this projectile moves.")]
    [SerializeField]
    private float Speed = 100.0f;

    public int Damage;
    public float CriticalDamage;
    public float Range;

    private GunHolder Owner;

    private Vector3 StartPos;

    protected bool Moving = true;

    private GameMode GM = null;

    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.position;
        GM = FindObjectOfType<GameMode>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Moving)
        {
            transform.position += transform.forward * Speed * Time.deltaTime;
            float Distance = Vector3.Distance(StartPos, transform.position);

            if (Distance >= Range)
            {
                Destroy(gameObject);
            }
        }
    }


    private void OnTriggerEnter(Collider Col)
    {
        Entity Other = Col.transform.GetComponent<Entity>();
        if (Other)
        {
            bool Crit = false;
            ///List<ContactPoint> Contacts = new List<ContactPoint>();
            //Col.GetContacts(Contacts);
            //for (int i = 0; i < Contacts.Count; ++i)
            //{
            //    if (Contacts[i].thisCollider.CompareTag("CritSpot"))
            //    {
            //        Crit = true;
            //    }
            //}
            Other.TakeDamage(Mathf.RoundToInt((Crit) ? Damage * CriticalDamage : Damage));
            GM.GetPlayer(0).GetComponent<PlayersPoints>().AddPoints(10);
        }
        Destroy(gameObject);
    }


    public void SetOwner(GunHolder NewOwner)
    {
        Owner = NewOwner;
    }
}
