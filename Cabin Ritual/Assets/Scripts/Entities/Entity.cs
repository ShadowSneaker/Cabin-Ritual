using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public struct Stat
{
    ///  Properties

    //[Header("Stat")]

    [Tooltip("Forces the Value and MaxValue to equal the same thing (Prioritieses MaxValue).")]
    public bool MatchValues;

    [Tooltip("The current value of this stat.")]
    public float Value;

    [Tooltip("The maximum value of this stat.")]
    public float MaxValue;



    [Header("Regeneration")]

    [Tooltip("Should this stat be able to regenerate?")]
    public bool CanRegen;

    [Tooltip("How long this entity needs to wait before health starts regenerating (in seconds).")]
    public float RegenDelay;

    [Tooltip("How fast this entity regenerates health per second.")]
    public float RegenRate;


    // Determines if this stat is regenerating.
    public bool IsRegenerating;



    /// Functions

    // Updates the value of this stat to the max stat.
    // Also updates the value if the max stat is lower than the value.
    // Note, this is only used in OnValidate().
    public void Update()
    {
        if (MatchValues)
        {
            Value = MaxValue;

            
        }


        if (Value > MaxValue)
        {
            Value = MaxValue;
        }
    }


    // Quick way to reset this stat's value to the max value.
    public void Reset()
    {
        Value = MaxValue;
    }


    // Regenerates the stat for a single frame.
    // Note: This should be called on FixedUpdate for performace improvements.
    // If ran on Update the stat will regen at a quicker rate.
    public void RegenFrame()
    {
        Value = Mathf.Clamp(Value + (RegenRate * Time.fixedDeltaTime), 0.0f, MaxValue);
    }
};


[RequireComponent(typeof(CharacterController))]
public class Entity : MonoBehaviour
{
    /// Properties


    [Header("Health")]

    // Represents if this entity is dead.
    [DisplayWithoutEdit()]
    public bool IsDead;

    [DisplayWithoutEdit()]
    public bool IsDown;

    [DisplayWithoutEdit()]
    public int DownCount;

    [Tooltip("Should this entity be allowed to get knocked down.")]
    [SerializeField]
    private bool HasDownState = false;
    

    [Tooltip("Should this entity be allowed to be damaged?")]
    public bool Invulnerable;

    [Tooltip("Represents how often this entity can be hit.")]
    public int Health = 3;

    [Tooltip("The maximum amount of health this entity has.")]
    [SerializeField]
    private int MaxHealth = 3;

    [Tooltip("How long this entity is immune from damage when attacked.")]
    public float ImmunityFrames = 0.5f;

    // Determines if this entity is currently immune.
    private bool IsImmune;



    [Header("Stamina")]

    [Tooltip("Should this entity consume stamina?")]
    public bool ConsumeStamina = true;

    [Tooltip("This entity's stamina value.")]
    public Stat Stamina;

    [Tooltip("How quickly stamina is depleated while sprinting (in Points per second).")]
    [SerializeField]
    private float SprintDecay = 3.0f;


    [Header("Movement")]

    // Determines if this entity is currently sprinting.
    [DisplayWithoutEdit()]
    [SerializeField]
    private bool Sprinting = false;

    // Determines if this entity is currently walking.
    [DisplayWithoutEdit()]
    [SerializeField]
    private bool Walking = false;

    // Determines if this entity is currently crouching.
    [DisplayWithoutEdit()]
    [SerializeField]
    private bool Crouching = false;

    // The speed this entity is currently moving at.
    [DisplayWithoutEdit()]
    [SerializeField]
    private float CurrentSpeed;

    [Tooltip("How fast the entity can walk.")]
    [SerializeField]
    private float WalkSpeed = 3.0f;

    [Tooltip("The default run speed. This is used when the player isn't holding the sprint button or walk button.")]
    [SerializeField]
    private float RunSpeed = 5.0f;

    [Tooltip("How fast the entity can sprint.")]
    [SerializeField]
    private float SprintSpeed = 10.0f;

    [Tooltip("How fast the entity moves while crouching.")]
    [SerializeField]
    private float CrouchSpeed = 2.5f;

    [Tooltip("How far down this entity should crouch")]
    [SerializeField]
    private float CrouchHeight = 0.4f;

    [Tooltip("How high this entity can jump.")]
    [SerializeField]
    private float JumpStrength = 5.0f;

    [Tooltip("How much influence gravity has on this entity")]
    [SerializeField]
    private float GravityScale = 1.0f;

    [Tooltip("Units that the entity can fall before a falling function is ran.")]
    [SerializeField]
    private float FallingThreshold = 10.0f;


    [Space(10)]


