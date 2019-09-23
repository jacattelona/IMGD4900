using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundGroup : MonoBehaviour
{
    bool isPlaying = false;                 //tells whether tracks are currently playing

    AudioSource[] sources;                  //array of audio sources for all children


    /// <summary>
    /// Gets audio sources of all children, stores in sources array
    /// </summary>
    void Awake()
    {
        sources = GetComponentsInChildren<AudioSource>();
    }

    /// <summary>
    /// Returns whether the array of Audio Sources is playing
    /// </summary>
    public bool GetPlaying()
    {
        return isPlaying;
    }

    /// <summary>
    /// Starts playing the sound associated with each audio source, keeping them all muted
    /// </summary>
    public void StartAll()
    {
        //Set isPlaying to true
        isPlaying = true;
        //Mute and stop every audio source
        foreach (AudioSource source in sources)
        {
            source.mute = true;
            source.Play();
        }
    }

    /// <summary>
    /// Stops playing sound associated with each audio source
    /// </summary>
    public void StopAll()
    {
        //Set isPlaying to false
        isPlaying = false;
        //Mute and stop every audio source
        foreach (AudioSource source in sources)
        {
            source.mute = true;
            source.Stop();
        }
    }

    /// <summary>
    /// Unmutes the source indicated by param "index"
    /// </summary>
    /// <param name="index"> Source number to unmute </param>
    public void UnMute(int index)
    {
        //Mute every audio source
        foreach (AudioSource source in sources)
        {
            source.mute = true;
        }
        //UnMute the indicated source
        sources[index].mute = false;
    }

    /// <summary>
    /// Swaps the output of all sources to AudioMixerGroup "group"
    /// </summary>
    /// <param name="group"> Group to route audio of sources through </param>
    public void UpdateGroup(AudioMixerGroup group)
    {
        //For every source, set the output to param group
        foreach (AudioSource source in sources)
        {
            source.outputAudioMixerGroup = group;
        }
    }

    public void UpdateEffect(float val)
    {
        if (val > 1.0f)
            val = 1.0f;
        if (val < 0)
            val = 0;
    }
}
