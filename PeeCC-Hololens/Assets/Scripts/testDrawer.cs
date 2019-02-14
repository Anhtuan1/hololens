using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testDrawer : MonoBehaviour {

    private Vector3 lastPos;
	// Use this for initialization
	void Start () {
        lastPos = this.transform.position;

	}
	
	// Update is called once per frame
	void Update () {
        //if (((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        //       || Input.GetMouseButton(0)))
        //{
            Vector3 newPos = this.transform.position;
        if (lastPos != newPos)
        {

            Plane objPlane = new Plane(Camera.main.transform.forward * -1, this.transform.position);
            Ray mRay = Camera.main.ScreenPointToRay(newPos);
            //Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            if (objPlane.Raycast(mRay, out rayDistance))
                this.transform.position = mRay.GetPoint(rayDistance);
            lastPos = newPos;
        };

        //}
	}
}
