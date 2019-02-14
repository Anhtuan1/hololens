using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class ClickButtonResizeBox : MonoBehaviour, IManipulationHandler
{

    [SerializeField]
    float ResizeSpeedFactor = 5f;

    [SerializeField]
    float ResizeScaleFactor = 10f;


    [SerializeField]
    bool AllowResizeWarp = false;


    [SerializeField]
    float MinScale = 0.1f;


    [SerializeField]
    float MaxScale = 10.5f;

    [SerializeField]
    bool resizingEnabled = true;

    Vector3 lastScale;

    private GameObject child;
    private GameObject parent_object;

    public void SetResizing(bool enabled)
    {
        resizingEnabled = enabled;
    }

    public void OnManipulationStarted(ManipulationEventData eventData)
    {
        InputManager.Instance.PushModalInputHandler(gameObject);
        parent_object = this.transform.parent.gameObject;
        Debug.Log(parent_object.name);
        child = parent_object.transform.GetChild(0).gameObject;
        lastScale = parent_object.transform.localScale;
        Destroy(parent_object.GetComponent<DragHand>());
        Destroy(parent_object.GetComponent<ClickButtonRotateBox>());

    }

    public void OnManipulationUpdated(ManipulationEventData eventData)
    {
        if (resizingEnabled)
        {
            Resize(eventData.CumulativeDelta);

            //sharing & messaging
            //SharingMessages.Instance.SendResizing(Id, eventData.CumulativeDelta);
        }
    }

    public void OnManipulationCompleted(ManipulationEventData eventData)
    {
       // InputManager.Instance.PopModalInputHandler();
        if (parent_object != null)
        {
            parent_object.AddComponent<DragHand>();
            Destroy(this.GetComponent<ClickButtonResizeBox>());
        }
    }

    public void OnManipulationCanceled(ManipulationEventData eventData)
    {
        //InputManager.Instance.PopModalInputHandler();
        if (parent_object != null)
        {
            parent_object.AddComponent<DragHand>();
            Destroy(this.GetComponent<ClickButtonResizeBox>());
        }
    }
    void Resize(Vector3 newScale)
    {
        float resizeX, resizeY, resizeZ;
        //if we are warping, honor axis delta, else take the x
        if (AllowResizeWarp)
        {
            resizeX = newScale.x * ResizeScaleFactor;
            resizeY = newScale.y * ResizeScaleFactor;
            //resizeZ = newScale.z * ResizeScaleFactor;
        }
        else
        {
            resizeX = resizeY = resizeZ = newScale.x * ResizeScaleFactor;
        }

        resizeX = Mathf.Clamp(lastScale.x + resizeX, MinScale, MaxScale);
        resizeY = Mathf.Clamp(lastScale.y + resizeY, MinScale, MaxScale);
        //resizeZ = Mathf.Clamp(lastScale.z + resizeZ, MinScale, MaxScale);

        if(parent_object != null)
        {
            parent_object.transform.localScale = Vector3.Lerp(transform.localScale,
            new Vector3(resizeX, resizeY, 0.01f),
            ResizeSpeedFactor);
        }
        
    }
}
