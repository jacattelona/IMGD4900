using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tablet : MonoBehaviour
{
    public int TabletNumber = -1;
    public TreeManager tree;
    public SoundManager sound;

    bool inRock = false;

    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController f;
    public Vector3 camLoc;
    public float camX;
    public float camY;
    
    // Start is called before the first frame update

    void OnTriggerEnter(Collider other)
    {
        //print(TabletNumber + "Enter");
        //tree.ChooseRock(TabletNumber);
        //sound.SetRock(TabletNumber);
        inRock = true;
    }

    void OnTriggerExit(Collider other)
    {
        //print(TabletNumber + "Exit");
        //tree.NoRock();
        //sound.SetRock(-1);
        inRock = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && inRock)
        {
            print("Clicked in rock " + TabletNumber);
            f.LockCam(camLoc, camY, camX);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetMouseButtonDown(1) && inRock)
        {
            print("Right Clicked in rock " + TabletNumber);
            f.UnlockCam();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
