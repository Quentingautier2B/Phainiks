using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    Vector3 startpos;
    float lerper;
    float lerper2;
    float lerper3;
    float maxColor;
    bool flag;
    public bool timelineEnd;
    [SerializeField] RawImage[] fadeOut;
    [SerializeField] MeshRenderer mat;
    [SerializeField] Animator pushForPlay;
    public static bool isPlaying;
    public FMOD.Studio.EventInstance mainMusic;
    private void Awake()
    {
        if (isPlaying)
        {

        }
        else
        {
            mainMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Tiles/Main");
            mainMusic.start();
            isPlaying = true;
        }
    }
    private void Start()
    {
        timelineEnd = false;
        flag = true;
        lerper = 0;
        lerper2 = 0;
        lerper3 = 0;
        maxColor = 1;
        startpos = Camera.main.transform.localPosition;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (timelineEnd)
            {
                if(flag)
                {
                    foreach (RawImage image in fadeOut)
                    {
                        StartCoroutine(Lerper2(image));
                    }
                    pushForPlay.enabled = false;
                    StartCoroutine(Lerper3());
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
        if (lerper >= 1)
        {
            Camera.main.transform.localPosition = new Vector3(0, 0, -15);
            yield return new WaitForSeconds(0.3f);
            SceneManager.LoadScene("Lvl_0,6");
        }
        else
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(Lerper());
        }
    }

    IEnumerator Lerper2(RawImage image)
    {
        lerper2 += Time.deltaTime;
        image.color = Color.Lerp(new Color(maxColor,maxColor,maxColor, 1), new Color(maxColor, maxColor, maxColor, 0), lerper2);
        if (lerper2 >= 1)
        {
            image.color = new Color(maxColor, maxColor, maxColor, 0);
        }
        else
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(Lerper2(image));
        }
    }
    IEnumerator Lerper3()
    {
        lerper3 += Time.deltaTime;
        mat.material.color = Color.Lerp(new Color(maxColor, maxColor, maxColor, maxColor), new Color(maxColor, maxColor, maxColor, 0), lerper3);
        if (lerper3 >= 1)
        {
            mat.material.color = new Color(maxColor, maxColor, maxColor, 0);
        }
        else
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(Lerper3());
        }
    }


}


