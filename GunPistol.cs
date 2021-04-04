using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPistol : MonoBehaviour
{

    GameObject thePlayer;
    public GunInfo EquippedGun;
    bool readytoshoot = true;
    float spreadvar;
    IEnumerable<int> multishotcount;
    bool even;
    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        bullet = GetComponent<
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void PistolReload(){
        int shotnumber = 
        if(Input.GetMouseButtonDown(0) && readytoshoot)
        {
            readytoshoot = false;
            PistolShoot()
        }
    }
    void PistolShoot()
    {

        bullet = Instantiate(EquippedGun.bullet, new Vector3(transform.position.x, transform.position.y, -9), transform.rotation * Quaternion.Euler(0, 0, spreadvar));
        StartCoroutine(PistolBulletModifier(bullet, EquippedGun));

        thePlayer.transform.Translate(new Vector2(0, -EquippedGun.recoil * Time.deltaTime));
    }


    public IEnumerator PistolBulletModifier(GameObject bullet, GunInfo shotby){
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
