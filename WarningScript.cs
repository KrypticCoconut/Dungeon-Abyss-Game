    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WarningScript : MonoBehaviour
{
    public int? index = null;
    public int? previndex = null;
    public int maxIndex = 1;
    public GameObject MusicPlayer;
    public bool reservesoundtime = false;

    public TextMeshProUGUI TextBox;
    // Start is called before the first frame update
    // Update is called once per frame
    
    public bool? Warning(string text)
    {
        previndex = index;
        gameObject.SetActive(true);
        TextBox.text = text;
        previndex = index;
        if(Input.GetKeyDown("a") || Input.GetKeyDown(KeyCode.LeftArrow)){
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

        if(Input.GetKeyDown("d") || Input.GetKeyDown(KeyCode.RightArrow)){
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
        if(index != null){
            int result = AnimationController();
            if(result != -1){ 
                gameObject.SetActive(false);
                return 1==result;

            }
            else{
                return null;
            }
        }
        else{
            return null;
        }
    }

    public void Sound(AudioClip soundclip){
        if(!reservesoundtime){
            MusicPlayer.GetComponent<MusicPlayer>().PlayMusic(soundclip);
        }
        else{
            reservesoundtime = false;
        }
    }
    int AnimationController(){
        if(previndex != index && previndex != null && index != null){
            gameObject.transform.GetChild((int)previndex).GetComponent<Animator>().SetBool("selected",false);
        }
        Animator indexAnim = gameObject.transform.GetChild((int)index).GetComponent<Animator>();
        indexAnim.SetBool ("selected", true);
        if(Input.GetKey(KeyCode.Space)){
            reservesoundtime = false;
            indexAnim.SetBool ("pressed", true);
        }

        else if(indexAnim.GetBool("pressed")){
            indexAnim.SetBool ("pressed", false);
            reservesoundtime = true;
            return (int)index;
        }
        return -1;
    }

}
