using UnityEngine;

[CreateAssetMenu (fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    
    // the name that will be displayed next to the item within the players inventory
    new public string name = "New name";
    // the icon that will determine what the item looks like in the players inventory
    public Sprite Icon = null;
    // the description given to the item when it is found in the players inventory
    public string Description;
    //durability of the item
    public int Durability;
    // a bool to determine if its a weapon
    public bool Weapon;
    // a bool to determoine if its a ritual item
    public bool RitualItem;
    // a bool to determine if the item is a letter item
    public bool LetterItem;
    // this is th etext that will be found in the letter 
    [TextArea]
    public string LetterText;

    [Header("if this item to be used instead of equiped then have this bool set true")]
    public bool use = false;

    

    // beyond this point is code for when you want an item to drop

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

    
    

    // this function will spawn a specific item where the player stands
    public void CabinLetter()
    {
        
            Instantiate(ItemDropped, new Vector3(DropXPos, DropYPos, DropZpos), Quaternion.identity);
        
       
    }


   
}
