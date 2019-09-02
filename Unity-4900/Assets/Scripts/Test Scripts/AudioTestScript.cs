using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script is meant to be used as demonstration and experimentation of  
 * the audio effect adjustments used in the complete project
 * The code may be ugly, or unreadable, or inefficient, but it will be re-written
 * in another script for the final project
 * This script is only needed to explore ideas
 */

/*
 * CURRENT CONTROLS
 * Left / Right Arrows: Decrease and Increase Reverb on the Drum Track
 * Up / Down Arrows: Increase and Decrease chorus on the Drum Track
 * 1-3: Select a "Drum" track to start Playing
 * 8-9: Select a "Lead" track to Play (can only be played when there is a drum track playing)
 * 0: Stop all tracks
 * 
 * Only 1 of the drum tracks is actually a drum line, I'm just reusing audio from a previous
 * project so I don't have to make more right now
 */

public class AudioTestScript : MonoBehaviour
{
    AudioSource aud;
    AudioChorusFilter chorus;
    AudioReverbFilter reverb;
    Material mat;
    Transform t;

    public AudioClip[] drums;
    public AudioChildren children;

    float cRate = .5f;
    float rRate = 2500f;

    //Get all audio components attached to the object
    void Start()
    {
        aud = GetComponent<AudioSource>();
        chorus = GetComponent<AudioChorusFilter>();
        reverb = GetComponent<AudioReverbFilter>();
        mat = GetComponentInChildren<Renderer>().material;
        t = this.transform;

    }

    //Updates the Reverb and Chorus
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            aud.Stop();
            aud.PlayOneShot(drums[0]);

            children.StopAll();
            children.StartAll();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            aud.Stop();
            aud.PlayOneShot(drums[1]);

            children.StopAll();
            children.StartAll();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            aud.Stop();
            aud.PlayOneShot(drums[2]);

            children.StopAll();
            children.StartAll();
        }

        if (aud.isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                children.UnMute(0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                children.UnMute(1);
            }
        }

        UpdateChorus();
        UpdateReverb();
    }

    //Val is a float value between 0 and 1, 0 representing min value and 1 representing max value
    void SetChorusDepth(float val)
    {
        if (val > 1.0f) val = 1.0f;
        if (val < 0f) val = 0;
        chorus.depth = val/2f;
    }

    //Val is a float between 0 and 1, 0 representing min and 1 representing max
    void SetReverbRoom(float val)
    {
        if (val > 1.0f) val = 1.0f;
        if (val < 0) val = 0;

        reverb.room = (val * 2500f) - 3000f;
    }

    //Reverb Room size is set to a variable between -3000 and -500
    void UpdateReverb()
    {
        //More reverb if you hold the Right Arrow
        if (Input.GetKey(KeyCode.RightArrow))
        {
            reverb.room += rRate * Time.deltaTime;
        }

        //Less reverb if you hold the Left Arrow
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            reverb.room -= rRate * Time.deltaTime;
        }

        //Make sure we don't exceed bounds
        if (reverb.room < -3000) reverb.room = -3000;
        if (reverb.room > -500) reverb.room = -500;

        //Set scale of cube based on Reverb. The more reverb, the bigger the cube
        float roomScale = reverb.room + 3000f;
        float trueScale = roomScale / (2500 / 5);
        trueScale += .5f;
        t.localScale = new Vector3(trueScale, trueScale, trueScale);

    }


    //ChorusDepth is set to a value between 0 and .5
    void UpdateChorus()
    {
        //More chorus if holding up arrow
        if (Input.GetKey(KeyCode.UpArrow) && chorus.depth < .5f)
            chorus.depth += cRate * Time.deltaTime;

        //Less chorus if holding down arrow
        else if (Input.GetKey(KeyCode.DownArrow) && chorus.depth > 0)
            chorus.depth -= cRate * Time.deltaTime;

        //Make sure chorus doesn't exceed bounds
        if (chorus.depth > .5f) chorus.depth = .5f;
        if (chorus.depth < 0) chorus.depth = 0;

        //Cube gets darker the less chorus is active
        float col = chorus.depth * 2f;
        mat.color = new Color(col, col, col);
    }
}
