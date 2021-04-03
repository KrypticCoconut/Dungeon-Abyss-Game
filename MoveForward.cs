using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    GameObject EZ;
    GameObject thePlayer;
    List<GunInfo> GunsList;
    GunInfo EquippedGun;
    RaycastHit2D hit;
    float damage;
    void Start()
    {
        thePlayer = GameObject.Find("Player");
        GunsList = thePlayer.GetComponent<guns>().GunList;
        EquippedGun = GunsList[thePlayer.GetComponent<guns>().equipped];

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.Translate(new Vector2(0, 1) * EquippedGun.BulletSpeed * Time.deltaTime);
        if (EquippedGun.BulletSpeed > 140)
        {
            hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.down), 12f);
            if (hit)
            {
                GetComponent<MoveForward>().Hit(hit.collider);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D enemy)
    {
        Hit(enemy);
    }

    public void Hit(Collider2D enemy)
    {
        if (enemy.CompareTag("enemy"))
        {
            bool Crit = Chance(EquippedGun.CC);
            if (Crit)
            {
                damage = EquippedGun.Damage * EquippedGun.CD;
            }
            else
            {
                damage = EquippedGun.Damage;
            }
            print(damage);
            enemy.GetComponent<enemyhealth>().health -= damage;
            if (enemy.GetComponent<enemyhealth>().health <= 0)
            {
                Instantiate(EquippedGun.HitEffect, enemy.transform.position, Quaternion.identity);
            }
            //Instantiate(EquippedGun.bu, transform.position, Quaternion.identity);
        }
        if (enemy.tag != "Player")
        {
            print(enemy.name);
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

