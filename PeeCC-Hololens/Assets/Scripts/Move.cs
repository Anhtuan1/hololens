using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour, IManipulationHandler, IInputClickHandler
{

    public Material material;
    private LineRenderer lineRenderer;
    private int counter = 0;
    private float dist;
    public float lineDrawSpeed = 6f;
    public float x_add = 0;
    public float y_add = 0;

    [SerializeField]
    float DragSpeed = 5f;

    [SerializeField]
    float DragScale = 5f;

    [SerializeField]
    float MaxDragDistance = 50f;

    Vector3 lastPosition;

    [SerializeField]
    bool draggingEnabled = true;
    public void SetDragging(bool enabled)
    {
        draggingEnabled = enabled;
    }

    public void OnManipulationStarted(ManipulationEventData eventData)
    {
        InputManager.Instance.PushModalInputHandler(gameObject);
        lastPosition = transform.position;


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

    public void OnManipulationCompleted(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();

    }

    public void OnManipulationCanceled(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
    }

    void Drag(Vector3 positon)
    {
        var targetPosition = lastPosition + positon * DragScale;
        if (Vector3.Distance(lastPosition, targetPosition) <= MaxDragDistance)
        {


            transform.position = Vector3.Lerp(transform.position, targetPosition, DragSpeed);

            //GameObject MyObject = Resources.Load("Line") as GameObject;
            //GameObject go = Instantiate(MyObject) as GameObject;
            //this.transform.position = new Vector3(transform.position.x+x_add, transform.position.y+y_add, transform.position.z);
            //go.transform.localScale = new Vector3(0.035f, 0.035f, 0.035f);

            // Debug.Log("X "+ targetPosition.x+" Y "+targetPosition.y+ " Z "+ targetPosition.z +" X1 "+ targetPosition.x+" Y1"+ targetPosition.y+" Z1 "+ targetPosition.z);

        }
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        transform.position = lastPosition;
    }

}
