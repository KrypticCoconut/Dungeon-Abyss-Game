using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelstarter : MonoBehaviour
{
    public GameObject corridoor;
    public GameObject floorTile;
    public GameObject BotTile;
    public GameObject TopTile;
    public GameObject RightTile;
    public GameObject LeftTile;
    public GameObject walltile;
    public Subdungeon dungeon;
    bool inited = false;
    // Start is called before the first frame update
    void Start()
    {
        dungeon = livegamedata.currentlevel;
    
        startlevel();
    }

    // Update is called once per frame
    public void Update() {
        if(inited){

        }
    }
    public void startlevel(){
        Subdungeon level = dungeon;
        Rect levelspace = new Rect(0f, 0f, level.room.width, level.room.height);
        DrawSingleRoom(levelspace);
        spawnobstacles(level.walls);
        GameObject.Find("Player").GetComponent<SpriteRenderer>().sortingOrder = 500;    
        GameObject.Find("Player").transform.position = levelspace.center;
        // StartCoroutine(GetComponent<completionchecker>().checker )
    }

    void DrawSingleRoom(Rect room){
            GameObject obj = new GameObject("level");
            GameObject wall;
			for (int i = (int)room.x; i < room.xMax; i++)
			{
				for (int j = (int)room.y; j < room.yMax; j++)
				{

                    if (i == room.xMin && j == room.yMin || i == room.xMax-1 && j == room.yMin || i == room.xMin && j == room.yMax-1 || i == room.xMax-1 && j == room.yMax-1 ){
                        
                    }
                    else if(i == room.xMax-1){
                        wall = Instantiate(RightTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                        wall.transform.SetParent(obj.transform);    
                    }
                    else if(j == room.yMax-1){
                        wall = Instantiate(TopTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                        wall.transform.SetParent(obj.transform);    
                    }
                    else if(i == room.x){
                        wall = Instantiate(LeftTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                        wall.transform.SetParent(obj.transform);    
                    }
                    else if(j ==  room.y){
                        wall = Instantiate(BotTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                        wall.transform.SetParent(obj.transform);    
                    }
                    else if (i != room.xMax-1 && j != room.yMax-1 && i != room.x && j !=  room.y){
                        wall = Instantiate(floorTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                        wall.transform.SetParent(obj.transform);
                    }
				}
			}
    }
    void spawnobstacles(List<Rect> walls){
        foreach(Rect wall in walls){
			for (int i = (int)wall.x; i <= wall.xMax; i++)
			{
				for (int j = (int)wall.y; j <= wall.yMax; j++)
				{
                    Instantiate(walltile, new Vector3(i, j, 0f), Quaternion.identity);
			    }
            }
        }
    }
    // List<GameObject> spawnenemies(Dictionary<Vector2, enemyclass> enemies){
        
    // }
}
