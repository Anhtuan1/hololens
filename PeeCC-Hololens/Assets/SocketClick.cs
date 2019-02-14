using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class SocketClick : MonoBehaviour, IInputClickHandler
{
    public GameObject SocketObject;
    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (SocketObject != null)
        {
            SocketInit socketComponent = SocketObject.GetComponent<SocketInit>();
            socketComponent.OnStart();
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
