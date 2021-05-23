using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class playerstats : MonoBehaviour
{
    public GameObject[] gunobjs;

    public GameObject info;
    public GameObject title;
    public Slider xpbar;
    public GameObject xptext;
    Animator animator;
    
    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();    
        animator.SetInteger("mode", 2);
    }
    public void enter()
    {
        if(livegamedata.paused){
            return;
        }
        animator.SetBool("selected", true);
    }
    public void exit(){
        if(livegamedata.paused){
            return;
        }
        animator.SetBool("selected", false);
    }

    public void click(){
        if(livegamedata.paused){
            return;
        }

        animator.SetBool("pressed", true);
        Invoke("stopclick", animator.GetCurrentAnimatorStateInfo(0).speed * animator.GetCurrentAnimatorStateInfo(0).length);
    }
    public void stopclick(){
        animator.SetBool("pressed", false);
        switchtoplayer();
        title.GetComponent<TextMeshProUGUI>().text = "player";
        string text = "";
        text += "Health: 300 <color=green>+" + ((livegamedata.currentdata.level/1000) * 15) + "</color> \n";
        text += "DamageModifier: 1x <color=green>+" + ((livegamedata.currentdata.level/1000) * .1f)+ "x</color> \n" ;
        text += "HealthRegen: 30/s <color=green>+" + ((livegamedata.currentdata.level/1000) * 5f) + "</color> \n\n\n";
        text += "Description: You gain +15 health, +0.1x damage, +5hp/s regen rate per level";
        info.GetComponent<TextMeshProUGUI>().text =text;
        xptext.GetComponent<TextMeshProUGUI>().text = "Level " + (livegamedata.currentdata.level +1) + " in " + (livegamedata.currentdata.xp - livegamedata.currentdata.level*1000) + "/1000";
        xpbar.value = livegamedata.currentdata.xp - livegamedata.currentdata.level*1000;
    }
    // Update is called once per frame
    public void switchtoplayer(){
        foreach(GameObject obj in gunobjs){
            obj.SetActive(false);
        }
        xptext.SetActive(true);
        xpbar.gameObject.SetActive(true);
    }
    public void switchtoguns(){
        xptext.SetActive(false);
        xpbar.gameObject.SetActive(false);
        foreach(GameObject obj in gunobjs){
            obj.SetActive(true);
        }
    }
    public void PlaySound(AudioClip clip){
        if(livegamedata.paused){
            return;
        }
        GameObject.Find("MusicPlayer").GetComponent<MusicPlayer>().PlayMusic(clip);
    }
}
