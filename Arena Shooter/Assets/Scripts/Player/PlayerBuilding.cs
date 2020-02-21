using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerBuilding : NetworkBehaviour
{
	public PlaceableData PlaceableData;
	Placeable currentPlaceable;

	private const float PlacementRange = 4;

	private void Update()
	{
		if(currentPlaceable.IsPlaceable && Input.GetMouseButtonDown(0))
		{
			Instantiate(PlaceableData.placeablePrefab, currentPlaceable.transform.position, currentPlaceable.transform.rotation);
			StopBuilding();
			return;
		}

		RaycastHit hit;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, PlacementRange, 1 << LayerMask.NameToLayer("Terrain")))
		{
			currentPlaceable.IsOnSurface = true;
			currentPlaceable.MoveToCursor(hit.point, hit.normal, transform.rotation.eulerAngles.y);
		}
		else
		{
			currentPlaceable.IsOnSurface = false;
			currentPlaceable.MoveToCursor(transform.position + Camera.main.transform.forward * PlacementRange, Vector3.up, transform.rotation.eulerAngles.y);
		}
	}

	public void EquipPlaceable(PlaceableData placeableData)
	{
		enabled = true;
		PlaceableData = placeableData;

		currentPlaceable = Instantiate(placeableData.placeableIndicatorPrefab).GetComponent<Placeable>();
	}

	public void StopBuilding()
	{
		enabled = false;
		if (currentPlaceable)
		{
			Destroy(currentPlaceable.gameObject);
		}
	}

	[Command]
	private void CmdBuildStructure()
	{
		
	}
}
