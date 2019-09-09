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
    public int Damage = 10;
    //Headshot Damage variable
    //public float HeadshotDamage = 20f;
    //Range variable
    public float Range = 100f;
    //Speed of fire variable
    public float FireRate = 15f;
    //force on Target object variable
    public float ImpactForce = 30f;

    [Header("Ammo")]

    //[Tooltip("The total amount of ammo this gun has.")]
    [DisplayWithoutEdit()]
    [SerializeField]
    private int TotalAmmo = 0;

    //Max ammo for gun vairable
    public int MaxAmmo = 10;
    //Current ammo of gun
    public int CurrentAmmo;
    //Amount of gun Clips
    public int Clips = 4;
    //Bullets per clip
    public int BulletsPerClip;


    //How long it takes to reload
    public float ReloadTime = 1f;
    //Is the gun reloading bool
    private bool IsReloading = false;

    [Tooltip("The sound that plays when the user fires the gun.")]
    [SerializeField]
    private AudioClip FireSound;

    [Tooltip("The sound that plays when the user is reloading.")]
    [SerializeField]
    private AudioClip ReloadSound;

    [Tooltip("The sound that plays when the user fires with no ammo.")]
    [SerializeField]
    private AudioClip EmptyClipSound;

    [Tooltip("")]
    [SerializeField]
    private AudioClip CockSound;

    // A reference to the audio source.
    private AudioSource Audio = null;


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

        Audio = GetComponent<AudioSource>();
        if (!Audio) Debug.LogWarning("Warning: Audio source not found.");
    }


    void OnValidate()
    {
        TotalAmmo = Clips * BulletsPerClip;
    }

    void OnEnable()
    {
        IsReloading = false;
        //Animator.SetBool("Reloading", false);
    }

    
    //void Update()
    //{
    //   
    //        //Displays ammo on the screen
    //        if (AmmoAmount)
    //            AmmoAmount.text = CurrentAmmo.ToString();
    //
    //        //if (IsReloading)
    //        //{
    //        //    return;
    //        //}
    //
    //        //Srarts the reloading sequence
    //        if (CurrentAmmo <= 0)
    //        {
    //            StartCoroutine(Reload());
    //            return;
    //        }
    //
    //        //Allows the player to reload gun by pressing R as long as the player isnt on max ammo
    //        if (Input.GetKeyDown(KeyCode.R) && CurrentAmmo != MaxAmmo)
    //        {
    //            StartCoroutine(Reload());
    //            return;
    //        }
    //
    //        //Checks to see if the bool for automatic gun is checked
    //        if (ToggleFullAuto == true)
    //        {
    //            //Automatic guns fire
    //            if (Input.GetButton("Fire1") && Time.time >= NextTimeToFire)
    //            {
    //                //How fast the gun can fire
    //                NextTimeToFire = Time.time + 1f / FireRate;
    //
    //                //Calls function
    //                Shoot();
    //
    //                GetComponent<AudioSource>().Play();
    //            }
    //
    //            if (Input.GetButtonUp("Fire1") == true)
    //            {
    //                GetComponent<AudioSource>().Stop();
    //            }
    //        }
    //        else
    //        {
    //            //Non automatic guns fire
    //            if (Input.GetButtonDown("Fire1") && Time.time >= NextTimeToFire)
    //            {
    //                //How fast the gun can fire
    //                NextTimeToFire = Time.time + 1f / FireRate;
    //
    //                //Animator.SetBool("Fire", true);
    //
    //                //Calls function
    //                Shoot();
    //
    //                GetComponent<AudioSource>().Play();
    //
    //
    //            }
    //
    //            if (Input.GetButtonUp("Fire1") == true)
    //            {
    //                GetComponent<AudioSource>().Stop();
    //                //Animator.SetBool("Fire", false);
    //            }
    //        }
    //    
    //}

    public virtual void Shoot()
    {
        if (CurrentAmmo > 0)
        {
            //Plays particle affect on fire
            MuzzleFlash.Play();

            --CurrentAmmo;
            NextTimeToFire = Time.time + 1f / FireRate;

            Audio.clip = FireSound;
            Audio.Play();
            Animator.SetBool("Fire", true);

            RaycastHit hit;
            if (Physics.Raycast(FpsCam.transform.position, FpsCam.transform.forward, out hit, Range))
            {
                Debug.DrawRay(FpsCam.transform.position, FpsCam.transform.forward, Color.red);

                //Does damage to 'Target'
                Entity Target = hit.transform.GetComponent<Entity>();
                if (Target != null)
                {
                    Target.TakeDamage(Damage);
                }


                //Causes force on object pusing it away from player
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * ImpactForce);
                }

                //Puts effect at bullet place of impact then delets from hierachy after 2 seconds
                GameObject impact = Instantiate(ImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impact, 1f);
            }
        }
        else
        {
            if (TotalAmmo > 0)
            {
                Reload();
            }
            else
            {
                // Play empty clip sound.
                Audio.clip = EmptyClipSound;
                Audio.Play();
            }
        }
    }


    // Used just to make reloading more streemlined instead of calling a coroutine.
    public void Reload()
    {
        StartCoroutine(ReloadTimer());
    }


    IEnumerator ReloadTimer()
    {
        Audio.clip = ReloadSound;
        Audio.Play();

        Debug.Log("Reloading");

        IsReloading = true;
       // Animator.SetBool("Fire", false);

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

    public virtual void StopShooting()
    {
        //GetComponent<AudioSource>().Stop();
        Animator.SetBool("Fire", false);
    }

}
