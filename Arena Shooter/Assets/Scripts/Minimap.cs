using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    public GameObject newMarkerPrefab;

    Transform localPlayer;
    public static List<Transform> targets = new List<Transform>();
    public static List<RectTransform> otherPlayerMarkers = new List<RectTransform>();
    public List<RectTransform> teamCrystals;

    public RectTransform localPlayerMarker;

    const float mapScalingMagic = 3.69f;


    private void Start()
    {
        localPlayer = NetworkClient.connection.identity.transform;
    }

    void Update()
    {
        if (Settings.rotateMinimap)
        {
            Vector3 newRotation = new Vector3(0, 0, Camera.main.transform.rotation.eulerAngles.y);
            GetComponent<RectTransform>().rotation = Quaternion.Euler(newRotation);
        }

        localPlayerMarker.localPosition = new Vector3(localPlayer.position.x * mapScalingMagic, localPlayer.position.z * mapScalingMagic);
        localPlayerMarker.localRotation = Quaternion.Euler(0, 0, -localPlayer.rotation.eulerAngles.y);

        foreach (RectTransform item in teamCrystals)
        {
            item.rotation = Quaternion.identity;
        }

        foreach (Transform target in targets)
        {
            target.localPosition = new Vector3(localPlayer.position.x * mapScalingMagic, localPlayer.position.z * mapScalingMagic);
            target.localRotation = Quaternion.Euler(0, 0, -localPlayer.rotation.eulerAngles.y);
        }
    }

    static void AddMapMarker(Transform newTarget)
    {
        
    }

    static void RemoveMapMarker(Transform removedTarget)
    {

    }
}
