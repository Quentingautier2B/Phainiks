using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LerpBackground : MonoBehaviour
{
    private Vector3 startpos;
    private float lerperFadeIn;
    private float lerperFadeOut;
    [SerializeField] MeshRenderer backgroundTuto, backgroundWorld1, backgroundWorld2, backgroundWorld3;
    [SerializeField] Material MatbackgroundTuto, MatbackgroundWorld1, MatbackgroundWorld2, MatbackgroundWorld3;
    [SerializeField] Button falseTutoLeft, tutoRight, world1Left, world1Right, world2Left, world2right, world3Left, falseworld3Right;
    CameraBehavior cam;
    private float lerper;

    private void Start()
    {
        cam = FindObjectOfType<CameraBehavior>();
        startpos = Camera.main.transform.localPosition;
        cam.zoomSlider.value = 0;
        lerperFadeIn = 0;
        lerperFadeOut = 0;
    }
    
    public void TutoToWorld1()
    {
        StartCoroutine(LerperFadeOut(backgroundTuto, 168f, falseTutoLeft, tutoRight));
        StartCoroutine(LerperFadeIn(backgroundWorld1, 180f, world1Left, world1Right));
    }

    public void World1ToWorld2()
    {
        StartCoroutine(LerperFadeOut(backgroundWorld1, 180f, world1Left, world1Right));
        StartCoroutine(LerperFadeIn(backgroundWorld2, 255f, world2Left, world2right));
    }

    public void World2ToWorld3()
    {
        StartCoroutine(LerperFadeOut(backgroundWorld2, 255f, world2Left, world2right));
        StartCoroutine(LerperFadeIn(backgroundWorld3, 255f, world3Left, falseworld3Right));
    }

    public void World3ToWorld2()
    {
        StartCoroutine(LerperFadeOut(backgroundWorld3,255f, world3Left, falseworld3Right));
        StartCoroutine(LerperFadeIn(backgroundWorld2,255f, world2Left, world2right));
    }

    public void World2ToWorld1()
    {
        StartCoroutine(LerperFadeOut(backgroundWorld2,255f, world2Left, world2right));
        StartCoroutine(LerperFadeIn(backgroundWorld1, 180f, world1Left, world1Right));
    }

    public void World1ToTuto()
    {
        StartCoroutine(LerperFadeOut(backgroundWorld1, 180f, world1Left, world1Right));
        StartCoroutine(LerperFadeIn(backgroundTuto, 168f, falseTutoLeft, tutoRight));
    }

    IEnumerator LerperFadeOut(MeshRenderer mat, float maxColor, Button Left, Button Right)
    {
        lerperFadeOut += Time.deltaTime * 2;
        mat.material.color = Color.Lerp(new Color(maxColor/255, maxColor/255, maxColor/255, 1), new Color(maxColor/255, maxColor/255, maxColor/255, 0), lerperFadeOut);
        if (lerperFadeOut >= 1)
        {
            mat.material.color = new Color(maxColor/255, maxColor/255, maxColor/255, 0);
            Left.interactable = false;
            Right.interactable = false;
            lerperFadeOut = 0;
        }
        else
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(LerperFadeOut(mat, maxColor, Left, Right));
        }
    }

    IEnumerator LerperFadeIn(MeshRenderer mat,float maxColor, Button Left, Button Right)
    {
        lerperFadeIn += Time.deltaTime * 2;
        mat.material.color = Color.Lerp(new Color(maxColor/255, maxColor/255, maxColor/255, 0), new Color(maxColor/255, maxColor/255, maxColor/255, 1), lerperFadeIn);
        if (lerperFadeIn >= 1)
        {
            mat.material.color = new Color(maxColor/255, maxColor/255, maxColor/255, 1);
            Left.interactable = true;
            Right.interactable = true;
            lerperFadeIn = 0;
        }
        else
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(LerperFadeIn(mat, maxColor, Left, Right));
        }
    }

    public void lerpIn()
    {
        StartCoroutine(LerperIn());
    }

    public void MatBackgroundChange()
    {

        backgroundTuto.material = MatbackgroundTuto;
        backgroundTuto.enabled = true;
        if(SceneChange.currentWorld == 0)
        {
            var transicol = backgroundTuto.material.color;
            transicol.a = 1;
            backgroundTuto.material.color = transicol;
            print(backgroundTuto.material.color);
        }
            

        backgroundWorld1.material = MatbackgroundWorld1;
        backgroundWorld1.enabled = true;
        if (SceneChange.currentWorld == 1)
        {
            var transicol = backgroundWorld1.material.color;
            transicol.a = 1;
            backgroundWorld1.material.color = transicol;

        }


        backgroundWorld2.material = MatbackgroundWorld2;
        backgroundWorld2.enabled = true;
        if (SceneChange.currentWorld == 2)
        {
            var transicol = backgroundWorld2.material.color;
            transicol.a = 1;
            backgroundWorld2.material.color = transicol;

        }

        backgroundWorld3.material = MatbackgroundWorld3;
        backgroundWorld3.enabled = true;
        if (SceneChange.currentWorld == 3)
        {
            var transicol = backgroundWorld3.material.color;
            transicol.a = 1;
            backgroundWorld3.material.color = transicol;

        }

    }

    IEnumerator LerperIn()
    {
        lerper += Time.deltaTime;
        Camera.main.transform.localPosition = Vector3.Lerp(startpos, new Vector3(7.5f, -1.02f, -9.59f), lerper);
        Camera.main.transform.localRotation = Quaternion.Lerp(Quaternion.identity, new Quaternion(-0.1731607f, -0.3162136f, 0.2146029f, 0.9077278f), lerper);
        if (lerper >= 1)
        {
            Camera.main.transform.localPosition = new Vector3(7.5f, -1.02f, -9.59f);
            MatBackgroundChange();
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(LerperIn());
        }
    }
}
