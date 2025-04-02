using System.Collections.Generic;
using UnityEngine;

public class PointInteretManager : MonoBehaviour
{
    public int nombrePointInteret;

    public List<Construction> constructionList;

    public void Start()
    {
        if (seed.Instance != null)
        {
            Random.InitState(seed.Instance.SeedValue);
        }

        int a = 0;

        if (nombrePointInteret > constructionList.Count)
        {
            nombrePointInteret = constructionList.Count - 1;
        }

        if (a == nombrePointInteret)
        {
            a = nombrePointInteret + 1;
        }

        while (a < nombrePointInteret)
        {
            int x = Random.Range(0, constructionList.Count);

            if (constructionList[x].pointInteret == false)
            {
                constructionList[x].pointInteret = true;
                a++;
               
            }
        }

        seed.Instance.SeedValue++;
    }
}
