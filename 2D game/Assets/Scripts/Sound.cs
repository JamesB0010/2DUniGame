//https://www.youtube.com/watch?v=6OT43pvUyfY credit for class brackeys 
using UnityEngine.Audio;
using UnityEngine;

//allows the class to be transformed into a form unity can work better with and can store and reconstruct later
[System.Serializable]

//The audio manager stores an array of Sounds, the sound class holds the information necessary to create an audio source for each of the sounds at runtime to be played by the audio manager
public class Sound
{
    public string name;

    //the actual sound to be played 
    public AudioClip clip;

    //the volume of the sound, this can be adjusted in the editor with a slider 
    [Range(0f, 1f)]
    public float volume;

    //the pitch of the sound, this can be adjusted in the editor with a slider
    [Range (0.1f, 3f)]
    public float pitch;

    //does the sound loop
    public bool loop;

    //the audio source which acually plays the sound 
    //this field can not be accessed in the inspector
    [HideInInspector]
    public AudioSource source;
}
