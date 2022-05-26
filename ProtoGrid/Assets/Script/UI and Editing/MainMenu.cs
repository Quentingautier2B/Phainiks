using System.Collections;
using System.Collections.Generic;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    Vector3 startpos;
    float lerper;
    bool flag;
    public bool timelineEnd;

    private void Start()
    {
        timelineEnd = false;
        flag = true;
        lerper = 0;
        startpos = Camera.main.transform.localPosition;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (timelineEnd)
            {
                if(flag)
                {
                    StartCoroutine(Lerper());
                    flag = false;
                }
            }
            else
            {
                Time.timeScale = 20;

            }

        }
    }


    public void TimelineEnd()
    {
        timelineEnd = true;
        Time.timeScale = 1;
    }

    IEnumerator Lerper()
    {
        lerper += Time.deltaTime;
        Camera.main.transform.localPosition = Vector3.Lerp(startpos, new Vector3(0, 0, -15), lerper);
        print(lerper);
        if (lerper >= 1)
        {
            Camera.main.transform.localPosition = new Vector3(0, 0, -15);           
            SceneManager.LoadScene("Lvl_0,6");
        }
        else
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(Lerper());
        }
    }
    
}


