﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private Entity Player = null;

    //a refernce to the InventoryUI script
    public InventoryUI InventoryUI;

    // a refernce to the arcadeUI script
    public ArcadeUI ArcadeUI;

    //a refernce to the Points UI
    public Image PointUI;

    // a refernce to the player points script
    private PlayersPoints PlayerPoints;

    // a refernce to the PauseUI
    public Image PauseScene;

    // A reference to the game over UI.
    public Image UIGameOver; 

    // The interactable objec the player is looking at.
    private InteractableObject LookingAt = null;

    // The hit information for the interaction raycast.
    private RaycastHit Hit = new RaycastHit();

    // This is the players inventory that is on this object.
    private Inventory PlayerInv = null;

    // A reference to the camera controller.
    private CameraController CamController = null;

    public Text AmmoCount;

    // A reference to the camera transform
    [Tooltip("A reference to the camera's transform, used to calculate the interaction raycast.")]
    [SerializeField]
    private Transform Cam = null;


    // A reference to the weapon holder.
    private GunHolder Holder = null;

    

    ///  Overridables

    // Start is called before the first frame update
    void Start()
    {
        Player = GetComponent<Entity>();
        PlayerInv = GetComponent<Inventory>();
        if (!Player) Debug.LogError("Error: Could not find the Entity component!");

        Holder = GetComponent<GunHolder>();

        CamController = GetComponent<CameraController>();

        Cursor.visible = ShowCursor;
        Cursor.lockState = (ShowCursor) ? CursorLockMode.None : CursorLockMode.Locked;


        PlayerPoints = FindObjectOfType<PlayersPoints>();
    }


    private void FixedUpdate()
    {
        ScanInteractable();
        UpdateAmmo(Holder.GetHeldWeapon());
    }


    void Update()
    {
        if (Player)
        {
            if (!PauseScript.GamePaused)
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

                    //if (Input.GetButtonDown("ArcadePoints"))
                    //{
                    //    PointUI.gameObject.SetActive(!PointUI.gameObject.activeSelf);
                    //}
                    //
                    //if (Input.GetButtonDown("Points"))
                    //{
                    //    ArcadeUI.OpenCloseUI();
                    //}

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

                    if (Input.GetButtonDown("Fire1"))
                    {
                        Holder.GetHeldWeapon().Fire();
                    }

                    if (Input.GetButtonUp("Fire1"))
                    {
                        Holder.GetHeldWeapon().StopFiring();
                    }

                    if (Input.GetButtonDown("Reload"))
                    {
                        Holder.GetHeldWeapon().Reload();
                    }

                    if (Input.GetButtonDown("Weapon1"))
                    {
                        Holder.SwapTo(0);
                    }

                    if (Input.GetButtonDown("Weapon2"))
                    {
                        Holder.SwapTo(1);
                    }

                    /*if (Input.GetButtonDown("SwapUp"))
                    {
                        Holder.SwapUp();
                    }

                    if (Input.GetButtonDown("SwapDown"))
                    {
                        Holder.SwapDown()
                    }*/
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

            if (Input.GetButtonDown("Cancel"))
            {
                PauseScript.TogglePause();
                PauseScene.gameObject.SetActive(!PauseScene.gameObject.activeSelf);
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
                    InventoryUI.ChangeFlavourText(LookingAt.GetFlavourText(this));
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

    public void UpdateAmmo(GunScript Gun)
    {
        AmmoCount.text = Gun.GetCurrentAmmo() + " / " + Gun.GetTotalAmmo().ToString();
    }

    public void GameOver()
    {
        PauseScript.Pause();
        //UIGameOver.gameObject.SetActive(true);
        PlayerPoints.EndPoints();
        SceneManager.LoadScene("Death");
    }
}