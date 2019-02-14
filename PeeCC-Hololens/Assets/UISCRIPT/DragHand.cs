using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class DragHand : MonoBehaviour, IManipulationHandler
{

    [SerializeField]
    float DragSpeed = 5f;

    [SerializeField]
    float DragScale = 5f;

    [SerializeField]
    float MaxDragDistance = 50f;

    Vector3 lastPosition;

    [SerializeField]
    bool draggingEnabled = true;

    private GameObject child;
    private GameObject parent_object;

    public void SetDragging(bool enabled)
    {
        draggingEnabled = enabled;
    }
    public void OnManipulationCanceled(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
    }

    public void OnManipulationCompleted(ManipulationEventData eventData)
    {
        //InputManager.Instance.PopModalInputHandler();
    }

    public void OnManipulationStarted(ManipulationEventData eventData)
    {
        InputManager.Instance.PushModalInputHandler(gameObject);
        lastPosition = transform.position;
        child = this.transform.GetChild(0).gameObject;


    }

    public void OnManipulationUpdated(ManipulationEventData eventData)
    {
        if (draggingEnabled)
        {
            Drag(eventData.CumulativeDelta);

            //sharing & messaging
            //SharingMessages.Instance.SendDragging(Id, eventData.CumulativeDelta);
        }
    }
    void Drag(Vector3 positon)
    {
        var targetPosition = lastPosition + positon * DragScale;
        if (Vector3.Distance(lastPosition, targetPosition) <= MaxDragDistance)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, DragSpeed);
            if(child != null)
            {
                child.transform.position = Vector3.Lerp(child.transform.position, targetPosition, DragSpeed);
            }
            
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
