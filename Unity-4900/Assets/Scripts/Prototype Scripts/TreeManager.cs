using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    int beatCount = 0;
    int count = 0;

    string[] rocks = { "Rock1", "Rock2", "Rock3", "Rock4", "Rock5" };
    string[] settings = { "CorrectDrum", "CorrectMelody", "CorrectEQ", "CorrectReverb", "CorrectChorus" };

    public enum TreeState
    {
        Nothing,
        Waiting,
        Dancing
    }

    Animator anim;
    TreeState state;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        state = TreeState.Nothing;
    }

    public void StartDancing()
    {
        //anim.SetBool("CorrectLoop", true);
        state = TreeState.Waiting;
    }

    public void StopDancing()
    {
        state = TreeState.Nothing;
        anim.SetBool("CorrectDrum", false);
    }

    public void ChooseRock(int index)
    {
        anim.SetBool(rocks[index], true);
        if (anim.GetBool(settings[index]))
        {
            anim.SetBool(settings[index], false);
            state = TreeState.Waiting;
        }
    }

    public void NoRock()
    {
        foreach(string rock in rocks)
        {
            anim.SetBool(rock, false);
            state = TreeState.Nothing;
        }
    }

    void FixedUpdate()
    {
        count++;

        if (beatCount == 2)
        {
            if (count == 34)
            {
                count = 0;
                beatCount = 0;
            }
        }

        else
        {
            if (count == 33)
            {
                count = 0;
                beatCount++;
            }
        }

        if (state == TreeState.Waiting && count == 0)
        {
            anim.SetBool("CorrectDrum", true);
            state = TreeState.Dancing;
        }

    }

    

}
