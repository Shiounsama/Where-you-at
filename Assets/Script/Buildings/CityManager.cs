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
    
    [SerializeField] private int _plateformWhereHiderIsIn;
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
                    MakePlateformFall();
                    return;
                }
            }
            index++;
        }
    }

    private void MakePlateformFall()
    {
        if (randomIndex == _plateformWhereHiderIsIn)
        {
            if(randomIndex + 1 < _plateforms.Count)
            {
                RemovePlateform(randomIndex + 1);
                randomIndex++;
            }
        }
        if(randomIndex >= _plateforms.Count)
        {
            randomIndex = 0;
        }
        randomIndex++;
    }
    private void RemovePlateform(int index)
    {
        _plateforms[index].MakePlateformDown();
    }
}
