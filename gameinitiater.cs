using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameinitiater : MonoBehaviour
{
    // Start is called before the first frame update
    public gameinitiater oneinstance;
    public delegate void initfunc();
    public List<initfunc> funcs = new List<initfunc>();
    void Start(){
        if(oneinstance == null){

            foreach(initfunc func in funcs){
                func();
            }
            GameData currentsave = SaveSystem.LoadSave();
            livegamedata.currentdungeon = currentsave.dungeon;
            livegamedata.currentdata = currentsave.data;
            oneinstance = this;
        }
        else{
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
