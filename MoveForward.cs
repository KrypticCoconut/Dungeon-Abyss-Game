using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public Collider2D enemyhit;
    public bool hit;
    public Vector2 trajectory = new Vector2(0,1);
    public bool enableraycast;

    private void OnTriggerEnter2D(Collider2D enemy)
    {
        if(enemy.tag != "Player"){
            hit = true;
            enemyhit = enemy;
        }
    }
    public void Update() {
        RaycastHit2D raycast;
        if(enableraycast){
            raycast = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.down), 12f);
            if(raycast.collider != null && raycast.collider.gameObject.name != "Player"){
                hit = true;
                enemyhit = raycast.collider;
            }
        }
    }

}

