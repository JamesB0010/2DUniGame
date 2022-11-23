//https://www.youtube.com/watch?v=6OT43pvUyfY credit for class brackeys 
using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    #region Variables

    //an array of all the sounds the audio manager can play
    public Sound[] sounds;

    //this ensures that there is only ever 1 audio manager instance
    public static AudioManager instance;

    public string[] ambientMusic;

    //selection will be randomised and is used to play a random ambient music
    public int selection;

    #endregion


    #region Setup
    // Start is called before the first frame update
    void Awake()
    {

        //if there is already an audio manager destroy this one
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }


        //this allows the audio manager to be used across different scenes without the music being interrupted
        DontDestroyOnLoad(gameObject);

        //setup an audiosource on all of the sounds and set its values appropriatley 
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;

            s.source.pitch = s.pitch;

            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        //start playing a random track from the ambient music array

        //ambient 1 perfect
        //ambient 3 very good
        selection = UnityEngine.Random.Range(0, ambientMusic.Length);
        Play(ambientMusic[selection]);

    }

    #endregion

    #region public functions
    //pause or play a track 
    public void PausePlay(string name, string pausePlay)
    {
        Sound s = Array.Find(sounds, s => s.name == name);
        if (s == null)
        {
            Debug.LogWarning("The sound: " + name + " Was not found to be faded down");
        }
        if (pausePlay == "Play")
        {
            s.source.UnPause();
        }
        else
        {
            s.source.Pause();
        }
    }


    //play a track
    public void Play(string name)
    {
        //return the sound where the name is equal to the name parameter
        Sound s = Array.Find(sounds, s => s.name == name);
        if (s == null)
        {
            Debug.LogWarning("The sound: " + name + " was not found");
            return;
        }
        if (s.source.isPlaying && name != "BigRevs")
        {
            //Debug.Log(s.name + " Is already playing");
            return;
        }
        s.source.PlayOneShot(s.source.clip);
    }


    //stop playing a track
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, s => s.name == name);
        if (s == null)
        {
            Debug.LogWarning("The sound: " + name + "could not be stopped because it cannot be found");
            return;
        }
        s.source.Stop();
    }

    //if a track is already playing return true else return false
    public bool isPlaying(string name)
    {
        Sound s = Array.Find(sounds, s => s.name == name);
        if (s == null)
        {
            Debug.LogWarning("The sound: " + name + "could not be found");
            return false;
        }
        return s.source.isPlaying;
    }

    //set the volume of an audio source 
    public void setVolume(string name, float volume)
    {

        Sound s = Array.Find(sounds, s => s.name == name);
        if (s == null)
        {
            Debug.LogWarning("The sound: " + name + "could not be found");
            return;
        }
        s.source.volume = volume;
    }
    #endregion
}
