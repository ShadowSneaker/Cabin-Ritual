using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    ///  Properties

    [Tooltip("Allows the user to input keys.")]
    [SerializeField]
    private bool AllowInputs = true;

    [Tooltip("Should the cursor be shown in game?")]
    [SerializeField]
    private bool ShowCursor = false;

    [Tooltip("Should the crouch be toggled instead of held?")]
    [SerializeField]
    private bool ToggleCrouch = false;

    [Tooltip("Should the game allow the user to interact?")]
    [SerializeField]
    private bool AllowInteraction = true;

    [Tooltip("Display the interaction into if the user hovers over an object that can be interacted?")]
    [SerializeField]
    private bool DisplayInteractable = true;

    [Tooltip("How far the user should be able to interact with objects.")]
    [SerializeField]
    private float RaycastLength = 100.0f;

    


    // A reference to the entity script.
    private Entity Player;

    //a refernce to the InventoryUI script
    public InventoryUI InventoryUI;

    // a refernce to the arcadeUI script
    public ArcadeUI ArcadeUI;

    // The interactable objec the player is looking at.
    private InteractableObject LookingAt;

    // The hit information for the interaction raycast.
    private RaycastHit Hit;

    // This is the players inventory that is on this object.
    private Inventory PlayerInv;

    // A reference to the camera controller.
    private CameraController CamController;

    // A reference to the camera transform
    [Tooltip("A reference to the camera's transform, used to calculate the interaction raycast.")]
    [SerializeField]
    private Transform Cam = null;


    

    ///  Overridables

    // Start is called before the first frame update
    void Start()
    {
        Player = GetComponent<Entity>();
        PlayerInv = GetComponent<Inventory>();
        if (!Player) Debug.LogError("Error: Could not find the Entity component!");

        CamController = GetComponent<CameraController>();
        
    }


    private void Awake()
    {
        Cursor.visible = ShowCursor;
    }


    private void FixedUpdate()
    {
        ScanInteractable();
    }


    void Update()
    {
        if (Player)
        {
            if (AllowInputs)
            {
                if (Input.GetButtonDown("Sprint"))
                {
                    Player.Sprint();
                }

                if (Input.GetButtonUp("Sprint"))
                {
                    Player.StopSprinting();
                }

                Player.Move(Input.GetAxis("MoveForward"), Input.GetAxis("MoveSideways"), Input.GetButtonDown("Jump"));


                if (Input.GetButtonDown("Interact"))
                {
                    Interact();
                    InventoryUI.DisableFlavourText();
                }

                if (Input.GetButtonDown("Inventory"))
                {
                    InventoryUI.Close_OpenUI();
                    PlayerInv.ChangeInventoryOpen();
                }

                if(Input.GetButtonDown("Points"))
                {
                    ArcadeUI.OpenCloseUI();
                }

                if (Input.GetButtonDown("Crouch"))
                {
                    if (ToggleCrouch && Player.IsCrouching)
                    {
                        Player.UnCrouch();
                    }
                    else
                    {
                        Player.Crouch();
                    }
                }

                if (Input.GetButtonUp("Crouch"))
                {
                    if (!ToggleCrouch)
                    {
                        Player.UnCrouch();
                    }
                }
            }
            else
            {
                Player.Move(0.0f, 0.0f, false);
            }

            if (CamController.enabled != AllowInputs)
            {
                CamController.enabled = AllowInputs;
            }
        }
    }



    /// Functions

    
    // Interacts with the object the player is looking at if it's an interactable object.
    void Interact()
    {
        if (AllowInteraction && LookingAt)
        {
            LookingAt.Interact();
        }
    }


    // Constantly checks the item the player is looking at to see if it's a interactable object.
    // Shoots a raycast starting from the camera location and moving 
    void ScanInteractable()
    {
        if (AllowInteraction)
        {
            if (Physics.Raycast(Cam.position, Cam.forward, out Hit, RaycastLength))
            {
                LookingAt = Hit.transform.GetComponent<InteractableObject>();
                if (LookingAt && DisplayInteractable)
                {
                    // Place code to draw to the screen.
                    InventoryUI.ChangeFlavourText(LookingAt.ScreenText);
                    InventoryUI.EnableFlavourText();
                }
            }
            else
            {
                LookingAt = null;
                InventoryUI.DisableFlavourText();
            }
        }
    }

    // Applies any effects that the player has from being damaged.
    public void ApplyPlayerEffects()
    {
        // UI.HurtImage.alpha = Player.Health / Player.MaxHealh;

        if (Player.Health == 1)
        {
            // Apply cripple debuff to the player.
        }
    }


    // A simple getter to get the players inventory for the use in the item pick up script
    public Inventory GetPlayerInv()
    {
        return PlayerInv;
    }

    // A function to return the looking at object
    public InteractableObject ReturnLookingAt()
    {
        return LookingAt;
    }
}
