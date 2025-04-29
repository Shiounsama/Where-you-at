using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJpriorite : MonoBehaviour
{
    public int priorite;
    public bool isCible;

    private void OnCollisionStay(Collision collision)
    {
        PNJpriorite prioVoisin;

        if (collision.gameObject.GetComponent<PNJpriorite>())
        {
           prioVoisin = collision.gameObject.GetComponent<PNJpriorite>();

            if (isCible)
            {
                return;
            }

            if (prioVoisin.isCible)
            {
                Destroy(this);
            }
            else if (prioVoisin.priorite > priorite)
            {
                Destroy(collision.gameObject);
            }
            else
                Destroy(this);
        }
        else return;

        
    }
}
