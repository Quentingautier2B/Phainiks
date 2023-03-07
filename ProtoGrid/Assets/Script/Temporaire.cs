using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Temporaire : MonoBehaviour
{
    [SerializeField] List<GameObject> CacheImage;
    [SerializeField] List<GameObject> CacheText;

    bool OnOff;

    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        Cursor.visible = false;

        if (Input.GetKeyDown(KeyCode.O) && !OnOff)
        {
            foreach (var  image in CacheImage)
            {
                image.GetComponent<Image>().enabled = false;
            }

            foreach (var item in CacheText)
            {
                item.GetComponent<TMP_Text>().enabled = false;
            }
            OnOff = true;
        }
        else if (Input.GetKeyDown(KeyCode.O) && OnOff)
        {
            foreach (var image in CacheImage)
            {
                image.GetComponent<Image>().enabled = true;
            }

            foreach (var item in CacheText)
            {
                item.GetComponent<TMP_Text>().enabled = true;
            }
            OnOff = false;
        }
    }
}
