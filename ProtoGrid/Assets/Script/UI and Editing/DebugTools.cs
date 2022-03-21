using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DebugTools : MonoBehaviour
{
    public bool debugModOn;
    public FMOD.Studio.EventInstance mainMusic;
    public FMOD.Studio.EventInstance redMusic;
    public FMOD.Studio.EventInstance blueMusic;
    public FMOD.Studio.EventInstance greenMusic;

    public GameObject System;
    public GameObject SecondarySystem;
    public GameObject Player;
    public GameObject Terrain;
    bool SceneLoaded;

    private void Awake()
    {
        
/*        mainMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Tiles/Main");
        redMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Tiles/Red");
        blueMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Tiles/Blue");
        greenMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Tiles/Green");*/

        //TileMusic(mainMusic, "Main");
        /*TileMusic(redMusic, "Red","VolumeRed");
        TileMusic(blueMusic, "Blue", "VolumeBlue");
        TileMusic(greenMusic, "Green", "VolumeGreen");*/
       

        /*        mainMusic.start();
                redMusic.start();
                blueMusic.start();
                greenMusic.start();*/

    }

    void TileMusic(FMOD.Studio.EventInstance musicInstance, string name, string parameterName)
    {
        musicInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Tiles/" + name);
        musicInstance.start();       
        musicInstance.setParameterByName(parameterName,-18);
    }

    private void OnDrawGizmos()
    {
        
        if (!SceneLoaded && !FindObjectOfType<GridGenerator>())
        {

            Instantiater(System);
            Instantiater(SecondarySystem);
            Instantiater(Player);
            Instantiater(Terrain);
            SceneLoaded = true;
        }
        

    }

    void Instantiater(GameObject obj)
    {
        if (!GameObject.Find(obj.name))
        {
            var inst = Instantiate(obj);
            inst.name = obj.name;
        }
    }

}
