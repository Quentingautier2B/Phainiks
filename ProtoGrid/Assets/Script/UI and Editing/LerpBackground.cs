using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LerpBackground : MonoBehaviour
{
    private float lerperFadeIn;
    private float lerperFadeOut;
    [SerializeField] MeshRenderer backgroundTuto;
    [SerializeField] MeshRenderer backgroundWorld1;
    [SerializeField] MeshRenderer backgroundWorld2;
    [SerializeField] MeshRenderer backgroundWorld3;
    [SerializeField] Button falseTutoLeft, tutoRight, world1Left, world1Right, world2Left, world2right, world3Left, falseworld3Right;

    private void Start()
    {
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
}
