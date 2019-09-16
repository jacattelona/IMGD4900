using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    int beat = 33;
    int beatCount = 0;
    int count = 0;
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
        anim.SetBool("CorrectLoop", false);
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

        if (count == 0)
            print("Beat");
        if (state == TreeState.Waiting && count == 0)
        {
            //print("GO");
            anim.SetBool("CorrectLoop", true);
            state = TreeState.Dancing;
        }

    }

    

}
