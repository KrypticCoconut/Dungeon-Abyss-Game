using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BackButton : MonoBehaviour
{
    public void LoadSceneMain(){
        if(livegamedata.paused){
            return;
        }
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
