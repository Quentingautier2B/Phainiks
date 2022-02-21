using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public int keyIndex;
    Color mesh;
    private void Awake()
    {
        if (keyIndex == 1)
        {
            mesh = Color.red;
            GetComponent<MeshRenderer>().material.color = mesh;
        }

        if (keyIndex == 2)
        {
            mesh = Color.blue;
            GetComponent<MeshRenderer>().material.color = mesh;
        }

        if (keyIndex == 3)
        {
            mesh = Color.black;
            GetComponent<MeshRenderer>().material.color = mesh;
        }
    }

}
