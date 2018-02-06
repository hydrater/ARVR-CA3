using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {
    public GameObject Panel;
    public GameObject HTPPanel;
	// Use this for initialization
	void Start () {
        Panel = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void QuitButton ()
    {
        Application.Quit();
    }

    public void StartButton()
    {
        Panel.SetActive(false);
    }

    public void BackButton()
    {
        HTPPanel.SetActive(false);
    }

    public void HowToPlay()
    {
        HTPPanel.SetActive(true);
    }
}
