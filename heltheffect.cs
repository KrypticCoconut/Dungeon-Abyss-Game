using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heltheffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        effect health = new effect("health", "800g", 5, healtheffect, "uprade");
    }

    // Update is called once per frame
    public void healtheffect(int tier, GameObject theplayer){
        livegamedata.currentdata.health += 50;
        SaveSystem.SaveAll(livegamedata.currentdungeon, livegamedata.currentdata);
    }
}
                                                                                                    