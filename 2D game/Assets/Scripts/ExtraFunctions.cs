using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraFunctions : MonoBehaviour
{
    #region map
    //credit to jessy https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/
    //map function, this returns a value after being mapped from one range to another 
    //for example 5 can be mapped from a range of 0 - 10 to a range of 0 - 100 this would return 50
    public static float Map(float value, float from1, float to1, float from2, float to2)
    {

        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
    #endregion

    #region Animate
    //credit to ketra games https://www.youtube.com/watch?v=MyVY-y_jK1I&t=182s
    //this function takes in parameters and returns the lerped value from a start to end (static numbers) the function must also recieve an elapsed time which will be kept track of by whatever is calling the function
    public static float Animate(float FirstValue, float EndValue, float Duration, float elapsedTime)
    {
        float percentageComplete = elapsedTime / Duration;
        return Mathf.Lerp(FirstValue, EndValue, percentageComplete);
    }
    #endregion

    #region Batch Play
    //the batch play and stop functions help to remove code duplication when playing multiple sounds at once
    public static void BatchPlay(string[] audios)
    {
        for (int i = 0; i < audios.Length; i++)
        {
            FindObjectOfType<AudioManager>().Play(audios[i]);
        }
    }
    #endregion

    #region BatchStop
    public static void BatchStop(string[] audios)
    {
        for(int i = 0; i < audios.Length; i++)
        {
            FindObjectOfType<AudioManager>().Stop(audios[i]);
        }
    }
    #endregion
}
