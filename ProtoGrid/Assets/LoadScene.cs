using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
public class LoadScene : MonoBehaviour
{
    public void SceneLoader(string yo)
    {
        yo = GetComponent<TMP_InputField>().text;
        SceneManager.LoadScene("Lvl_" + yo);

    }
    
}
