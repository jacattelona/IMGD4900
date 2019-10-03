using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private MusicGroup drum, rhythm, lead;                      //Music groups
    [SerializeField]
    private Tablet drumTablet, pianoTablet, guitarTablet;       //Music tablets
    [SerializeField]
    private FinalRock finalRock;                                //final tablet
    [SerializeField]
    private Tree tree;                                          //dancing tree

    private int numCorrectTracks = 0;                           //current number of correct Tracks
    private int numCorrectSliders = 0;                          //current number of correct Sliders


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
