using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject StagePanel;

    private bool clicked = false;
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
        Debug.Log("Menu Click");
        if (clicked)
        {
            Debug.Log("Menu Click true");
            StagePanel.SetActive(false);
            clicked = false;
        }
        else
        {
            Debug.Log("Menu Click false");
            StagePanel.SetActive(true);
            clicked = true;
        }
    }
}
