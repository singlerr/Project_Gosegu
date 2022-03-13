using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This file is to be deprecated.
[Serializable]
public class AnimationHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public Image animation_slide;
    public List<Sprite> slide_set;
    public int index = 1;

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        animation_slide = GameObject.Find("Canvas").GetComponent<Image>();
        if (Input.GetKeyDown("space"))
            try
            {
                Debug.Log(slide_set[index]);
                Debug.Log(animation_slide);
                animation_slide.sprite = slide_set[index];
                index++;
            }
            catch (Exception e)
            {
                // end of dialogue 
            }
    }
}