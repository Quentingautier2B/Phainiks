using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if  UNITY_EDITOR
using UnityEditor;
#endif
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
        #if UNITY_EDITOR
        if (!SceneLoaded && !FindObjectOfType<GridGenerator>())
        {

            PrefabUtility.InstantiatePrefab(System);
            PrefabUtility.InstantiatePrefab(SecondarySystem);
            PrefabUtility.InstantiatePrefab(Player);
            PrefabUtility.InstantiatePrefab(Terrain);
            SceneLoaded = true;
        }
        #endif

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
