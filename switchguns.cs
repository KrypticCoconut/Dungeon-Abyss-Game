using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchguns : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject currentgun;
    public GameObject gun1;
    public GameObject gun2;
    public GameObject changewithGun1;
    public GameObject changewithGun2;
    public AudioClip click;
    GameObject Musicplayer;
    void Start()
    {
        Musicplayer = GameObject.Find("MusicPlayer");
    }

    // Update is called once per frame

    public void switchwithgun1(){
        if(livegamedata.paused){
            return;
        }
        Musicplayer.GetComponent<MusicPlayer>().PlayMusic(click);
        if(currentgun == gun2){
            livegamedata.currentdata.equipped[1] = livegamedata.currentdata.equipped[0];
            gun2 = gun1;
            gun2.GetComponent<gunselect>().badge1.SetActive(false);
            gun2.GetComponent<gunselect>().badge2.SetActive(true);
            currentgun.GetComponent<gunselect>().badge2.SetActive(false);
        }
        livegamedata.currentdata.equipped[0] = currentgun.GetComponent<gunselect>().gun;
        gun1.GetComponent<gunselect>().badge1.SetActive(false);
        currentgun.GetComponent<gunselect>().badge1.SetActive(true);
        gun1 = currentgun;
        currentgun.GetComponent<gunselect>().stopclick();
        SaveSystem.SaveAll(livegamedata.currentdungeon, livegamedata.currentdata);
    }
    public void switchwithgun2(){
        if(livegamedata.paused){
            return;
        }
        Musicplayer.GetComponent<MusicPlayer>().PlayMusic(click);
        if(currentgun == gun1){
            livegamedata.currentdata.equipped[0] = livegamedata.currentdata.equipped[1];
            gun1 = gun2;
            gun1.GetComponent<gunselect>().badge2.SetActive(false);
            gun1.GetComponent<gunselect>().badge1.SetActive(true);
            currentgun.GetComponent<gunselect>().badge1.SetActive(false);
        }
        livegamedata.currentdata.equipped[1] = currentgun.GetComponent<gunselect>().gun;
        gun2.GetComponent<gunselect>().badge2.SetActive(false);
        currentgun.GetComponent<gunselect>().badge2.SetActive(true);
        gun2 = currentgun;
        currentgun.GetComponent<gunselect>().stopclick();
        SaveSystem.SaveAll(livegamedata.currentdungeon, livegamedata.currentdata);
    }
}
                                