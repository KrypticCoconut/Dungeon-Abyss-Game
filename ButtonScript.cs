 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using UnityEngine.Events;
 using TMPro;
 using UnityEngine.SceneManagement;
 using UnityEngine.UI;
 using System.Linq;
 using UnityEngine.EventSystems;
 public class ButtonScript : MonoBehaviour {
    public GameObject cam;
    public Animator animator;
    public Subdungeon dungeon;
    public bool clone = false;
    public GameObject innerframe;
    public TextMeshProUGUI title;
    public TextMeshProUGUI Warning;
    public TextMeshProUGUI difficulty;
    public bool accessable = false;
    public Image panel;
    public GameObject Board;
    public GameObject Note;

    public void Initiate() {
        if(!clone){
            Destroy(this.gameObject);
        }
        else{

            if(dungeon.connections != null ){
                if(dungeon.connections != new List<string> {}){
                    foreach(string connection in dungeon.connections){
                        if(Subdungeon.GetConn(connection).completed){
                            accessable = true;
                        }
                    }
                }
            }
 
            if(dungeon.startroom){
                accessable = true;
            }
            if(!accessable){ 
                for (int childno = 1; childno <= dungeon.roomobject.transform.childCount; childno++){
                    SpriteRenderer renderer = dungeon.roomobject.transform.GetChild(childno-1).GetComponent<SpriteRenderer>();
                    renderer.color = new Color(200f,200f,200f,.1f);
                }
                foreach(GameObject corridor in dungeon.corridorobjects){
                    for (int childno = 1; childno <= corridor.transform.childCount; childno++){
                        SpriteRenderer renderer = corridor.transform.GetChild(childno-1).GetComponent<SpriteRenderer>();
                        renderer.color = new Color(200f,200f,200f,.1f);
                    }
                }
                Destroy(this.gameObject);
            }
        }
    }
    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if(animator.GetBool("selected") == true){
            animator.SetBool("selected", false);
            Invoke("SwitchToTrue", animator.GetCurrentAnimatorStateInfo(0).speed * animator.GetCurrentAnimatorStateInfo(0).length);
        }
        else{
            SwitchToTrue();
        } 
        cam.GetComponent<butparent>().UpdateTarget(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, cam.transform.position.z));

        

    }
    void SwitchToTrue(){
        animator.SetBool("selected", true);
        title.text =  "Room " + dungeon.DebugID;
        if(dungeon.Bossroom){
            Warning.text = "Warning: BossRoom";
            Warning.color = Color.red;
        }
        else{
            Warning.text = "Warning: None";
            Warning.color = Color.gray;
        }
        if(dungeon.completed){
            Note.SetActive(true);
            Color color =  innerframe.GetComponent<Image>().color;
            color.a = .5f;
            innerframe.GetComponent<Image>().color = color;

            color =  innerframe.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color;
            color.a = .5f;
            innerframe.GetComponent<Image>().color = color;
            innerframe.GetComponent<Button>().enabled = false;
        }
        else{
            Note.SetActive(false);
            innerframe.GetComponent<Button>().enabled = true;

            Color color =  innerframe.GetComponent<Image>().color;
            color.a = 1f;
            innerframe.GetComponent<Image>().color = color;

            color =  innerframe.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color;
            color.a = 1f;
            innerframe.GetComponent<Image>().color = color;

            innerframe.GetComponent<levelselectscript>().function = MainFunction;
            innerframe.GetComponent<Button>().enabled = true;
        }
    }
    public void MainFunction(){
        animator.SetBool("selected", false);
        dungeon.completed = true;
        Board.GetComponent<BoardStarter>().LoadLevel(dungeon);
    }

 }