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
                Debug.Log("JE SUIS CONNE");
                Destroy(this);
            }
            else if (prioVoisin.priorite > priorite)
            {
                Debug.Log("JE SUIS A COTE DE " + prioVoisin.isCible);
                Destroy(collision.gameObject);
            }
            else
                Destroy(this);
        }
        else return;

        
    }
}
