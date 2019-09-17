using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tablet : MonoBehaviour
{
    public int TabletNumber = -1;
    public TreeManager tree;
    public SoundManager sound;
    // Start is called before the first frame update

    void OnTriggerEnter(Collider other)
    {
        //print(TabletNumber + "Enter");
        tree.ChooseRock(TabletNumber);
        sound.SetRock(TabletNumber);
    }

    void OnTriggerExit(Collider other)
    {
        //print(TabletNumber + "Exit");
        tree.NoRock();
        sound.SetRock(-1);
    }
}
