using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class StartScreen : MonoBehaviour
{
    CanvasGroup cg;
    bool fading = false;
    // Start is called before the first frame update

    private Animator theAnimation;
    private GameObject FPSParent;

    void Start()
    {
        cg = this.GetComponent<CanvasGroup>();
        FPSParent = GameObject.Find("FPSController");
        FPSParent.GetComponent<FirstPersonController>().enabled = false;
        theAnimation = GameObject.Find("FirstPersonCharacter").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("w") || Input.GetKeyDown("a")
            || Input.GetKeyDown("s") || Input.GetKeyDown("d") && !fading)
        {
            theAnimation.SetBool("weDroppin", true);
            fading = true;
        }

        if (fading)
        {
            cg.alpha -= Time.deltaTime * .5f;
            if (cg.alpha <= 0)
            {
                Destroy(this.gameObject);
                theAnimation.enabled = false;
                FPSParent.GetComponent<FirstPersonController>().enabled = true;
            }
        }
    }
}
