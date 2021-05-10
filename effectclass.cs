using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effect
{
    public static IDictionary<string, effect> effects = new Dictionary<string, effect>();
    public string cost;
    public string name;
    public string type;
    public int tiers;
    public delegate void Effectfunc(int tier, GameObject theplayer ); 
    public Effectfunc shootfunc;
    public effect(string name, string cost, int tiers, Effectfunc func, string type){
        this.name = name;
        this.name = name;
        this.tiers = tiers;
        this.shootfunc = func;
        this.type = type;
        effects.Add(name, this);

    }
}
