using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BackButton : MonoBehaviour
{
    public string scene;
    public void LoadSceneMain(){
        if(livegamedata.paused){
            return;
        }
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}
