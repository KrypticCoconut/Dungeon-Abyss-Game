using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public static class SaveSystem{
    public static void SaveAll(Subdungeon dungeon, PlayerData data){
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/player.save";
        FileStream stream = new FileStream(path, FileMode.Create);
        SaveDataAll save = new SaveDataAll();
        save.initall(dungeon, data); 
        formatter.Serialize(stream, save);
        stream.Close();


    }
    // public static void SaveDungeon(Subdungeon dungeon){
    //     BinaryFormatter formatter = new BinaryFormatter();
    //     string path = Application.persistentDataPath + "/player.save";
    //     FileStream stream = new FileStream(path, FileMode.Create);

    //     SaveDataAll save = new SaveDataAll();
    //     SaveDataPlayerData data = (formatter.Deserialize(stream) as SaveDataAll).playerData;
    //     save.initdung(dungeon, data); 
    //     formatter.Serialize(stream, save);
    //     stream.Close();
    // }
    // public static void SavePlayerData(PlayerData data){
    //     BinaryFormatter formatter = new BinaryFormatter();
    //     string path = Application.persistentDataPath + "/player.save";
    //     FileStream stream = new FileStream(path, FileMode.Create);

    //     SaveDataAll save = new SaveDataAll();
    //     SaveDataDungeon dungeon = (formatter.Deserialize(stream) as SaveDataAll).dungeon;
    //     save.initplayerdata(dungeon, data); 
    //     formatter.Serialize(stream, save);
    //     stream.Close();
    // }


    public static GameData LoadSave(){
        string path = Application.persistentDataPath + "/player.save";
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveDataAll data =  formatter.Deserialize(stream) as SaveDataAll;

            if(data.playerData == null){
                SaveDataAll temp = new SaveDataAll();
                PlayerData playerdata = new PlayerData();
                playerdata.createdefaultsetting();
                temp.playerData = new SaveDataPlayerData(playerdata);
                data.playerData = temp.playerData;
                temp.dungeon = data.dungeon;
                formatter.Serialize(stream, data);
            }
            if(data.dungeon == null){
                PlayerData playerData = ConvPlayerData(data.playerData);
                stream.Close();
                return new GameData(null, playerData);
            }
            stream.Close();
            return ConvSaveData(data);
        }
        else{
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);
            SaveDataAll data =  new SaveDataAll();
            PlayerData playerdata = new PlayerData();
            playerdata.createdefaultsetting();
            data.initall(null, playerdata);
            formatter.Serialize(stream, data);
            stream.Close();
            return new GameData(null, playerdata);
        }
    }
    


    public static GameData ConvSaveData(SaveDataAll data){

        Subdungeon dungeon = ConvDungSaveData(data.dungeon);
        PlayerData playerdata = ConvPlayerData(data.playerData);
        Subdungeon.root = dungeon;
        GameData GameData =  new GameData(dungeon, playerdata);
        return GameData;
    }   

    public static PlayerData ConvPlayerData(SaveDataPlayerData data){
        PlayerData playerdata = new PlayerData();
        playerdata.armor = data.armor;
        playerdata.health = data.health;
        foreach(int gunindex in data.owned){
            playerdata.owned.Add(GunInfo.allguns[gunindex]);
        }
        foreach(int equippedindex in data.equipped){
            
            playerdata.equipped.Add(playerdata.owned[equippedindex]);
        }
        return playerdata;
    }
    public static Subdungeon ConvDungSaveData(SaveDataDungeon data){
        if(data == null){
            return null;
        }
        Rect roomz = new Rect(data.room[0], data.room[1], data.room[2], data.room[3]);
        Rect spawnareaz = new Rect(data.spawnarea[0], data.spawnarea[1], data.spawnarea[2], data.spawnarea[3]);
        Rect roomspacez = new Rect(data.roomspace[0], data.roomspace[1], data.roomspace[2], data.roomspace[3]);
        

        List<Rect> corridors = new List<Rect>(); 
        foreach(float[] corridoor in data.corridoors){
            corridors.Add(new Rect(corridoor[0], corridoor[1], corridoor[2], corridoor[3]));
        }

        List<Rect> walls = new List<Rect>(); 
        foreach(float[] wall in data.walls){
            walls.Add(new Rect(wall[0], wall[1], wall[2], wall[3]));
        }
        Subdungeon dungeon = new Subdungeon(roomspacez, data.pattern);
        dungeon.room = roomz;
        dungeon.spawnarea = spawnareaz;
        dungeon.startroom = data.startroom;

        dungeon.corridors = corridors;
        dungeon.walls = walls;
        dungeon.pattern = data.pattern;
        dungeon.completed = data.completed;

        dungeon.connections = data.connections;
        dungeon.splithor = data.splithor;
        dungeon.DebugID = data.DebugID;
        
        if(data.left != null){
            dungeon.left = ConvDungSaveData(data.left);
        }
        else{
            dungeon.left = null;
        }
        if(data.right != null){
            dungeon.right = ConvDungSaveData(data.right);
        }
        else{
            dungeon.right = null;
        }
        


        return dungeon;
    }
    public static void MakeNewSave(){

    }
}
