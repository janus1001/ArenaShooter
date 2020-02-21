using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerBuilding : NetworkBehaviour
{
	public Placeable currentPlaceable;

	private const float PlacementRange = 4;

	private void Start()
	{
		currentPlaceable = GameObject.Find("Health Pad Indicator").GetComponent<Placeable>();
	}

	private void Update()
	{
		RaycastHit hit;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, PlacementRange, 1 << LayerMask.NameToLayer("Terrain")))
		{
			currentPlaceable.IsOnSurface = true;
			currentPlaceable.MoveToCursor(hit.point, hit.normal);
		}
		else
		{
			currentPlaceable.IsOnSurface = false;
			currentPlaceable.MoveToCursor(transform.position + Camera.main.transform.forward * PlacementRange, Vector3.up);
		}
	}

	[Command]
	private void CmdBuildStructure()
	{
		
	}
}
