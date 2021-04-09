using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class SaveDataDungeon{
    public SaveDataDungeon left;
    public SaveDataDungeon right;
    public bool startroom;
    public bool? splithor;
    public int DebugID;
    public bool completed;
    public List<string> connections = new List<string>();
    public float[] roomspace = new float[4];
    public float[] room = new float[4];
    public float[] spawnarea = new float[4];
    public string pattern;
    
    public List<float[]> corridoors = new List<float[]>(); 
    public List<float[]> walls = new List<float[]>(); 
    public void MainStructure(Subdungeon dungeon){
        if(dungeon == null){
            return;
        }
        roomspace[0] = dungeon.roomspace.x;
        roomspace[1] = dungeon.roomspace.y;
        roomspace[2] = dungeon.roomspace.width;
        roomspace[3] = dungeon.roomspace.height;

        spawnarea[0] = dungeon.spawnarea.x;
        spawnarea[1] = dungeon.spawnarea.y;
        spawnarea[2] = dungeon.spawnarea.width;
        spawnarea[3] = dungeon.spawnarea.height;

        splithor = dungeon.splithor;

        DebugID = dungeon.DebugID;

        completed = dungeon.completed;
        room[0] = dungeon.room.x;
        room[1] = dungeon.room.y;
        room[2] = dungeon.room.width;
        room[3] = dungeon.room.height;
        
        startroom = dungeon.startroom;

        pattern = dungeon.pattern;

        connections = dungeon.connections; 

        foreach(Rect corridoor in dungeon.corridors){
            corridoors.Add(new float[] {corridoor.x,corridoor.y, corridoor.width,corridoor.height});
        }
        foreach(Rect wall in dungeon.walls){
            walls.Add(new float[] {wall.x,wall.y, wall.width,wall.height});
        }

        if(dungeon.left != null){
            left = new SaveDataDungeon(dungeon.left);
        }
        else{
            left = null;
        }
        if(dungeon.right != null){
            right = new SaveDataDungeon(dungeon.right);
        }
        else{
            right = null;
        }        
    }
    public SaveDataDungeon(Subdungeon dungeon){
        MainStructure(dungeon);
    }
}

[System.Serializable]
public class SaveDataPlayerData{
    public int health;
    public int armor;
    public List<string> owned = new List<string>();
    public List<string> equipped = new List<string>(new string[2]);
    public SaveDataPlayerData(PlayerData data){
        this.health = data.health;
        this.armor = data.armor;
        foreach(GunInfo gun in data.owned){
            owned.Add(gun.name);
        }
        foreach(GunInfo gun in data.equipped){
            equipped.Add(gun.name);
        }
    }
}

[System.Serializable]
public class SaveDataAll{
    public SaveDataDungeon dungeon;
    public SaveDataPlayerData playerData;

    public void initall(Subdungeon subdung, PlayerData data){
        if(subdung == null){
            dungeon = null;
        }
        else{
            dungeon = new SaveDataDungeon(subdung);
        }
        playerData = new SaveDataPlayerData(data);
    }
    public void initdung(Subdungeon subdung, SaveDataPlayerData data){
        dungeon = new SaveDataDungeon(subdung);
        playerData = data;
    }
    public void initplayerdata(SaveDataDungeon subdung, PlayerData data){
        dungeon = subdung;
        playerData = new SaveDataPlayerData(data);
    }
}




public class GameData{
    public Subdungeon dungeon;
    public PlayerData data;
    public GameData(Subdungeon dung, PlayerData data){
        if(dung == null){
            dungeon = null;
        }
        else{
            dungeon = dung;
        }
        if(data == null){
            this.data = null;
        }
        else{
            this.data = data;
        }
    }

}

public class PlayerData{
    public List<GunInfo> owned = new List<GunInfo>();  
    public List<GunInfo> equipped = new List<GunInfo>();
    public static List<string> startingguns = new List<string>() {"pistol", "doublepistol"};
    public int health;
    public int armor;

    public void createdefaultsetting(){
        armor = 0;
        health = 300;
        foreach(string gun in startingguns){
            GunInfo temp;
            GunInfo.Guns.TryGetValue(gun, out temp);

            owned.Add(temp);
            equipped.Add(temp);
        }
    }
}

public static class livegamedata{
    public static Subdungeon currentlevel;
    public static bool paused = false;

}