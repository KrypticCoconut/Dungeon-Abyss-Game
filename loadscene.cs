using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class loadscene : MonoBehaviour
{
    public Animator animator;
    public IEnumerator Loadingscreen(string scenename){
        livegamedata.paused = true;
        AsyncOperation loadoperation =  SceneManager.LoadSceneAsync(scenename);
        animator.SetBool("loading", true);
        while (!loadoperation.isDone){
            float progress = Mathf.Clamp01(loadoperation.progress/0.9f);
            if (loadoperation.progress >= 0.9f){
                livegamedata.paused = false;
            }

            yield return null;
        }
    }
    public void startanim(){
        livegamedata.paused = true;
        animator.SetBool("loading", true);
    }

    public void stopanim(){
        livegamedata.paused = false;
        animator.SetBool("loading", false);
    }
    
}
