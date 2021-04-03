using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Transform cam;
    public Vector2 targetpos;
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        targetpos = player.transform.position;
        Vector2 cameraFollowPosition = targetpos;
        Vector2 currentcampos = new Vector2(cam.transform.position.x, cam.position.y);

        Vector2 cameraMoveDir = (cameraFollowPosition - currentcampos).normalized;
        float distance = Vector3.Distance(cameraFollowPosition, currentcampos);
        float cameraMoveSpeed = 2f;

        if (distance > 0.1f) {
            Vector2 newCameraPosition = currentcampos + cameraMoveDir * distance * cameraMoveSpeed * Time.deltaTime;

            float distanceAfterMoving = Vector3.Distance(newCameraPosition, cameraFollowPosition);

            if (distanceAfterMoving > distance) {
                newCameraPosition = cameraFollowPosition;
            }

            cam.transform.position = new Vector3(newCameraPosition.x, newCameraPosition.y, cam.transform.position.z);
        }
    }
}
