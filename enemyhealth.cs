using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyhealth : MonoBehaviour
{
    public float health = 100f;
    // Start is called before the first frame update
    private void Update()
    {   
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
