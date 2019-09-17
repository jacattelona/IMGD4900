using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioMixer mixer;

    public SoundGroup drums;
    public SoundGroup leads;

    public AudioMixerGroup[] eqs;

    public TreeManager tree;

    int currentRock = -1;
    float chorusVal = 0;

    void Update()
    {
        int audioChoice = 0;
        if (currentRock == 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                audioChoice = 1;
            if (Input.GetKeyDown(KeyCode.Alpha2))
                audioChoice = 2;
            if (Input.GetKeyDown(KeyCode.Alpha3))
                audioChoice = 3;
        }

        if (currentRock == 1 && tree.IsCorrect(0))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                audioChoice = 8;
            if (Input.GetKeyDown(KeyCode.Alpha2))
                audioChoice = 9;
        }

        if (currentRock == 2 && tree.IsCorrect(1))
        {
            bool temp = false;
            if (Input.GetKey(KeyCode.Alpha1))
            {
                chorusVal -= (Time.deltaTime * .2f);
                temp = true;
            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                chorusVal += (Time.deltaTime * .2f);
                temp = true;
            }

            if (chorusVal > 1.0f)
                chorusVal = 1.0f;
            if (chorusVal < 0)
                chorusVal = 0;
            //print(chorusVal);
            if (temp)
            {
                SetChorus(chorusVal);
                if (chorusVal > .5f && chorusVal < .7f)
                    tree.StartDancing(currentRock);
                else
                    tree.StopDancing(currentRock);
            }
        }


        if (audioChoice != 0)
        {
            if (!drums.GetPlaying() && audioChoice <= 4)
            {
                drums.StartAll();
                leads.StartAll();

                drums.UnMute(audioChoice - 1);
                if (audioChoice == 2)
                    tree.StartDancing(currentRock);
                else if (audioChoice != 2)
                    tree.StopDancing(currentRock);
            }

            else if (drums.GetPlaying())
            {
                if (audioChoice <= 4)
                {
                    drums.UnMute(audioChoice - 1);
                    if (audioChoice == 2)
                        tree.StartDancing(currentRock);
                    else if (audioChoice != 2)
                        tree.StopDancing(currentRock);
                }

                else
                {
                    leads.UnMute(audioChoice - 8);
                    if (audioChoice == 8)
                        tree.StartDancing(currentRock);
                    else
                        tree.StopDancing(currentRock);
                }

            }


        }
    }

    public void SetRock(int rock)
    {
        currentRock = rock;
    }

    /// <summary>
    /// Sets the chorus depth of this sound group
    /// </summary>
    /// <param name="val"> value between 0 and 1 </param>
    public void SetChorus(float val)
    {
        if (val > 1.0f) val = 1.0f;
        if (val < 0f) val = 0;
        //chorus.depth = val / 2f;
        mixer.SetFloat("DepthLevel", val / 2f);
    }


    /// <summary>
    /// Sets the reverb room size for this sound group
    /// </summary>
    /// <param name="val"> value between 0 and 1 </param>
    public void SetReverb(float val)
    {
        if (val > 1.0f) val = 1.0f;
        if (val < 0) val = 0;
        mixer.SetFloat("RoomLevel", (val * 2500f) - 3000f);
    }
}
