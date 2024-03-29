﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;    //allows us to mess with Unity audio
using UnityEngine.UI;  //allows us to interact with UI
using UnityEngine.UIElements;  //allows us to interact with the UI objects
using Button1 = UnityEngine.UIElements.Button;  //Button 1 (each rock has 3 buttons)
using Button2 = UnityEngine.UIElements.Button;  //Button 2
using Button3 = UnityEngine.UIElements.Button;  // Button 3

public class SoundManager : MonoBehaviour
{
    public AudioMixer mixer;

    public SoundGroup drums;
    public SoundGroup leads;
    public SoundGroup rhythm;
    public TreeManager tree;

    SoundGroup currentGroup;

    int currentRock = -1;
    float chorusVal = 0;
    
    // The following bools will be used for the sequence of appearance
    bool drumbuttoncorrect = false; // if the correct drum loop is chosen, change to true
    bool pianobuttoncorrect = false;    // if the correct paino loop is chosen change to true
    bool guitarbuttoncorrect = false;   //if the correct guitar loop is chosen change to true
    bool drumslidercorrect = false;     // if the correct level of effect is chosen, change to true
    bool pianoslidercorrect = false;    // if the correct level of effect is chosen, change to true
    bool guitarslidercorrect = false;   // if the correct level of effect is chosen, change to true
    
   // public string Button1 { get; private set; }
   // public string Button1 { get; private set; }
   // public string Button1 { get; private set; }

    void Start()
    {
        drums.StartAll();
        leads.StartAll();
        rhythm.StartAll();
    }

    void Update()
    {
        int audioChoice = 0;
        
         //summary
        // if the drum track was the correct choice, show the next rock UI
        if (drumbuttoncorrect)
        {
            //show pianobutton
        }

        //summary
        // if the piano track was correctly chosen, show the guitar UI
        if (pianobuttoncorrect)
        {
            // show guitar button
        }

        //summary
        // if the guitar button was chosen correctly, show the drum slider
        if (guitarbuttoncorrect)
        {
            //show drumslider
        }

        //summary
        // if the drum slider was chosen correctly show the piano slider
        if (drumslidercorrect)
        {
            //show piano slider
        }

        //summary
        // if the piano slider was correct show the guitar slider
        if (pianoslidercorrect)
        {
            // show guitar slider
        }

        //summary
        //if the guitar slider was chosen correctly show the final button
        if (guitarslidercorrect)
        {
            //show final button
        }
        
       
        // THIS IS PART OF WHERE THE ISSUES ARE!!!!
        // IF THIS ISN'T THE PROBLEM THEN THE PROBLEM IS IN THE SCENE UI COMPONENT FUNCTION
        //since all of the rocks have their own buttons and they are all named Button1 Button2 and Button3 the audio trakc will be chosen based off of what rock you are at
        if (currentRock >= 0)
        {
            if (Input.GetButtonDown("Button1"))   //if you press 1st button on a rock, you get the first audio choice
                audioChoice = 1;
            if (Input.GetButtonDown("Button2"))   //if you press 2nd button on a rock, you get the second audio choice
                audioChoice = 2;
            if (Input.GetButtonDown("Button3"))   // if you press 3rd button on a rock, you get the third audio choice
                audioChoice = 3;

            if (audioChoice != 0)
            {
                if (!drums.GetPlaying() && audioChoice <= 4)    //If this is the beginning of the choices, start all of the tracks
                {
                    drums.StartAll();
                    leads.StartAll();
                    rhythm.StartAll();

                    drums.UnMute(audioChoice - 1);  //This plays the audio
                }

                else if (drums.GetPlaying())
                {
                    currentGroup.UnMute(audioChoice - 1);
                }
            }
        }
    }

    public void SetRock(int rock)
    {
        currentRock = rock;
        if (rock == -1)
            currentGroup = null;
        if (rock == 0)
            currentGroup = drums;
        if (rock == 1)
            currentGroup = rhythm;
        if (rock == 2)
            currentGroup = leads;
    }



    public void ButtonClick()
    {
        print("Pressed the no parameter button");
    }

    public void ButtonClick2(int index)
    {
        print("Pressed rock " + index);
    }
}
