using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GunSniper : MonoBehaviour
{
    // Start is called before the first frame update    
    GameObject thePlayer;
    GunInfo EquippedGun;
    bool readytoshoot = true;
    float spreadvar;
    IEnumerable<int> multishotcount;
    bool even;   
    GameObject bullet;
    public Sprite icon;
    public Sprite specialicon;
    public GameObject blackbullet;

    // Start is called before the first frame update
    void Awake(){
        GetComponent<gameinitiater>().funcs.Add(Initer);
    }
    void Initer()
    {
        GunUiInfo info = new GunUiInfo(icon,specialicon,"Armor piercing bullets", "bullets pass through targets (including walls) if they kill them. the gun also gets +3 crit damage");
        GunInfo sniper = new GunInfo("sniper",0, 80, 1.5f, 500, 1, 80, 3f, GetComponent<GunClasses>().single_shot_effect, blackbullet, false, 0, SniperReload, info, 5000);
        EquippedGun = sniper;
        GunInfo.Guns.Add(sniper.name, sniper);
    }

    // Update is called once per frame
    public void SniperReload(GameObject thePlayer){
        this.thePlayer = thePlayer;
        if(Input.GetMouseButtonDown(0))
        {
            if(readytoshoot){
                readytoshoot = false;
                SniperShoot();
                Invoke("reload", EquippedGun.FireRate);
            }
        }
    }
    public void SniperShoot()
    {
        bullet = Instantiate(EquippedGun.bullet, new Vector3(thePlayer.transform.GetChild(0).position.x, thePlayer.transform.GetChild(0).position.y, -9), thePlayer.transform.GetChild(0).rotation * Quaternion.Euler(0, 0, spreadvar));
        StartCoroutine(SniperBulletModifier(bullet, EquippedGun));
        thePlayer.transform.Translate(new Vector2(0, -EquippedGun.recoil * Time.deltaTime));
    }


    public IEnumerator SniperBulletModifier(GameObject bullet, GunInfo shotby){
        if( shotby.BulletSpeed > 140){
            bullet.GetComponent<MoveForward>().enableraycast = true;
        }
        foreach(int x in Enumerable.Range(1, 5)){
            yield return null;
            print("loopiung");
            while(!bullet.GetComponent<MoveForward>().hit){
                bullet.transform.Translate(new Vector2(0, 1) * shotby.BulletSpeed * Time.deltaTime);
                yield return null;
                print("hit");
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
                
                enemy.GetComponent<mobeffects>().sethp((int)(enemy.GetComponent<mobeffects>().hp - damage), null, EquippedGun.HitEffect);
                if(enemy.GetComponent<mobeffects>().isactive == true){
                    break;
                }

            }
            else if(enemy.CompareTag("Obstacles")){
                Destroy(enemy.gameObject);
            }
            bullet.GetComponent<MoveForward>().hit = false;
        }
        Destroy(bullet);

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
