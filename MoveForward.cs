using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public Collider2D enemyhit;
    public bool hit;
    public Vector2 trajectory = new Vector2(0,1);
    public bool enableraycast;
    public bool bounce;
    public Collision2D collision;
    public Vector3 lastvelocity;

    private void OnCollisionEnter2D(Collision2D enemy)
    {
        this.collision = enemy;
        if(enemy.gameObject.tag != "Player"){
            hit = true;
            enemyhit = enemy.collider;
        }
        if(bounce){

            var speed = lastvelocity.magnitude;
            var direction = Vector3.Reflect(lastvelocity.normalized,enemy.contacts[0].normal);
            GetComponent<Rigidbody2D>().velocity = direction * Mathf.Max(speed, 0f);
            print("direction: "+ direction +", speed: " + speed);
        }

    }

    public void Update() {
        lastvelocity = GetComponent<Rigidbody2D>().velocity;
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

