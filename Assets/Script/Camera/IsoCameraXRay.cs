using System.Collections.Generic;
using UnityEngine;

public class IsoCameraXRay : MonoBehaviour
{
    private List<GameObject> allGameObjectInVision = new List<GameObject>();

    private Camera cam;

    public LayerMask layerToCheck;

    public Vector3 boxSize;

    public float minSizeBeforeXRay;

    private void Start()
    {
        cam = transform.GetComponent<Camera>();
    }

    public void GetAllGameObjectInVision()
    {
        if (allGameObjectInVision.Count > 0)
        {
            SetVisibilityOfObjects(true);
            allGameObjectInVision.Clear();
        }
        if (cam.orthographicSize < minSizeBeforeXRay)
        {
            foreach (RaycastHit col in Physics.BoxCastAll(transform.position, boxSize, transform.forward, Quaternion.identity, 1000f, layerToCheck))
            {
                if (!allGameObjectInVision.Contains(col.collider.transform.gameObject))
                {
                    allGameObjectInVision.Add(col.collider.transform.gameObject);
                }
            }
            SetVisibilityOfObjects(false);
        }
    }

    public void SetVisibilityOfObjects(bool isVisible)
    {
        foreach (GameObject go in allGameObjectInVision)
        {
            go.SetActive(isVisible);
        }
    }
}