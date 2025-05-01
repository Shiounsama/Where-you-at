using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJpriorite : MonoBehaviour
{
    public bool isCible;
    public int priorite;
    public float tailleSphere = 0.4f;

    public void CheckVoisins()
    {
        Debug.Log("Je lance le check");

        Collider[] voisins = Physics.OverlapSphere(transform.position, tailleSphere);

        foreach (Collider collider in voisins)
        {
            if (collider.gameObject == this.gameObject) continue;

            PNJpriorite prioVoisin = collider.GetComponent<PNJpriorite>();
            if (prioVoisin != null)
            {
                if (prioVoisin.isCible)
                {
                    Debug.Log("L'autre est une cible, je me d�truis");
                    Destroy(this.gameObject);
                    return;
                }

                if (isCible)
                {
                    Debug.Log("Je suis une cible, je d�truis l'autre");
                    Destroy(prioVoisin.gameObject);
                    return;
                }

                if (prioVoisin.priorite < priorite)
                {
                    Debug.Log("L'autre a une priorit� plus faible, je le d�truis");
                    Destroy(prioVoisin.gameObject);
                    return;
                }
                else
                {
                    Debug.Log("L'autre a une priorit� plus �lev�e ou �gale, je me d�truis");
                    Destroy(this.gameObject);
                    return;
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, tailleSphere);
    }
}