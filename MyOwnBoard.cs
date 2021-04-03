using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
public class Subdungeon

{
    public List<Rect> walls = new List<Rect>();
    public GameObject roomobject;
    public bool? splithor;
    public List<string> connections = new List<string>();
    public Subdungeon left, right;
    public bool completed = false;
    public List<GameObject> corridorobjects = new List<GameObject>();
    public Rect roomspace;
    public Rect room;
    public Rect spawnarea;
    public bool Bossroom = false;
    public List<Rect> corridors = new List<Rect>();
    public int DebugID = 0;
    private static int debugCounter = 0;
    public static Subdungeon root;
    public string pattern;
    public string difficulty;
    public bool startroom = false;

    public Subdungeon(Rect mrect, string pattern)
    {
        this.pattern = pattern;
        roomspace = mrect;
        DebugID = debugCounter;
        debugCounter++;
    }

    public static Subdungeon GetConn(string patternt){
        Subdungeon returnvalue = root;
        foreach(char c in patternt){
            if(c == 'r'){
                returnvalue = returnvalue.right;
            }
            else if(c=='l'){
                returnvalue = returnvalue.left;
            }
        }
        return returnvalue;
    }
    public bool AmILeaf()
    {
        return left == null && right == null;
    }
    public void AddConnection(Subdungeon dungeon){
        connections.Add(dungeon.pattern);
        dungeon.connections.Add(this.pattern);
        dungeon.CreateCorridorBetween(this, dungeon);
    }
    public void CreateCorridorBetween(Subdungeon left, Subdungeon right) {

        Rect lroom = left.room;
        Rect rroom = right.room;


        // attach the corridor to a random point in each room
        Vector2 lpoint = new Vector2 ((int)Random.Range (lroom.x + 1, lroom.xMax - 1), (int)Random.Range (lroom.y + 1, lroom.yMax - 1));
        Vector2 rpoint = new Vector2 ((int)Random.Range (rroom.x + 1, rroom.xMax - 1), (int)Random.Range (rroom.y + 1, rroom.yMax - 1));

        // always be sure that left point is on the left to simplify the code
        if (lpoint.x > rpoint.x) {
            Vector2 temp = lpoint;
            lpoint = rpoint;
            rpoint = temp;
        }
            
        int w = (int)(lpoint.x - rpoint.x);
        int h = (int)(lpoint.y - rpoint.y);


        // if the points are not aligned horizontally
        if (w != 0) { 
            // choose at random to go horizontal then vertical or the opposite
            if (Random.Range (0, 1) > 2) {
                // add a corridor to the right
                left.corridors.Add (new Rect (lpoint.x, lpoint.y, Mathf.Abs (w) + 1, 1));

                // if left point is below right point go up
                // otherwise go down
                if (h < 0) { 
                    left.corridors.Add (new Rect (rpoint.x, lpoint.y, 1, Mathf.Abs (h)));
                } else {
                    left.corridors.Add (new Rect (rpoint.x, lpoint.y, 1, -Mathf.Abs (h)));
                }
            } else {
                // go up or down
                if (h < 0) {
                    left.corridors.Add (new Rect (lpoint.x, lpoint.y, 1, Mathf.Abs (h)));
                } else {
                    left.corridors.Add (new Rect (lpoint.x, rpoint.y, 1, Mathf.Abs (h)));
                }

                // then go right
                left.corridors.Add (new Rect (lpoint.x, rpoint.y, Mathf.Abs (w) + 1, 1));
            }
        } else {
            // if the points are aligned horizontally
            // go up or down depending on the positions
            if (h < 0) {
                left.corridors.Add (new Rect ((int)lpoint.x, (int)lpoint.y, 1, Mathf.Abs (h)));
            } else {
                left.corridors.Add (new Rect ((int)rpoint.x, (int)rpoint.y, 1, Mathf.Abs (h)));
            }
        } 
    }
    public List<Subdungeon> GetLeafs(){
        List<Subdungeon> leafs = new List<Subdungeon>(); 
        if(AmILeaf())
        {
            leafs.Add(this);
            return leafs;
        }
        if(left != null)
        {
            leafs.AddRange(left.GetLeafs());
        }


        if (right != null)
        {
            leafs.AddRange(right.GetLeafs());
        }

        return leafs;
    }

