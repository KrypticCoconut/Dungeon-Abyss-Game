using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunClasses : MonoBehaviour
{
    // Start is called before the first frame update
    public bool awake = true;
    public List<GunInfo> GunList;
    public GameObject single_shot_effect;
    public GameObject bullet;
    public int equipped;
    public static GunClasses oneinstance;
    // Start is called before the first frame update
    public void Awake()
    {
        if(oneinstance == null){
            oneinstance = this;
            // GunInfo pistol = new GunInfo("pistol",0, 40, 1, 20, 1, 10, 1, single_shot_effect, bullet, false, 0);
            // GunInfo doublepistol = new GunInfo("doublepistol",0, 40, 1, 20, 2, 10, 1, single_shot_effect, bullet, false, 0);
            // GunInfo revolver = new GunInfo("revolver",5, 70, 1, 80, 1, 40, 1, single_shot_effect, bullet, false, 0);
            // GunInfo rifle = new GunInfo("rifle",0, 60, .2f, 30, 1, 30, 1.5f, single_shot_effect, bullet, true, 0);
            // GunInfo machinegun = new GunInfo("machinegun",10, 40, .07f, 35, 1, 15, .5f, single_shot_effect, bullet, true, 0);
            // GunInfo sniper = new GunInfo("sniper",0, 200, 1.5f, 500, 1, 80, 3f, single_shot_effect, bullet, false, 500);
            // GunInfo shotgun = new GunInfo("shotgun",0, 60, .7f, 60, 7, 40, 1.5f, single_shot_effect, bullet, false, 0);
        }
        else{
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public class guns : MonoBehaviour
{
    public List<GunInfo> GunList;
    public GameObject single_shot_effect;
    public GameObject bullet;
    public int equipped;
    // Start is called before the first frame update
    public void Awake()
    {


        // GunInfo pistol = new GunInfo("pistol",0, 40, 1, 20, 1, 10, 1, single_shot_effect, bullet, false, 0);
        // GunInfo.Guns.Add(name, pistol);
        //GunInfo doublepistol = new GunInfo("doublepistol",0, 40, 1, 20, 2, 10, 1, single_shot_effect, bullet, false, 0);
        // GunInfo revolver = new GunInfo("revolver",5, 70, 1, 80, 1, 40, 1, single_shot_effect, bullet, false, 0);
        // GunInfo rifle = new GunInfo("rifle",0, 60, .2f, 30, 1, 30, 1.5f, single_shot_effect, bullet, true, 0);
        // GunInfo machinegun = new GunInfo("machinegun",10, 40, .07f, 35, 1, 15, .5f, single_shot_effect, bullet, true, 0);
        // GunInfo sniper = new GunInfo("sniper",0, 200, 1.5f, 500, 1, 80, 3f, single_shot_effect, bullet, false, 500);
        // GunInfo shotgun = new GunInfo("shotgun",0, 60, .7f, 60, 7, 40, 1.5f, single_shot_effect, bullet, false, 0);
    }
}
public class GunInfo
{
    public delegate void Shootfunc(); 
    public Shootfunc shootfunc;
    public string name;
    public static IDictionary<string, GunInfo> Guns = new Dictionary<string, GunInfo>();

    public float BulletSpread;
    public float BulletSpeed;
    public float FireRate;
    public float Damage;
    public float multishot;
    public float CC;
    public float CD;
    public float recoil;
    public GameObject HitEffect;
    public GameObject bullet;
    public bool AllowButtonHold;

    public GunInfo(string name, float BulletSpread, float BulletSpeed, float FireRate, float Damage, float multishot, float CC, float CD, GameObject HitEffect, GameObject bullet, bool AllowButtonHold, float recoil, Shootfunc function)
    {
        this.shootfunc = function;
        this.name = name;
        this.BulletSpread = BulletSpread;
        this.BulletSpeed = BulletSpeed;
        this.FireRate = FireRate;
        this.Damage = Damage;
        this.multishot = multishot;
        this.CC = CC;
        this.CD = CD;
        this.HitEffect = HitEffect;
        this.bullet = bullet;
        this.AllowButtonHold = AllowButtonHold;
        this.recoil = recoil;

    }
}

