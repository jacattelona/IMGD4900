using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;

public class SliderEvent : UnityEvent<int>{}

public class MusicGroup : MonoBehaviour
{
    [SerializeField]
    int groupNumber = -1;
    [SerializeField]
    AudioMixer mixer;

    string flange = "Flange";
    string chorus = "Chorus";
    string reverb = "Reverb";

    private AudioSource[] sources;                              //List of Audio sources of children objects
    private bool isPlaying = false;                             //Indicates whether children Audio Sources are playing

    private int fadeIn = -1;                                    //AudioSource index to be faded in
    private int fadeOut = -1;                                   //AudioSource index to be faded out
    private float fadeVal = 0;                                  //value to set AudioSource volume to during fade
    private float fadeMult = .5f;                               //multiplier for fade time
    

    private int correctTrack = 0;
    private float correctValue = .5f;
    public UnityEvent correctTrackEvent;
    public UnityEvent incorrectTrackEvent;
    public SliderEvent correctValueEvent;
    public SliderEvent incorrectValueEvent;

    /// <summary>
    /// Called on creation
    /// gets the audio sources of its children
    /// creates correct value Unity Events
    /// </summary>
    void Awake()
    {
        sources = GetComponentsInChildren<AudioSource>();
        correctTrackEvent = new UnityEvent();
        incorrectTrackEvent = new UnityEvent();
        correctValueEvent = new SliderEvent();
        incorrectValueEvent = new SliderEvent();
    }

    /// <summary>
    /// Update fade value and volume for sources if fadeVal > 0
    /// </summary>
    void Update()
    {
        //if fadeVal > 0
        if (fadeVal > 0f)
        {
            //increase fadeVal by time since last call
            fadeVal += Time.deltaTime * fadeMult;

            //if fadeIn is a valid target, increase volume of fadeIn source
            if (fadeIn >= 0)
            {
                sources[fadeIn].volume = fadeVal;
            }

            //if fadeOut is a valid target, decrease volume of fadeOut source
            if (fadeOut >= 0)
            {
                sources[fadeOut].volume = 1.0f - fadeVal;
            }

            //if fadeVal >= 0, we are done fading, set fadeVal to 0
            if (fadeVal >= 1.0f)
            {
                fadeVal = 0;
            }

        }
    }

    /// <summary>
    /// Starts playing track associated with each audio source
    /// </summary>
    public void StartAll()
    {
        //Set isPlaying to true
        isPlaying = true;

        //Set volume of each Audio Source to 0 and play
        foreach(AudioSource source in sources)
        {
            source.volume = 0;
            source.Play();
        }
    }

    /// <summary>
    /// Stops playing track associated with each audio source
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

    public void SetCorrect(int track, float val)
    {
        correctTrack = track;
        correctValue = val;
    }

    /// <summary>
    /// Tells whether the children audio sources are playing
    /// </summary>
    /// <returns> bool: isPlaying </returns>
    public bool IsPlaying()
    {
        return isPlaying;
    }

    /// <summary>
    /// Unmutes the source indicated by param "index"
    /// If the indicated source is already unmuted, function does nothing
    /// Invokes the correctTrackEvent if the correctTrack was unmuted
    /// </summary>
    /// <param name="index"> Source number to unmute </param>
    public void UnMute(int index)
    {
        //if index is not the currently unmuted source
        if (fadeIn != index)
        {
            //if the index is the correct track
            if (index == correctTrack)
            {
                //fire correct track event
                correctTrackEvent.Invoke();
            }

            //if index is not the correct track, and the correct track was previously playing
            else if (index != correctTrack && fadeIn == correctTrack)
            {
                //fire incorrect track event
                incorrectTrackEvent.Invoke();
            }

            //set current track to fade out, set index track to fade in
            fadeOut = fadeIn;
            fadeIn = index;
            
            //give fadeVal a positive value to start the fade
            fadeVal = .01f;
        }
    }


    public void UpdateEffect(float val)
    {
        if (val > 1.0f)
            val = 1.0f;
        if (val < 0)
            val = 0;

        //reverb
        if (groupNumber == 0)
        {
            UpdateReverb(val);
        }

        //chorus
        else if (groupNumber == 1)
        {
            UpdateChorus(val);
        }

        //flange
        else if (groupNumber == 2)
        {
            UpdateFlange(val);
        }
    }

    void UpdateChorus(float val)
    {
        float f = 0;
        mixer.GetFloat(chorus, out f);
        f *= 2f;

        if (InRange(val) && !InRange(f))
        {
            correctValueEvent.Invoke(groupNumber);
            print("chorus invoked");
        }

        if (InRange(f) && !InRange(val))
        {
            incorrectValueEvent.Invoke(groupNumber);
        }


        mixer.SetFloat(chorus, val / 2f);
    }

    void UpdateReverb(float val)
    {
        //-500 to -3000
        float f = 0;
        mixer.GetFloat(reverb, out f);
        f += 3000;
        f /= 2500f;

        if (InRange(val) && !InRange(f))
        {
            correctValueEvent.Invoke(groupNumber);
            print("reverb invoked");
        }

        if (InRange(f) && !InRange(val))
        {
            incorrectValueEvent.Invoke(groupNumber);
        }

        mixer.SetFloat(reverb, (val * 2500f) - 3000);
    }

    void UpdateFlange(float val)
    {
        float f = 0;
        mixer.GetFloat(flange, out f);

        if (InRange(val) && !InRange(f))
        {
            correctValueEvent.Invoke(groupNumber);
            print("flange invoked");
        }

        if (InRange(f) && !InRange(val))
        {
            incorrectValueEvent.Invoke(groupNumber);
        }

        mixer.SetFloat(flange, val);
    }

    bool InRange(float val)
    {
        return (val < correctValue + .1f && val > correctValue - .1f);
    }
}
