using Mirror;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class FreezeDragMovement : NetworkBehaviour, IEndDragHandler, IBeginDragHandler
{
    public Transform cameraToFreeze;
    private bool isIso;

    public void OnBeginDrag(PointerEventData eventData)
    {
        cameraToFreeze = Camera.main.transform;

        if (cameraToFreeze != null)
        {
            if (CheckComponent(typeof(Camera360)) && cameraToFreeze.GetComponent<Camera360>().enabled == true)
            {
                isIso = false;
                cameraToFreeze.GetComponent<Camera360>().enabled = false;
            }
            if (CheckComponent(typeof(IsoCameraDrag)) && cameraToFreeze.GetComponent<IsoCameraDrag>().enabled == true)
            {
                isIso = true;
                cameraToFreeze.GetComponent<IsoCameraDrag>().enabled = false;
            }
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (cameraToFreeze != null)
        {
            if (CheckComponent(typeof(Camera360)) && !isIso)
            {
                cameraToFreeze.GetComponent<Camera360>().enabled = true;
            }
            if (CheckComponent(typeof(IsoCameraDrag)) && isIso)
            {
                cameraToFreeze.GetComponent<IsoCameraDrag>().enabled = true;
            }
        }
    }

    public bool CheckComponent(Type behaviourType)
    {
        if (cameraToFreeze.GetComponent(behaviourType) != null)
        {
            return true;
        }
        return false;
    }
}
