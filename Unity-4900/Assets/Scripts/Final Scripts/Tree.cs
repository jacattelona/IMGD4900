using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    const float GLOWMAX = 1.5f;         //constant glow maximum
    const int BEATMAX = 2;              //constant beat maximum
    const int COUNTMAX = 33;            //constant count maximum
    const int MAXCORRECT = 3;           //constant max number of correct tracks

    [SerializeField]
    Renderer tree;                      //Renderer for the tree game object
   
    [SerializeField]
    Renderer altar;                     //Renderer for the altar game object

    [SerializeField]
    GameObject victoryParticles;        //Tree particle effect

    int beatCount = 0;                  //number of beats
    int count = 0;                      //frames counting to each beat

    public enum TreeState               //Enum describing tree's current state
    {
        Nothing,
        Increasing,
        Decreasing
    }

    TreeState state;                    //treestate enum

    bool glowing = false;               //bool indicating tree is lighting up
    float glowVal = 0f;                 //value tree is glowing

    Animator anim;                      //Tree animator component

    
    /// <summary>
    /// Gets components
    /// deactivates particles and sets renderer emissions to 0
    /// </summary>
    void Start()
    {
        //Get animator
        anim = GetComponent<Animator>();

        //Set state to nothing
        state = TreeState.Nothing;

        //Disable victory particles and set emission glow to 0
        victoryParticles.SetActive(false);
        tree.material.SetColor("_EmissionColor", new Color(1.0f, 1.0f, 1.0f, 1.0f) * glowVal);
        altar.material.SetColor("_EmissionColor", new Color(1.0f, 1.0f, 1.0f, 1.0f) * glowVal);
    }


    /// <summary>
    /// Sets current state to increased dance
    /// </summary>
    public void StartDancing()
    {
        //if the tree was about to go to a lower dance, revert back to normal because the correct track was chosen
        if (state == TreeState.Decreasing)
        {
            state = TreeState.Nothing;
        }
        //otherwise, notify that the tree is ready to progress dancing
        else
        {
            state = TreeState.Increasing;
        }

    }

    /// <summary>
    /// sets current state to decreased dance
    /// </summary>
    public void StopDancing()
    {
        //if the tree was about to go to a higher dance, revert back to normal because an incorrect track was chosen
        if (state == TreeState.Increasing)
        {
            state = TreeState.Nothing;
        }
        //otherwise, notify that the tree is ready to regress dancing
        else
        {
            state = TreeState.Decreasing;
        }
    }

    /// <summary>
    /// Called 50 times/second
    /// starts and stops dances on 90 bpm beats
    /// At 50 frames per second, one beat == 33.3333 frames
    /// </summary>
    void FixedUpdate()
    {
        //increment count
        count++;

        //if this is the third beat, reset at count == 34
        if (beatCount == BEATMAX)
        {
            //set count to 0, beatCount to 0
            if (count == COUNTMAX + 1)
            {
                count = 0;
                beatCount = 0;
            }
        }

        //otherwise, reset at count == 33
        else
        {
            //set count to 0, increment beatCount
            if (count == COUNTMAX)
            {
                count = 0;
                beatCount++;
            }
        }

        //if on an exact beat (count == 0)
        if (count == 0)
        {
            //if waiting to progress dancing
            if (state == TreeState.Increasing)
            {
                //get the current dance number from Animator object and increment (as long as numCorrect < 3)
                int numCorrect = anim.GetInteger("NumCorrect");
                if (numCorrect < MAXCORRECT)
                {
                    anim.SetInteger("NumCorrect", numCorrect + 1);
                }

                //Set state to nothing
                state = TreeState.Nothing;
            }

            //if waiting to regress dancing
            if (state == TreeState.Decreasing)
            {
                //get the current dance number from Animator object and decrement (as long as numCorrect > 0)
                int numCorrect = anim.GetInteger("NumCorrect");
                if (numCorrect > 0)
                {
                    anim.SetInteger("NumCorrect", numCorrect - 1);
                }
                //Set state to nothing
                state = TreeState.Nothing;
            }
        }
        
        //If currently increasing the glow of the tree
        if (glowing)
        {
            //stop if glowVal is already at max (1.5f)
            if (glowVal >= GLOWMAX)
            {
                glowing = false;
                return;
            }
            //increase glowVal
            glowVal += .01f;
            tree.material.SetColor("_EmissionColor", new Color(1.0f, 1.0f, 1.0f, 1.0f) * glowVal);
            altar.material.SetColor("_EmissionColor", new Color(1.0f, 1.0f, 1.0f, 1.0f) * glowVal);
        }

    }

    /// <summary>
    /// Activate victory particles and set glowing to true
    /// </summary>
    public void Victory()
    {
        victoryParticles.SetActive(true);
        glowing = true;
    }
}
