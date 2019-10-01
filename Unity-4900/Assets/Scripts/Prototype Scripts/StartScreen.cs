using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    CanvasGroup cg;
    bool fading = false;
    // Start is called before the first frame update
    void Start()
    {
        cg = this.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("w") || Input.GetKeyDown("a")
            || Input.GetKeyDown("s") || Input.GetKeyDown("d") && !fading)
        {
            fading = true;
        }

        if (fading)
        {
            cg.alpha -= Time.deltaTime * .5f;
            if (cg.alpha <= 0)
                Destroy(this.gameObject);
        }
    }
}
