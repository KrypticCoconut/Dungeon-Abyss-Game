using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPistol : MonoBehaviour
{

    GameObject thePlayer;
    GunInfo EquippedGun;
    bool readytoshoot = true;
    float spreadvar;
    IEnumerable<int> multishotcount;
    float damagedone;
    bool even;
    int shotnumber;
    GameObject bullet;
    public Sprite icon;
    public Sprite specialicon;
    // Start is called before the first frame update
    void Awake(){
        GetComponent<gameinitiater>().funcs.Add(Initer);
    }
    void Initer()
    {
        GunUiInfo info = new GunUiInfo(icon,specialicon,"Ace", "Every 4th shot does 2 times the damage done in the last 3 shots");
        GunInfo pistol = new GunInfo("pistol",0, 40, 1, 20, 1, 10, 1, GetComponent<GunClasses>().single_shot_effect, GetComponent<GunClasses>().bullet, false, 0, PistolReload, info, 1000);
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
        bullet = Instantiate(EquippedGun.bullet, new Vector3(thePlayer.transform.GetChild(0).position.x, thePlayer.transform.GetChild(0).position.y, -9), thePlayer.transform.GetChild(0).rotation   * Quaternion.Euler(0, 0, spreadvar));
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
                damage = damagedone*2;
                shotnumber = 0;
            }
            else{
                damagedone += damage;
            }
            enemy.GetComponent<mobeffects>().sethp((int)(enemy.GetComponent<mobeffects>().hp - damage), null, EquippedGun.HitEffect);
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
