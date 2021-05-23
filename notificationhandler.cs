using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class notificationhandler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject notificationobj;
    public GameObject notificationobjcopy;

    void Start()
    {
        SceneManager.activeSceneChanged += OnSceneChange;

        GameObject canvas = GameObject.Find("Canvas");
        if(canvas){
            notificationobjcopy = Instantiate(notificationobj);
            notificationobjcopy.transform.parent = canvas.transform;
            notificationobjcopy.transform.position = notificationobjcopy.transform.parent.TransformPoint(0,-612,0);
        }
    }

    // Update is called once per frame
    void Update()
    {
           
    }
    public void OnSceneChange(Scene current, Scene next){
        GameObject canvas = GameObject.Find("Canvas");
        if(canvas){
            notificationobjcopy = Instantiate(notificationobj);
            notificationobjcopy.transform.parent = canvas.transform;
            notificationobjcopy.transform.position = notificationobjcopy.transform.parent.TransformPoint(0,-612,0);
        }

    }
}
