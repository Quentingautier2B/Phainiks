using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    #region variable
    public int doorIndex;
    Color mesh;
    #endregion

    private void Awake()
    {
        
        if(doorIndex == 1)
        {
            mesh = Color.red;
            GetComponent<MeshRenderer>().material.color = mesh;
        }        
        
        if(doorIndex == 2)
        {
            mesh = Color.blue;
            GetComponent<MeshRenderer>().material.color = mesh;
        }       
        
        if(doorIndex == 3)
        {
            mesh = Color.black;
            GetComponent<MeshRenderer>().material.color = mesh;
        }
       
    }

    private void OnDrawGizmos()
    {
        if (doorIndex == 1)
        {
            mesh = Color.red;
            GetComponent<MeshRenderer>().sharedMaterial.color = mesh;
        }

        if (doorIndex == 2)
        {
            mesh = Color.blue;
            GetComponent<MeshRenderer>().sharedMaterial.color = mesh;
        }

        if (doorIndex == 3)
        {
            mesh = Color.black;
            GetComponent<MeshRenderer>().sharedMaterial.color = mesh;
        }
    }
}