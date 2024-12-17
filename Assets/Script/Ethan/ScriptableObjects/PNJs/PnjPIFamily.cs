using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Asset/PnjPIFamily")]
public class PnjPIFamily : ScriptableObject
{
    public List<GameObject> listOfPnjPI;

    public List<int> listOfValueUsed;

    public bool isAllPnjUsed;

    public GameObject GetPrefab()
    {
        if (listOfPnjPI.Count > 0)
        {
            int x = Random.Range(0, listOfPnjPI.Count);

            while (listOfValueUsed.Contains(x))
            {
                x = Random.Range(0, listOfPnjPI.Count);
                if (listOfValueUsed.Count >= listOfPnjPI.Count)
                {
                    isAllPnjUsed = true;

                    return listOfPnjPI[0].gameObject;
                }
            }

            listOfValueUsed.Add(x);

            if (listOfValueUsed.Count <= listOfPnjPI.Count)
            {
                return listOfPnjPI[x].gameObject;
            }
            return listOfPnjPI[0].gameObject;
        }

        return null;
    }

    public void ResetListOfPnjPI()
    {
        listOfValueUsed.Clear();
        isAllPnjUsed = false;
    }
}
