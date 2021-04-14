using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using TMPro;
public class BoardStarter : MonoBehaviour
{
    public GameObject Backbutton;
    public GameObject ButtonObj;
    public GameObject cam;
    public Subdungeon dungeon;
    public GameObject temp;

    public GameObject loader;

    public List<Subdungeon> leafs;
    // Start is called before the first frame update
    public void MakeButtons(List<Subdungeon> leafs, Subdungeon dungeon){
        GameObject button;
        Rect rect;
        foreach(Subdungeon leaf in leafs){
            rect = leaf.room;
            button = Instantiate(ButtonObj, rect.center, Quaternion.identity);
            button.transform.position = new Vector3(button.transform.position.x, button.transform.position.y, -8);            
            button.transform.localScale = new Vector2(rect.width,rect.height);
            button.GetComponent<ButtonScript>().dungeon = leaf;
            button.GetComponent<ButtonScript>().clone = true;
            button.GetComponent<ButtonScript>().Initiate();
            // button.transform.parent = Canvas.transform;
        }

    }
    void Start()
    {

        loader.GetComponent<loadscene>().startanim();
        MyOwnBoard vars = GetComponent<MyOwnBoard>();
        dungeon = SaveSystem.LoadSave().dungeon;
        MyOwnBoard BoardScript = GetComponent<MyOwnBoard>();
        BoardScript.DrawCorridors(dungeon);
        BoardScript.DrawRooms(dungeon);
        leafs = dungeon.GetLeafs();
        MakeButtons(leafs, dungeon);
        GetComponent<scroller>().bounds = new Rect(dungeon.roomspace.x, dungeon.roomspace.y, dungeon.roomspace.width, dungeon.roomspace.height);
        loader.GetComponent<loadscene>().stopanim();

        cam.transform.position = new Vector3(dungeon.roomspace.center.x, dungeon.roomspace.center.y, cam.transform.position.z);
        cam.GetComponent<butparent>().UpdateZoom(dungeon.roomspace.height/2);
    }


    // Update is called once per frame

    public void LoadLevel(Subdungeon level){
        livegamedata.currentlevel = level;
        GetComponent<scroller>().enabled = false;
        Backbutton.SetActive(false);
        StartCoroutine(loader.GetComponent<loadscene>().Loadingscreen("LevelScene"));
    }

}
