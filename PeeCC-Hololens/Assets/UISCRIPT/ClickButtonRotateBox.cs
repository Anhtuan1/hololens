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
        
    }

    public void OnManipulationCompleted(ManipulationEventData eventData)
    {
        // throw new System.NotImplementedException();
        
        if (parent_object != null)
        {
            
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
        
        if (parent_object != null)
        {
            switch (rotateAxis)
            {
                case 'X':
                    transform.rotation = Quaternion.Euler((lastRotation.x + rotation.x) * RotateSpeed, lastRotation.y, lastRotation.z);
                    
                    break;
                case 'Y':
                    parent_object.transform.rotation = Quaternion.Euler(lastRotation.x, (lastRotation.y - rotation.y) * RotateSpeed, lastRotation.z);
                    
                    break;
                case 'Z':
                    transform.rotation = Quaternion.Euler(lastRotation.x, lastRotation.y, (lastRotation.z + rotation.z) * RotateSpeed);
                    
                    break;
                default:
                    
                    break;
            }
        }
    }
}
