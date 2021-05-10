using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class playereffects : MonoBehaviour
{
    public float damagetakenmultiplier = 1;
    public float damagedonemultiplier = 1;
    public float hp = 300;
    public GameObject healthbar;

    public void init(float maxhp){
        hp = maxhp;
        if(healthbar != null){
            healthbar.GetComponent<HealthBar>().SetMaxHealth(hp);
        }
    }
    public void hit(float damage, GameObject hiteffect){
        hp -= damage*damagetakenmultiplier;
        
        if(hp <= 0){
            if(hiteffect !=null){
                Instantiate(hiteffect, transform.position, transform.rotation);
            }
            if(healthbar != null){
                healthbar.GetComponent<HealthBar>().SetHealth(hp);
            }
            Destroy(gameObject);
        }
    }
}
