using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class playereffects : MonoBehaviour
{
    public float hp = 300;
    public float maxhp = 300;
    public GameObject healthbar;

    public void init(float maxhp){
        this.maxhp = maxhp;
        hp = maxhp;
        if(healthbar != null){
            healthbar.GetComponent<HealthBar>().SetMaxHealth(hp);
        }
    }
    public void sethp(){
        if(healthbar){
            healthbar.GetComponent<HealthBar>().SetHealth(hp);
        }
    }
}
