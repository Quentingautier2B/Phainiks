using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [TextArea]
    [SerializeField] string Notes = "Comment Here.";
    Reset reset;
    GridGenerator gridG;


    public List<string> Inventory;

    private void Awake()
    {
        reset = FindObjectOfType<Reset>();
        gridG = FindObjectOfType<GridGenerator>();
        
    }
    private void Update()
    {
        var yPos = transform.position;
        yPos.y = gridG.grid[(int)transform.position.x, (int)transform.position.z].transform.position.y + 1.5f ;
        transform.position = yPos;      
    }

}
