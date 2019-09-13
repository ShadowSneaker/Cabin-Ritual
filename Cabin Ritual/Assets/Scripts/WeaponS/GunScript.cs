using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunScript : MonoBehaviour
{
    [Header("Gun Stats")]

    [DisplayWithoutEdit()]
    [SerializeField]
    private bool IsFiring = false;

    [Tooltip("Represents if the gun can fire.")]
    [SerializeField]
    protected bool Active = false;

    [Tooltip("The amount of damage that is applied to the hit object.")]
    [SerializeField]
    public int Damage = 10;

    [Tooltip("The distance the bullet travels when shot.")]
    [SerializeField]
    public float Range = 100.0f;

    [Tooltip("Speed of fire variable (in bullets per second).")]
    [SerializeField]
    public float FireRate = 15.0f;

    [Tooltip("Determines how much this weapon sways while holding the weapon. (Currently doesn't work).")]
    [SerializeField]
    private float WeaponSway = 0.0f;


    [Tooltip("Force applied on the hit object.")]
    [SerializeField]
    public float ImpactForce = 30.0f;


    //[Header("Accuracy")]
    

    private float SpreadIncreaseSpeed = 5.0f;

    private float SpreadDecreaseSpeed = 2.0f;

    private float SpreadMax = 10.0f;

    private float SpreadMin = 0.0f;

    private float CurrentSpread = 0.0f;

    [Tooltip("How close to the center of the screen the bullet will hit (Goes from 0 - 100).")]
    [SerializeField]
    private float Accuracy = 80.0f;


    [Header("Ammo")]



    // The total ammo this gun has in reserve.
    [DisplayWithoutEdit()]
    [SerializeField]
    private int TotalAmmo = 0;

    [Tooltip("Should this gun consume ammo when fired.")]
    [SerializeField]
    private bool ConsumeAmmo = true;

    [Tooltip("The amount of bullets per clip.")]
    [SerializeField]
    private int ClipSize = 10;
    
    [Tooltip("The current amount of ammo in the clip.")]
    [SerializeField]
    private int CurrentAmmo;

    [Tooltip("How many clips this gun has.")]
    [SerializeField]
    private int Clips = 4;


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

    // This probably shouldn't be here but ok.
    public GameObject ImpactEffect;

    // A referance to the animator script on this gun.
    private Animator Anim;

    // This also probably shouldn't be here. The UI should just know what gun is currently selected and display that.
    public Text AmmoAmount;

    //private float NextTimeToFire = 0f;

    


    void Awake()
    {        
        if (CurrentAmmo == -1)
            CurrentAmmo = ClipSize;

        Anim = GetComponent<Animator>();
        if (!Anim) Debug.LogWarning("Warning: Animator not found.");

        Audio = GetComponent<AudioSource>();
        if (!Audio) Debug.LogWarning("Warning: Audio source not found.");
    }


    void OnValidate()
    {
        TotalAmmo = Clips * ClipSize;
    }

    void OnEnable()
    {
        IsReloading = false;
        Anim.SetBool("Reloading", false);
    }


    private void FixedUpdate()
    {
        if (CurrentSpread >= SpreadMin)
        {
            ApplySpread(Time.fixedDeltaTime * SpreadDecreaseSpeed * -1.0f);
        }
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
        if (Active)
        {
            if (AmmoAmount)
                AmmoAmount.text = CurrentAmmo.ToString() + ("/10");

                if (ConsumeAmmo || CurrentAmmo > 0)
            {
                IsFiring = true;
                Anim.SetBool("Fire", true);
            }
        }
    }


    public virtual void StopShooting()
    {
        Anim = GetComponent<Animator>();
        if (!Anim) Debug.LogWarning("Warning: Animator not found.");
        IsFiring = false;
        Anim.SetBool("Fire", false);
    }


    protected void FireDelay(float Delay)
    {
        StartCoroutine(FireDelayTimer(Delay));
    }


    private IEnumerator FireDelayTimer(float Delay)
    {
        yield return new WaitForSeconds(Delay);
        Active = true;
    }


    protected void ShootBullet(int ProjectileIndex = 0, bool ShouldConsumeAmmo = true)
    {
        if (!ConsumeAmmo ||CurrentAmmo > 0)
        {
            CaluclateAccuracy();
            if (CheckCollision())
            {
            
                RaycastHit Hit;
                if (Physics.Raycast(FpsCam.transform.position, FpsCam.transform.forward, out Hit, Range))
                {
                    Debug.DrawRay(FpsCam.transform.position, FpsCam.transform.forward, Color.red);

                    Entity Target = Hit.transform.GetComponent<Entity>();
                    if (Target != null)
                    {
                        Target.TakeDamage(Damage);
                    }

                    if (Hit.rigidbody != null)
                    {
                        Hit.rigidbody.AddForce(-Hit.normal * ImpactForce);
                    }

                    GameObject Impact = Instantiate(ImpactEffect, Hit.point, Quaternion.LookRotation(Hit.normal));
                    Destroy(Impact, 1.0f);
                }
            }

            Audio.clip = FireSound;
            Audio.Play();

            if (ConsumeAmmo && ShouldConsumeAmmo)
            {
                --CurrentAmmo;
            }

        }
        else
        {
            IsFiring = false;
            Reload();
        }






        //if (CurrentAmmo > 0)
        //{
        //    //Plays particle affect on fire
        //    MuzzleFlash.Play();
        //
        //    --CurrentAmmo;
        //    //NextTimeToFire = Time.time + 1f / FireRate;
        //
        //    Audio.clip = FireSound;
        //    Audio.Play();
        //    Anim.SetBool("Fire", true);
        //
        //    RaycastHit hit;
        //    if (Physics.Raycast(FpsCam.transform.position, FpsCam.transform.forward, out hit, Range))
        //    {
        //        Debug.DrawRay(FpsCam.transform.position, FpsCam.transform.forward, Color.red);
        //
        //        //Does damage to 'Target'
        //        Entity Target = hit.transform.GetComponent<Entity>();
        //        if (Target != null)
        //        {
        //            Target.TakeDamage(Damage);
        //        }
        //
        //
        //        //Causes force on object pusing it away from player
        //        if (hit.rigidbody != null)
        //        {
        //            hit.rigidbody.AddForce(-hit.normal * ImpactForce);
        //        }
        //
        //        //Puts effect at bullet place of impact then delets from hierachy after 2 seconds
        //        GameObject impact = Instantiate(ImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        //        Destroy(impact, 1f);
        //    }
        //}
        //else
        //{
        //    if (TotalAmmo > 0)
        //    {
        //        Reload();
        //    }
        //    else
        //    {
        //        // Play empty clip sound.
        //        Audio.clip = EmptyClipSound;
        //        Audio.Play();
        //    }
        //}
    }


    // Used just to make reloading more streemlined instead of calling a coroutine.
    public void Reload()
    {
        if (CurrentAmmo != ClipSize && TotalAmmo > 0 || Input.GetKeyDown(KeyCode.R))
        {
            Active = false;

            Audio.clip = ReloadSound;
            Audio.Play();

            Anim.SetBool("Reloading", true);

            StartCoroutine(ReloadTimer());
        }
    }  
  


    IEnumerator ReloadTimer()
    {
        yield return new WaitForSeconds(ReloadTime);
        Active = true;

        if (TotalAmmo >= ClipSize || (CurrentAmmo + TotalAmmo) > ClipSize)
        {
            CurrentAmmo = ClipSize;
            TotalAmmo -= ClipSize - CurrentAmmo;
        }
        else
        {
            if (CurrentAmmo + TotalAmmo <= ClipSize)
            {
                CurrentAmmo += TotalAmmo;
                TotalAmmo = 0;
            }
            else
            {
                CurrentAmmo = TotalAmmo;
                TotalAmmo = 0;
            }
        }

        Anim.SetBool("Reloading", false);




        //Audio.clip = ReloadSound;
        //Audio.Play();
        //
        //Debug.Log("Reloading");
        //
        //IsReloading = true;
        //Animator.SetBool("Fire", false);
        //
        //GetComponent<AudioSource>().Stop();
        //
        //Animator.SetBool("Reloading", true);
        //
        //yield return new WaitForSeconds(ReloadTime - .25f);
        //
        //GetComponent<AudioSource>().Stop();
        //
        //Animator.SetBool("Reloading", false);
        //
        //yield return new WaitForSeconds(.25f);
        //
        //
        //CurrentAmmo = MaxAmmo;
        //
        //IsReloading = false;
    }    

    //IEnumerator GunFire()
    //{
    //    yield return new WaitForSeconds(FireRate - .25f);
    //}

    // Returns if the user can shoot.
    private bool CheckCollision()
    {
        // TODO.
        return true;
    }


    private void ApplySpread(float Amount)
    {
        CurrentSpread = Mathf.Clamp(Amount + CurrentSpread, SpreadMin, SpreadMax);
    }


    private void CaluclateAccuracy()
    {
        SpreadMin = 100 - Accuracy;
        SpreadMax = 100 - (Accuracy + 20.0f);
    }


    protected void IncreaseSpread()
    {
        CurrentSpread = Mathf.Clamp(SpreadIncreaseSpeed + CurrentSpread, SpreadMin, SpreadMax);
    }


    private float GetSpread()
    {
        return CurrentSpread / 10.0f;
    }


    protected bool CanFire()
    {
        return Active && IsFiring;
    }


    // Gets the delay between each bullet when firing.
    protected float GetFireRate()
    {
        return 60.0f / FireRate / 60.0f;
    }
}
