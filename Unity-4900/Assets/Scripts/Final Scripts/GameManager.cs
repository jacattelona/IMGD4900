using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private MusicGroup drum, rhythm, lead;
    [SerializeField]
    private Tablet drumTablet, pianoTablet, guitarTablet;
    [SerializeField]
    private FinalRock finalRock;
    [SerializeField]
    private Tree tree;

    private int numCorrectTracks = 0;
    private int numCorrectSliders = 0;

    /// <summary>
    /// Add listeners for all music group and tablet events
    /// </summary>
    void Awake()
    {



    }

    /// <summary>
    /// Start all music groups
    /// </summary>
    void Start()
    {
        drum.StartAll();
        rhythm.StartAll();
        lead.StartAll();

        drum.correctTrackEvent.AddListener(CorrectTrack);
        drum.incorrectTrackEvent.AddListener(IncorrectTrack);
        drum.correctValueEvent.AddListener(CorrectSlider);
        drum.incorrectValueEvent.AddListener(IncorrectSlider);

        rhythm.correctTrackEvent.AddListener(CorrectTrack);
        rhythm.incorrectTrackEvent.AddListener(IncorrectTrack);
        rhythm.correctValueEvent.AddListener(CorrectSlider);
        rhythm.incorrectValueEvent.AddListener(IncorrectSlider);

        lead.correctTrackEvent.AddListener(CorrectTrack);
        lead.incorrectTrackEvent.AddListener(IncorrectTrack);
        lead.correctValueEvent.AddListener(CorrectSlider);
        lead.incorrectValueEvent.AddListener(IncorrectSlider);

        drumTablet.tabletActivate.AddListener(FocusMusic);
        pianoTablet.tabletActivate.AddListener(FocusMusic);
        guitarTablet.tabletActivate.AddListener(FocusMusic);
    }


    /// <summary>
    /// Called when a music group unmutes the correct track
    /// </summary>
    void CorrectTrack()
    {
        numCorrectTracks++;
        print("Hell yeah, " + numCorrectTracks);

        tree.StartDancing();
        //If there are now 3 correct tracks
        if (numCorrectTracks == 3)
        {
            //Activate all sliders
            drumTablet.ActivateSlider();
            pianoTablet.ActivateSlider();
            guitarTablet.ActivateSlider();
        }
    }

    /// <summary>
    /// Called when a music group unmutes the incorrect track, after it previously had unmuted the correct one
    /// </summary>
    void IncorrectTrack()
    {
        numCorrectTracks--;

        print("Hell nah, " + numCorrectTracks);

        tree.StopDancing();
        //If there are no longer 3 correct tracks
        if (numCorrectTracks != 3)
        {
            //Deactivate all sliders
            drumTablet.DeactivateSlider();
            pianoTablet.DeactivateSlider();
            guitarTablet.DeactivateSlider();
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
        numCorrectSliders++;

        print("AWESOME " + numCorrectSliders);

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

        //if all sliders are correct
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
        numCorrectSliders--;
        print("NOOOOO " + numCorrectSliders);

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
    /// 
    /// </summary>
    /// <param name="group"></param>
    void FocusMusic(int group)
    {
        string rock = "";
        if (group == 0)
            rock = "drums";
        if (group == 1)
            rock = "piano";
        if (group == 2)
            rock = "guitar";
        if (group == -1)
            rock = "none";

        print("Focusing on rock " + rock);
    }
}