    public bool split(float minRoomSize, int roomsleft ){
        bool splithorizantal;   
        if (!AmILeaf()){
            splithor = null;
            return false;
        }

        if (roomspace.width / roomspace.height >= 1.25) //random tingz?
        {
            splithorizantal = false;
        }
        else if (roomspace.height / roomspace.width >= 1.25) //random tingz?
        {
            splithorizantal = true;
        }   
        else
        {
            splithorizantal = Random.Range(0.0f, 1.0f) > 0.5; // more random tingz?
        }
        splithor = splithorizantal;

        if (Mathf.Min(roomspace.height, roomspace.width) / 2 < minRoomSize + 8) //if room is too smoll it will be leaf
        {
            splithor = null ;
            throw new System.InvalidOperationException("space too small");
        }
        
        if (splithorizantal)
        {
            if((minRoomSize+8)*(roomsleft/2) > roomspace.width - (minRoomSize+8)*(roomsleft/2)){
                throw new System.InvalidOperationException("space too small");
            }
            int splitf = (int)Random.Range((minRoomSize+8)*(roomsleft/2), (roomspace.height - (minRoomSize+8)*(roomsleft/2))); //random split axis

            left = new Subdungeon(new Rect(roomspace.x, roomspace.y, roomspace.width, splitf), pattern + "l");
            right = new Subdungeon(
                new Rect(roomspace.x, roomspace.y + splitf, roomspace.width, roomspace.height - splitf), pattern + "r");

        }
        else
        {
            if((minRoomSize+8)*(roomsleft/2) > roomspace.width - (minRoomSize+8)*(roomsleft/2)){
                throw new System.InvalidOperationException("space too small");
            }
            int splitf = (int)Random.Range((minRoomSize+8)*(roomsleft/2), (roomspace.width - (minRoomSize+8)*(roomsleft/2)));

            left = new Subdungeon(new Rect(roomspace.x, roomspace.y, splitf, roomspace.height), pattern + "l");
            right = new Subdungeon(
                new Rect(roomspace.x + splitf, roomspace.y, roomspace.width - splitf, roomspace.height), pattern + "r");

        }

        return true;
    }
    public Subdungeon GetRoom() {
    if (AmILeaf()) {
        return this;
    }
    if (left != null) {
        Subdungeon lroom = left.GetRoom ();
        return lroom;
    }
    if (right != null) {
        Subdungeon rroom = right.GetRoom ();
        return rroom;
    }


    return null;
    // workaround non nullable structs
    }
    public Subdungeon GetRight() {
        if (AmILeaf()) {
            return this;
        }
        if (right != null) {
            Subdungeon rightroom = right.GetRight();
            return rightroom;
        }
        return null;
    }
    public Subdungeon GetLeft() {
        
        if (AmILeaf()) {
            return this;
        }
        if (left != null) {
            Subdungeon leftroom = left.GetLeft();
            return leftroom;
        }
        return null;
    }
    public void touching(Subdungeon dungeon){

        int xmin;
        int ymin;
        int xmax;
        int ymax;
        xmin = (int)roomspace.x;
        xmax = (int)roomspace.x + (int)roomspace.width;
        ymin = (int)roomspace.y;
        ymax = (int)roomspace.y + (int)roomspace.height;

        //top
        if((Enumerable.Range(xmin, xmax).Contains((int)dungeon.roomspace.x) 
        || Enumerable.Range(xmin, xmax).Contains((int)dungeon.roomspace.x + (int)dungeon.roomspace.width)) 
        && dungeon.roomspace.y == ymax){
            AddConnection(dungeon);
            return;
        }

        //bot
        if((Enumerable.Range(xmin, xmax).Contains((int)dungeon.roomspace.x) 
        || Enumerable.Range(xmin, xmax).Contains((int)dungeon.roomspace.x + (int)dungeon.roomspace.width)) 
        && dungeon.roomspace.yMax  == ymin){
            AddConnection(dungeon);
            return;
        }
        if((Enumerable.Range(ymin, ymax).Contains((int)dungeon.roomspace.y) 
        || Enumerable.Range(ymin, ymax).Contains((int)dungeon.roomspace.y + (int)dungeon.roomspace.height)) 
        && dungeon.roomspace.x  == xmax){
            AddConnection(dungeon);
            return;
        }
        if((Enumerable.Range(ymin, ymax).Contains((int)dungeon.roomspace.y) 
        || Enumerable.Range(ymin, ymax).Contains((int)dungeon.roomspace.y + (int)dungeon.roomspace.height)) 
        && dungeon.roomspace.xMax  == xmin){
            AddConnection(dungeon);
            return;
        }

    }
    public void CreateRoom(int MinRoomSiz){
        if(right != null){
            right.CreateRoom(MinRoomSiz);
        }
        if(left != null){
            left.CreateRoom(MinRoomSiz);
        }

        if (AmILeaf()){
            int roomWidth = (int)Random.Range(upperround(MinRoomSiz+2,(int)roomspace.width), roomspace.width);
            int roomHeight = (int)Random.Range(upperround(MinRoomSiz+2,(int)roomspace.height), roomspace.height);
            int roomX = (int)Random.Range(1, roomspace.width - roomWidth);
            int roomY = (int)Random.Range(1, roomspace.height - roomHeight);
 
            room = new Rect(roomspace.x + roomX, roomspace.y + roomY, roomWidth, roomHeight);
            spawnarea = new Rect(1,1,room.width-2, room.height-2);

        }
    }
    public int upperround(int MinRoomSiz, int MaxRoomSiz){
        if(MaxRoomSiz <= MinRoomSiz){
            return MinRoomSiz;
        }
        else if(MaxRoomSiz*(4/8) >= MinRoomSiz) {
            return Random.Range(MinRoomSiz, MaxRoomSiz*(2/4));
        }
        else{
            return Random.Range(MinRoomSiz, MaxRoomSiz);
        }
    }

}
public class MyOwnBoard : MonoBehaviour
{
    public int MinRoomSiz;
    public GameObject corridoor;
    public GameObject floorTile;
    public GameObject walltile;
    public GameObject BotTile;
    public GameObject TopTile;
    public GameObject RightTile;
    public GameObject LeftTile;
    public void CreateCorr(Subdungeon root){
        List<Subdungeon> leafs = root.GetLeafs();
        foreach(Subdungeon leaf in leafs){
            foreach(Subdungeon p in leafs){
                if(!leaf.connections.Contains(p.pattern)){
                    leaf.touching(p);
                }
            }
        }
    }
    public void MakeRandBoss(int roomnos, Subdungeon dungeon){
        List<Subdungeon> leafs = dungeon.GetLeafs();
        float chance = 0;
        float chanceinc = 100/(float)leafs.Count - roomnos;

        foreach(Subdungeon leaf in leafs){
            if(roomnos <= 0){
                break;
            }
            int per = Random.Range(0,100);
            if(per <= chance){
                leaf.Bossroom = true;
                roomnos += -1;
            }
            chance +=  chanceinc;
        }
    }
    public void DrawRooms(Subdungeon dungeon) {
        GameObject obj;
        GameObject floor = null;
        GameObject wall;
        if (dungeon.AmILeaf()){
            obj = new GameObject("room " +dungeon.DebugID);
			for (int i = (int)dungeon.room.x; i < dungeon.room.xMax; i++)
			{
				for (int j = (int)dungeon.room.y; j < dungeon.room.yMax; j++)
				{
                    floor = null;
                    wall  = null;
                    if (i == dungeon.room.xMin && j == dungeon.room.yMin || i == dungeon.room.xMax-1 && j == dungeon.room.yMin || i == dungeon.room.xMin && j == dungeon.room.yMax-1 || i == dungeon.room.xMax-1 && j == dungeon.room.yMax-1 ){
                        
                    }
                    else if(i == dungeon.room.xMax-1){
                        wall = Instantiate(RightTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                        wall.transform.SetParent(obj.transform);    
                    }
                    else if(j == dungeon.room.yMax-1){
                        wall = Instantiate(TopTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                        wall.transform.SetParent(obj.transform);    
                    }
                    else if(i == dungeon.room.x){
                        wall = Instantiate(LeftTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                        wall.transform.SetParent(obj.transform);    
                    }
                    else if(j ==  dungeon.room.y){
                        wall = Instantiate(BotTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                        wall.transform.SetParent(obj.transform);    
                    }
                    else if (i != dungeon.room.xMax-1 && j != dungeon.room.yMax-1 && i != dungeon.room.x && j !=  dungeon.room.y){
                        floor = Instantiate(floorTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                        floor.transform.SetParent(obj.transform);
                    }
				}
			}
            dungeon.roomobject = obj;
        }
        else{
            DrawRooms(dungeon.left);
            DrawRooms(dungeon.right);
        }
    }
    public void CreateObstacle(Subdungeon dungeon){
        if(dungeon.AmILeaf()){
            Rect room = dungeon.spawnarea;
            bool splith;
            if(room.width/room.height > 1.25f){
                splith = false;
            }
            else if(room.height/room.width > 1.25f){
                splith = true;
            }
            else{
                splith = Random.Range(1,3) == 2;
            }

            int wallrange = (int)Random.Range(3, 6);

            List<Vector2> points = new List<Vector2>();
            if(splith){
                int split = (int)Mathf.Floor(room.height/(wallrange*3));
                foreach(int temp in Enumerable.Range(1,split)){
                    int end = temp * (wallrange*3);
                    int start = ((temp-1)* (wallrange*3)) + 1;
                    Rect range = new Rect(1+wallrange, start + wallrange, room.width-(wallrange*2),wallrange);
                    Vector2 randompoint = new Vector3(Random.Range(range.x, range.xMax), Random.Range(range.y, range.yMax));
                    points.Add(new Vector2(randompoint.x, randompoint.y));
                    
                }
            }
            else{
                int split = (int)Mathf.Floor(room.width/(  wallrange*3));
                foreach(int temp in Enumerable.Range(1,split)){
                    int end = temp * (wallrange*3);
                    int start = ((temp-1)* (wallrange*3)) + 1;

                    Rect range = new Rect(start+wallrange, 1+wallrange + wallrange, wallrange,room.height-(wallrange*2));
                    Vector2 randompoint = new Vector3(Random.Range(range.x, range.xMax), Random.Range(range.y, range.yMax));
                    points.Add(new Vector2(randompoint.x, randompoint.y));
                    
                }
            }

            foreach(Vector2 point in points){
                int direction = (int)Random.Range(1,5);
                if(direction == 1){
                    dungeon.walls.Add(new Rect (point.x, point.y, 0, wallrange));
                }
                if(direction == 2){
                    dungeon.walls.Add(new Rect (point.x, (point.y - wallrange) +1, 0, wallrange));
                }
                if(direction == 3){
                    dungeon.walls.Add(new Rect (point.x, point.y, wallrange, 0));
                }
                if(direction == 4){
                    dungeon.walls.Add(new Rect ((point.x - wallrange) +1, point.y, wallrange, 0));
                }
            }
        }
        else{
            CreateObstacle(dungeon.left);
            CreateObstacle(dungeon.right);
        }
    }

    public void CreateBSP(Subdungeon dungeon, int roomsleft)
    {
        if(dungeon.split((int)MinRoomSiz,roomsleft)){
            roomsleft = roomsleft/2;
            if(roomsleft != 1){
                CreateBSP(dungeon.left, roomsleft);
                CreateBSP(dungeon.right, roomsleft);
            }  
        }
        else {
            throw new System.InvalidOperationException("space too small");
        }

    }
    public void DrawCorridors(Subdungeon subDungeon) {
        if (subDungeon == null) {
        return;
        }
        
        DrawCorridors (subDungeon.left);
        DrawCorridors (subDungeon.right);
        GameObject obj; 
        foreach (Rect corridor in subDungeon.corridors) {
        obj = new GameObject("corridoor");
        for (int i = (int)corridor.x; i < corridor.xMax; i++) {
            for (int j = (int)corridor.y; j < corridor.yMax; j++) {
                GameObject instance = Instantiate (corridoor, new Vector3 (i, j, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent (obj.transform);
            }
        }
        subDungeon.corridorobjects.Add(obj);
        }
    }
    public void MakeDungeon(int Bossrooms, int spaceX, int spaceY, int MinRoomSize, int nodes)
    {

        MinRoomSiz = MinRoomSize;
        Subdungeon dungeon = new Subdungeon(new Rect(0, 0, spaceX, spaceY),"");
        Subdungeon.root = dungeon;
        CreateBSP(dungeon, nodes);
        dungeon.CreateRoom(MinRoomSiz);
        MakeRandBoss(Bossrooms,dungeon);
        dungeon.GetLeft().startroom = true;
        CreateCorr(dungeon);
        CreateObstacle(dungeon);

        GameData currentdata = SaveSystem.LoadSave();
        currentdata.dungeon = dungeon;
        SaveSystem.SaveAll(currentdata.dungeon, currentdata.data);
    }

}
