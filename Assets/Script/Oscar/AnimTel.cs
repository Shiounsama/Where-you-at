using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class AnimTel : MonoBehaviour
{
    public GameObject phone;
    public GameObject endPhone;

    public void Animation()
    {
        transform.DOMoveY(phone.transform.position.y, 2f);
    }
    public void AnimationCancel()
    {
        transform.DOMoveY(endPhone.transform.position.y, 2f);
    }
}
