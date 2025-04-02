using UnityEngine;

public class PNJClothe : MonoBehaviour
{
    [Header("CollectionOfClotheAppliedToOurPNJ")]
    public ClotheCollection[] frontHairClothe;
    public ClotheCollection[] backHairClothe;
    public ClotheCollection eyesClothe;
    public ClotheCollection plusClothe;
    public ClotheCollection mouthClothe;
    public ClotheCollection[] hautClothe;
    public ClotheCollection[] basClothe;
    public ClotheCollection[] shoesClothe;
    public ClotheCollection[] corpsClothe;
    public ClotheCollection[] teteClothe;

    [Header("BodyPartOfOurPNJ")]
    public SpriteRenderer frontHairClotheImage;
    public SpriteRenderer backHairClotheImage;
    public SpriteRenderer eyesClotheImage;
    public SpriteRenderer plusClotheImage;
    public SpriteRenderer mouthClotheImage;
    public SpriteRenderer hautClotheImage;
    public SpriteRenderer basClotheImage;
    public SpriteRenderer shoesClotheImage;
    public SpriteRenderer corpsClotheImage;
    public SpriteRenderer teteClotheImage;

    public void Start()
    {
        choseHair();
        choseCorps();
        chooseExpression(eyesClothe, eyesClotheImage);
        chooseExpression(plusClothe, plusClotheImage);
        chooseExpression(mouthClothe, mouthClotheImage);
        chooseClothe(hautClothe, hautClotheImage);
        chooseClothe(basClothe, basClotheImage);
        chooseClothe(shoesClothe, shoesClotheImage);
    }

    public void chooseClothe(ClotheCollection[] allCollection, SpriteRenderer spriteRenderer)
    {
        int randomNumberListClothe = Random.Range(0, allCollection.Length);
        int randomNumberClothe = Random.Range(0, allCollection[randomNumberListClothe].clotheList.Count);
        Sprite spriteClothe = allCollection[randomNumberListClothe].ReturnRandomClothe(randomNumberClothe);

        spriteRenderer.sprite = spriteClothe;
    }

    public void chooseExpression(ClotheCollection allCollection, SpriteRenderer spriteRenderer)
    {
        int randomNumberListClothe = 0 ;

        if (allCollection == plusClothe)
        {
            randomNumberListClothe = Random.Range(0, allCollection.clotheList.Count+1);
            if(randomNumberListClothe == allCollection.clotheList.Count)
            {
                spriteRenderer.sprite = null;
                return;
            }
        }

        else
        {
            randomNumberListClothe = Random.Range(0, allCollection.clotheList.Count);
        }

        Sprite spriteClothe = allCollection.ReturnRandomClothe(randomNumberListClothe);

        spriteRenderer.sprite = spriteClothe;
    }

    public void choseHair()
    {
        int randomNumberListFrontHair = Random.Range(0, frontHairClothe.Length);
        int randomNumberListBackHair = Random.Range(0, backHairClothe.Length);

        int randomNumberHair = Random.Range(0, frontHairClothe[randomNumberListFrontHair].clotheList.Count);

        Sprite spriteFrontHair = frontHairClothe[randomNumberListFrontHair].ReturnRandomClothe(randomNumberHair);
        Sprite spriteBackHair = backHairClothe[randomNumberListBackHair].ReturnRandomClothe(randomNumberHair);


        frontHairClotheImage.sprite = spriteFrontHair;
        backHairClotheImage.sprite = spriteBackHair;
    }

    public void choseCorps()
    {
        int randomNumberListCorps = 0;
        int randomNumberListTete = Random.Range(0, teteClothe.Length);

        int randomNumberHair = Random.Range(0, corpsClothe[randomNumberListCorps].clotheList.Count);

        Sprite spriteCorps = corpsClothe[randomNumberHair].ReturnRandomClothe(randomNumberListCorps);
        Sprite spriteTete = teteClothe[randomNumberHair].ReturnRandomClothe(randomNumberListTete);


        corpsClotheImage.sprite = spriteCorps;
        teteClotheImage.sprite = spriteTete;
    }
}
