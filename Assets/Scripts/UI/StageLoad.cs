using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageLoad : MonoBehaviour
{
    public InputField input;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Click()
    {
        if (input.text == null || input.text.Trim(' ') == "")
        {
            Debug.Log("Click return");
            return;
        }
        GameObject obj = GameObject.Find("SceneControl");
        SceneControl sc = obj.GetComponent<SceneControl>();
        sc.loadStage(input.text.Trim(' '));
    }
}
