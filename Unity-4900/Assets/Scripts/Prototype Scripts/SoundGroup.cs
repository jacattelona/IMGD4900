using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundGroup : MonoBehaviour
{
    public TreeManager tree;
    public int effectNum;
    public float effectVal;
    public AudioMixer mixer;

    bool isPlaying = false;                 //tells whether tracks are currently playing

    AudioSource[] sources;                  //array of audio sources for all children


    int correct = 0;
    float currentFade = 0f;
    int fadeIn = -1;
    int fadeOut = -1;

    /// <summary>
    /// Gets audio sources of all children, stores in sources array
    /// </summary>
    void Awake()
    {
        sources = GetComponentsInChildren<AudioSource>();
    }

    void Update()
    {
        if (currentFade > 0f)
        {
            currentFade += Time.deltaTime * .5f;
            if (fadeIn >= 0)
                sources[fadeIn].volume = currentFade;
            if (fadeOut >= 0)
                sources[fadeOut].volume = 1.0f - currentFade;
            if (currentFade >= 1.0f)
                currentFade = 0;
        }
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
            source.volume = 0;
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
            source.volume = 0;
            source.Stop();
        }
    }

    /// <summary>
    /// Unmutes the source indicated by param "index"
    /// </summary>
    /// <param name="index"> Source number to unmute </param>
    public void UnMute(int index)
    {
        if (index == correct && fadeIn != index)
            tree.StartDancing();

        else if (index != correct && fadeIn == correct)
            tree.StopDancing();

        fadeOut = fadeIn;
        fadeIn = index;
        currentFade = .01f;
        print("Unmuted " + index + ", Muted "+ fadeOut);
    }


    public void UpdateEffect(float val)
    {
        if (val > 1.0f)
            val = 1.0f;
        if (val < 0)
            val = 0;

        //reverb
        if (effectNum == 0)
        {
            UpdateReverb(val);
        }

        //chorus
        else if (effectNum == 1)
        {
            UpdateChorus(val);
        }

        //flange
        else if (effectNum == 2)
        {
            UpdateFlange(val);
        }
    }

    void UpdateChorus(float val)
    {
        float f = 0;
        mixer.GetFloat("Chorus", out f);
        f *= 2f;

        if (InRange(val) && !InRange(f))
        {
            print("In Range");
            GameObject.Find("piano_rock_01").GetComponent<ParticleSystem>().Play();
            GameObject.Find("Final_Rock").GetComponent<FinalRock>().UpdateCorrect(1);
        }

        if (InRange(f) && !InRange(val))
        {
            print("Out of Range");
            GameObject.Find("piano_rock_01").GetComponent<ParticleSystem>().Stop();
            GameObject.Find("Final_Rock").GetComponent<FinalRock>().UpdateCorrect(-1);
        }


        mixer.SetFloat("Chorus", val/2f);
    }

    void UpdateReverb(float val)
    {
        //-500 to -3000
        float f = 0;
        mixer.GetFloat("Reverb", out f);
        f += 3000;
        f /= 2500f;

        if (InRange(val) && !InRange(f))
        {
            print("In Range");
            GameObject.Find("drum_rock_01").GetComponent<ParticleSystem>().Play();
            GameObject.Find("Final_Rock").GetComponent<FinalRock>().UpdateCorrect(1);
        }

        if (InRange(f) && !InRange(val))
        {
            print("Out of Range");
            GameObject.Find("drum_rock_01").GetComponent<ParticleSystem>().Stop();
            GameObject.Find("Final_Rock").GetComponent<FinalRock>().UpdateCorrect(-1);
        }

        mixer.SetFloat("Reverb", (val * 2500f) - 3000);
    }

    void UpdateFlange(float val)
    {
        float f = 0;
        mixer.GetFloat("Flange", out f);

        if (InRange(val) && !InRange(f))
        {
            print("In Range");
            GameObject.Find("guitar_rock_01").GetComponent<ParticleSystem>().Play();
            GameObject.Find("Final_Rock").GetComponent<FinalRock>().UpdateCorrect(1);
        }

        if (InRange(f) && !InRange(val))
        {
            print("Out of Range");
            GameObject.Find("guitar_rock_01").GetComponent<ParticleSystem>().Stop();
            GameObject.Find("Final_Rock").GetComponent<FinalRock>().UpdateCorrect(-1);
        }

        mixer.SetFloat("Flange", val);
    }

    bool InRange(float val)
    {
        return (val < effectVal + .1f && val > effectVal - .1f);
    }
}
