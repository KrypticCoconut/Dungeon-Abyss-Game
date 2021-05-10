using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class gunselect : MonoBehaviour
{   
    Animator animator;
    GameObject musicplayer;    
    public string gunname;
    public TextMeshProUGUI name;
    public GameObject buybutton;
    public TextMeshProUGUI info;
    // public GameObject CC;
    // public GameObject CD;
    // public GameObject Damage;
    // public GameObject FireRate;
    public GameObject badge1;
    public GameObject badge2;
    public GameObject switchguns;
    GameObject Setgun1;
    GameObject Setgun2; 
    public GunInfo gun;
    bool done = false;
    public TextMeshProUGUI price;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        musicplayer = GameObject.Find("MusicPlayer");    
        animator.SetInteger("mode", 2);

        GunInfo.Guns.TryGetValue(gunname, out gun);
        transform.GetChild(0).GetComponent<Image>().sprite =  gun.uiInfo.icon;
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = gun.name;
        if(gun.name == livegamedata.currentdata.equipped[0].name){
            this.switchguns.GetComponent<switchguns>().gun1 = gameObject;
            badge1.SetActive(true);
            stopclick();
        }
        if(gun.name == livegamedata.currentdata.equipped[1].name){
            this.switchguns.GetComponent<switchguns>().gun2= gameObject;
            badge2.SetActive(true);
        }
        done = true;

    }

    // Update is called once per frame
    public void enter(){  
        if(!done){
            return;
        }
        animator.SetBool("selected", true);

    }
    public void exit(){
        if(!done){
            return;
        }
        animator.SetBool("selected", false);
    }
    public void click(){
        if(!done){
            return;
        }
        animator.SetBool("pressed", true);
        Invoke("stopclick", animator.GetCurrentAnimatorStateInfo(0).speed * animator.GetCurrentAnimatorStateInfo(0).length);
    }
    public void stopclick(){
        this.switchguns.GetComponent<switchguns>().currentgun = gameObject;
        Setgun1 = this.switchguns.GetComponent<switchguns>().changewithGun1;
        Setgun2 = this.switchguns.GetComponent<switchguns>().changewithGun2;
        animator.SetBool ("pressed", false);
        animator.SetBool ("selected", false);
        price.text = "price: " + gun.money + "  <sprite=0>";
        buybutton.GetComponent<buygun>().name = gun.name;
        buybutton.GetComponent<buygun>().gunobj = gameObject;
        if(!livegamedata.currentdata.owned.Contains(gun)){
            price.gameObject.SetActive(true);
            buybutton.SetActive(true);

            if(!(livegamedata.currentdata.coins >= gun.money)){
                buybutton.GetComponent<Button>().enabled = false;
                buybutton.GetComponent<Image>().color = new Color(1,1,1,0.5f);
                price.GetComponent<TextMeshProUGUI>().color = new Color(1,1,1,0.5f);
            }
            else{
                buybutton.GetComponent<Image>().color = new Color(1,1,1,1f);
                price.GetComponent<TextMeshProUGUI>().color = new Color(1,1,1,1f);
            }
        }
        else{
            price.gameObject.SetActive(false);
            buybutton.SetActive(false);
            Setgun1.GetComponent<Button>().enabled = true;
            Setgun1.GetComponent<Image>().color = new Color(1,1,1,1f);
            Setgun2.GetComponent<Button>().enabled = true;
            Setgun2.GetComponent<Image>().color = new Color(1,1,1,1f);
            if(gun == livegamedata.currentdata.equipped[0]){
                Setgun1.GetComponent<Button>().enabled = false;
                Setgun1.GetComponent<Image>().color = new Color(1,1,1,0.5f);
            }
            if(gun == livegamedata.currentdata.equipped[1]){
                Setgun2.GetComponent<Button>().enabled = false;
                Setgun2.GetComponent<Image>().color = new Color(1,1,1,0.5f);
            }
        }   
        name.text = gun.name;
        string info = "";
        info += "Damage: " + gun.Damage + "\n";
        info += "FireRate: " + gun.FireRate + "\n";
        info += "Crit Chance: " + gun.CC + "\n";
        info += "Crit Damage: " + gun.CD + "\n";
        this.info.text = info;
    }
    public void PlaySound(AudioClip clip){
        GameObject.Find("MusicPlayer").GetComponent<MusicPlayer>().PlayMusic(clip);
    }
}
