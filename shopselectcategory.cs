using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopselectcategory : MonoBehaviour
{   
    Animator animator;
    GameObject musicplayer;
    public GameObject screenloader;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        musicplayer = GameObject.Find("MusicPlayer");    
        animator.SetInteger("mode", 1);
    }

    // Update is called once per frame
    public void enter(){
        animator.SetBool("selected", true);
    }
    public void exit(){
        animator.SetBool("selected", false);
    }
    public void click(){
        animator.SetBool("pressed", true);
        Invoke("stopclick", animator.GetCurrentAnimatorStateInfo(0).speed * animator.GetCurrentAnimatorStateInfo(0).length);
    }
    void stopclick(){
        animator.SetBool ("pressed", false);
        animator.SetBool ("selected", false);
        StartCoroutine(screenloader.GetComponent<loadscene>().Loadingscreen("guns"));
    }
    public void PlaySound(AudioClip clip){
        GameObject.Find("MusicPlayer").GetComponent<MusicPlayer>().PlayMusic(clip);
    }
}
