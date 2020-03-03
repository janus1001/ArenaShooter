using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/New Item", order = 1)]
public class BaseItem : ScriptableObject
{
    public GameObject itemPrefab;

    protected virtual void EquipItem()
    {

    }

    protected virtual void UnEquipItem()
    {

    }

    protected virtual void UseItem()
    {

    }
}
