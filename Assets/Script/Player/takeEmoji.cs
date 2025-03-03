using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class takeEmoji : MonoBehaviour
{
    public Camera thisCamera;

    private void Start()
    {
        thisCamera = this.GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = thisCamera.ScreenPointToRay(Input.mousePosition); 
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) 
            {
                if (hit.collider.CompareTag("emojiRecup"))
                {
                    Debug.Log("PLOP !");
                    Destroy(hit.collider.gameObject); 
                }
            }
        }
    }
}
