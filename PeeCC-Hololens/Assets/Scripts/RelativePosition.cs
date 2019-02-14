using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativePosition : MonoBehaviour {
    public GameObject Gameobject;
    private Vector3 offset;
	// Use this for initialization
	void Start () {
        offset = transform.position - Gameobject.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = Gameobject.transform.position + offset;
	}
}
