using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "NewPlaceable", menuName = "Inventory/New Placeable", order = 1)]
public class PlaceableData : ScriptableObject
{
    public string objectName = "New MyScriptablePlaceable";
    public GameObject placeablePrefab; 
    public GameObject placeableIndicatorPrefab;
}