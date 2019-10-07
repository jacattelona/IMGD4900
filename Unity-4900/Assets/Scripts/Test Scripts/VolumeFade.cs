using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeFade : MonoBehaviour
{
    public AudioMixer mixer;

    float volume = 0;
    bool focus = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //volume++;
            //mixer.SetFloat("TestVol", volume);
            focus = true;
        }

        if (focus)
        {
            if (volume < 5.0f)
            {
                volume += Time.deltaTime;
                mixer.SetFloat("TestVol", volume);
            }
        }
    }
}
