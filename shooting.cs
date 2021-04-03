using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class shooting : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject thePlayer;
    GunInfo EquippedGun;
    bool readytoshoot = true;
    float spreadvar;
    IEnumerable<int> multishotcount;
    bool even;
    float multishotangle;
    void Start()
    {
        GameData currentdata = SaveSystem.LoadSave();
        thePlayer = GameObject.Find("Player");
        EquippedGun = currentdata.data.equipped[0];
        print(EquippedGun.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (EquippedGun.AllowButtonHold && Input.GetMouseButton(0) && readytoshoot)
        {
            Shoot();
            readytoshoot = false;
            Invoke("ReadyToShoot", EquippedGun.FireRate);
        }
        else if(Input.GetMouseButtonDown(0) && readytoshoot)
        {
            Shoot();
            readytoshoot = false;
            Invoke("ReadyToShoot", EquippedGun.FireRate);
        }
    }

    void Shoot()
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
                Instantiate(EquippedGun.bullet, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation * Quaternion.Euler(0, 0, spreadvar + multishotangle));
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
            Instantiate(EquippedGun.bullet, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation * Quaternion.Euler(0, 0, spreadvar));
        }

         thePlayer.transform.Translate(new Vector2(0, -EquippedGun.recoil * Time.deltaTime));
    }

    void ReadyToShoot()
    {
        readytoshoot = true;
    }
}
