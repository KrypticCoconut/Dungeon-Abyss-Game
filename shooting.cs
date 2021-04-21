using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
public class shooting : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject thePlayer;
    public PlayerData temp;
    public GunInfo EquippedGun;
    bool readytoshoot = true;
    float spreadvar;
    IEnumerable<int> multishotcount;
    bool even;
    public GameObject bullet;
    float multishotangle;
    PlayerData currentdata;
    void Start()
    {
        thePlayer = GameObject.Find("Player");
        currentdata = livegamedata.currentdata;
        EquippedGun = currentdata.equipped[0];
        print(transform.InverseTransformPoint(transform.position) + ", " + transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            EquippedGun = currentdata.equipped[0];
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            EquippedGun = currentdata.equipped[1];
        }
        if(EquippedGun.level >= 10){
            
        }
        EquippedGun.shootfunc(thePlayer);
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
        }
        else if(Input.GetMouseButtonDown(0) && readytoshoot && !EquippedGun.AllowButtonHold)
        {
            readytoshoot = false;
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
                    print("trye");
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
            enemy.GetComponent<enemyhealth>().health -= damage;
            if (enemy.GetComponent<enemyhealth>().health <= 0)
            {
                Instantiate(EquippedGun.HitEffect, enemy.transform.position, Quaternion.identity);
            }

        }
        if (enemy.tag != "Player")
        {
            Destroy(gameObject);
        }
    }




}
