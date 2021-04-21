using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class GunDoublePistol : MonoBehaviour
{
    Transform[] enemies;
    GameObject thePlayer;
    public GunInfo EquippedGun;
    bool readytoshoot = true;
    float spreadvar;
    IEnumerable<int> multishotcount;
    bool even;
    public GameObject bullet;
    float multishotangle;

    // Start is called before the first frame update
    void Awake(){
        GetComponent<gameinitiater>().funcs.Add(Initer);
    }
    public void Initer()
    {
        // print("adding");
        // thePlayer = GameObject.Find("Player");
        GunInfo doublepistol = new GunInfo("doublepistol",0, 40, 1, 20, 2, 10, 1, GetComponent<GunClasses>().single_shot_effect, GetComponent<GunClasses>().bullet, false, 0, DoublePistolReload);
        EquippedGun = doublepistol;
        GunInfo.Guns.Add(doublepistol.name, doublepistol);
    }

    // Update is called once per frame
    public void DoublePistolReload(GameObject thePlayer){
        this.thePlayer = thePlayer;
        if(Input.GetMouseButtonDown(0) && readytoshoot)
        {
            readytoshoot = false;
            DoublePistolShoot();
            Invoke("reload", EquippedGun.FireRate);
        }
    }
    public void DoublePistolShoot()
    {
        if (EquippedGun.multishot % 2 == 0)
        {
            even = true;
            multishotangle = ((EquippedGun.multishot-1)/2) *-20;
        }
        else
        {
            even = false;
            multishotangle = (EquippedGun.multishot/2) * -10;
        }
        GameObject enemytohit = GetClosestEnemy();
        GameObject shootingpoint = thePlayer.transform.GetChild(0).gameObject;
        multishotcount = Enumerable.Range(1, (int)EquippedGun.multishot);
            // bullet = Instantiate(EquippedGun.bullet, new Vector3(shootingpoint.transform.position.x, shootingpoint.transform.position.y, -9 ), thePlayer.transform.rotation);
            // StartCoroutine(DoublePistolBulletModifier(bullet, EquippedGun, enemytohit));
        foreach (int i in multishotcount)
        {
            bullet = Instantiate(EquippedGun.bullet, new Vector3(shootingpoint.transform.position.x, shootingpoint.transform.position.y, -9 ), thePlayer.transform.rotation * Quaternion.Euler(0, 0, multishotangle));
            StartCoroutine(DoublePistolBulletModifier(bullet, EquippedGun, enemytohit));
            multishotangle += 20;
        }
    }


    public IEnumerator DoublePistolBulletModifier(GameObject bullet, GunInfo shotby, GameObject enemytohit){
        if( shotby.BulletSpeed > 140){
            bullet.GetComponent<MoveForward>().enableraycast = true;
        }


        if(enemytohit){
            print(enemytohit.name);
            Vector2 distance = bullet.transform.InverseTransformPoint(new Vector2(enemytohit.transform.position.x, enemytohit.transform.position.y)).normalized;
            print("distance: " + bullet.transform.InverseTransformDirection(new Vector2(enemytohit.transform.position.x, enemytohit.transform.position.y)) + " : " + bullet.transform.InverseTransformDirection(bullet.transform.position)) ;
            Vector2 previousdistance = new Vector2(0,1);
            float followingsteepness  = .005f;
            while(!bullet.GetComponent<MoveForward>().hit){
                distance = bullet.transform.InverseTransformPoint(new Vector2(enemytohit.transform.position.x, enemytohit.transform.position.y)).normalized;
                Vector2 diff = distance - previousdistance;
                if(Mathf.Abs(diff.x) >= followingsteepness){
                    if(diff.x >= followingsteepness){
                        distance.x = previousdistance.x + followingsteepness;
                    }
                    else if(diff.x <= -followingsteepness){
                        distance.x = previousdistance.x - followingsteepness;
                    }
                }
                if(Mathf.Abs(diff.y) >= followingsteepness){
                    if(diff.x >= followingsteepness){
                        distance.y = previousdistance.y + followingsteepness;
                    }
                    else if(diff.y <= -followingsteepness){
                        distance.y = previousdistance.y - followingsteepness;
                    }
                }
                previousdistance = distance;
                bullet.transform.Translate(distance * shotby.BulletSpeed * Time.deltaTime);
                print(previousdistance + ": " + distance);
                yield return new WaitForEndOfFrame();
            }
                
        }
        else{
            print("enemy not found");
            while(!bullet.GetComponent<MoveForward>().hit){
                bullet.transform.Translate(new Vector2(0, 1) * (shotby.BulletSpeed-1) * Time.deltaTime);
                yield return new WaitForEndOfFrame();   
            }
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
    public GameObject GetClosestEnemy(){
        GameObject[] hitColliders = Physics2D.OverlapCircleAll(thePlayer.transform.position, 20f).Select(value => value.gameObject).ToArray();
        GameObject closest = null; 
        foreach(GameObject hit in hitColliders){
            if(hit.tag  == "Obstacles"){
                if(closest == null){
                    closest = hit;
                    continue;
                }
                float distance = Vector3.Distance(hit.transform.position, thePlayer.transform.position);
                if(distance < Vector3.Distance(closest.transform.position, thePlayer.transform.position)){
                    closest = hit;
                }
            }
        }

        if(closest != null){

            return closest;
        }
        else{
            return null;
        }
    }
}
