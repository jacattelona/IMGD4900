﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var fwd = Camera.main.transform.forward;
        fwd.y = -270;
        transform.rotation = Quaternion.LookRotation(fwd);
    }
}
