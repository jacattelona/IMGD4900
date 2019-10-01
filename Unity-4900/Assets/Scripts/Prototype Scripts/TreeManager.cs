using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    public GameObject tree;
    public GameObject altar;
    public GameObject victoryParticles;

    int beatCount = 0;
    int count = 0;

    public enum TreeState
    {
        Nothing,
        Increasing,
        Decreasing,
        Dancing,
    }

    bool glowing = false;
    float glowVal = 0f;

    Animator anim;
    TreeState state;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        state = TreeState.Nothing;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        victoryParticles.SetActive(false);

        //tree.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        tree.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(1.0f, 1.0f, 1.0f, 1.0f) * 0.0f);
        altar.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(1.0f, 1.0f, 1.0f, 1.0f) * 0.0f);
        //tree.GetComponent<Renderer>().material.SetColor("_EMISSION", new Color(0.0f, 0.0f, 0.0f, 1.0f));
        //altar.GetComponent<Renderer>().material.SetColor("_EMISSION", new Color(0.0f, 0.0f, 0.0f, 1.0f));

    }
    

    public void StartDancing()
    {
        if (state == TreeState.Decreasing)
        {
            state = TreeState.Nothing;
        }
        else
        {
            state = TreeState.Increasing;
        }

    }

    public void StopDancing()
    {
        if (state == TreeState.Increasing)
        {
            state = TreeState.Nothing;
        }
        else
        {
            state = TreeState.Decreasing;
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

        if (state == TreeState.Increasing && count == 0)
        {
            int numCorrect = anim.GetInteger("NumCorrect");
            anim.SetInteger("NumCorrect", numCorrect + 1);
            state = TreeState.Dancing;

            if (numCorrect+1 == 3)
            {
                //GameObject rock = GameObject.Find("Final_Rock");
                //rock.GetComponent<FinalRock>().Activate();
                GameObject.Find("piano_rock_01").GetComponent<Tablet>().ActivateSlider();
                GameObject.Find("guitar_rock_01").GetComponent<Tablet>().ActivateSlider();
                GameObject.Find("drum_rock_01").GetComponent<Tablet>().ActivateSlider();
            }
        }

        if (state == TreeState.Decreasing && count == 0)
        {
            int numCorrect = anim.GetInteger("NumCorrect");
            if (numCorrect > 0)
                anim.SetInteger("NumCorrect", numCorrect - 1);
            state = TreeState.Dancing;

            if (numCorrect - 1 != 3)
            {
                //GameObject rock = GameObject.Find("Final_Rock");
                //rock.GetComponent<FinalRock>().Activate();
                GameObject.Find("piano_rock_01").GetComponent<Tablet>().DeactivateSlider();
                GameObject.Find("guitar_rock_01").GetComponent<Tablet>().DeactivateSlider();
                GameObject.Find("drum_rock_01").GetComponent<Tablet>().DeactivateSlider();
            }
        }

        if (glowing)
        {
            if (glowVal >= 1.5f)
            { 
                glowing = false;
                return;
            }
            glowVal += .01f;
            tree.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(1.0f, 1.0f, 1.0f, 1.0f) * glowVal);
            altar.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(1.0f, 1.0f, 1.0f, 1.0f) * glowVal);


        }

    }


    public void Victory()
    {
        victoryParticles.SetActive(true);
        glowing = true;
    }



}
