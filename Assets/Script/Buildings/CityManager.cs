using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    private void Start()
    {
        ChoosePlateform();
    }

    public void SetHiderPlateform(GameObject PlateformToCheck)
    {
        int index = 0;
        
        foreach (var plateform in _plateforms)
        {
            foreach (var plateformPlateform in plateform._plateforms)
            {
                if (plateformPlateform == PlateformToCheck)
                {
                    _plateformWhereHiderIsIn = index;
                    return;
                }
            }
            index++;
        }
    }
    
    private void ChoosePlateform()
    {
        if (randomIndex == _plateformWhereHiderIsIn)
        {
            randomIndex++;
            RemovePlateform(randomIndex);
        }
        else
        {
            RemovePlateform(randomIndex);
        }
        randomIndex++;
    }
    private void RemovePlateform(int index)
    {
        _plateforms[index].MakePlateformDown();
    }
}
