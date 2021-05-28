using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class completionchecker : MonoBehaviour
{
    public bool levelfinished;
    public bool playerdied;
    public GameObject player;
    public IEnumerator checker(List<GameObject> enemies){
        // check if level finished
        levelfinished = true;      
        foreach(GameObject enemy in enemies){
            mobeffects effects = enemy.GetComponent<mobeffects>();
            if(effects.isactive){
                levelfinished = false;      
            }
        }
        if(levelfinished && player.GetComponent<mobeffects>().isactive){
            finished();
        }
        else if(!player.GetComponent<mobeffects>().isactive){
            failed();
        }
        yield return null;
    }


    // Update is called once per frame
    void failed(){
        return;
    }
    void finished()
    {
        return;
    }
}
