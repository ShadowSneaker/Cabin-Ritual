using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public struct Stat
{
    ///  Properties

    //[Header("Stat")]

    [Tooltip("Updates the value of this stat to equal the maximum value.")]
    public bool UpdateStat;

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
        if (UpdateStat)
        {
            UpdateStat = false;
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

    [Tooltip("Should this entity be allowed to be damaged?")]
    public bool Invulnerable;

    [Tooltip("This entity's health value.")]
    public Stat Health;

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

    [Tooltip("Runs when the entity dies")]
    public UnityEvent OnDeath;


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
    



    /// Overridables

    
    // Updates values in editor.
    public void OnValidate()
    {
        CurrentSpeed = RunSpeed;

        Health.Update();
        Stamina.Update();
    }


    // Start is called before the first frame update
    void Start()
    {
        Character = GetComponent<CharacterController>();
        if (!Character) Debug.LogError("Error: Could not find the CharacterController component!");

        Audio = GetComponent<AudioSource>();
        if (!Audio) Debug.LogError("Error: Could not find the CharacterController component!");


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

        if (Health.IsRegenerating)
        {
            Health.RegenFrame();
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
            TakeDamage(Health.MaxValue * ((FallDistance - MinVelocity) / (TerminalVelocity - MinVelocity)));
        }
    }


    // Applies damage to this entity.
    // @param Damage - The amount of damage this entity will take.
    public void TakeDamage(float Damage)
    {
        if (!IsDead && !Invulnerable && !IsImmune)
        {
            Health.IsRegenerating = false;
            StopCoroutine(StartHealthRegen());
            Health.Value -= Damage;
            Health.Value = Mathf.Clamp(Health.Value, 0.0f, Health.MaxValue);

            if (Health.Value <= 0.0f)
            {
                IsDead = true;
                
                Audio.clip = DeathSound;
                Audio.Play();

                if (OnDeath != null)
                {
                    OnDeath.Invoke();
                }
            }
            else
            {
                Audio.clip = HurtSound;
                Audio.Play();

                StartCoroutine(StartHealthRegen());
            }
        }
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

        }
    }


    public void UnCrouch()
    {
        if (Crouching)
        {
            // Do raycast up to see if the entity can stand up
            {
                Character.height += CrouchHeight;
                Character.center += new Vector3(0.0f, CrouchHeight / 2.0f, 0.0f);
                Crouching = false;
                CurrentSpeed = RunSpeed;
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


    private IEnumerator StartHealthRegen()
    {
        yield return new WaitForSeconds(Health.RegenDelay);
        Health.IsRegenerating = true;
    }


    // Resets all main stats of this entity.
    public void ResetEntity()
    {
        Health.Reset();
        Stamina.Reset();
        IsDead = false;
        CurrentSpeed = RunSpeed;
        IsImmune = false;
    }


    public bool IsCrouching
    {
        get
        {
            return Crouching;
        }
    }


    public bool IsSprinting
    {
        get
        {
            return Sprinting;
        }
    }


    public bool IsWalking
    {
        get
        {
            return Walking;
        }
    }

}
