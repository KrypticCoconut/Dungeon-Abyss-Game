using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonClick : MonoBehaviour
{
    public GameObject loader;
    public MenuButtonController controller;
    public Animator animator;
    public GameObject WarningWindow;
    public GameObject panel;

    public GameObject canvas;
    bool runfunc;

    public bool invoked;
    [SerializeField] int thisIndex;
    // Start is called before the first frame update
    void Start()
    {
        for(int childno = 1; childno <= gameObject.transform.parent.childCount; childno++){
            if(gameObject.transform.parent.GetChild(childno-1).transform == gameObject.transform){
                thisIndex = childno-1;
                break;
            }
        }

    }

    void Update()
    {
        if(livegamedata.paused){
            return;
        }
		if(controller.index == thisIndex)
		{   
            animator.SetBool ("selected", true);
            if(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return) && runfunc != true){
                animator.SetBool ("pressed", true);
                controller.reservesoundtime = false;

                if(!invoked){
                    invoked = true;
                    Invoke("FuncAfterAnim", animator.GetCurrentAnimatorStateInfo(0).speed * animator.GetCurrentAnimatorStateInfo(0).length);
                }
            }
        }else{
    
            animator.SetBool ("selected", false);
        }
        if(runfunc){
            StartCoroutine(loader.GetComponent<loadscene>().Loadingscreen("CreateNewDungeon"));
        }
    }

    void Sound(AudioClip sound){
        controller.PlaySound(sound);
    }

    void FuncAfterAnim(){
        controller.reservesoundtime = true;
        animator.SetBool ("pressed", false);
        runfunc = true;
        invoked = false;
    }

}
