using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetStar : MonoBehaviour
{
    WorldManager world;
    float levelIndex;
    RawImage Star1, Star2, Star3;
    Texture onT, offT;
    SaveSystem saved;
    private void OnEnable()
    {
        Star1 = transform.Find("Star/Star 1").GetComponent<RawImage>();
        Star2 = transform.Find("Star/Star 2").GetComponent<RawImage>();
        Star3 = transform.Find("Star/Star 3").GetComponent<RawImage>();

        world = transform.parent.parent.GetComponent<WorldManager>();

        onT = world.ActivatedStar;
        offT = world.UnactivatedStar;

        saved = FindObjectOfType<SaveSystem>();
        levelIndex = world.LevelsOfTheWorld[int.Parse(this.name) - 1];
        if(saved.levelScore.ContainsKey("Lvl_" + Mathf.Floor(levelIndex) + "," + Mathf.RoundToInt((levelIndex - Mathf.FloorToInt(levelIndex)) * 10)))
        {
            if (saved.levelScore["Lvl_" + Mathf.Floor(levelIndex) + "," + Mathf.RoundToInt((levelIndex - Mathf.FloorToInt(levelIndex)) * 10)] == 1)
            {
                Star1.texture = onT;
                Star1.color = Color.white;

                Star2.texture = offT;
                Star2.color = Color.black;

                Star3.texture = offT;
                Star3.color = Color.black;

            }
            else if (saved.levelScore["Lvl_" + Mathf.Floor(levelIndex) + "," + Mathf.RoundToInt((levelIndex - Mathf.FloorToInt(levelIndex)) * 10)] == 2)
            {
                Star1.texture = onT;
                Star1.color = Color.white;

                Star2.texture = onT;
                Star2.color = Color.white;

                Star3.texture = offT;
                Star3.color = Color.black;
            }

            else if (saved.levelScore["Lvl_" + Mathf.Floor(levelIndex) + "," + Mathf.RoundToInt((levelIndex - Mathf.FloorToInt(levelIndex)) * 10)] == 3)
            {
                Star1.texture = onT;
                Star1.color = Color.white;

                Star2.texture = onT;
                Star2.color = Color.white;

                Star3.texture = onT;
                Star3.color = Color.white;
            }
        }
        else
        {
            //print(name + "Lvl_" + Mathf.Floor(levelIndex) + "," + Mathf.RoundToInt((levelIndex - Mathf.FloorToInt(levelIndex))*10));
        }
        
    }
}
