using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class enemyclass{
    public static Dictionary<string, enemyclass> allenemies = new Dictionary<string, enemyclass>();
    public static List<enemyclass> allenemieslist = new List<enemyclass>();
    public string name;
    public IEnumerable<int> depth;
    public GameObject enemyobj;
    public enemyclass(GameObject enemy, string name, IEnumerable<int> depth){

        this.enemyobj = enemy;
        this.name = name;
        this.depth = depth;
        allenemies.Add(name , this);
        allenemieslist.Add(this);
    }
    public static List<enemyclass> getenemiesindepth(int depth){
        List<enemyclass> returnl = new List<enemyclass>();
        foreach(enemyclass enemy in allenemieslist){
            if(enemy.depth.Contains(depth)){
                returnl.Add(enemy);
            }
        }
        return returnl;
    }
}