using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class notification : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Vector3 minpos = new Vector3(0,-600,0);
    public Vector3 maxpos = new Vector3(0,-474,0);
    public string queue = "";
    public bool moving;
    public void Start() {
        minpos = new Vector3(0,-600,0);
        maxpos = new Vector3(0,-474,0);
        minpos = text.transform.parent.TransformPoint(minpos);
        maxpos = text.transform.parent.TransformPoint(maxpos);


    }
    // Start is called before the first frame update
    public IEnumerator NotifyHandler(string message)
    {
        while(queue != ""){
            yield return null;
        }
        text.text = message;
        queue = message;
        StartCoroutine(GoTo(maxpos));
        while(moving == true){
            yield return null;
        }
        StartCoroutine(GoTo(minpos));
        while(moving == true){
            yield return null;
        }
        queue = "";
    }



    // Update is called once per frame
    IEnumerator GoTo(Vector3 targetpos)
    {
        moving =true;
        Vector3 currentpos = text.transform.position;
        Vector3 cameraMoveDir = (targetpos -  text.transform.position).normalized;

    
        float distance = Vector3.Distance(targetpos, text.transform.position);
        float cameraMoveSpeed = 2f;

        while(distance > 0.1f) {
            distance = Vector3.Distance(targetpos, text.transform.position);
            Vector3 newCameraPosition = text.transform.position + cameraMoveDir * distance * cameraMoveSpeed * Time.deltaTime;

            float distanceAfterMoving = Vector3.Distance(newCameraPosition, targetpos);

            if (distanceAfterMoving > distance) {
                newCameraPosition = targetpos;
            }

            text.transform.position = newCameraPosition;
            yield return null;
        }
        moving = false;
    }
}
