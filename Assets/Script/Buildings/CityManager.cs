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
public class CityManager : NetworkBehaviour
{
    [SerializeField] private List<Plateform> _plateforms = new List<Plateform>();
    
    [SerializeField] private int _plateformWhereHiderIsIn;
    [SerializeField] private int randomIndex;

    [TargetRpc]
    public void SetHiderPlateform(NetworkConnection conn, GameObject PlateformToCheck)
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
    
    private void MakePlateformFall()
    {
        if (randomIndex == _plateformWhereHiderIsIn)
        {
            RemovePlateform(randomIndex + 1);
        }
        randomIndex++;
    }
    private void RemovePlateform(int index)
    {
        _plateforms[index].MakePlateformDown();
    }
}
