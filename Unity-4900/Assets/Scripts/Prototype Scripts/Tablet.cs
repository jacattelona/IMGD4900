using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tablet : MonoBehaviour
{
    bool inRock = false;

    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController character;
    public Vector3 camLocation;
    public float angleX;
    public float angleY;

    public Material[] mats1;
    public Material[] mats2;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ParticleSystem>().Stop();
        transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        GetComponent<Renderer>().materials = mats1;
    }


    void OnTriggerEnter(Collider other)
    {
        inRock = true;
    }

    void OnTriggerExit(Collider other)
    {
        inRock = false;
    }

    protected void Update()
    {
        if (Input.GetMouseButtonDown(0) && inRock)
        {
            //print("Clicked in rock " + TabletNumber);
            character.LockCam(camLocation, angleY, angleX);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetMouseButtonDown(1) && inRock)
        {
            //print("Right Clicked in rock " + TabletNumber);
            character.UnlockCam();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void ActivateSlider()
    {
        transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        GetComponent<Renderer>().materials = mats2;
    }

    public void DeactivateSlider()
    {
        transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        GetComponent<Renderer>().materials = mats1;
    }
}
