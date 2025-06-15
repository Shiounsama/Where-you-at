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
            plateform.transform.DOShakePosition(
                duration: 3f,
                strength: new Vector3(0, 0.6f, 0),
                vibrato: 8,
                randomness: 10,
                snapping: false,
                fadeOut: true
            )
            .OnComplete(() =>
            {
                plateform.transform.DOMoveY(
                    plateform.transform.localPosition.y - 30f,
                    0.5f
                )
                .SetEase(Ease.InSine)
                .OnComplete(() => plateform.SetActive(false));
            });
        }
    }
}
public class CityManager : MonoBehaviour
{
    [SerializeField] private List<Plateform> _plateforms = new List<Plateform>();

    public int _plateformWhereHiderIsIn;

    public void checkSol()
    {
        List<cityNumber> allCity = new List<cityNumber>(FindObjectsOfType<cityNumber>());

        foreach (cityNumber uwu in allCity)
        {
            _plateforms[uwu.zone]._plateforms.Add(uwu.gameObject);

            BoxCollider[] colliders = uwu.GetComponentsInChildren<BoxCollider>();

            foreach (BoxCollider col in colliders)
            {
                GameObject go = col.gameObject;

                if (go != uwu.gameObject)
                {
                    _plateforms[uwu.zone]._plateforms.Add(go);
                }
            }
        }
    }

    public void SetHiderPlateform(GameObject PlateformToCheck)
    {
        int index = 0;

        foreach (var plateform in _plateforms)
        {
            foreach (var plateformPiece in plateform._plateforms)
            {
                if (plateformPiece == PlateformToCheck)
                {
                    _plateformWhereHiderIsIn = index;
                    return;
                }
            }
            index++;
        }
    }

    public void MakePlateformFall()
    {
        for (int i = 0; i < _plateforms.Count; i++)
        {
            if (i != _plateformWhereHiderIsIn)
            {
                RemovePlateform(i);
            }
        }
    }

    private void RemovePlateform(int index)
    {
        _plateforms[index].MakePlateformDown();
    }
}
