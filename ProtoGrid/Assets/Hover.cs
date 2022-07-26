using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class Hover : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    public GameObject Stars;
    RectTransform starsVisu;
    SceneChange sChange;
    float lerper;
    bool lerping;
    private void Start()
    {
        lerping = true;
        starsVisu = Stars.transform.Find("stars").GetComponent<RectTransform>();
        sChange = GridGenerator.Instance.sceneChange;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (lerping)
        {
            lerper = 0;
            lerping = false;
            StartCoroutine(Lerper(starsVisu.anchoredPosition.x, 0, starsVisu,3));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {

            lerper = 0;
            
            if(!lerping)
            {
                StopAllCoroutines();
               // StopCoroutine(Lerper(starsVisu.anchoredPosition.x, 0, starsVisu, 3));
            }
            lerping = false;
            StartCoroutine(Lerper(starsVisu.anchoredPosition.x, -161, starsVisu,3));

    }

    private void OnDisable()
    {
        lerping = true;
        starsVisu.anchoredPosition = new Vector2(-161, starsVisu.anchoredPosition.y);
    }

    IEnumerator Lerper(float startLerp, float endLerp, RectTransform moveObj, float speed)
    {

        lerper += Time.deltaTime * speed;
        var vec = moveObj.anchoredPosition;
        vec.x = Mathf.Lerp(startLerp, endLerp, lerper);
        moveObj.anchoredPosition = vec;

        if (lerper >= 1)
        {
            moveObj.anchoredPosition = new Vector2(endLerp, moveObj.anchoredPosition.y);
            lerper = 0;
            lerping = true;
            yield return new WaitForEndOfFrame();

        }
        else
        {

            yield return new WaitForEndOfFrame();
            StartCoroutine(Lerper(startLerp, endLerp, moveObj, speed));
        }
    }
}
