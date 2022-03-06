using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class AnimationHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public Image animation_slide;
    public List<Sprite> slide_set;
    public int index = 1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animation_slide = GameObject.Find("Canvas").GetComponent<Image>();
        if(Input.GetKeyDown("space")){
                try{
                    Debug.Log(slide_set[index]);
                    Debug.Log(animation_slide);
                    animation_slide.sprite = slide_set[index];
                    index++;
                } catch (Exception e){
                    // end of dialogue 
                }
            }
    }
}
