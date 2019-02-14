using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class textScript : MonoBehaviour {

    public GameObject cursor;
    public GameObject drawItem;
    public TextMesh textMesh;
    // Use this for initialization

    public void drawTest()
    {
        textMesh.text = "start";
        var objects = GameObject.FindGameObjectsWithTag("pen");
        Debug.Log("object length: " + objects.Length);
        if (objects.Length == 0)
        {
            try
            {
                appendPen();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex, this);
            }            
        }
        else
        {
            try
            {
                destroyPen();
                appendPen();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex, this);
            }
        }
    }

    public void endDrawTest()
    {
        textMesh.text = "end";
        try
        {
            destroyPen();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex, this);
        }
    }

    private void appendPen()
    {
        Vector3 p = new Vector3(cursor.transform.position.x, cursor.transform.position.y, cursor.transform.position.z);
        Quaternion quater = new Quaternion();
        quater.Set(75, 75, 75, 1);
        Instantiate(drawItem, p, quater);
    }

    private void destroyPen()
    {
        var objects = GameObject.FindGameObjectsWithTag("pen");
        foreach (var obj in objects)
        {
            Destroy(obj);
        }
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
