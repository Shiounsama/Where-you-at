using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStateOfObjects : MonoBehaviour
{
    public List<GameObject> objectToChangeState;

    public void Activate()
    {
        foreach(GameObject obj in objectToChangeState)
        {
            obj.SetActive(true);
        }
    }

    public void Deactivate()
    {
        foreach(GameObject obj in objectToChangeState)
        {
            obj.SetActive(false);
        }
    }
}