using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonController : MonoBehaviour
{

    public int? index = null;
    bool keyDown;
    int maxIndex;
    public bool reservesoundtime;
    public GameObject MusicObject;
    
    // Start is called before the first frame update
    void Start()
    {
        MusicObject = GameObject.Find("MusicPlayer");
        maxIndex = gameObject.transform.GetChild(1).childCount - 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(livegamedata.paused){
            return;
        }
        if(Input.GetKeyDown("w") || Input.GetKeyDown(KeyCode.UpArrow)){
            if(index == null){
                index = maxIndex+1;
            }
            if(index >= 0){
                index--;
                if(index < 0){
                    index = maxIndex;
                }
            }
        }

        if(Input.GetKeyDown("s") || Input.GetKeyDown(KeyCode.DownArrow)){
            if(index == null){
                index = -1;
            }
            if(index <= maxIndex){
                index++; 
                if(index > maxIndex){
                    index = 0;
                }
            }
        }
    }

    public void PlaySound(AudioClip sound){
        if(!reservesoundtime){
            MusicObject.GetComponent<MusicPlayer>().PlayMusic(sound);
        }
        else{
            reservesoundtime = false;
        }
    }
}
