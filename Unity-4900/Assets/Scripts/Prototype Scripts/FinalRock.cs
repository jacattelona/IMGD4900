using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalRock : Tablet
{
    enum State
    {
        UnderGround,
        Emerging,
        AboveGround
    }

    private State state = State.UnderGround;

    private Vector3 underGround;
    private Vector3 aboveGround;

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (state == State.Emerging)
        {

        }
    }
}
