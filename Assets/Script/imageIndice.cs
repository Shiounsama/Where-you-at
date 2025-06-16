using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class imageIndice : MonoBehaviour
{
    public List<Sprite> images = new List<Sprite>();
    public Image selfImage;

    void Start()
    {
        selfImage.sprite = images[manager.nombrePartie];
        
    }

}
