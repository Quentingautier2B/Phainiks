using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SaveSystem : MonoBehaviour
{
    public Dictionary<string, int> levelScore = new Dictionary<string, int>();
    DebugTools debT;

    private void Awake()
    {
        debT = GetComponent<DebugTools>();    
        
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            levelScore.Clear();
            SaveScore(0);
        }
    }

    private void Start()
    {
        Dictionary<string, int> levelScore = new Dictionary<string, int>();
        LoadScore();

        if(!levelScore.ContainsKey(SceneManager.GetActiveScene().name) && debT.World != 0)
        {
            levelScore.Add(SceneManager.GetActiveScene().name, 0);
        }

    }

    public void SaveScore(int score)
    {
        if(levelScore == null)
        {
            levelScore = new Dictionary<string, int>();
            levelScore.Add(SceneManager.GetActiveScene().name, 0);
        }
        levelScore[SceneManager.GetActiveScene().name] = score;
        Saver.SaveScore(this);
    }

    public void LoadScore()
    {
        ScoreData data = Saver.LoadScore();
        print(data.score);
        levelScore = data.score;


    }
}
