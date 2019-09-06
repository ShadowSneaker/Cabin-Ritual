using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public float Damage = 10f;
    public float Range = 100f;
    public float FireRate = 15f;
    public float ImpactForce = 30f;

    public int MaxAmmo = 10;
    public int CurrentAmmo;
    public float ReloadTime = 1f;

    public Camera FpsCam;
    public ParticleSystem MuzzleFlash;
    public GameObject ImpactEffect;

    private float NextTimeToFire = 0f;


    void Start()
    {
        if (CurrentAmmo == -1)
            CurrentAmmo = MaxAmmo;
    }
    // Update is called once per frame
    void Update()
    {

        if(CurrentAmmo <= 0)
        {
            Reload();
            return;
        }

        if(Input.GetButtonDown("Fire1")&& Time.time >= NextTimeToFire)
        {
            //How fast the gun can fire
            NextTimeToFire = Time.time + 1f / FireRate; 

            //Calls function
            Shoot();

            GetComponent<AudioSource>().Play();
        }

        if (Input.GetButtonUp("Fire1") == true)
        {
            GetComponent<AudioSource>().Stop();
        }
    }

    void Shoot()
    {
        //Plays particle affect on fire
        MuzzleFlash.Play();

        CurrentAmmo--;

        RaycastHit hit;
        if(Physics.Raycast(FpsCam.transform.position, FpsCam.transform.forward, out hit, Range))
        {
            Debug.DrawRay(FpsCam.transform.position, FpsCam.transform.forward, Color.red);
            Debug.Log(hit.transform.name);

            //Does damage to 'target'
            Target target = hit.transform.GetComponent<Target>();
            if(target != null)
            {
                target.TakeDamage(Damage);
            }

            //Causes force on object pusing it away from player
            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * ImpactForce);
            }

            //Puts effect at bullet place of impact then delets from hierachy after 2 seconds
            GameObject impact = Instantiate(ImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 1f);
        }
    }

    void Reload()
    {
        Debug.Log("Reloading");
        CurrentAmmo = MaxAmmo;

    }
}
