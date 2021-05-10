using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class buygun : MonoBehaviour
{
    public string name;
    public GameObject gunobj;
    GameObject Musicplayer;
    public AudioClip click;
    // Start is called before the first frame update
    void Start()
    {
        Musicplayer = GameObject.Find("MusicPlayer");
    }
    void buycurrentgun(){
        Musicplayer.GetComponent<MusicPlayer>().PlayMusic(click);
        GunInfo gun; 
        GunInfo.Guns.TryGetValue(name, out gun);
        livegamedata.currentdata.owned.Add(gun);
        livegamedata.currentdata.coins -= gun.money;
        
        gunobj.GetComponent<gunselect>().stopclick();
        SaveSystem.SaveAll(livegamedata.currentdungeon, livegamedata.currentdata);  
    }
}
