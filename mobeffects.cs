using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class mobeffects : MonoBehaviour
{
    public int extradmg = 0;
    public int extrafr = 0;
    public int extracc = 0;
    public int extracd = 0;
    public bool isactive = true;
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
    public void sethp(int hp, GameObject hiteffect, GameObject Destroyeffect){
        this.hp = hp;
        if(healthbar){
            healthbar.GetComponent<HealthBar>().SetHealth(hp);
        }
        if(hp < 0){
            isactive = false;
            Instantiate(Destroyeffect, transform.position, transform.rotation);
            transform.gameObject.SetActive(false);
        }
        else if(hiteffect){
            Instantiate(hiteffect, transform.position, transform.rotation);
        }
    }
}
