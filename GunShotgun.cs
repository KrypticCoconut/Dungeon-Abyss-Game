using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;    
public class GunShotgun : MonoBehaviour
{
    // Start is called before the first frame update
    Transform[] enemies;
    GameObject thePlayer;
    public GunInfo EquippedGun;
    bool readytoshoot = true;
    public Sprite icon;
    public Sprite specialicon;
    IEnumerable<int> multishotcount;
    bool even;
    GameObject bullet;
    float multishotangle;
    public Dictionary<GameObject, float> counter = new Dictionary<GameObject, float>();

    // Start is called before the first frame update
    void Awake(){
        GetComponent<gameinitiater>().funcs.Add(Initer);
    }
    public void Initer()
    {
        GunUiInfo info = new GunUiInfo(icon,specialicon,"Lucky shot", "The shotgun fires 6-12 bullets, each bullet does 150 percent the damage of the previous one on a single target.");
        GunInfo shotgun = new GunInfo("shotgun",0, 10, 2, 60, 2, 10, 1, GetComponent<GunClasses>().single_shot_effect, GetComponent<GunClasses>().bullet, false, 0, ShotgunReload, info, 6000);
        EquippedGun = shotgun;
        GunInfo.Guns.Add(shotgun.name, shotgun);
    }

    // Update is called once per frame
    public void ShotgunReload(GameObject thePlayer){
        this.thePlayer = thePlayer;
        if(Input.GetMouseButtonDown(0) && readytoshoot)
        {
            counter = new Dictionary<GameObject, float>();
            readytoshoot = false;
            ShotgunShoot();
            Invoke("reload", EquippedGun.FireRate);
        }
    }
    public void ShotgunShoot()
    {
        int multishot = Random.Range(6,12);
        if (multishot % 2 == 0)
        {
            even = true;
            multishotangle = ((multishot-1)/2) *-10;
        }
        else
        {
            even = false;
            multishotangle = (multishot/2) * -10;
        }

        GameObject shootingpoint = thePlayer.transform.GetChild(0).gameObject;
        multishotcount = Enumerable.Range(1, (int)multishot);
            // bullet = Instantiate(EquippedGun.bullet, new Vector3(shootingpoint.transform.position.x, shootingpoint.transform.position.y, -9 ), thePlayer.transform.rotation);
            // StartCoroutine(DoublePistolBulletModifier(bullet, EquippedGun, enemytohit));

        foreach (int i in Enumerable.Range(1, multishot))
        {
            bullet = Instantiate(EquippedGun.bullet, new Vector3(shootingpoint.transform.position.x, shootingpoint.transform.position.y, -9 ), thePlayer.transform.rotation * Quaternion.Euler(0, 0, multishotangle));
            StartCoroutine(ShotgunBulletModifier(bullet, EquippedGun));
            multishotangle += 10;
            if (multishotangle == 0 && even == false)
            {
                multishotangle += 10;
            }
        }
    }


    public IEnumerator ShotgunBulletModifier(GameObject bullet, GunInfo shotby){
        if( shotby.BulletSpeed > 140){
            bullet.GetComponent<MoveForward>().enableraycast = true;
        }

        GameObject enemytohit = GetClosestEnemy(bullet);
        if(enemytohit){
            float speed = Time.deltaTime;
            Vector2 distance = bullet.transform.InverseTransformPoint(new Vector2(enemytohit.transform.position.x, enemytohit.transform.position.y)).normalized;
            Vector2 previousdistance = new Vector2(0,1);
            float followingsteepness  = 1f * Time.deltaTime;
            while(!bullet.GetComponent<MoveForward>().hit){
                if(enemytohit == null){
                    print("null");
                    while(!bullet.GetComponent<MoveForward>().hit){
                        bullet.transform.Translate(distance * shotby.BulletSpeed * Time.deltaTime);
                        yield return null;
                    }
                    break;
                }
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
                bullet.transform.Translate(distance * shotby.BulletSpeed * speed);
                yield return new WaitForEndOfFrame();
            }
                
        }
        else{
            while(!bullet.GetComponent<MoveForward>().hit){
                bullet.transform.Translate(new Vector2(0, 1) * shotby.BulletSpeed * Time.deltaTime);
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
           if(counter.ContainsKey(enemy.gameObject)){
                damage = counter[enemy.gameObject] * 1.5f;
                counter[enemy.gameObject] = damage;
            }
            else{
                counter.Add(enemy.gameObject , damage);
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
    public GameObject GetClosestEnemy(GameObject obj){
        GameObject[] hitColliders = Physics2D.OverlapCircleAll(obj.transform.position, 20f).Select(value => value.gameObject).ToArray();
        GameObject closest = null; 
        foreach(GameObject hit in hitColliders){
            if(hit.tag  == "enemy"){
                if(closest == null){
                    closest = hit;
                    continue;
                }
                float distance = Vector3.Distance(hit.transform.position, obj.transform.position);
                if(distance < Vector3.Distance(closest.transform.position, obj.transform.position)){
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
