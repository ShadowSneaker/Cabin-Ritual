using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunScript : MonoBehaviour
{
    //Inspector bool to be check for guns
    [Tooltip("Is it an aotomatic gun?")]
    [SerializeField]
    public bool ToggleFullAuto = false;

    //Damage variable 
    public float Damage = 10f;
    //Headshot Damage variable
    //public float HeadshotDamage = 20f;
    //Range variable
    public float Range = 100f;
    //Speed of fire variable
    public float FireRate = 15f;
    //force on target object variable
    public float ImpactForce = 30f;

    //Max ammo for gun vairable
    public int MaxAmmo = 10;
    //Current ammo of gun
    public int CurrentAmmo;
    //How long it takes to reload
    public float ReloadTime = 1f;
    //Is the gun reloading bool
    private bool IsReloading = false;

    public Camera FpsCam;
    public ParticleSystem MuzzleFlash;
    public GameObject ImpactEffect;    
    public Animator Animator;
    public Text AmmoAmount;

    private float NextTimeToFire = 0f;


    void Start()
    {        
        if (CurrentAmmo == -1)
            CurrentAmmo = MaxAmmo;
    }

    void OnEnable()
    {
        IsReloading = false;
        Animator.SetBool("Reloading", false);
    }

    
    void Update()
    {
        //Displays ammo on the screen
        if (AmmoAmount)
        AmmoAmount.text = CurrentAmmo.ToString();

        if(IsReloading)
        {
            return;
        }
        
        //Srarts the reloading sequence
        if(CurrentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        //Allows the player to reload gun by pressing R as long as the player isnt on max ammo
        if (Input.GetKeyDown(KeyCode.R) && CurrentAmmo != MaxAmmo)
        {
            StartCoroutine(Reload());
            return;
        }

        //Checks to see if the bool for automatic gun is checked
        if (ToggleFullAuto == true)
        {
            //Automatic guns fire
            if (Input.GetButton("Fire1") && Time.time >= NextTimeToFire)
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
        else
        {
            //Non automatic guns fire
            if (Input.GetButtonDown("Fire1") && Time.time >= NextTimeToFire)
            {
                //How fast the gun can fire
                NextTimeToFire = Time.time + 1f / FireRate;

                //Animator.SetBool("Fire", true);

                //Calls function
                Shoot();

                GetComponent<AudioSource>().Play();

                
            }

            if (Input.GetButtonUp("Fire1") == true)
            {
                GetComponent<AudioSource>().Stop();
                //Animator.SetBool("Fire", false);
            }
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

    IEnumerator Reload()
    { 

        IsReloading = true;
       // Animator.SetBool("Fire", false);
        Debug.Log("Reloading");

        GetComponent<AudioSource>().Stop();

        Animator.SetBool("Reloading", true);
    
        yield return new WaitForSeconds(ReloadTime - .25f);
    
        GetComponent<AudioSource>().Stop();
    
        Animator.SetBool("Reloading", false);
    
        yield return new WaitForSeconds(.25f);
        

        CurrentAmmo = MaxAmmo;
    
        IsReloading = false;
    }

    //IEnumerator GunFire()
    //{
    //    yield return new WaitForSeconds(FireRate - .25f);
    //}

}
