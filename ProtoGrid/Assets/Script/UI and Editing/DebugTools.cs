using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class DebugTools : MonoBehaviour
{
    public bool debugModOn;

    public GameObject System;
    public GameObject SecondarySystem;
    public GameObject Player;
    public GameObject Terrain;
    bool SceneLoaded;

    private void OnDrawGizmos()
    {
        if (!SceneLoaded && !FindObjectOfType<GridGenerator>())
        {
        PrefabUtility.InstantiatePrefab(System);
        PrefabUtility.InstantiatePrefab(SecondarySystem);
        PrefabUtility.InstantiatePrefab(Player);
        PrefabUtility.InstantiatePrefab(Terrain);
        SceneLoaded = true;
        }

    }

    void Instantiater(GameObject obj)
    {
        if (!GameObject.Find(obj.name))
        {
            var inst = Instantiate(obj);
            inst.name = obj.name;
        }
    }

}
