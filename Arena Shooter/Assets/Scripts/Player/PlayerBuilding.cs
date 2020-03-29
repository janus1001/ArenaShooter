using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerBuilding : MonoBehaviour
{
	public PlaceableData PlaceableData;
	Placeable currentPlaceable;

	private const float PlacementRange = 4;

	private float buildCooldown = 0;

	private void Update()
	{
		buildCooldown -= Time.deltaTime;
		if(!currentPlaceable)
		{
			currentPlaceable = Instantiate(PlaceableData.placeableIndicatorPrefab).GetComponent<Placeable>();
		}

		if (currentPlaceable.IsPlaceable && Input.GetMouseButtonDown(0))
		{
			if (buildCooldown < 0)
			{
				buildCooldown = 1f;
				PlayerEntityNetwork.localPlayer.CmdBuild(currentPlaceable.transform.position, currentPlaceable.transform.rotation, PlaceableData.placeablePrefab.name, Inventory.localInventory.currentInventoryIndex);
				return;
			}
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

	private void OnDestroy()
	{
		if (currentPlaceable)
			Destroy(currentPlaceable.gameObject);
	}

	private void OnDisable()
	{
		if (currentPlaceable)
			Destroy(currentPlaceable.gameObject);
	}
}
