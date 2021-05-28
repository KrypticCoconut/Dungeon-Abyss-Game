using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GunMachineGun : MonoBehaviour
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
    public GameObject bouncebullet;
    // Start is called before the first frame update
    void Awake(){
        GetComponent<gameinitiater>().funcs.Add(Initer);
    }
    void Initer()
    {
        GunUiInfo info = new GunUiInfo(icon,specialicon,"Bouncing bullets", "bullets will bouncy off walls 2 times");
        GunInfo machinegun = new GunInfo("machinegun",10, 40, .07f, 35, 1, 15, .5f, GetComponent<GunClasses>().single_shot_effect, bouncebullet, true, 0, MGReload, info, -10);
        EquippedGun = machinegun;
        GunInfo.Guns.Add(machinegun.name, machinegun);
    }

    // Update is called once per frame
    public void MGReload(GameObject thePlayer){
        this.thePlayer = thePlayer;
        if(Input.GetMouseButton(0))
        {
            if(readytoshoot){
                readytoshoot = false;
                MGShoot();
                Invoke("reload", EquippedGun.FireRate);
            }
        }
    }
    public void MGShoot()
    {
        bullet = Instantiate(EquippedGun.bullet, new Vector3(thePlayer.transform.GetChild(0).position.x, thePlayer.transform.GetChild(0).position.y, -9), thePlayer.transform.GetChild(0).rotation * Quaternion.Euler(0, 0, spreadvar));
        StartCoroutine(MGBulletModifier(bullet, EquippedGun));
        thePlayer.transform.Translate(new Vector2(0, -EquippedGun.recoil * Time.deltaTime));
    }


    public IEnumerator MGBulletModifier(GameObject bullet, GunInfo shotby){
        bullet.GetComponent<MoveForward>().bounce = true;
        if( shotby.BulletSpeed > 140){
            bullet.GetComponent<MoveForward>().enableraycast = true;
        }
        int bouncetimes = 2;
        Vector2 velocity = new Vector2(0, 1);   
        foreach(int x in Enumerable.Range(1, bouncetimes)){
            while(!bullet.GetComponent<MoveForward>().hit){
                bullet.transform.Translate(velocity.normalized  * Time.deltaTime * EquippedGun.BulletSpeed);
                // bullet.GetComponent<Rigidbody2D>().AddRelativeForce(velocity  * Time.deltaTime * EquippedGun.BulletSpeed);
                yield return new WaitForEndOfFrame();
            }
            Collider2D enemy = bullet.GetComponent<MoveForward>().enemyhit;
            if (enemy.CompareTag("enemy") )
            {
                float damage;
                bool Crit = Chance(EquippedGun.CC);
                if (Crit)
                {
                    damage = ((EquippedGun.Damage) * 2) * EquippedGun.CD;
                }
                else
                {
                    damage = (EquippedGun.Damage);
                }
                enemy.GetComponent<mobeffects>().sethp((int)(enemy.GetComponent<mobeffects>().hp - damage), null, EquippedGun.HitEffect);
                break;
            }
            else{
                Collision2D collision = bullet.GetComponent<MoveForward>().collision; 
                Vector2 dir = Vector2.Reflect(bullet.transform.TransformDirection(velocity.normalized), collision.contacts[0].normal);
                velocity = bullet.transform.InverseTransformDirection(dir);
                bullet.GetComponent<MoveForward>().hit = false;
            }
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
