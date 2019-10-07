using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class MenuPan : MonoBehaviour
{
    private Animator theAnimation;
    private GameObject FPSParent;

    // Start is called before the first frame update
    void Start()
    {
        FPSParent = GameObject.Find("FPSController");
        theAnimation = gameObject.GetComponent<Animator>();
        FPSParent.GetComponent<FirstPersonController>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
