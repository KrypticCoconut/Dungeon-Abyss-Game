using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scroller : MonoBehaviour
{
    public GameObject camera;
    public int moveamount = 30;
    public int edgesize = 20;
    public Rect bounds;
    private void Update() {
        if(livegamedata.paused){
            return;
        }
        if(Input.mousePosition.x > Screen.width - edgesize && camera.transform.position.x <= bounds.xMax){
            camera.GetComponent<butparent>().UpdateTarget(new Vector3(camera.transform.position.x+moveamount, camera.transform.position.y, camera.transform.position.z));
        }
        if(Input.mousePosition.x < edgesize && camera.transform.position.x >= bounds.xMin ){
            camera.GetComponent<butparent>().UpdateTarget(new Vector3(camera.transform.position.x-moveamount, camera.transform.position.y, camera.transform.position.z));
        }
        if(Input.mousePosition.y > Screen.height -edgesize && camera.transform.position.y <= bounds.yMax){
            camera.GetComponent<butparent>().UpdateTarget(new Vector3(camera.transform.position.x, camera.transform.position.y+moveamount, camera.transform.position.z));
        }
        if(Input.mousePosition.y < edgesize && camera.transform.position.y >= bounds.yMin){
            camera.GetComponent<butparent>().UpdateTarget(new Vector3(camera.transform.position.x, camera.transform.position.y-moveamount, camera.transform.position.z));
        }
        if(Input.mouseScrollDelta.y > .1){
            camera.GetComponent<butparent>().UpdateZoom(Input.mouseScrollDelta.y);
        }
        else if(Input.mouseScrollDelta.y < -0.1){
            camera.GetComponent<butparent>().UpdateZoom(Input.mouseScrollDelta.y);
        }
    }
}
