using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class ClickButtonRotateBox : MonoBehaviour , IManipulationHandler
{

    float RotationFactor = 20;
    float RotateSpeed = 25f;
    Vector3 lastRotation;
    Vector3 scale;
    public char rotateAxis = 'Y';

    private GameObject child;
    private GameObject parent_object;

    public void OnManipulationCanceled(ManipulationEventData eventData)
    {
        //throw new System.NotImplementedException();
        if (parent_object != null)
        {
            parent_object.AddComponent<DragHand>();
            Destroy(this.GetComponent<ClickButtonRotateBox>());
        }
        Debug.Log("END ROTATE Canceled");
    }

    public void OnManipulationCompleted(ManipulationEventData eventData)
    {
        // throw new System.NotImplementedException();
        Debug.Log("END ROTATE Completed");
        if (parent_object != null)
        {
            Debug.Log("END ROTATE Completed REMOVE");
            parent_object.AddComponent<DragHand>();
            Destroy(this.GetComponent<ClickButtonRotateBox>());
        }
        
    }

    public void OnManipulationStarted(ManipulationEventData eventData)
    {
        InputManager.Instance.PushModalInputHandler(gameObject);
        parent_object = this.transform.parent.gameObject;
        child = parent_object.transform.GetChild(0).gameObject;
        lastRotation = parent_object.transform.rotation.eulerAngles;
        scale = parent_object.transform.localScale;
        Destroy(parent_object.GetComponent<DragHand>());
        Destroy(parent_object.GetComponent<ClickButtonResizeBox>());

    }

    public void OnManipulationUpdated(ManipulationEventData eventData)
    {
        var rotation = new Vector3(eventData.CumulativeDelta.y * RotationFactor,
                eventData.CumulativeDelta.x * RotationFactor,
                eventData.CumulativeDelta.z * RotationFactor);

        Rotate(rotation);
    }

    void Rotate(Vector3 rotation)
    {
        //Debug.Log(" X = " + rotation.x + "... Y = " + rotation.y + "... Z = " + rotation.z);
        if (parent_object != null)
        {
            switch (rotateAxis)
            {
                case 'X':
                    transform.rotation = Quaternion.Euler((lastRotation.x + rotation.x) * RotateSpeed, lastRotation.y, lastRotation.z);
                    //transform.Rotate(Vector3.right * rotation.x);
                    //xObj.selected = true;
                    //yObj.selected = false;
                    //zObj.selected = false;
                    break;
                case 'Y':
                    parent_object.transform.rotation = Quaternion.Euler(lastRotation.x, (lastRotation.y - rotation.y) * RotateSpeed, lastRotation.z);
                    //transform.Rotate(Vector3.down * rotation.y);
                    //yObj.selected = true;
                    //xObj.selected = false;
                    //zObj.selected = false;
                    break;
                case 'Z':
                    transform.rotation = Quaternion.Euler(lastRotation.x, lastRotation.y, (lastRotation.z + rotation.z) * RotateSpeed);
                    //transform.Rotate(Vector3.forward * rotation.z);
                    //zObj.selected = true;
                    //yObj.selected = false;
                    //xObj.selected = false;
                    break;
                default:
                    //xObj.selected = false;
                    //yObj.selected = false;
                    //zObj.selected = false;
                    break;
            }
        }
    }
}
