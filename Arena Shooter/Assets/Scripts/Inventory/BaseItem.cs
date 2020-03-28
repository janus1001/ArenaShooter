using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/New Item", order = 1)]
public class BaseItem : ScriptableObject
{
    public GameObject itemPrefab;
    public GameObject itemViewportPrefab; // Object attached to camera, to be added to local player on equip
    public Sprite itemIcon;

    public int maxStack = 1;
}

public static class ItemSerializer
{
    public static void WriteItem(this NetworkWriter writer, BaseItem item)
    {
        // no need to serialize the data, just the name of the item
        writer.WriteString(item.name);
    }

    public static BaseItem ReadItem(this NetworkReader reader)
    {
        // load the same item by name.  The data will come from the asset in Resources folder
        return Resources.Load<BaseItem>("Items/" + reader.ReadString());
    }
}
