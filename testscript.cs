using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class testscript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
{   
        string path = Application.persistentDataPath + "/player.save";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        SaveDataAll data =  new SaveDataAll();
        PlayerData playerdata = new PlayerData();
        playerdata.createdefaultsetting();
        data.initall(null, playerdata);
        formatter.Serialize(stream, data);
        stream = new FileStream(path, FileMode.Open);
        data =  formatter.Deserialize(stream) as SaveDataAll;
        print(data.dungeon);
    }

    // Update is called once per frame
    GameData func1()
    {
        return new GameData(null, new PlayerData());
    }
}
