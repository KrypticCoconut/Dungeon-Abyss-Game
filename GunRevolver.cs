using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRevolver : MonoBehaviour
{
    GameObject thePlayer;
    GunInfo EquippedGun;
    bool readytoshoot = true;
    GameObject bullet;
    public Sprite icon;
    public Sprite specialicon;


    int maxstacks = 10;
    int stacks = 0;
    float reloadperstack = .05f;
    float CCperstack = 5f;
    float damageperstack = 10f;
    float recoilperstack = 1f;
    // Start is called before the first frame update
    void Awake(){
        GetComponent<gameinitiater>().funcs.Add(Initer);
    }
    void Initer()
    {
        GunUiInfo info = new GunUiInfo(icon,specialicon,"Rapid rounds", "Every Consecutives shot hit gives a stack(max 10 stacks) of rapid rounds which increases crit by 5, damage by 10, reload by .05 seconds and bullet spread");
        GunInfo revolver = new GunInfo("revolver",5, 70, 1, 80, 1, 40, 1, GetComponent<GunClasses>().single_shot_effect, GetComponent<GunClasses>().bullet, false, 0, RevolverReload, info, 3000);
        EquippedGun = revolver;
        GunInfo.Guns.Add(revolver.name, revolver);
    }
    public IEnumerator stackscooldown(int currentstacks, float stackscd){
        yield return new WaitForSeconds(stackscd);
        if(stacks < currentstacks+1){
            stacks = 0;
        }

    }

    // Update is called once per frame
    public void RevolverReload(GameObject thePlayer){
        this.thePlayer = thePlayer;
        if(Input.GetMouseButtonDown(0) && readytoshoot)
        {
            readytoshoot = false;
            RevolverShoot();
            Invoke("reload", EquippedGun.FireRate - (reloadperstack*stacks));
        }
    }
    public void RevolverShoot()
    {
        bullet = Instantiate(EquippedGun.bullet, new Vector3(thePlayer.transform.position.x, thePlayer.transform.position.y, -9), thePlayer.transform.rotation * Quaternion.Euler(0, 0, UnityEngine.Random.Range(-EquippedGun.BulletSpread + (recoilperstack*stacks), EquippedGun.BulletSpread + (recoilperstack*stacks) + 1)));
        StartCoroutine(RevolverBulletModifier(bullet, EquippedGun));
        thePlayer.transform.Translate(new Vector2(0, -EquippedGun.recoil * Time.deltaTime));
    }


    public IEnumerator RevolverBulletModifier(GameObject bullet, GunInfo shotby){
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
            bool Crit = Chance(EquippedGun.CC + (CCperstack * stacks));
            if (Crit)
            {
                damage = EquippedGun.Damage * EquippedGun.CD;
            }
            else
            {
                damage = EquippedGun.Damage + (damageperstack * stacks);
            }
            enemy.GetComponent<playereffects>().hit(damage, EquippedGun.HitEffect);
            stacks += 1;
            StartCoroutine(stackscooldown(stacks, 2));
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



/*
public class RevolverPistol : MonoBehaviour
{
    GameObject thePlayer;
    GunInfo EquippedGun;
    bool readytoshoot = true;
    GameObject bullet;
    public Sprite icon;
    public Sprite specialicon;


    public int maxstacks = 10;
    public int stacks = 0;
    public float reloadperstack = .05f;
    public float CCperstack = 5f;
    public float damageperstack = 10f;
    public float recoilperstack = 2f;
    // Start is called before the first frame update
    void Awake(){
        GetComponent<gameinitiater>().funcs.Add(Initer);
    }
    void Initer()
    {
        GunUiInfo info = new GunUiInfo(icon,specialicon,"Rapid rounds", "Every Consecutives shot hit gives a stack(max 10 stacks) of rapid rounds which increases crit by 5%, damage by 10, reload by .05 seconds and bullet spread");
        GunInfo revolver = new GunInfo("revolver",5, 70, 1, 80, 1, 40, 1, GetComponent<GunClasses>().single_shot_effect, GetComponent<GunClasses>().bullet, false, 0, RevolverReload, info, 3000);
        EquippedGun = revolver;
        GunInfo.Guns.Add(revolver.name, revolver);
    }

    // Update is called once per frame
    public void RevolverReload(GameObject thePlayer){
        this.thePlayer = thePlayer;
        if(Input.GetMouseButtonDown(0) && readytoshoot)
        {
            readytoshoot = false;
            RevolverShoot();
            Invoke("reload", EquippedGun.FireRate);
        }
    }
    public void RevolverShoot()
    {
        bullet = Instantiate(EquippedGun.bullet, new Vector3(thePlayer.transform.position.x, thePlayer.transform.position.y, -9), thePlayer.transform.rotation);
        StartCoroutine(RevolverBulletModifier(bullet, EquippedGun));
        thePlayer.transform.Translate(new Vector2(0, -EquippedGun.recoil * Time.deltaTime));
    }


    public IEnumerator RevolverBulletModifier(GameObject bullet, GunInfo shotby){
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
            enemy.GetComponent<playereffects>().hit(damage, EquippedGun.HitEffect);

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
*/