using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
public class LoadScene : MonoBehaviour
{
    DebugTools debugTools;

    private void Start()
    {
        debugTools = FindObjectOfType<DebugTools>();
    }
    public void SceneLoader(string yo)
    {
      /*  debugTools.mainMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);


        debugTools.mainMusic.r();*/



        yo = GetComponent<TMP_InputField>().text;
        if (yo == "PACMAN")
        {
            SceneManager.LoadScene("PACMAN");
        }
        else
        {
            SceneManager.LoadScene("Lvl_" + yo);

        }

    }
    
}
