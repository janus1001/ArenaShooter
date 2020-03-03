using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

public class ObjectToPrefabReplacer : MonoBehaviour
{
    public string nameOfObjectToReplace;
    public GameObject prefabToUse;
}

[CustomEditor(typeof(ObjectToPrefabReplacer))]
public class ObjectToPrefabReplacerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Replace Prefabs"))
        {
            ShowPopup(HowManyObjects());
        }
    }

    private void ShowPopup(int numberOfObjects)
    {
        ReplacePopup window = CreateInstance<ReplacePopup>();
        window.objectReference = this;
        window.numberOfItemsToReplace = numberOfObjects;
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
        window.ShowPopup();
    }

    public int HowManyObjects()
    {
        List<GameObject> gameObjects = FindObjectsOfType<GameObject>().ToList();
        for (int i = gameObjects.Count - 1; i >= 0; i--)
        {
            if(!gameObjects[i].activeInHierarchy || !gameObjects[i].name.Contains(((ObjectToPrefabReplacer)target).nameOfObjectToReplace))
            {
                gameObjects.RemoveAt(i);
            }
        }
        return gameObjects.Count;
    }

    public void ReplaceAllObjects()
    {
        List<GameObject> gameObjects = FindObjectsOfType<GameObject>().ToList();
        for (int i = gameObjects.Count - 1; i >= 0; i--)
        {
            if (!gameObjects[i].activeInHierarchy || !gameObjects[i].name.Contains(((ObjectToPrefabReplacer)target).nameOfObjectToReplace))
            {
                gameObjects.RemoveAt(i);
            }
        }

        foreach (GameObject gameObject in gameObjects)
        {
            GameObject instantiatedObject = (GameObject)PrefabUtility.InstantiatePrefab(((ObjectToPrefabReplacer)target).prefabToUse, gameObject.transform);
            instantiatedObject.transform.localPosition = Vector3.zero;
            instantiatedObject.transform.localRotation = Quaternion.identity;
            Vector3 scale = gameObject.transform.lossyScale;
            instantiatedObject.transform.parent = GameObject.Find("World").transform;
            instantiatedObject.transform.localScale = scale;
            
            gameObject.SetActive(false);
        }
    }
}

public class ReplacePopup : EditorWindow
{
    public ObjectToPrefabReplacerEditor objectReference;
    public int numberOfItemsToReplace = 0;

    void OnGUI()
    {
        EditorGUILayout.LabelField("This will replace " + numberOfItemsToReplace + " objects. Proceed?", EditorStyles.popup);
        GUILayout.Space(70);
        if (GUILayout.Button("OK"))
        {
            objectReference.ReplaceAllObjects();
            Close();
        }
        if (GUILayout.Button("Cancel"))
        {
            Close();
        }
    }
}