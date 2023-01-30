using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreData
{
    public Dictionary<string, int> score = new Dictionary<string, int>();
    
    public ScoreData(SaveSystem saveSystems)
    {
        score = saveSystems.levelScore;
    }

}
