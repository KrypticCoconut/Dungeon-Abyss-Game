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

    }

    // Update is called once per frame
    GameData func1()
    {
        return new GameData(null, new PlayerData());
    }
}
