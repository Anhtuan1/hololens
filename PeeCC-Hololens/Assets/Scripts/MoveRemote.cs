using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRemote : MonoBehaviour {

    public GameObject remoteui;
    // Use this for initialization
    void Start () {
        
    }

    public void OnMoveRemoteUI()
    {
        remoteui.AddComponent<LookAt>();
    }


    public void OnStopMoveRemoteUI()
    {
        Destroy(remoteui.GetComponent<LookAt>());

    }

    // Update is called once per frame
    void Update () {
		
	}
}
