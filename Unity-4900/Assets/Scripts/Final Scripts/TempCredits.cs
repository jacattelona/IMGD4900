using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCredits : MonoBehaviour
{
    [SerializeField]
    GameObject can;

    // Start is called before the first frame update
    void Start()
    {
        can.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            can.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            can.SetActive(false);
        }
    }
}
