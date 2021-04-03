using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Generator : MonoBehaviour
{
    public GameObject blocker;
    public List<WayRooms> CreatedWayRooms = new List<WayRooms>();
    int currentWayRoom = 0;
    int node = 0;

    public void Generate(List<GameObject> CombatRoomTiles, List<GameObject> WayRoomTiles, List<GameObject> ConnectorsRoomTiles)
    {
        InitialTile(CombatRoomTiles, WayRoomTiles, ConnectorsRoomTiles);
    }

    void InitialTile(List<GameObject> CombatRoomTiles, List<GameObject> WayRoomTiles, List<GameObject> ConnectorsRoomTiles)
    {
        WayRooms startTile =  new WayRooms(Instantiate(WayRoomTiles[currentWayRoom], new Vector3(transform.position.x, transform.position.z, 1), transform.rotation));
        startTile.node = node;
        CreatedWayRooms.Add(startTile);

        int chancetospawn = 50;
        int LeftToSpawn = 2; //UnityEngine.Random.Range(2, WayRoomTiles[0].spawnCount + 1);
        foreach (int spawnpoint in Enumerable.Range(1, CreatedWayRooms[currentWayRoom].spawnCount))
        {
            if(CreatedWayRooms[currentWayRoom].spawnCount - (spawnpoint -1) == LeftToSpawn)
            {
                chancetospawn = 100;
            }
            bool SpawnBool = Chance(chancetospawn);
            if (SpawnBool)
            {
                GameObject spawnpointobject = CreatedWayRooms[currentWayRoom].spawns[spawnpoint - 1].gameObject;
                Quaternion blockerrot = spawnpointobject.transform.rotation;
                Vector3 blockerpos = MoveLocalposPosBy(spawnpointobject, new Vector3(0, ConnectorsRoomTiles[0].gameObject.transform.localScale.y /2, 0));
                GameObject Pathobj = Instantiate(ConnectorsRoomTiles[0], blockerpos, blockerrot);
                Pathobj.transform.parent = spawnpointobject.transform;
                CreatedWayRooms[currentWayRoom].SpawnsDecided.Add(new Connectors(Pathobj));

                LeftToSpawn--;
                if (LeftToSpawn == 0)
                    chancetospawn = 0;
            }
            else
            {
                GameObject spawnpointobject = CreatedWayRooms[currentWayRoom].spawns[spawnpoint-1].gameObject;
                Quaternion blockerrot = spawnpointobject.transform.rotation;
                Vector3 blockerpos = MoveLocalposPosBy(spawnpointobject, new Vector3(0, -blocker.transform.localScale.y/2, -blocker.transform.localScale.z / 2));
                print(blockerpos);
                GameObject blockerobj = Instantiate(blocker, blockerpos , blockerrot);
                blockerobj.transform.parent = spawnpointobject.transform;
            }
        }
        LoopedGeneration(CombatRoomTiles, WayRoomTiles, ConnectorsRoomTiles);
        //print(string.Join(" ", CreatedWayRooms[currentWayRoom].SpawnsDecided));
    }

    void LoopedGeneration(List<GameObject> CombatRoomTiles, List<GameObject> WayRoomTiles, List<GameObject> ConnectorsRoomTiles)
    {
        foreach(Connectors connector in CreatedWayRooms[currentWayRoom].SpawnsDecided)
        {
            GameObject pickedrandom = pickrandomroom(WayRoomTiles, CreatedWayRooms[currentWayRoom].node);
            print(MoveLocalposPosBy(connector.Prefab.gameObject, pickedrandom.transform.localScale / 2));
            //Instantiate(pickedrandom, MoveLocalposPosBy(connector.Prefab.gameObject, pickedrandom.transform.localScale /2), connector.spawn.transform.rotation);
        }
    }

    public bool Chance(float chance)
    {
        int random = UnityEngine.Random.Range(1, 101);
        if (random <= chance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    Vector3 MoveLocalposPosBy(GameObject localposobject, Vector3 moveby)
    {
        Vector3 localpos = localposobject.transform.InverseTransformDirection(localposobject.transform.position);
        Vector3 finalpos = localposobject.transform.TransformDirection(localpos + moveby);
        return finalpos;
    }

    GameObject pickrandomroom(List<GameObject> rooms, int node)
    {
        float decreasefactor = -15;
        float startingchance = 65 * UnityEngine.Mathf.Pow(decreasefactor,node);
        if(startingchance < 0)
        {
            startingchance = 0;
        }
        int count = rooms.Count;
        foreach (int i in Enumerable.Range(1, count))
        {
            if (i == count)
            {
                return rooms[i - 1];
            }
            bool spawned = Chance(startingchance);
            if (spawned)
            {
                return rooms[i - 1];
            }
        }
        print("conditions not satisfied.... for some reason");
        return rooms[count - 1];

    }
}
