using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public List<Transform> allies;

    void Update()
    {
        Vector3 newRotation = new Vector3(0, 0, Camera.main.transform.rotation.eulerAngles.y);
        GetComponent<RectTransform>().rotation = Quaternion.Euler(newRotation);
    }
}
