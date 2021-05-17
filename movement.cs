using System.Collections;
using System.Collections.Generic;


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float speed = 10; //speed
    public Camera cam;
    public GameObject crosshair;
    public float angle;
    public float targetzoom;
    private Vector2 mousepos;
    private Rigidbody2D rb; //rigid body component to change rb stuff
    private Vector2 wasdinput; //wasd input
    bool held;
    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        mousepos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        crosshair = Instantiate(crosshair, mousepos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        wasdinput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        mousepos = cam.ScreenToWorldPoint(Input.mousePosition);
        crosshair.transform.position = mousepos;
        Vector2 Lookdir = mousepos - rb.position;
        angle = Mathf.Atan2(Lookdir.x * -1, Lookdir.y) * Mathf.Rad2Deg;
    }

    void FixedUpdate()
    {
        if(held){
            if(speed < 25){
                speed = speed + 3f * Time.deltaTime;
            }
            if(cam.orthographicSize < 20){
                cam.orthographicSize += 2 * Time.deltaTime;
            }
        }
        else{
            speed = 10;
            targetzoom = 10;
            if(cam.orthographicSize > targetzoom){
                cam.orthographicSize += -1 * Time.deltaTime;
            }
        }
        if(wasdinput.x != 0f || wasdinput.y != 0f){
            held = true;

        }
        else{
            held = false;
        }
        transform.Translate(transform.InverseTransformDirection(wasdinput) * speed * Time.deltaTime);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                                                                                                                                   
    }
}
