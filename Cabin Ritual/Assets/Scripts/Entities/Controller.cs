using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    ///  Properties

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
    public InventoryUI inventoryUI;

    // The interactable objec the player is looking at.
    private InteractableObject LookingAt;

    // The hit information for the interaction raycast.
    private RaycastHit Hit;

    // the Flavour text that displays to the screen
    public Text FlavourText;


    // A reference to the camera transform
    [Tooltip("A reference to the camera's transform, used to calculate the interaction raycast.")]
    [SerializeField]
    private Transform Cam = null;

    

    ///  Overridables

    // Start is called before the first frame update
    void Start()
    {
        Player = GetComponent<Entity>();
        
        if (!Player) Debug.LogError("Error: Could not find the Entity component!");
        
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
            if (Input.GetButtonDown("Sprint"))
            {
                Player.Sprint();
            }

            if (Input.GetButtonUp("Sprint"))
            {
                Player.StopSprinting();
            }

            if (Input.GetAxis("MoveForward") != 0.0f || Input.GetAxis("MoveSideways") != 0.0f)
            {
                Player.Move(Input.GetAxis("MoveForward"), Input.GetAxis("MoveSideways"), Input.GetButtonDown("Jump"));
            }

            if (Input.GetButtonDown("Interact"))
            {

                Interact();
                FlavourText.enabled = false;
            }

            if(Input.GetButtonDown("Inventory"))
            {
                
                 inventoryUI.Close_OpenUI();
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
                    FlavourText.text = LookingAt.ScreenText;

                    FlavourText.enabled = true;
                }
            }
            else
            {
                LookingAt = null;

                FlavourText.enabled = false;
            }
        }
    }
}
