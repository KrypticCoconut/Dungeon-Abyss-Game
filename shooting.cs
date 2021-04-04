﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class shooting : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject thePlayer;
    public GunInfo EquippedGun;
    bool readytoshoot = true;
    float spreadvar;
    IEnumerable<int> multishotcount;
    bool even;
    public GameObject bullet;
    float multishotangle;
    void Start()
    {
        print("hi");
        GameData currentdata = SaveSystem.LoadSave();
        thePlayer = GameObject.Find("Player");
        EquippedGun = currentdata.data.equipped[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (EquippedGun.AllowButtonHold && Input.GetMouseButton(0) && readytoshoot)
        {
            readytoshoot = false;
            DefaultShoot();
            Invoke("readytoshoot", EquippedGun.FireRate);
        }
        else if(Input.GetMouseButtonDown(0) && readytoshoot && !EquippedGun.AllowButtonHold)
        {
            print(readytoshoot);
            readytoshoot = false;
            print("switched" + ": " + readytoshoot);
            DefaultShoot();
            Invoke("ReadyToShoot", EquippedGun.FireRate);
            print("ending2: " + readytoshoot);  
        }
    }

    void DefaultShoot()
    {
        multishotcount = Enumerable.Range(1, (int)EquippedGun.multishot+1);
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
            print("intantiated: " + readytoshoot);
            StartCoroutine(DefaultBulletModifier(bullet, EquippedGun));
        }

        thePlayer.transform.Translate(new Vector2(0, -EquippedGun.recoil * Time.deltaTime));
        print("ending: " + readytoshoot);
    }

    void ReadyToShoot()
    {
        print("switched again");
        readytoshoot = true;
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
}
