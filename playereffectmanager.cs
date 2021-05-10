using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playereffectmanager : MonoBehaviour
{
    float maxhealth;
    public GameObject healthbar;    

    // Start is called before the first frame update
    void Start()
    {
        maxhealth = livegamedata.currentdata.health;
        GetComponent<playereffects>().init(maxhealth);
    }
}
