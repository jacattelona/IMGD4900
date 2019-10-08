using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    const float MAT_MAX = .45f;
    const float MAT_RATE = .225f;

    enum MatState
    {
        None,
        Increasing,
        Decreasing
    }
    MatState matState = MatState.None;
    [SerializeField]
    Material glowMat;

    [SerializeField]
    private MusicGroup drum, rhythm, lead, end;                      //Music groups
    [SerializeField]
    private Tablet drumTablet, pianoTablet, guitarTablet;       //Music tablets
    [SerializeField]
    private FinalRock finalRock;                                //final tablet
    [SerializeField]
    private Tree tree;                                          //dancing tree

    private int numCorrectTracks = 0;                           //current number of correct Tracks
    private int numCorrectSliders = 0;                          //current number of correct Sliders

    enum VolState
    {
        None,
        Increasing,
        Decreasing
    }
    VolState volState = VolState.None;

    float volume = 0f;
    int volumeFocus = -1;
    float volumeRate = 10f;
    float volumeMax = 7f;
    string[] volumeLevels = { "DrumVolume", "RhythmVolume", "LeadVolume" };
    public AudioMixer mixer;

    /// <summary>
    /// Makes cursor invisible
    /// Start all music groups
    /// Adds listeners to all events
    /// </summary>
    void Start()
    {
        //lock cursor and make invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Starts music groups
        drum.StartAll();
        rhythm.StartAll();
        lead.StartAll();
        end.StartAll();

        //add listeners to drum music group
        drum.correctTrackEvent.AddListener(CorrectTrack);
        drum.incorrectTrackEvent.AddListener(IncorrectTrack);
        drum.correctValueEvent.AddListener(CorrectSlider);
        drum.incorrectValueEvent.AddListener(IncorrectSlider);

        //add listeners to rhythm music group
        rhythm.correctTrackEvent.AddListener(CorrectTrack);
        rhythm.incorrectTrackEvent.AddListener(IncorrectTrack);
        rhythm.correctValueEvent.AddListener(CorrectSlider);
        rhythm.incorrectValueEvent.AddListener(IncorrectSlider);

        //add listeners to lead music group
        lead.correctTrackEvent.AddListener(CorrectTrack);
        lead.incorrectTrackEvent.AddListener(IncorrectTrack);
        lead.correctValueEvent.AddListener(CorrectSlider);
        lead.incorrectValueEvent.AddListener(IncorrectSlider);

        //add listeners to tablets
        drumTablet.tabletActivate.AddListener(FocusMusic);
        pianoTablet.tabletActivate.AddListener(FocusMusic);
        guitarTablet.tabletActivate.AddListener(FocusMusic);

        glowMat.SetFloat("_node_3398", 0);

        drum.SetCorrect((int)Random.Range(0, 5.99f), Random.Range(.2f, .8f));
        rhythm.SetCorrect((int)Random.Range(0, 4.99f), Random.Range(.2f, .8f));
        lead.SetCorrect((int)Random.Range(0, 4.99f), Random.Range(.2f, .8f));
    }

    void Update()
    {
        if (volState != VolState.None)
        {
            volume += Time.deltaTime * volumeRate;

            for (int x = 0; x < volumeLevels.Length; x++)
            {
                string paramName = volumeLevels[x];

                if (x == volumeFocus)
                {
                    if (volState == VolState.Increasing)
                    {
                        mixer.SetFloat(paramName, 0 + volume);
                    }
                    else if (volState == VolState.Decreasing)
                    {
                        mixer.SetFloat(paramName, volumeMax - volume);
                    }
                }

                else if (x != volumeFocus)
                {
                    if (volState == VolState.Increasing)
                    {
                        mixer.SetFloat(paramName, 0 - volume);
                    }

                    else if (volState == VolState.Decreasing)
                    {
                        mixer.SetFloat(paramName, (volumeMax * -1) + volume);
                    }
                }
            }

            if (volume >= volumeMax)
            {
                volume = 0;

                if (volState == VolState.Decreasing)
                {
                    volumeFocus = -1;
                }

                volState = VolState.None;
            }
        }


        if (matState == MatState.Increasing)
        {
            float val = glowMat.GetFloat("_node_3398");
            if (val < MAT_MAX)
            {
                val += Time.deltaTime * MAT_RATE;
                glowMat.SetFloat("_node_3398", val);
            }
            else
            {
                matState = MatState.None;
            }
        }
        else if (matState == MatState.Decreasing)
        {
            float val = glowMat.GetFloat("_node_3398");
            if (val > 0)
            {
                val -= Time.deltaTime * MAT_RATE;
                glowMat.SetFloat("_node_3398", val);
            }
            else
            {
                matState = MatState.None;
            }
        }
    }


    /// <summary>
    /// Called when a music group unmutes the correct track
    /// </summary>
    void CorrectTrack()
    {
        //increment correct track
        numCorrectTracks++;

        //tell tree to start dancing
        tree.StartDancing();

        //If there are now 3 correct tracks
        if (numCorrectTracks == 3)
        {
            //Activate all sliders
            drumTablet.ActivateSlider();
            pianoTablet.ActivateSlider();
            guitarTablet.ActivateSlider();

            matState = MatState.Increasing;
        }
    }

    /// <summary>
    /// Called when a music group unmutes the incorrect track, after it previously had unmuted the correct one
    /// </summary>
    void IncorrectTrack()
    {
        //decrement correct track
        numCorrectTracks--;

        //tell the tree to stop dancing
        tree.StopDancing();

        //If there are no longer 3 correct tracks
        if (numCorrectTracks != 3)
        {
            //Deactivate all sliders
            drumTablet.DeactivateSlider();
            pianoTablet.DeactivateSlider();
            guitarTablet.DeactivateSlider();

            matState = MatState.Decreasing;
        }
    }

    /// <summary>
    /// Called when a music group sets its slider to the correct value
    /// </summary>
    /// <param name="group">
    /// music group number corresponding to tablet
    /// 0 = drumTablet
    /// 1 = pianoTablet
    /// 2 = guitarTablet
    /// </param>
    void CorrectSlider(int group)
    {
        //increment correct Sliders
        numCorrectSliders++;

        //Activate particles of the proper tablet
        if (group == 0)
        {
            drumTablet.ActivateParticles();
        }

        else if (group == 1)
        {
            pianoTablet.ActivateParticles();
        }

        else if (group == 2)
        {
            guitarTablet.ActivateParticles();
        }

        //if all sliders are correct, activate final rock
        if (numCorrectSliders == 3)
        {
            finalRock.Activate();
        }
    }

    /// <summary>
    /// Called when a music group sets its slider to the incorrect value, after it previously had set it correctly
    /// </summary>
    /// <param name="group"> 
    /// music group number corresponding to tablet
    /// 0 = drumTablet
    /// 1 = pianoTablet
    /// 2 = guitarTablet
    /// </param>
    void IncorrectSlider(int group)
    {
        //decrement correct sliders
        numCorrectSliders--;

        //DeActivate particles of proper tablet
        if (group == 0)
        {
            drumTablet.DeactivateParticles();
        }

        else if (group == 1)
        {
            pianoTablet.DeactivateParticles();
        }

        else if (group == 2)
        {
            guitarTablet.DeactivateParticles();
        }
    }

    /// <summary>
    /// Raises the volume of the associated group, lowers all others
    /// </summary>
    /// <param name="group"> group number to adjust volume of </param>
    void FocusMusic(int group)
    {
        if (group != -1 && group != volumeFocus)
        {
            volumeFocus = group;
            volState = VolState.Increasing;
        }
        else if (group == -1 && volumeFocus != -1)
        {
            volState = VolState.Decreasing;
        }
    }
}
