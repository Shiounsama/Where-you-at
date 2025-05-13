using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Mirror;

[System.Serializable]
public class Plateform
{
    public List<GameObject> _plateforms = new List<GameObject>();

    public void MakePlateformDown()
    {
        foreach (var plateform in _plateforms)
        {
            plateform.transform.DOMoveY(plateform.transform.localPosition.y - 30f, 0.5f).SetEase(Ease.InSine).OnComplete(() =>
                plateform.SetActive(false));
        }
    }
}
public class CityManager : MonoBehaviour
{
    [SerializeField] private List<Plateform> _plateforms = new List<Plateform>();
    
    public int _plateformWhereHiderIsIn;
    [SerializeField] private int randomIndex;
    
    public void SetHiderPlateform(GameObject PlateformToCheck)
    {
        print("SetHiderPlateform");
        int index = 0;
        
        foreach (var plateform in _plateforms)
        {
            foreach (var plateformPlateform in plateform._plateforms)
            {
                if (plateformPlateform == PlateformToCheck)
                {
                    _plateformWhereHiderIsIn = index;
                    //MakePlateformFall();
                    return;
                }
            }
            index++;
        }
    }

    public void MakePlateformFall()
    {
        Debug.Log("Dans MakePlateformFall");

        for(int i = 0; i< _plateforms.Count; i++ )

        if (i != _plateformWhereHiderIsIn)
        {
            
            RemovePlateform(i);
             
        }
    }

    private void RemovePlateform(int index)
    {
        _plateforms[index].MakePlateformDown();
    }
}
