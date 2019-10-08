using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    Animator anim;
    float timeMax = 0;
    float timer = 0;
    Tree tree;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        tree = GameObject.Find("tree_animated_01").GetComponent<Tree>();
        tree.victoryEvent.AddListener(Unfold);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeMax > 0)
        {
            timer += Time.deltaTime;
            if (timer > timeMax)
            {
                timeMax = 0;
                anim.SetBool("Activated", true);
            }
        } 
    }

    void Unfold()
    {
        timeMax = Random.Range(0.1f, 4f);
    }
}
