using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathHighlighter : MonoBehaviour
{
    [TextArea]
    [SerializeField] string Notes = "Comment Here.";
    #region variables
    GridGenerator gridGenerator;
    GridTiles[,] grid;
    StepAssignement stepAssignement;
    #endregion

    private void Awake()
    {
        gridGenerator = GetComponent<GridGenerator>();
        grid = gridGenerator.grid;
        stepAssignement = GetComponent<StepAssignement>();      
    }

    public void PathAssignment(int x, int y,int height,int step)
    {
        int i = step;
        if (i>0) 
        TestFourDirection(x, y, height, i - 1);
        grid[x, y].highLight = true;
    }

    void TestFourDirection(int x, int y, int height, int step)
    {
        if (stepAssignement.TestDirection(x, y,height ,step, 1))
        {
            if (grid[x, y+1])
            {
                grid[x, y].highLight = true;
                PathAssignment(x, y+1, height,step);
            }
        }
        else if (stepAssignement.TestDirection(x, y,height, step, 2))
        {
            if (grid[x+1, y])
            {
                
                grid[x, y].highLight = true;
                PathAssignment(x+1, y,height, step);
            }
        }           
        else if (stepAssignement.TestDirection(x, y,height, step, 3))
        {
            if (grid[x, y-1])
            {
                grid[x, y].highLight = true;
                PathAssignment(x, y-1,height,step);
            }
        }            
        else if (stepAssignement.TestDirection(x, y,height, step, 4))
        {
            if (grid[x-1, y])
            {
                grid[x, y].highLight = true;
                PathAssignment(x-1, y,height,step);
            }
        }           
    }
}
