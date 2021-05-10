using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
public class shooting : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject thePlayer;
    PlayerData temp;
    GunInfo EquippedGun;
    bool readytoshoot = true;
    float spreadvar;
    IEnumerable<int> multishotcount;
    bool even;
    public GameObject bullet;
    public GameObject gun1;
    bool readytoswitch = true;
    public GameObject gun2;
    float multishotangle;
    PlayerData currentdata;
    void Start()
    {
        thePlayer = GameObject.Find("Player");
        currentdata = livegamedata.currentdata;
        EquippedGun = currentdata.equipped[0];
        if(currentdata.equipped[0].level >= 10){
            gun1.transform.GetChild(0).GetComponent<Image>().sprite = currentdata.equipped[0].uiInfo.specialicon;
        }
        else{
            gun1.transform.GetChild(0).GetComponent<Image>().sprite = currentdata.equipped[0].uiInfo.icon;
        }
        if(currentdata.equipped[1].level >= 10){
            gun2.transform.GetChild(0).GetComponent<Image>().sprite = currentdata.equipped[1].uiInfo.specialicon;
        }
        else{
            gun2.transform.GetChild(0).GetComponent<Image>().sprite = currentdata.equipped[1].uiInfo.icon;
        }
        switchgun(gun1, gun2);

    }

    // Update is called once per frame
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && readytoswitch){
            EquippedGun = currentdata.equipped[0];
            switchgun(gun1, gun2);
            readytoswitch = false;
            Invoke("switchguns", 2);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2) && readytoswitch){
            switchgun(gun2, gun1);
            EquippedGun = currentdata.equipped[1];
            readytoswitch = false;
            Invoke("switchguns", 2);
        }
        if(EquippedGun.level >= 10){
            EquippedGun.shootfunc(thePlayer);
        }
        else{
            DefaultReload();
        }
    }
    void switchgun(GameObject gun, GameObject othergun){

        gun.GetComponent<Image>().color = new Color(1,1,1,1f);
        gun.transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1,1f);
        othergun.GetComponent<Image>().color = new Color(1,1,1,.5f);
        othergun.transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1,.5f);
    }
    void switchguns(){
        readytoswitch = true;
    }
    void ReadyToShoot()
    {
        readytoshoot = true;
    }
    public bool Chance(float chance)
    {
        int random = UnityEngine.Random.Range(1, 100);
        if (random <= chance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void DefaultReload(){
        if (EquippedGun.AllowButtonHold && Input.GetMouseButton(0) && readytoshoot)
        {
            readytoshoot = false;
            Invoke("ReadyToShoot", EquippedGun.FireRate);
        }
        else if(Input.GetMouseButtonDown(0) && readytoshoot && !EquippedGun.AllowButtonHold)
        {
            readytoshoot = false;
            Invoke("ReadyToShoot", EquippedGun.FireRate);
            DefaultShoot();
        }
    }

    void DefaultShoot()
    {
        multishotcount = Enumerable.Range(1, (int)EquippedGun.multishot);
        if (EquippedGun.multishot != 1)
        {
            if (EquippedGun.multishot % 2 == 0)
            {
                even = true;
                multishotangle = ((EquippedGun.multishot-1)/2) *-10;
            }
            else
            {
                even = false;
                multishotangle = (EquippedGun.multishot/2) * -10;
            }
            foreach (int i in multishotcount)
            {
                if (EquippedGun.BulletSpread != 0)
                {
                    spreadvar = UnityEngine.Random.Range(-EquippedGun.BulletSpread, EquippedGun.BulletSpread + 1);
                }
                bullet = Instantiate(EquippedGun.bullet, new Vector3(transform.position.x, transform.position.y, -9 ), transform.rotation * Quaternion.Euler(0, 0, spreadvar + multishotangle));
                StartCoroutine(DefaultBulletModifier(bullet, EquippedGun));
                multishotangle += 10;
                if (multishotangle == 0 && even == false)
                {
                    multishotangle += 10;
                }
            }
        }
        else
        {
            if (EquippedGun.BulletSpread != 0)
            {
                spreadvar = UnityEngine.Random.Range(-EquippedGun.BulletSpread, EquippedGun.BulletSpread);
            }
            bullet = Instantiate(EquippedGun.bullet, new Vector3(transform.position.x, transform.position.y, -9), transform.rotation * Quaternion.Euler(0, 0, spreadvar));
            StartCoroutine(DefaultBulletModifier(bullet, EquippedGun));
        }

        thePlayer.transform.Translate(new Vector2(0, -EquippedGun.recoil * Time.deltaTime));
    }


    public IEnumerator DefaultBulletModifier(GameObject bullet, GunInfo shotby){
        bool hit = bullet.GetComponent<MoveForward>().hit;
        if( shotby.BulletSpeed > 140){
            bullet.GetComponent<MoveForward>().enableraycast = true;
        }
        while(!hit){
            hit = bullet.GetComponent<MoveForward>().hit;
            bullet.transform.Translate(new Vector2(0, 1) * shotby.BulletSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        
        Collider2D enemy = bullet.GetComponent<MoveForward>().enemyhit;
         if (enemy.CompareTag("enemy"))
        {
            float damage;
            bool Crit = Chance(EquippedGun.CC);
            if (Crit)
            {
                damage = EquippedGun.Damage * EquippedGun.CD;
            }
            else
            {
                damage = EquippedGun.Damage;
            }
            enemy.GetComponent<playereffects>().hit(damage, EquippedGun.HitEffect);
        }
        if (enemy.tag != "Player")
        {
            Destroy(bullet);
        }
    }
}
