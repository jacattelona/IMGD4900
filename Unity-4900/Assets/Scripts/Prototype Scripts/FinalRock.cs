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

    //public Vector3 underGround;
    public Vector3 aboveGround;
    public GameObject dirt;
    private Vector3 velocity = Vector3.zero;

    private int numCorrect = 0;

    void Start()
    {
        dirt.SetActive(false);
    }
    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.P))
            Activate();

        if (state == State.Emerging)
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, aboveGround, ref velocity, 2f);

            if (Vector3.Distance(transform.localPosition, aboveGround) <= .05f)
            {
                dirt.SetActive(false);
                state = State.AboveGround;
            }
        }
    }

    void Activate()
    {
        if (state == State.UnderGround)
        {
            state = State.Emerging;
            dirt.SetActive(true);
        }
    }

    public void UpdateCorrect(int val)
    {
        numCorrect += val;
        print(numCorrect);
        if (numCorrect >= 3 && state == State.UnderGround)
            Activate();
    }
}
