using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    int beatCount = 0;
    int count = 0;

    int dance = -1;

    string[] rocks = { "Rock1", "Rock2", "Rock3", "Rock4", "Rock5" };
    string[] settings = { "CorrectDrum", "CorrectMelody", "CorrectChorus", "CorrectReverb", "CorrectEQ" };

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
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void StartDancing(int index)
    {
        //anim.SetBool("CorrectLoop", true);
        dance = index;
        state = TreeState.Waiting;
    }

    public void StopDancing(int index)
    {
        state = TreeState.Nothing;
        anim.SetBool(settings[index], false);
    }

    public void ChooseRock(int index)
    {
        anim.SetBool(rocks[index], true);
        dance = index;
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

    public bool IsCorrect(int index)
    {
        return anim.GetBool(settings[index]);
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
            //if (dance == 0)
            //    anim.SetBool("CorrectDrum", true);
            //if (dance == 1)
            //    anim.SetBool("CorrectMelody", true);
            //if (dance == 2)
            //    anim.SetBool("CorrectChorus", true);
            anim.SetBool(settings[dance], true);
            state = TreeState.Dancing;
        }

    }

    

}
