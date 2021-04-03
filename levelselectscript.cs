using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class levelselectscript : MonoBehaviour
{
    // Start is called before the first frame update
    public delegate void MainFunctionDelegate();
    public MainFunctionDelegate function = null;
    public Button button;
    private void Start() {
        button.onClick.AddListener(TaskOnClick);
    }
    public void TaskOnClick() {
        if(function != null){
            function();
        }   
    }
}
