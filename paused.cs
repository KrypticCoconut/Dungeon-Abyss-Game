using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class paused : MonoBehaviour
{
    public Slider sound;
    GameObject MusicPlayer;
    public GameObject panel;
    public GameObject loadscene;
    public GameObject[] UIObjects; 
    // Start is called before the first frame update
    public void OnValueChanged()
    {
        if(livegamedata.paused){
            MusicPlayer.GetComponent<AudioSource>().volume = sound.value;
            livegamedata.currentdata.volume = sound.value;
        }
    }

    public void savebutton(){
        loadscene.GetComponent<loadscene>().startanim();
        SaveSystem.SaveAll(livegamedata.currentdungeon, livegamedata.currentdata);
        loadscene.GetComponent<loadscene>().stopanim();
        StartCoroutine(MusicPlayer.GetComponent<notificationhandler>().notificationobjcopy.GetComponent<notification>().NotifyHandler("Game saved!"));
    }
    public void closebutton(){
        loadscene.GetComponent<loadscene>().startanim();
        SaveSystem.SaveAll(livegamedata.currentdungeon, livegamedata.currentdata);
        loadscene.GetComponent<loadscene>().startanim();
        if(SceneManager.GetActiveScene().name == "LevelScene"){
            loadscene.GetComponent<loadscene>().Loadingscreen("GameScene");
        }
        else{
            Application.Quit();
        }

        
    }
    // Update is called once per frame
    void Start() {
        StartCoroutine("oneframeafterstart");
    }
    IEnumerator oneframeafterstart(){
        yield return new WaitForEndOfFrame();
        MusicPlayer = GameObject.Find("MusicPlayer");
        MusicPlayer.GetComponent<AudioSource>().volume = livegamedata.currentdata.volume;
        sound.value = livegamedata.currentdata.volume;
        livegamedata.paused = false;
        panel.SetActive(false);
        foreach(GameObject UIobject in UIObjects){
            UIobject.SetActive(false);
        }
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !livegamedata.paused){
            StartCoroutine("startpause");
        }
    }
    IEnumerator startpause(){
        yield return null;
        livegamedata.paused = true;
        panel.SetActive(true);
        foreach(GameObject UIobject in UIObjects){
            UIobject.SetActive(true);
        }
        while(livegamedata.paused){
            if(Input.GetKeyDown(KeyCode.Escape)){
                livegamedata.paused = false;
                foreach(GameObject UIobject in UIObjects){
                    UIobject.SetActive(false);
                }
                panel.SetActive(false);
            }
            yield return null;
        }
    }

}
