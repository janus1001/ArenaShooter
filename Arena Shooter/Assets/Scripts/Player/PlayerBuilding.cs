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
		currentPlaceable = GameObject.Find("Placeable").GetComponent<Placeable>();
	}

	private void Update()
	{
		RaycastHit hit;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, PlacementRange))
		{
			Debug.Log("Hit");
			currentPlaceable.IsOnSurface = true;
			currentPlaceable.MoveToCursor(hit.point, hit.normal);
		}
		else
		{
			Debug.Log("Not Hit");
			currentPlaceable.IsOnSurface = false;
			currentPlaceable.MoveToCursor(transform.position + Camera.main.transform.forward * PlacementRange, Vector3.up);
		}
	}

	[Command]
	private void CmdBuildStructure()
	{

	}
}
