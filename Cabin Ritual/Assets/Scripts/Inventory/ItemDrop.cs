using UnityEngine;

[CreateAssetMenu(fileName = "New ItemDrop", menuName = "Inventory/ItemDrop")]
public class ItemDrop : Item
{
    // this script is very specific. if you require something to drop an item 
    // eg the letter in the tutroial of the game
    // then you use this scriptable object instead of the normal item
    //because this script will handle all the normal item stuff with some extra bits

    [Header("prefab that you want created after its used")]
    public GameObject ItemDropped;

    [Tooltip("The X position you want the Key dropped at")]
    public int DropXPos;
    [Tooltip("The Y position you want the Key dropped at")]
    public int DropYPos;
    [Tooltip("The Z position you want the Key dropped at")]
    public int DropZpos;

    [SerializeField]
    //this is so that items can only be used once
    bool used = false;

    // this function will spawn a specific item where the player stands
    public void CabinLetter()
    {
        if(!used)
        {
            Instantiate(ItemDropped, new Vector3(DropXPos, DropYPos, DropZpos), Quaternion.identity);
        }

        used = true;
    }
}
