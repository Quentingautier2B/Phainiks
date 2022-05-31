using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class DebugTools : MonoBehaviour
{
    public float levelIndex;
    public int World;
    public bool debugModOn;
    public FMOD.Studio.EventInstance mainMusic;

    public GameObject System;
    public GameObject SecondarySystem;
    public GameObject Player;
    public GameObject Terrain;
    public GameObject Decor;
    bool SceneLoaded;
    static bool isPlaying = false;
    private void Awake()
    {
/*        FMOD.Studio.PLAYBACK_STATE playbackState;
        mainMusic.getPlaybackState(out playbackState);
        bool isPlaying = playbackState != FMOD.Studio.PLAYBACK_STATE.STOPPED;*/

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



    private void OnDrawGizmos()
    {
        
        if (!SceneLoaded && !FindObjectOfType<GridGenerator>())
        {

            Instantiater(System);
            Instantiater(SecondarySystem);
            Instantiater(Player);
            Instantiater(Terrain);
            Instantiater(Decor);
            SceneLoaded = true;
        }
        

    }
    private void OnApplicationQuit()
    {
        
        
    }
    void Instantiater(GameObject obj)
    {
        if (!GameObject.Find(obj.name))
        {
#if UNITY_EDITOR
            Selection.activeObject = PrefabUtility.InstantiatePrefab(obj);
            var inst = Selection.activeObject as GameObject;
#endif

#if !UNITY_EDITOR
            var inst = Instantiate(obj);
#endif
            inst.name = obj.name;

        }
    }

}
