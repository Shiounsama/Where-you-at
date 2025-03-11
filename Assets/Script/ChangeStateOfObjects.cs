using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class test
{
    public GameObject objectToChangeState;
    public bool stateToTake;
}

public class ChangeStateOfObjects : MonoBehaviour
{
    public List<test> objectsToModify;

    public void ChangeState()
    {
        foreach (test obj in objectsToModify)
        {
            obj.objectToChangeState.SetActive(obj.stateToTake);
        }
    }

    public void Backward()
    {
        foreach (test obj in objectsToModify)
        {
            obj.objectToChangeState.SetActive(!obj.stateToTake);
        }
    }
}