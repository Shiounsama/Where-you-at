using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateListOfObject : MonoBehaviour
{
    public List<GameObject> objectToChangeState;

    public void SwitchState()
    {
        foreach(GameObject obj in objectToChangeState)
        {
            obj.SetActive(!obj.activeSelf);
        }
    }
}
