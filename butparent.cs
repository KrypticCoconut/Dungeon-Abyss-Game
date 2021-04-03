using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class butparent : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3? targetpos = null;
    public GameObject cam;
    public float targetsize;
    public float sizeincspeed = 4;

    private void Start() {
        cam = gameObject;    
    }
    void Update() {
        if(targetpos != null){

            Vector3 cameraFollowPosition = (Vector3)targetpos;
            cameraFollowPosition.z = transform.position.z;

            Vector3 cameraMoveDir = (cameraFollowPosition - transform.position).normalized;
            float distance = Vector3.Distance(cameraFollowPosition, transform.position);
            float cameraMoveSpeed = 2f;

            if (distance > 0.1f) {
                Vector3 newCameraPosition = transform.position + cameraMoveDir * distance * cameraMoveSpeed * Time.deltaTime;

                float distanceAfterMoving = Vector3.Distance(newCameraPosition, cameraFollowPosition);

                if (distanceAfterMoving > distance) {
                    // Overshot the target
                    newCameraPosition = cameraFollowPosition;
                }

                transform.position = newCameraPosition;
            }
        }
        float currentsize = GetComponent<Camera>().orthographicSize;
        float sizdistance = (int)targetsize - (int) currentsize;
        if(sizdistance > .1 || sizdistance < -0.1){
            int direction = (int)new Vector2(0, targetsize - currentsize).normalized.y;
            float newpos = currentsize + (Mathf.Abs(sizeincspeed * sizdistance * Time.deltaTime) * direction);
            float distanceafterchange = newpos - currentsize;
            if((distanceafterchange > sizdistance && direction == 1) || (distanceafterchange < sizdistance && direction == -1)){
                newpos = targetsize;
            }
            GetComponent<Camera>().orthographicSize = newpos;
        }

    }

    public void UpdateTarget(Vector3 target) => this.targetpos = target;
    public void UpdateZoom(float size){
        float newpos = targetsize + size;
        if(newpos > 20){
            targetsize = newpos;
        }
    }
}
