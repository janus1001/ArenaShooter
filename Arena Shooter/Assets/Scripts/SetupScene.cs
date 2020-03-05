#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SetupScene : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
}

[CustomEditor(typeof(SetupScene))]
public class SetupSceneEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Setup Scene"))
        {
            foreach (GameObject item in ((SetupScene)target).objectsToSpawn)
            {
                PrefabUtility.InstantiatePrefab(item);
            }
            Debug.Log("Spawned objects to the scene!");
            DestroyImmediate(((SetupScene)target).gameObject);
        }
    }
}
#endif