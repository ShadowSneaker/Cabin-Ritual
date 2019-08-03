using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    public enum ESpawnAugments
    {
        VisibleOnly,    // Only spawns if the spawn zone is visible by a player.
        HiddenOnly,     // Only spawns if the spawn zone is not visible by a player.
        Both            // Can spawn if the spawn zone is in the player's sight or not.
    }


    ///  Properties

    [Tooltip("The augments for spawning an object")]
    [SerializeField]
    private ESpawnAugments SpawnAugments = ESpawnAugments.HiddenOnly;

    [Tooltip("Keys that are ignored by this spawn zone")]
    [SerializeField]
    private string[] BlacklistKeys;


    // The keys used to spawn objects.
    //internal string[] Keys;

    // A reference to the game mode.
    private GameMode GM;

    private bool Active = false;

    
    /// Overridables


    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.FindObjectOfType<GameMode>();
        if (!GM)
        {
            Debug.LogError("Game Mode not found. Spawn Zone: " + this + (" will not work!"));
        }
    }


    private void FixedUpdate()
    {
        ValidateSpawner();
    }


    /// Functions

    private void ValidateSpawner()
    {
        bool Check = true;

        bool IsNear = false;

        for (int i = 0; i < GM.GetPlayerCount(); ++i)
        {
            if (Vector3.Distance(transform.position, GM.GetPlayer(i).transform.position) <= GM.GetMaxSpawnDistance)
            {
                IsNear = true;
                break;
            }
        }

        if (IsNear)
        {
            for (int i = 0; i < GM.GetPlayerCount(); ++i)
            {
                if (Physics.Linecast(GM.GetPlayer(i).transform.position, transform.position, 11))
                {
                    if (SpawnAugments == ESpawnAugments.VisibleOnly)
                    {
                        Check = false;
                        break;
                    }
                }
                else
                {
                    if (SpawnAugments == ESpawnAugments.HiddenOnly)
                    {
                        Check = false;
                        break;
                    }
                }
            }
        }
        else
        {
            Check = false;
        }


        // Update the game mode only if the value has changed.
        if (Check != Active)
        {
            Active = Check;
            if (Active)
            {
                GM.AddSpawnZone(this);
            }
            else
            {
                GM.RemoveSpawnZone(this);
            }
        }
    }
}
