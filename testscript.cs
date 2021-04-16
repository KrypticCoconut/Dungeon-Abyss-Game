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
    private void Update() {
        transform.Translate(new Vector2(0,.05f));
    }
}
