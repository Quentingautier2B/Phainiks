using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [TextArea]
    [SerializeField] string Notes = "Comment Here.";
    Reset reset;

    public List<string> Inventory;

    private void Awake()
    {
        reset = FindObjectOfType<Reset>();
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            reset.resetTimer = 0;
    }

}
