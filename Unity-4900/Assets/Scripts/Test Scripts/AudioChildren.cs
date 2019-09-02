using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioChildren : MonoBehaviour
{
    AudioSource[] sources;
    // Start is called before the first frame update
    void Awake()
    {
        sources = GetComponentsInChildren<AudioSource>();
    }

    public void StartAll()
    {
        foreach (AudioSource source in sources)
        {
            source.Play();
            source.mute = true;
        }
    }

    public void StopAll()
    {
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
}
