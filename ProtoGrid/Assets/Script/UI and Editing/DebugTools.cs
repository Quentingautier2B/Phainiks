using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

            Instantiater(System);
            Instantiater(SecondarySystem);
            Instantiater(Player);
            Instantiater(Terrain);
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
