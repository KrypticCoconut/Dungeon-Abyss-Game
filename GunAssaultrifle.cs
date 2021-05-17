using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAssaultrifle : MonoBehaviour
{
    GameObject thePlayer;
    GunInfo EquippedGun;
    bool readytoshoot = true;
    float spreadvar;
    IEnumerable<int> multishotcount;
    bool even;   
    GameObject bullet;
    public Sprite icon;
    public Sprite specialicon;

    public int maxstacks = 30;
    public int stacks = 0;
    public float damageperstack = 50;
    public float reloadperstack = .05f;
    // Start is called before the first frame update
    void Awake(){
        GetComponent<gameinitiater>().funcs.Add(Initer);
    }
    void Initer()
    {
        GunUiInfo info = new GunUiInfo(icon,specialicon,"Soulstealer", "bullets give 5hp regen, holding the fire button slows firerate but increases damage by 50 and doubles damage dealt");
        GunInfo rifle = new GunInfo("rifle",0, 60, .2f, 30, 1, 30, 1.5f, GetComponent<GunClasses>().single_shot_effect, GetComponent<GunClasses>().bullet, true, 0, RifleReload, info, 3000);
        EquippedGun = rifle;
        GunInfo.Guns.Add(rifle.name, rifle);
    }

    // Update is called once per frame
    public void RifleReload(GameObject thePlayer){
        this.thePlayer = thePlayer;
        if(Input.GetMouseButton(0))
        {
            if(readytoshoot){
                readytoshoot = false;
                RifleShoot();
                stacks +=1;
                Invoke("reload", EquippedGun.FireRate + (stacks * reloadperstack));
                print(stacks +": " +reloadperstack * stacks);
            }
        }
        else{
            stacks = 0;
        }
    }
    public void RifleShoot()
    {
        bullet = Instantiate(EquippedGun.bullet, new Vector3(thePlayer.transform.position.x, thePlayer.transform.position.y, -9), thePlayer.transform.rotation * Quaternion.Euler(0, 0, spreadvar));
        StartCoroutine(RifleBulletModifier(bullet, EquippedGun));
        thePlayer.transform.Translate(new Vector2(0, -EquippedGun.recoil * Time.deltaTime));
    }


    public IEnumerator RifleBulletModifier(GameObject bullet, GunInfo shotby){
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
                damage = ((EquippedGun.Damage + (stacks * damageperstack)) * 2) * EquippedGun.CD;
            }
            else
            {
                damage = ((EquippedGun.Damage + (stacks * damageperstack)) *2);
            }
            if(thePlayer.GetComponent<playereffects>().hp+5 > thePlayer.GetComponent<playereffects>().maxhp){
                thePlayer.GetComponent<playereffects>().hp += 5;
            }
            
            enemy.GetComponent<playereffects>().hp -= damage;
            enemy.GetComponent<playereffects>().sethp();
            if(enemy.GetComponent<playereffects>().hp <= 0 ){
                Instantiate(EquippedGun.HitEffect, enemy.transform.position, enemy.transform.rotation);
                Destroy(enemy.gameObject);
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
