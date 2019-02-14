﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;

public class ClickCall : MonoBehaviour, IInputClickHandler
{

    public Button button4;
public void OnInputClicked(InputClickedEventData eventData)
    {
        button4.GetComponent<Button>().onClick.Invoke();
        Debug.Log("CALL");
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
