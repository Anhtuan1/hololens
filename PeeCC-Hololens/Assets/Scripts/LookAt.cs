using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {

    
    Vector3 startposition;
    Vector3 startRotate;
   
    void Start()
    {
        
        startposition = this.transform.position;
        

    }
    void Update()
    {
        // startRotate = target.transform.Rotate;
        //this.transform.position = new Vector3(target.position.x+ startposition.x, target.position.y+ startposition.y, target.position.z+ startposition.z);
        //this.transform.LookAt(mainCamera.transform);
        // this.transform.Rotate(target.Rotate);
        //this.transform.eulerAngles = new Vector3(mainCamera.transform.eulerAngles.x, mainCamera.transform.eulerAngles.y,180);
        Camera mainCamera = Camera.main;
        this.transform.position = mainCamera.ScreenToWorldPoint(new Vector3(500, 400, 3));
    }
}
