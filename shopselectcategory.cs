using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopselectcategory : MonoBehaviour
{   
    Animator animator;
    GameObject musicplayer;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        musicplayer = GameObject.Find("MusicPlayer");    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void PlaySound(AudioClip sound){
        
    }
}
