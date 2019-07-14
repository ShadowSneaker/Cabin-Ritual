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
}
