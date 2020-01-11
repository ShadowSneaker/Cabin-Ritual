using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    [Tooltip("Should this gun consume bullets when fired.")]
    [SerializeField]
    private bool InfiniteAmmo = false;

    [Tooltip("Should this gun consume clips when reloading.")]
    [SerializeField]
    private bool InfiniteClips = false;

    [Tooltip("Represents if this weapon can fire.")]
    [SerializeField]
    protected bool Active = true;



    [Header("Gun Stats")]


    [Tooltip("The total amount of ammo the gun has (This value should not be edited by the user unless giving the player more ammo during runtime through the editor).")]
    [SerializeField]
    private int TotalAmmo = 0;

    [Tooltip("How many times this gun can shoot before needing to reload.")]
    [SerializeField]
    private int CurrentAmmo = 10;

    [Tooltip("The maximum amount of ammo the gun has in the clip.")]
    [SerializeField]
    private int ClipSize = 10;

    [Tooltip("How many clips the user has (For total ammo: (ClipCount * ClipSize) + CurrentAmmo).")]
    [SerializeField]
    private int ClipCount = 3;

    [Tooltip("The amount of damage this gun will deal to the hit object.")]
    [SerializeField]
    private int Damage = 10;

    [Tooltip("The increased damage this gun does on critical hits (Note this is multiplied onto the damage value).")]
    [SerializeField]
    private float CriticalDamage = 1.5f;

    [Tooltip("Represents the amount of sway this weapon has (the higher the value the worse the swaying is).")]
    [SerializeField]
    private float Stability = 30.0f;

    [Tooltip("Represents how much this gun kicks up the camera.")]
    [SerializeField]
    private float Recoil = 10.0f;

    [Tooltip("How far this weapon can shoot.")]
    [SerializeField]
    private float Range = 50.0f;

    [Tooltip("How long it takes for this gun to reload (In seconds).")]
    [SerializeField]
    private float ReloadSpeed = 0.2f;

    [Tooltip("How fast ths gun shoots (in bps).")]
    [SerializeField]
    protected float FireRate = 10;

    [Tooltip("Determines the how zoomed in the scope is.")]
    [SerializeField]
    private float ADSAmount = 10.0f;



    [Header("Projectiles")]


    [Tooltip("Should the gun only shoot the specified projectile index.")]
    [SerializeField]
    private bool OnlyShootIndex = true;

    [Tooltip("The index that this gun should shoot (Only used if OnlyShootIndex is true).")]
    [SerializeField]
    private int ProjectileIndex = 0;

    // TODO: Create a projectile class and set this to that.
    [Tooltip("The types of projectiles that this weapon can spawn.")]
    [SerializeField]
    private List<ProjectileScript> ProjectileTypes = null;

    [Tooltip("The location where the bullet spawns")]
    [SerializeField]
    private Transform MuzzleLocation = null;



    [Header("Accuracy")]


    [Tooltip("How close to the center of the screen this gun shoots (Range is 0 - 100, where 100 is perfect accuracy).")]
    [SerializeField]
    private float Accuracy = 50.0f;

    [Tooltip("The speed in which the gun will lose accuracy per shot.")]
    [SerializeField]
    private float SpreadIncreaseSpeed = 0.2f;

    [Tooltip("The speed in which the gun will regain accuracy while not shooting.")]
    [SerializeField]
    private float SpreadDecreaseSpeed = 0.001f;



    [Header("Sounds")]


    [Tooltip("The sound that this gun makes when it fires.")]
    [SerializeField]
    private AudioClip FireSound = null;

    [Tooltip("The sound that htis gun makes when it reloads.")]
    [SerializeField]
    private AudioClip ReloadSound = null;

    [Tooltip("The sound that this gun makes when the user shoots, but there is no ammo to shoot.")]
    [SerializeField]
    private AudioClip EmptyClipSound = null;

    // A reference of the attached AudioSource component.
    // If this is not set then the sound will be played at the gun location.
    private AudioSource Audio = null;


    [Header("Particles")]

    [Tooltip("The particle system for the muzzle flash")]
    [SerializeField]
    private ParticleSystem MuzzleFlash = null;



    /// Private members


    // Set on Start(), Determines the maximum amount of spread this gun can have. Calculated off of Accuracy.
    private float MaxSpread = 0.0f;

    // Set on Start(), Determines the current amount of spread this gun will shoot with.
    private float CurrentSpread = 0.0f;

    // Set on Start(), Determines the lowest amount of spread this gun can have. Calculated off of Accuracy.
    private float MinSpread = 0.0f;

    // Represents if this gun is trying to fire.
    protected bool Firing = false;

    // A reference to the owner of this gun.
    private GunHolder Owner = null;

    // Holds the results of the firing raycast.
    RaycastHit ObjectHit;

    // A reference to the Animator component.
    private Animator Anim;



    public void Reset()
    {
        CurrentAmmo = ClipSize;
        TotalAmmo = ClipSize * ClipCount;
    }


    // Updates the value in the editor so you can see the values update in real time.
    private void OnValidate()
    {
        CurrentAmmo = Mathf.Clamp(ClipSize, 0, ClipSize);
        TotalAmmo = ClipSize * ClipCount;
    }


    // Gets all the references to the core components.
    // Sets up the minimum and maximum spread amount.
    void Awake()
    {
        Anim = GetComponent<Animator>();
        if (!Anim) Debug.LogWarning("Warning: Animator component not found.");

        Audio = GetComponent<AudioSource>();


        CalculateAccuracy();
        CurrentSpread = MinSpread;


        // TODO: Remove this, this is just so I stop getting warnings that values aren't being used.
        Stability = ADSAmount;
        ADSAmount = Recoil;
        Recoil = Stability;
    }


    // Constantly decreases the bullet spread until the spread is equal to the minimum spread amount.
    private void FixedUpdate()
    {
        if (CurrentSpread > MinSpread)
        {
            ApplySpread(Time.fixedTime * SpreadDecreaseSpeed);
        }
    }


    // Fires the gun - Should be overrided in child classes to actually fire the gun.
    // Tells the gun that the user is trying to fire the gun assuming that the gun has
    // enough ammo (or infinite ammo) to fire.
    public virtual void Fire()
    {
        if (Active)
        {
            if (!InfiniteAmmo || CurrentAmmo > 0)
            {
                Firing = true;
            }

            if (TotalAmmo > 0)
            {
                if (CurrentAmmo <= 0)
                {
                    Reload();
                }
            }
            else
            {
                PlaySound(EmptyClipSound);
            }
        }
    }


    // Called when the user releases the trigger key.
    // Tells the gun that the user is no longer trying to shoot.
    public virtual void StopFiring()
    {
        Firing = false;
    }


    // Determines what should happen when the user presses right mouse button.
    // The defaut behavour will be to aim down sights however this can be
    // overwritten in child classes to have a different behavour (or extra behavour).
    public virtual void AltFire()
    {
        // TODO: Do Aim down sights.
        // Get owner.
        // Get a reference to the camera.
        if (Owner)
        {

        }
    }


    // Actually fires the projectile along with playing sounds and animation.
    // @param ProjectileIndexOverride - Overrides the current ProjectileIndex settings and forces that index to be fired. If the Value is < 0 then it will default to the ProjectileIndex settings.
    // @param ConsumeAmmo - Should this bullet take ammo away from the user (Used for weapons like shotguns that only consume 1 bullet per shell count).
    // @return - Returns a reference to the fired projectile.
    protected ProjectileScript ShootBullet(int ProjectileIndexOverride = -1, bool ConsumeAmmo = true)
    {
        if (CurrentAmmo > 0 || InfiniteAmmo)
        {
            Active = false;
            ProjectileScript Projectile;
            CalculateAccuracy();
            if (CheckCollision())
            {
                int Index = (ProjectileIndexOverride > -1) ? ProjectileIndexOverride : GetProjectileIndex();
                if (!ProjectileTypes[Index])
                {
                    Debug.Log("Warning: selected projectile index is not set.");
                    return null;
                }

                Projectile = Instantiate<ProjectileScript>(ProjectileTypes[Index], MuzzleLocation.position, MuzzleLocation.rotation);
                float RandomX = Random.Range(-CurrentSpread, CurrentSpread);
                float RandomY = Random.Range(-CurrentSpread, CurrentSpread);

                Projectile.transform.Rotate(RandomX, RandomY, 0.0f);

                Projectile.Damage = Damage;
                Projectile.CriticalDamage = CriticalDamage;
                Projectile.Range = Range;
                Projectile.SetOwner(Owner);
            }
            else
            {
                // Bullets should always spawn.
                // this is the code for bullet penetration.
                Projectile = null;
            }


            //Anim.SetBool("Firing", true);
            PlaySound(FireSound);

            if (MuzzleFlash)
            {
                MuzzleFlash.Play();
            }

            if (ConsumeAmmo && !InfiniteAmmo)
            {
                --CurrentAmmo;
            }
            

            return Projectile;
        }
        else
        {
            Firing = false;
            return null;
        }
    }


    // Starts the graphical setup for reloading and starts the timer for the actual reloaded values.
    // The current ammo will be set after the ReloadSpeed duration.
    // The gun can only reload if the current ammo is not equal to the clip size.
    public void Reload()
    {
        if (CurrentAmmo < ClipSize)
        {
            Active = false;
            PlaySound(ReloadSound);
            Anim.SetBool("Reloading", true);
            StartCoroutine(ReloadTimer());
        }
    }


    // Does the actual reloading. This will adjust the values in TotalAmmo and CurrentAmmo to reload the weapon.
    // There are two criteria's for reloading. 1: The gun has enough total ammo for a full reload. 2. The user does not have enough total ammo for a full reload.
    // In the first case, we want to only remove the amount that we take. So we find how much ammo is missing for the current clip and minus that off of the total ammo.
    // Then set the clip to be full.
    // In the second case, we need to add the leftover ammo to our current ammo count then reduce the total ammo to 0.
    private void RefillGun()
    {
        Active = true;
        if (TotalAmmo > (ClipSize - CurrentAmmo))
        {
            TotalAmmo -= (ClipSize - CurrentAmmo);
            CurrentAmmo = ClipSize;
        }
        else
        {
            CurrentAmmo += TotalAmmo;
            TotalAmmo = 0;
        }

        Anim.SetBool("Reloading", false);
    }


    // Checks to see if this gun is poking through a wall in world space.
    // casts a raycast from the barrel of the gun backwards towards the shoulder of the character.
    // @return - Returns true if the user's arm/gun is clipping through a wall.
    // Not currently done.
    private bool CheckCollision()
    {
        return true;
    }


    // Sets the spread for this guns accuracy.
    protected void ApplySpread(float Amount)
    {
        CurrentSpread = Mathf.Clamp(CurrentSpread - Amount, MinSpread, MaxSpread);
    }


    // Does some math to invert the percentage of the accuracy, then divides them by a constant value to reduce the spread amount to a reasonable amount.
    private void CalculateAccuracy()
    {
        const float DivisableFactor = 20.0f;
        MinSpread = Mathf.Clamp((100.0f - Accuracy) / DivisableFactor, 0.0f, 100.0f);
        MaxSpread = Mathf.Clamp((100.0f - (Accuracy - 20.0f)) / DivisableFactor, 0.0f, 100.0f);
    }


    // Gets the current spread value that is being calculated before launching the projectile.
    protected float GetSpread()
    {
        return CurrentSpread / 10.0f;
    }


    // Increases the spread amount of the gun. This is capped to the max spread value.
    protected void IncreaseSpread()
    {
        CurrentSpread = Mathf.Clamp(SpreadIncreaseSpeed + CurrentSpread, MinSpread, MaxSpread);
    }



    public void EndAnimation()
    {
        if (Anim.GetBool("Firing") == true)
        {
            Anim.SetBool("Firing", false);
        }
    }


    // Starts the coroutine for the delay of the fire delay. Additionally prevents the user from firing until the timer is over.
    protected void StartFireDelay()
    {
        Active = false;
        StartCoroutine(FireDelay());
    }


    // Just waits for the timer to finish before refilling the gun.
    private IEnumerator ReloadTimer()
    {
        yield return new WaitForSeconds(ReloadSpeed);
        RefillGun();
    }


    // The coroutine for the delay between each bullet. Enables the gun when completed.
    private IEnumerator FireDelay()
    {
        yield return new WaitForSeconds(GetFireRate());
        Active = true;
    }


    // Plays an inputted sound clip.
    // Clip - The clip that should be played.
    protected void PlaySound(AudioClip Clip)
    {
        if (Clip)
        {
            if (Audio)
            {
                Audio.clip = Clip;
                Audio.Play();
            }
            else
            {
                AudioSource.PlayClipAtPoint(Clip, transform.position);
            }
        }
    }


    // Checks to see if the gun can fire.
    public bool CanFire()
    {
        return Firing && Active;
    }


    // Converts the user inputted firerate into the time between each shot.
    protected float GetFireRate()
    {
        return 60.0f / FireRate / 60.0f;
    }


    // Finds what projectile should be fired.
    protected int GetProjectileIndex()
    {
        return(OnlyShootIndex) ? ProjectileIndex : Random.Range(0, ProjectileTypes.Count);
    }


    public float GetCurrentAmmo()
    {
        return CurrentAmmo;
    }


    public float GetTotalAmmo()
    {
        return TotalAmmo;
    }


    // Sets the owner of this weapon.
    public void SetOwner(GunHolder NewOwner)
    {
        Owner = NewOwner;
    }


    // Returns the maximum amount of ammo this gun has.
    // (ClipSize * ClipCount)
    // Used to determine if ammo has been used yet in this gun.
    public int GetMaximumAmmo()
    {
        return ClipSize * ClipCount;
    }
}