    [Tooltip("Should the entity slide down slopes steeper than the SlopeSlideAngle?")]
    [SerializeField]
    private bool SlideWhenOverSlopeLimit = true;

    [Tooltip("Should this entity be forced to slide on objects with the 'Slide' tag?")]
    [SerializeField]
    private bool SlideOnTaggedObjects = false;

    [Tooltip("The speed this entity slides down slopes.")]
    [SerializeField]
    private float SlideSpeed = 12.0f;


    [Space(10)]


    [Tooltip("Should this entity be allowed to take fall damage?")]
    [SerializeField]
    private bool AllowFallDamage = true;

    [Tooltip("The amount of distance this entity will need to fall in order to be instantly killed when they hit the ground.")]
    [SerializeField]
    private float TerminalVelocity = 200.0f;

    [Tooltip("The minimum distance this entity will need to fall to start taking fall damage.")]
    [SerializeField]
    private float MinVelocity = 100.0f;

    [Tooltip("Should this entity be allowed to be controlled while in air?")]
    [SerializeField]
    private bool AirControl = true;

    [Tooltip("The percentage of control this entity should have while in air.")]
    [SerializeField]
    private float AirControlScale = 0.25f;

    [Tooltip("Small amounts of this results in bumping when walking down slopes, but large amounts results in falling too fast.")]
    [SerializeField]
    private float AntiBumpFactor = 0.75f;

    [Tooltip("The entity must be on the ground for this many physics frames before being able to jump again; Set to 0 to allow bunny hopping.")]
    [SerializeField]
    private int AntiBunnyHopFactor = 1;


    [Header("Sounds")]

    [Tooltip("The sound this entity makes when hurt.")]
    public AudioClip HurtSound;

    [Tooltip("The sound this entity makes when killed")]
    public AudioClip DeathSound;

    [Tooltip("The sound this entity makes when sprinting.")]
    public AudioClip SprintSound;



    [Header("Events")]

    [Tooltip("Runs when the entity dies.")]
    public UnityEvent OnDeath;

    [Tooltip("Runs when the entity gets hit.")]
    public UnityEvent OnHurt;

    [Tooltip("Runs when the entity is healed.")]
    public UnityEvent OnHeal;


    [Header("Other")]

    [Tooltip("A reference to the camera's transform")]
    public Transform Cam = null;


    private Vector3 MoveDirection = Vector3.zero;

    private bool Grounded = false;

    // A reference to the character controller component.
    private CharacterController Character;

    private RaycastHit Hit;
    private float FallStartLevel;
    private bool Falling;
    private float SlideLimit;
    private float RayDistance;
    private Vector3 ContactPoint;
    private bool AllowControl = false;
    private int JumpTimer;

    private float CrouchCameraHeight;
    private float CameraHeight;

    // A reference to the Audio Source component.
    private AudioSource Audio;

    // When true. Tries to stand every fixed frame until space above entity is clear.
    private bool AttemptStand = false;

    // Allows movement functions to work.
    private bool CanMove = true;




    /// Overridables


    // Updates values in editor.
    public void OnValidate()
    {
        CurrentSpeed = RunSpeed;
        
        Stamina.Update();
    }


    // Start is called before the first frame update
    void Start()
    {
        Character = GetComponent<CharacterController>();
        if (!Character) Debug.LogError("Error: Could not find the CharacterController component!");

        Audio = GetComponent<AudioSource>();
        //if (!Audio) Debug.LogError("Error: Could not find the AudioSource component!");


        // Initialise startup values.
        CurrentSpeed = RunSpeed;
        RayDistance = Character.height * 0.5f + Character.radius;
        SlideLimit = Character.slopeLimit;
        JumpTimer = AntiBunnyHopFactor;

        if (Cam)
        {
            CameraHeight = Cam.localPosition.y;
            CrouchCameraHeight = Cam.localPosition.y - CrouchHeight;
        }
    }


    private void FixedUpdate()
    {
        if (Stamina.IsRegenerating)
        {
            Stamina.RegenFrame();
        }


        if (AttemptStand)
        {
            UnCrouch();
        }


        if (Cam)
        {
            if (Crouching)
            {
                if (Cam.localPosition.y > CrouchCameraHeight)
                {
                    if (Cam.localPosition.y - (CrouchHeight * Time.deltaTime * 8.0f) < CrouchCameraHeight)
                    {
                        Cam.localPosition = new Vector3(Cam.localPosition.x, CrouchCameraHeight, Cam.localPosition.z);
                    }
                    else
                    {
                        Cam.localPosition -= new Vector3(0.0f, CrouchHeight * Time.deltaTime * 8.0f, 0.0f);
                    }
                }
            }
            else
            {
                if (Cam.localPosition.y < CameraHeight)
                {
                    if (Cam.localPosition.y + (CrouchHeight * Time.deltaTime * 8) > CameraHeight)
                    {
                        Cam.localPosition = new Vector3(Cam.localPosition.x, CameraHeight, Cam.localPosition.z);
                    }
                    else
                    {
                        Cam.localPosition += new Vector3(0.0f, CrouchHeight * Time.deltaTime * 8.0f, 0.0f);
                    }
                }
            }
        }
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        ContactPoint = hit.point;
    }


