using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Sets and applies all the game rules directing the game in a specified direction.
[RequireComponent(typeof(ObjectPool))]
public class GameMode : MonoBehaviour
{
    private struct PlayerInfo
    {
        public Entity Player;
        public Controller PlayerController;
    }


    [Tooltip("How many players are in the game (1 - 4")]
    [SerializeField]
    private int PlayerCount = 1;

    [Tooltip("Forces the inputted entities to become the players.")]
    [SerializeField]
    private Controller[] StaticPlayers = null;

    
    // A reference to all the players in the game.
    private PlayerInfo[] Players;

    // A reference to the attached Object Pool component.
    protected ObjectPool Pool;


    /// Overridables



    // Start is called before the first frame update
    void Start()
    {
        Players = new PlayerInfo[PlayerCount];

        if (StaticPlayers.Length != 0)
        {
            for (int i = 0; i < StaticPlayers.Length; ++i)
            {
                Players[i].PlayerController = StaticPlayers[i];
                Players[i].Player = StaticPlayers[i].GetComponent<Entity>();
            }
        }
        else
        {
            // Find controllers instead of entities incase there are pre-placed entities in the world.
            Controller[] Controllers = FindObjectsOfType<Controller>();
            for (int i = 0; i < PlayerCount; ++i)
            {
                Players[i].PlayerController = Controllers[i];
                Players[i].Player = Controllers[i].GetComponent<Entity>();
            }
        }
    }

    
    /// Functions

    
    // Checks a position to see if something can spawn there.
    // Things it checks for are: If the position is not blocked and
    // if the player can see that position (and vision is not blocked).
    // @param Location - The location to check to see if something can spawn there.
    // @return - Returns true if something can spawn there.
    protected bool VerifyLocation(Vector3 Location)
    {
        return true;
    }


    public Controller GetPlayerController(int Index)
    {
        return Players[Index].PlayerController;
    }


    public Entity GetPlayer(int Index)
    {
        return Players[Index].Player;
    }
}
