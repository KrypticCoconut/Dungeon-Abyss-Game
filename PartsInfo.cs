using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PartsInfo : MonoBehaviour
{
    public GameObject four_way_room;
    public GameObject three_way_room;
    public GameObject two_way_room;
    public GameObject large_combat_room;
    public GameObject medium_combat_room;
    public GameObject small_combat_room;
    public GameObject treasure_room;
    public GameObject path_connector;

    public List<GameObject> CombatRoomTiles = new List<GameObject>();
    public List<GameObject> WayRoomTiles = new List<GameObject>();
    public List<GameObject> ConnectorsRoomTiles = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        WayRoomTiles.Add(four_way_room);

        //print(string.Join(" ", fourwayroom.spawns));
        //Connectors straightconnector = new Connectors(path_connector);
        ConnectorsRoomTiles.Add(path_connector);
        GetComponent<Generator>().Generate(CombatRoomTiles, WayRoomTiles, ConnectorsRoomTiles);
    }


}
public class CombatRoom
{
    public GameObject Prefab;
    public Vector2 size;
    public CombatRoom(GameObject Prefab)
    {
        this.Prefab = Prefab;
        this.size = new Vector2(Prefab.transform.localScale.x, Prefab.transform.localScale.y);

    }
}

public class WayRooms
{
    public int node;
    public List<Connectors> SpawnsDecided = new List<Connectors>();
    public GameObject Prefab;
    public Vector2 size;
    public int spawnCount;
    public List<GameObject> spawns = new List<GameObject>();
    public WayRooms(GameObject Prefab)
    {
        this.Prefab = Prefab;
        this.spawnCount = Prefab.transform.childCount - 1;
        this.size = new Vector2(Prefab.transform.localScale.x, Prefab.transform.localScale.y);
        foreach(int spawn in Enumerable.Range(1, spawnCount))
        {
            spawns.Add(this.Prefab.transform.GetChild(spawn).gameObject);
        }
    }
}
public class Connectors
{
    public GameObject Prefab;
    public Vector2 size;
    public int spawnCount;
    public GameObject spawn;
    public Connectors(GameObject Prefab)
    {
        this.Prefab = Prefab;
        this.spawnCount = 1;
        this.size = new Vector2(Prefab.transform.localScale.x, Prefab.transform.localScale.y);
        this.spawn = Prefab.transform.GetChild(1).gameObject;
    }
}   
