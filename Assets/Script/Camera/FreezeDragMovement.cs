using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;

public class FreezeDragMovement : NetworkBehaviour, IEndDragHandler, IBeginDragHandler
{
    public Transform cameraToFreeze;

    public void OnBeginDrag(PointerEventData eventData)
    {
        cameraToFreeze = Camera.main.transform;

        if(cameraToFreeze != null )
        {
            if (cameraToFreeze.GetComponent<Camera360>() != null)
            {
                cameraToFreeze.GetComponent<Camera360>().enabled = false;
            }
            if (cameraToFreeze.GetComponent<IsoCameraDrag>() != null)
            {
                cameraToFreeze.GetComponent<IsoCameraDrag>().enabled = false;
            }
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if(cameraToFreeze != null)
        {
            if (cameraToFreeze.GetComponent<Camera360>() != null)
            {
                cameraToFreeze.GetComponent<Camera360>().enabled = true;
            }
            if (cameraToFreeze.GetComponent<IsoCameraDrag>() != null)
            {
                cameraToFreeze.GetComponent<IsoCameraDrag>().enabled = true;
            }
        }
    }
}
