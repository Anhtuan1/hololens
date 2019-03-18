using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class ClickCreateBoxImage : MonoBehaviour, IInputClickHandler
{
    public GameObject parent_box = null;
    private GameObject get_child = null;
    Material boxMaterial;
    public int click_count = 0;
    public GameObject[] bbox;
    public float sec = 14f;


    public void OnInputClicked(InputClickedEventData eventData)
    {
        Debug.Log(eventData.InputSource.ToString());
        Destroy(this.GetComponent<ClickImageAction>());
        
        if (parent_box != null)
        {
            
            if (click_count == 0)
            {
                CreateBox(parent_box);
            }
            else
            {

                DestroyBox(parent_box);

            }
            click_count = 1;            

        }

    }

    private void DestroyBox(GameObject obj)
    {
        if (bbox != null && bbox.Length != 0)
        {
            for (int i = 0; i < bbox.Length; i++)
            {
                bbox[i].SetActive(true);
            }
            StartCoroutine(LateCall(obj));
        }

    }

    IEnumerator LateCall(GameObject obj)
    {

        yield return new WaitForSeconds(sec);

        for (int i = 0; i < bbox.Length; i++)
        {
            bbox[i].SetActive(false);
        }

    }

    private void ShowBox(GameObject obj)
    {
        if (bbox != null && bbox.Length != 0)
        {
            for (int i = 0; i < bbox.Length; i++)
            {
                bbox[i].SetActive(true);
            }
        }

    }

    private void CreateBox(GameObject obj)
    {
        float gap = 0.006f;
        float gap2 = 0.006f;
        float gap3 = 0.002f;
        var parentObj = new GameObject();
        parentObj.transform.position = obj.transform.position;
        parentObj.name = "Parent Temporary Object";
        obj.transform.SetParent(parentObj.transform);

        float changerotate = (obj.transform.rotation.eulerAngles.y - 180) * (obj.transform.localScale.y);
        Debug.Log(changerotate);

        Vector3 center = obj.GetComponent<Collider>().bounds.center;
        Vector3 size = obj.GetComponent<Collider>().bounds.size;

       





        GameObject meshA = GameObject.CreatePrimitive(PrimitiveType.Cube);
        meshA.name = "Outer Box A";

        meshA.transform.position = new Vector3(center.x - (size.x) / 2 - gap / 2, center.y - (size.y) / 2 - gap / 2, center.z - (size.z) / 2 - gap / 2);
        meshA.transform.localScale = new Vector3(gap, gap2, gap3);

        meshA.GetComponent<Renderer>().material = boxMaterial;
		//Remove Box Collider from the outer box //obstructing when click
        Destroy(meshA.GetComponent<BoxCollider>());              
        meshA.transform.parent = obj.transform;
        meshA.AddComponent<BoxCollider>();
        meshA.AddComponent<ClickButtonResizeBox>();

        

        GameObject meshB = GameObject.CreatePrimitive(PrimitiveType.Cube);
        meshB.name = "Outer Box B";


        meshB.transform.position = new Vector3(center.x + (size.x) / 2 + gap / 2, center.y - (size.y) / 2 - gap / 2, center.z - (size.z) / 2 - gap / 2);
        meshB.transform.localScale = new Vector3(gap, gap2, gap3);

        meshB.GetComponent<Renderer>().material = boxMaterial;
		//Remove Box Collider from the outer box //obstructing when click
        Destroy(meshB.GetComponent<BoxCollider>());              
        meshB.transform.parent = obj.transform;
        meshB.AddComponent<BoxCollider>();
        meshB.AddComponent<ClickButtonResizeBox>();        


        GameObject meshC = GameObject.CreatePrimitive(PrimitiveType.Cube);
        meshC.name = "Outer Box C";

        meshC.transform.position = new Vector3(center.x + (size.x) / 2 + gap / 2, center.y - (size.y) / 2 - gap / 2, center.z + (size.z) / 2 + gap / 2);
        meshC.transform.localScale = new Vector3(gap, gap2, gap3);

        meshC.GetComponent<Renderer>().material = boxMaterial;
		//Remove Box Collider from the outer box //obstructing when click
        Destroy(meshC.GetComponent<BoxCollider>());              
        meshC.transform.parent = obj.transform;
        meshC.AddComponent<BoxCollider>();
        meshC.AddComponent<ClickButtonResizeBox>();
        

        GameObject meshD = GameObject.CreatePrimitive(PrimitiveType.Cube);
        meshD.name = "Outer Box D";

        meshD.transform.position = new Vector3(center.x - (size.x) / 2 - gap / 2, center.y - (size.y) / 2 - gap / 2, center.z + (size.z) / 2 + gap / 2);
        meshD.transform.localScale = new Vector3(gap, gap2, gap3);

        meshD.GetComponent<Renderer>().material = boxMaterial;
		//Remove Box Collider from the outer box //obstructing when click
        Destroy(meshD.GetComponent<BoxCollider>());              
        meshD.transform.parent = obj.transform;
        meshD.AddComponent<BoxCollider>();
        meshD.AddComponent<ClickButtonResizeBox>();

        GameObject meshE = GameObject.CreatePrimitive(PrimitiveType.Cube);
        meshE.name = "Outer Box E";

        meshE.transform.position = new Vector3(center.x - (size.x) / 2 - gap / 2, center.y + (size.y) / 2 + gap / 2, center.z - (size.z) / 2 - gap / 2);
        meshE.transform.localScale = new Vector3(gap, gap2, gap3);

        meshE.GetComponent<Renderer>().material = boxMaterial;
		//Remove Box Collider from the outer box //obstructing when click
        Destroy(meshE.GetComponent<BoxCollider>());              
        meshE.transform.parent = obj.transform;
        meshE.AddComponent<BoxCollider>();
        meshE.AddComponent<ClickButtonResizeBox>();

        GameObject meshF = GameObject.CreatePrimitive(PrimitiveType.Cube);
        meshF.name = "Outer Box F";

        meshF.transform.position = new Vector3(center.x + (size.x) / 2 + gap / 2, center.y + (size.y) / 2 + gap / 2, center.z - (size.z) / 2 - gap / 2);
        meshF.transform.localScale = new Vector3(gap, gap2, gap3);

        meshF.GetComponent<Renderer>().material = boxMaterial;
		//Remove Box Collider from the outer box //obstructing when click
        Destroy(meshF.GetComponent<BoxCollider>());              
        meshF.transform.parent = obj.transform;
        meshF.AddComponent<BoxCollider>();
        meshF.AddComponent<ClickButtonResizeBox>();


        GameObject meshG = GameObject.CreatePrimitive(PrimitiveType.Cube);
        meshG.name = "Outer Box F";

        meshG.transform.position = new Vector3(center.x + (size.x) / 2 + gap / 2, center.y + (size.y) / 2 + gap / 2, center.z + (size.z) / 2 + gap / 2);
        meshG.transform.localScale = new Vector3(gap, gap2, gap3);

        meshG.GetComponent<Renderer>().material = boxMaterial;
		//Remove Box Collider from the outer box //obstructing when click
        Destroy(meshG.GetComponent<BoxCollider>());              
        meshG.transform.parent = obj.transform;
        meshG.AddComponent<BoxCollider>();
        meshG.AddComponent<ClickButtonResizeBox>();

        GameObject meshH = GameObject.CreatePrimitive(PrimitiveType.Cube);
        meshH.name = "Outer Box H";

        meshH.transform.position = new Vector3(center.x - (size.x) / 2 - gap / 2, center.y + (size.y) / 2 + gap / 2, center.z + (size.z) / 2 + gap / 2);
        meshH.transform.localScale = new Vector3(gap, gap2, gap3);

        meshH.GetComponent<Renderer>().material = boxMaterial;
		//Remove Box Collider from the outer box //obstructing when click
        Destroy(meshG.GetComponent<BoxCollider>());              
        meshH.transform.parent = obj.transform;
        meshH.AddComponent<BoxCollider>();
        meshH.AddComponent<ClickButtonResizeBox>();


        GameObject meshP = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        meshP.name = "Outer Box P";

        meshP.transform.position = new Vector3(center.x - (size.x) / 2 - gap / 2, center.y - gap / 2, center.z - (size.z) / 2 - gap / 2);
        meshP.transform.localScale = new Vector3(gap, gap2, gap3);

        meshP.GetComponent<Renderer>().material = boxMaterial;
		//Remove Box Collider from the outer box //obstructing when click
        Destroy(meshP.GetComponent<BoxCollider>());              
        meshP.transform.parent = obj.transform;
        meshP.AddComponent<BoxCollider>();
        meshP.AddComponent<ClickButtonRotateBox>();

        GameObject meshQ = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        meshQ.name = "Outer Box Q";


        meshQ.transform.position = new Vector3(center.x + (size.x) / 2 + gap / 2, center.y - gap / 2, center.z - (size.z) / 2 - gap / 2);
        meshQ.transform.localScale = new Vector3(gap, gap2, gap3);

        meshQ.GetComponent<Renderer>().material = boxMaterial;
		//Remove Box Collider from the outer box //obstructing when click
        Destroy(meshQ.GetComponent<BoxCollider>());              
        meshQ.transform.parent = obj.transform;
        meshQ.AddComponent<BoxCollider>();
        meshQ.AddComponent<ClickButtonRotateBox>();


        GameObject meshM = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        meshM.name = "Outer Box M";

        meshM.transform.position = new Vector3(center.x + (size.x) / 2 + gap / 2, center.y - gap / 2, center.z + (size.z) / 2 + gap / 2);
        meshM.transform.localScale = new Vector3(gap, gap2, gap3);

        meshM.GetComponent<Renderer>().material = boxMaterial;
		//Remove Box Collider from the outer box //obstructing when click
        Destroy(meshM.GetComponent<BoxCollider>());              
        meshM.transform.parent = obj.transform;
        meshM.AddComponent<BoxCollider>();
        meshM.AddComponent<ClickButtonRotateBox>();


        GameObject meshN = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        meshN.name = "Outer Box D";

        meshN.transform.position = new Vector3(center.x - (size.x) / 2 - gap / 2, center.y - gap / 2, center.z + (size.z) / 2 + gap / 2);
        meshN.transform.localScale = new Vector3(gap, gap2, gap3);

        meshN.GetComponent<Renderer>().material = boxMaterial;
		//Remove Box Collider from the outer box //obstructing when click
        Destroy(meshN.GetComponent<BoxCollider>());              
        meshN.transform.parent = obj.transform;
        meshN.AddComponent<BoxCollider>();
        meshN.AddComponent<ClickButtonRotateBox>();

        //DrawLine(meshA.transform.position, meshB.transform.position);
        Debug.DrawLine(meshA.transform.position, meshB.transform.position, Color.red);
        Debug.DrawLine(Vector3.zero, new Vector3(1, 0, 0), Color.red);
        Vector3 mainPointPos = meshA.transform.position;
        Vector3 pointPos = meshB.transform.position;

        GL.Begin(GL.LINES);
        GL.Color(Color.red);
        
        GL.End();


        bbox = new GameObject[12];
        bbox[0] = meshA;
        bbox[1] = meshB;
        bbox[2] = meshC;
        bbox[3] = meshD;
        bbox[4] = meshE;
        bbox[5] = meshF;
        bbox[6] = meshG;
        bbox[7] = meshH;
        bbox[8] = meshP;
        bbox[9] = meshQ;
        bbox[10] = meshM;
        bbox[11] = meshN;
    }
}
