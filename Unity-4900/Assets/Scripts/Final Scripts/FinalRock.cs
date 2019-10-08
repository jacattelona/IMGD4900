using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalRock : Tablet
{
    enum RockState
    {
        UnderGround,
        Emerging,
        AboveGround
    }

    private RockState rockState = RockState.UnderGround;

    //public Vector3 underGround;
    public Vector3 aboveGround;
    public GameObject dirt;
    public AudioClip emerge;

    private Vector3 velocity = Vector3.zero;

    private int numCorrect = 0;

    protected override void Awake()
    {
        tabletActivate = new TabletEvent();
        aud = GetComponent<AudioSource>();
    }

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

        if (rockState == RockState.Emerging)
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, aboveGround, ref velocity, 3f);

            if (Vector3.Distance(transform.localPosition, aboveGround) <= .05f)
            {
                dirt.SetActive(false);
                rockState = RockState.AboveGround;
            }
        }
    }

    public void Activate()
    {
        if (rockState == RockState.UnderGround)
        {
            rockState = RockState.Emerging;
            dirt.SetActive(true);
            //aud.PlayOneShot(emerge);
            aud.PlayOneShot(emerge, .5f);
        }
    }

    public void UpdateCorrect(int val)
    {
        numCorrect += val;
        print(numCorrect);
        if (numCorrect >= 3 && rockState == RockState.UnderGround)
            Activate();
    }
}
