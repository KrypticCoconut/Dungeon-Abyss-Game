using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MediumD : MonoBehaviour
{
    public MenuButtonController controller;
    public GameObject loadobj;
    public Animator animator;
    public TextMeshProUGUI TextBox;
    [SerializeField] int thisIndex;
    bool runfunc;

    public bool invoked;
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
            TextBox.text = "This dungeon type is fairly large and will need some advanced weaponry to finish, this includes one bossfight";
            animator.SetBool ("selected", true);
            if(Input.GetKey(KeyCode.Space) && runfunc != true){
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
            loadobj.GetComponent<loadscene>().startanim();
            gameObject.transform.parent.GetComponentInParent<MyOwnBoard>().MakeDungeon(0,500,500,16,16,2);
            loadobj.GetComponent<loadscene>().stopanim();
            runfunc = false;
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
