using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioChildren : MonoBehaviour
{
    public bool isPlaying = false;

    AudioSource[] sources;
    // Start is called before the first frame update
    void Awake()
    {
        sources = GetComponentsInChildren<AudioSource>();
    }

    public void StartAll()
    {
        isPlaying = true;
        foreach (AudioSource source in sources)
        {
            source.Play();
            source.mute = true;
        }
    }

    public void StopAll()
    {
        isPlaying = false;
        foreach (AudioSource source in sources)
        {
            source.Stop();
        }
    }

    public void UnMute(int index)
    {
        foreach (AudioSource source in sources)
        {
            source.mute = true;
        }
        sources[index].mute = false;
    }

    public void UpdateMixer(AudioMixerGroup group)
    {
        foreach (AudioSource source in sources)
        {
            source.outputAudioMixerGroup = group;
        }
    }
}
