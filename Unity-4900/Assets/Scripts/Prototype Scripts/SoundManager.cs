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

    void Update()
    {
        int audioChoice = 0;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            audioChoice = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            audioChoice = 2;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            audioChoice = 3;

        if (Input.GetKeyDown(KeyCode.Alpha8))
            audioChoice = 8;
        if (Input.GetKeyDown(KeyCode.Alpha9))
            audioChoice = 9;

        if (audioChoice != 0)
        {
            if (!drums.GetPlaying() && audioChoice < 4)
            {
                drums.StartAll();
                leads.StartAll();

                drums.UnMute(audioChoice - 1);
            }

            else if (drums.GetPlaying())
            {
                if (audioChoice < 4)
                {
                    drums.UnMute(audioChoice - 1);
                    if (audioChoice == 2)
                        tree.StartDancing();
                    else if (audioChoice != 2)
                        tree.StopDancing();
                }

                else
                {
                    leads.UnMute(audioChoice - 8);
                }

            }


        }
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