    /// Functions

    // Moves this entity in a specified direction.
    // Note: This multiplies the current speed to the direction.
    // @param Direction - The X, Y, Z direction this entity should move in.
    public void Move(float InputX, float InputY, bool Jump)
    {
        if (CanMove && !IsDead)
        {
            if (Sprinting && ConsumeStamina)
            {
                Stamina.Value = Mathf.Clamp(Stamina.Value - SprintDecay * Time.deltaTime, 0.0f, Stamina.MaxValue);

                if (Stamina.Value <= 0.0f)
                {
                    StopSprinting();
                }
            }

            float InputModifyFactor = (InputX != 0.0f && InputY != 0.0f) ? 0.7071f : 1.0f;

            if (Grounded)
            {
                bool Sliding = false;

                if (Physics.Raycast(transform.position, Vector3.down, out Hit, RayDistance))
                {
                    if (Vector3.Angle(Hit.normal, Vector3.up) > SlideLimit)
                    {
                        Sliding = true;
                    }
                }
                // Sometimes the raycast stright down from the center can faile on steep slopes
                // So if the above raycast didn't catch anything, raycast down from the stored ControllerCollideHit point instead.
                else
                {
                    Physics.Raycast(ContactPoint + Vector3.up, Vector3.down, out Hit);
                    if (Vector3.Angle(Hit.normal, Vector3.up) > SlideLimit)
                    {
                        Sliding = true;
                    }
                }


                if (Falling)
                {
                    Falling = false;
                    if (transform.position.y < FallStartLevel - FallingThreshold)
                    {
                        OnGroundHit(FallStartLevel - transform.position.y);
                    }
                }


                if ((Sliding && SlideWhenOverSlopeLimit) || (SlideOnTaggedObjects && Hit.collider.tag == "Slide"))
                {
                    Vector3 HitNormal = Hit.normal;
                    MoveDirection = new Vector3(HitNormal.x, -HitNormal.y, HitNormal.z);
                    Vector3.OrthoNormalize(ref HitNormal, ref MoveDirection);
                    MoveDirection *= SlideSpeed;
                    AllowControl = false;
                }
                else
                {
                    MoveDirection = new Vector3(InputX * InputModifyFactor, -AntiBumpFactor, InputY * InputModifyFactor);
                    MoveDirection = transform.TransformDirection(MoveDirection) * CurrentSpeed;
                    AllowControl = true;
                }

                // Jump only when the button has been released and the player has been grounded for a given number of frames.
                if (!Jump)
                {
                    ++JumpTimer;
                }
                else if (JumpTimer >= AntiBunnyHopFactor)
                {
                    MoveDirection.y = JumpStrength;
                    JumpTimer = 0;
                }
            }
            else
            {
                // If this entity stepped over a cliff or something, set the height at which we started falling.
                if (!Falling)
                {
                    Falling = true;
                    FallStartLevel = transform.position.y;
                }

                // If air controll is allowed, check movement but don't touch the y component.
                if (AirControl && AllowControl)
                {
                    // TODO: So the problem here is because I don't have any velocity value to add the percentile movement input to. At the moment it's the Inputted speed added by the percentile which means at a minimum it's 100% Speed.
                    // Changing this will forcably slow the character to the percentile which is also wrong.
                    // What I need to do is have a constant velocity that gets raised/lowered while in air that is changed by the input speed percentile.

                    // Something Like:
                    // MoveDirection = Mathf.Camp(Velocity + (InputX * (CurrentSpeed * AirControlScale) * InputModifyFactor, -MaxVel, MaxVel);

                    // However I'm not sure this is possible with this current setup. I will investigate this solution more in the future.

                    // This is just here to remove the warning that this variable isn't being used.
                    if (AirControlScale > 0.0f)
                    {

                    }

                    MoveDirection.x = InputX * CurrentSpeed * InputModifyFactor;
                    MoveDirection.z = InputY * CurrentSpeed * InputModifyFactor;
                    MoveDirection = transform.TransformDirection(MoveDirection);
                }
            }
        }


        // Apply gravity.
        MoveDirection.y += Physics.gravity.y * GravityScale * Time.deltaTime;

        // Move the entity, and set grounded true or false depending on wether we're standing on something.
        Grounded = (Character.Move(MoveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
    }


    private void OnGroundHit(float FallDistance)
    {
        // TODO: Create a terminal velocity property and a min velocity property to calculate fall damage.
        // Should also create a TakeFallDamage bool property.

        if (AllowFallDamage)
        {
            TakeDamage(Health * Mathf.RoundToInt((FallDistance - MinVelocity) / (TerminalVelocity - MinVelocity)));
        }
    }


    // Applies damage to this entity.
    // @param Damage - The amount of damage this entity will take.
    public void TakeDamage(int Damage = 1)
    {
        if (!Invulnerable && !IsImmune && Damage != 0)
        {
            Health -= Damage;

            if (Health <= 0)
            {
                if (HasDownState)
                {
                    if (DownCount > 0)
                    {
                        Die();
                    }
                    else
                    {
                        IsDown = true;
                        CanMove = false;
                        // TODO: Set to down animation.
                    }
                }
                else
                {
                    Die();
                }
            }
            else
            {
                if (OnHurt != null)
                {
                    OnHurt.Invoke();
                }

                Audio.clip = HurtSound;
                Audio.Play();
            }
        }
    }


    // Kills this entity.
    private void Die()
    {
        IsDead = true;

        Audio.clip = DeathSound;
        Audio.Play();


        if (OnDeath != null)
        {
            OnDeath.Invoke();
        }
    }


    public void Heal(int Amount = 1)
    {
        if (!IsDead && Amount != 0)
        {
            Health = Mathf.Clamp(Health + Amount, 0, MaxHealth);

            if (OnHeal != null)
            {
                OnHeal.Invoke();
            }
        }
    }


    public void HelpUp()
    {
        Heal();
        IsDown = false;
        CanMove = true;

        // TODO: Play getting up animation.
    }


    private void SetDamageDebuffs()
    {

    }


    public void Walk()
    {
        if (!Sprinting)
        {
            Walking = false;
            CurrentSpeed = WalkSpeed;
        }
    }


    public void StopWalking()
    {
        if (!Sprinting)
        {
            Walking = false;
            CurrentSpeed = RunSpeed;
        }
    }


    public void Sprint()
    {
        if (Stamina.Value > 0.0f && !Walking)
        {
            Stamina.IsRegenerating = false;
            CurrentSpeed = SprintSpeed;
            Sprinting = true;
        }
    }


    public void StopSprinting()
    {
        if (!Walking)
        {
            Sprinting = false;
            CurrentSpeed = RunSpeed;

            StartCoroutine(StartStaminaRegen());
        }
    }


    public void Crouch()
    {
        if (Character && !Crouching)
        {
            Character.height -= CrouchHeight;
            Character.center -= new Vector3(0.0f, CrouchHeight / 2.0f, 0.0f);
            Crouching = true;
            StopSprinting();
            StopWalking();
            CurrentSpeed = CrouchSpeed;
            AttemptStand = false;
        }
    }


    public void UnCrouch()
    {
        if (Crouching)
        {
            // Do raycast up to see if the entity can stand up
            if (!Physics.Raycast(transform.position, Vector3.up, Character.height + CrouchHeight / 2.0f))
            {
                Character.height += CrouchHeight;
                Character.center += new Vector3(0.0f, CrouchHeight / 2.0f, 0.0f);
                Crouching = false;
                CurrentSpeed = RunSpeed;
                AttemptStand = false;
            }
            else
            {
                AttemptStand = true;
            }
        }
    }


    private IEnumerator StartStaminaRegen()
    {
        yield return new WaitForSeconds(Stamina.RegenDelay);
        if (!Sprinting)
        {
            Stamina.IsRegenerating = true;
        }
    }


    // Resets all main stats of this entity.
    public void ResetEntity()
    {
        Health = MaxHealth;
        Stamina.Reset();
        IsDead = false;
        CurrentSpeed = RunSpeed;
        IsImmune = false;
    }


    // Returns true if this entity is currently crouching.
    public bool IsCrouching
    {
        get
        {
            return Crouching;
        }
    }


    // Returns true if this entity is currently sprinting.
    public bool IsSprinting
    {
        get
        {
            return Sprinting;
        }
    }


    // Returns true if this entity is currently walking.
    public bool IsWalking
    {
        get
        {
            return Walking;
        }
    }


    // Returns the current speed of this entity.
    public float GetSpeed
    {
        get
        {
            return CurrentSpeed;
        }
    }

}
