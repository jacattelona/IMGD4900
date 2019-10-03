using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TabletEvent : UnityEvent<int> {}

public class Tablet : MonoBehaviour
{
    bool inRock = false;                //Determines whether character is in the rock trigger

    public int tabletNum;               //number of this tablet
    public TabletEvent tabletActivate;  //event fired when interacting with this tablet
                                        //first person character controller
    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController character;
    public Vector3 camLocation;         //World location to move camera to when interacting with this tablet
    public float angleX;                //xAngle to tilt camera when interacting with this tablet
    public float angleY;                //yAngle to tilt camera when interacting with this tablet

    public Material[] mats1;            //materials to use when sliders are not activated
    public Material[] mats2;            //materials to use when sliders are activated
    
    /// <summary>
    /// Called on creation. Creates tabletActivate event
    /// </summary>
    void Awake()
    {
        tabletActivate = new TabletEvent();
    }


    /// <summary>
    /// Sets tablet to default state before the first frame
    /// </summary>
    void Start()
    {
        //Stops particle system
        DeactivateParticles();
        //deactivates slider
        DeactivateSlider();
    }

    /// <summary>
    /// sets inRock to true on entering trigger
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        inRock = true;
    }

    /// <summary>
    /// Sets inRock to false on exiting trigger
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        inRock = false;
    }

    /// <summary>
    /// Called every frame
    /// </summary>
    protected void Update()
    {
        //If player left clicks while in the rock trigger
        if (Input.GetMouseButtonDown(0) && inRock)
        {
            //lock camera to proper location and angle
            character.LockCam(camLocation, angleY, angleX);

            //make cursor visible
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            //fire tabletActivate event
            tabletActivate.Invoke(tabletNum);
        }

        //If player right clicks while in the rock trigger
        else if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown("escape")) && inRock)
        {
            //unlock camera
            character.UnlockCam();

            //make cursor invisible
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //fire tabletActivate event with deactivate case (-1)
            tabletActivate.Invoke(-1);
        }
    }

    /// <summary>
    /// Activates slider object, adds glowing material
    /// </summary>
    public void ActivateSlider()
    {
        transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        GetComponent<Renderer>().materials = mats2;
    }

    /// <summary>
    /// Deactivates slider object, removes glowing material
    /// </summary>
    public void DeactivateSlider()
    {
        transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        GetComponent<Renderer>().materials = mats1;
    }

    /// <summary>
    /// Activates particle effects
    /// </summary>
    public void ActivateParticles()
    {
        GetComponent<ParticleSystem>().Play();
    }

    /// <summary>
    /// Deactivates particle effects
    /// </summary>
    public void DeactivateParticles()
    {
        GetComponent<ParticleSystem>().Stop();
    }
}
