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
    public float damagedone;
    bool even;
    public int shotnumber;
    public GameObject bullet;
    // Start is called before the first frame update
    void Awake(){
        GetComponent<gameinitiater>().funcs.Add(Initer);
    }
    void Initer()
    {
        //thePlayer = GameObject.Find("Player");
        GunInfo pistol = new GunInfo("pistol",0, 40, 1, 20, 1, 10, 1, GetComponent<GunClasses>().single_shot_effect, GetComponent<GunClasses>().bullet, false, 0, PistolReload);
        EquippedGun = pistol;
        GunInfo.Guns.Add(pistol.name, pistol);
    }

    // Update is called once per frame
    public void PistolReload(GameObject thePlayer){
        this.thePlayer = thePlayer;
        if(Input.GetMouseButtonDown(0) && readytoshoot)
        {
            readytoshoot = false;
            PistolShoot();
            Invoke("reload", EquippedGun.FireRate);
        }
    }
    public void PistolShoot()
    {
        shotnumber++;
        bullet = Instantiate(EquippedGun.bullet, new Vector3(thePlayer.transform.position.x, thePlayer.transform.position.y, -9), thePlayer.transform.rotation * Quaternion.Euler(0, 0, spreadvar));
        StartCoroutine(PistolBulletModifier(bullet, EquippedGun));
        thePlayer.transform.Translate(new Vector2(0, -EquippedGun.recoil * Time.deltaTime));
    }


    public IEnumerator PistolBulletModifier(GameObject bullet, GunInfo shotby){
        if( shotby.BulletSpeed > 140){
            bullet.GetComponent<MoveForward>().enableraycast = true;
        }
        while(!bullet.GetComponent<MoveForward>().hit){
            bullet.transform.Translate(new Vector2(0, 1) * shotby.BulletSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        Collider2D enemy = bullet.GetComponent<MoveForward>().enemyhit;
        if (enemy.CompareTag("enemy") )
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
            if(shotnumber == 4){
                damage = damagedone*2 + damage;
                shotnumber = 0;
            }
            else{
                damagedone += damage;
            }
            enemy.GetComponent<enemyhealth>().health -= damage;
            if (enemy.GetComponent<enemyhealth>().health <= 0)
            {
                Instantiate(EquippedGun.HitEffect, enemy.transform.position, Quaternion.identity);
            }

        }
        if (enemy.tag != "Player")
        {
            Destroy(bullet);
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
    public void reload(){
        readytoshoot = true;
    }
}
