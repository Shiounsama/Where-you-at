using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SeekerAudio : MonoBehaviour
{
    [HideInInspector]
    public Vector3 projectedCameraPos;
    [HideInInspector]
    public Transform cityTransform;

    [SerializeField] private LayerMask groundLayerMask;

    private AudioListener m_audioListener;
    private Camera m_camera;

    private void Awake()
    {
        m_audioListener = GetComponentInChildren<AudioListener>();
        m_camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (!cityTransform)
            return;

        Ray ray = new Ray(m_camera.transform.position, m_camera.transform.forward);
        Plane hPlane = new Plane(Vector3.up, new Vector3(0, cityTransform.position.y, 0));

        if (hPlane.Raycast(ray, out float distance))
        {
            projectedCameraPos = ray.GetPoint(distance);
        }

#if UNITY_EDITOR
        Debug.DrawLine(transform.position, projectedCameraPos);
#endif
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(projectedCameraPos, .5f);
    }
#endif
}
