﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class ClickSofa : MonoBehaviour, IInputClickHandler
{

    Material boxMaterial;

    public Material lineMat;

    public GameObject mainPoint;


    public void OnInputClicked(InputClickedEventData eventData)
    {
        //GameObject MyObject = this.gameObject;
        GameObject go = this.gameObject;
        Destroy(go.GetComponent<LookAt>());
        //go.transform.position = new Vector3(0, -0.75f, 2);
        //go.transform.localScale *= 2f;
        go.AddComponent<ClickCreateBox>();
        //go.AddComponent<TapToPlace>();
        go.GetComponent<ClickCreateBox>().parent_box = go;
        go.AddComponent<BoxCollider>();
        go.AddComponent<DragHand>();
        Destroy(go.GetComponent<ClickSofa>());
        
        float gap = 0.02f;
        var parentObj = new GameObject();
        parentObj.transform.position = go.transform.position;
        parentObj.name = "Parent Temporary Object";
        go.transform.SetParent(parentObj.transform);

        float changerotate = (go.transform.rotation.eulerAngles.y - 180) * (go.transform.localScale.y);
        

        Vector3 center = go.GetComponent<Collider>().bounds.center;
        Vector3 size = go.GetComponent<Collider>().bounds.size;

        

        GameObject meshA1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        meshA1.name = "Outer Box A";

        meshA1.transform.position = new Vector3(center.x - (size.x) / 2 - gap / 2, center.y - (size.y) / 2 - gap / 2, center.z - (size.z) / 2 - gap / 2);
        meshA1.transform.localScale = new Vector3(0, 0, 0);

        meshA1.GetComponent<Renderer>().material = boxMaterial;
        Destroy(meshA1.GetComponent<BoxCollider>());              //Remove Box Collider from the outer box //obstructing when click
        meshA1.transform.parent = go.transform;

    }

    private void deleteOb(GameObject obj)
    {

    }

    

    private void OnDisplayDetected(MixedRealityCameraManager.DisplayType displayType)
    {
        

    }
    void Scale(bool up = true)
    {
        this.gameObject.transform.localScale *= up ? 1.15f : 0.75f;

    }

}
