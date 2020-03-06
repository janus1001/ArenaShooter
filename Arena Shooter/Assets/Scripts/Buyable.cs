using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuyOption", menuName = "Inventory/New Buying Option", order = 1)]
public class Buyable : ScriptableObject
{
    public BaseItem itemToBuy;
    public int moneyPrice;
    public int tokenPrice;

    // Temporary, to be replaced with BaseItem
    public int id;
}
