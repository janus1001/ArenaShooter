using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuyOption", menuName = "Inventory/New Buying Option", order = 1)]
public class Buyable : ScriptableObject
{
    public BaseItem itemToBuy;
    public bool priceInTokens = false;
    public int price;
}
