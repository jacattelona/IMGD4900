using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;

public class SliderEvent : UnityEvent<int>{}

public class MusicGroup : MonoBehaviour
{
    [SerializeField]
    int groupNumber = -1;                                       //group number of this music group
    [SerializeField]
    AudioMixer mixer;                                           //audio mixer to change values of

    const string FLANGE = "Flange";                             //constant flange variable string
    const string CHORUS = "Chorus";                             //constant chorus variable string
    const string REVERB = "Reverb";                             //constant reverb variable string

    private AudioSource[] sources;                              //List of Audio sources of children objects
    private bool isPlaying = false;                             //Indicates whether children Audio Sources are playing

    private int fadeIn = -1;                                    //AudioSource index to be faded in
    private int fadeOut = -1;                                   //AudioSource index to be faded out
    private float fadeVal = 0;                                  //value to set AudioSource volume to during fade
    private float fadeMult = .5f;                               //multiplier for fade time
    

    private int correctTrack = 0;                               //correct track number, set by GameManager
    private float correctValue = .5f;                           //correct slider value, set by GameManager
    public UnityEvent correctTrackEvent;                        //Event invoked on unMuting the correct track
    public UnityEvent incorrectTrackEvent;                      //Event invoked on unMuting the incorrect track
    public SliderEvent correctValueEvent;                       //Event invoked on choosing the correct slider value
    public SliderEvent incorrectValueEvent;                     //Event invoked on choosing the incorrect slider value

    /// <summary>
    /// Called on creation
    /// gets the audio sources of its children
    /// creates correct value Unity Events
    /// </summary>
    void Awake()
    {
        //Get audio sources
        sources = GetComponentsInChildren<AudioSource>();
        //create all events
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

    /// <summary>
    /// Sets the correctTrack and correctValue variables
    /// </summary>
    /// <param name="track"> value to set correctTrack to </param>
    /// <param name="val"> value to set correctValue to </param>
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

    /// <summary>
    /// Updates a specific audio effect dependent on group number
    /// </summary>
    /// <param name="val"> value between 0 and 1 to set the audio effect to </param>
    public void UpdateEffect(float val)
    {
        //give val a max of 1.0 and a min of 0
        if (val > 1.0f)
            val = 1.0f;
        if (val < 0)
            val = 0;

        //If groupNumber == 0, update reverb
        if (groupNumber == 0)
        {
            UpdateReverb(val);
        }

        //If groupNumber == 1, update chorus
        else if (groupNumber == 1)
        {
            UpdateChorus(val);
        }

        //fIf groupNumber ==2, update flange
        else if (groupNumber == 2)
        {
            UpdateFlange(val);
        }
    }

    /// <summary>
    /// Sets the chorus depth  to a value between 0 and .5f
    /// Invokes necessary slider event
    /// </summary>
    /// <param name="val"> value to set chorus to</param>
    void UpdateChorus(float val)
    {
        //Chorus depth must be a value between 0 and .5
        //Get current chorus depth value, convert to float between 0 and 1
        float f = 0;
        mixer.GetFloat(CHORUS, out f);
        f *= 2f;

        //If val is in range when it was not previously, invoke correctValueEvent
        if (InRange(val) && !InRange(f))
        {
            correctValueEvent.Invoke(groupNumber);
        }

        //If val isn't in range when it was previously, invoke incorrectValueEvent
        if (InRange(f) && !InRange(val))
        {
            incorrectValueEvent.Invoke(groupNumber);
        }

        //set chorus to val/2, ensures depth between 0 and .5
        mixer.SetFloat(CHORUS, val / 2f);
    }

    /// <summary>
    /// Sets reverb room to a value between -3000 and 500
    /// </summary>
    /// <param name="val"></param>
    void UpdateReverb(float val)
    {
        //Reverb room must be a value between -3000 and -500
        //Get current room value, convert to float between 0 and 1
        float f = 0;
        mixer.GetFloat(REVERB, out f);
        f += 3000;
        f /= 2500f;

        //If val is in range when it was not previously, invoke correctValueEvent
        if (InRange(val) && !InRange(f))
        {
            correctValueEvent.Invoke(groupNumber);
        }

        //If val isn't in range when it was previously, invoke incorrectValueEvent
        if (InRange(f) && !InRange(val))
        {
            incorrectValueEvent.Invoke(groupNumber);
        }

        //set reverb to val*2500 - 3000
        mixer.SetFloat(REVERB, (val * 2500f) - 3000);
    }

    /// <summary>
    /// Sets the flange depth to a value between 00 and 1
    /// </summary>
    /// <param name="val"></param>
    void UpdateFlange(float val)
    {
        //flange depth must be a value between 0 and 1
        float f = 0;
        mixer.GetFloat(FLANGE, out f);

        //If val is in range when it was not previously, invoke correctValueEvent
        if (InRange(val) && !InRange(f))
        {
            correctValueEvent.Invoke(groupNumber);
        }

        //If val isn't in range when it was previously, invoke incorrectValueEvent
        if (InRange(f) && !InRange(val))
        {
            incorrectValueEvent.Invoke(groupNumber);
        }

        //set flange to val
        mixer.SetFloat(FLANGE, val);
    }

    /// <summary>
    /// Determines whether given value is in range of correct value
    /// </summary>
    /// <param name="val"> float value between 0 and 1 to test</param>
    /// <returns> returns true if val is within .1 of correctValue in either direction </returns>
    bool InRange(float val)
    {
        return (val < correctValue + .1f && val > correctValue - .1f);
    }
}
