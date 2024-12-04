using UnityEngine;

public class PNJClothe : MonoBehaviour
{
    [Header("CollectionOfClotheAppliedToOurPNJ")]
    public ClotheCollection frontHairClothe;
    public ClotheCollection backHairClothe;
    public ClotheCollection eyesClothe;
    public ClotheCollection plusClothe;
    public ClotheCollection mouthClothe;
    public ClotheCollection hautClothe;
    public ClotheCollection basClothe;
    public ClotheCollection shoesClothe;

    [Header("BodyPartOfOurPNJ")]
    public SpriteRenderer frontHairClotheImage;
    public SpriteRenderer backHairClotheImage;
    public SpriteRenderer eyesClotheImage;
    public SpriteRenderer plusClotheImage;
    public SpriteRenderer mouthClotheImage;
    public SpriteRenderer hautClotheImage;
    public SpriteRenderer basClotheImage;
    public SpriteRenderer shoesClotheImage;

    public void Start()
    {
        manager.Instance.seed++;
        GenerateRandomClothe(frontHairClotheImage, frontHairClothe);
        GenerateRandomClothe(backHairClotheImage, backHairClothe);
        GenerateRandomClothe(eyesClotheImage, eyesClothe);
        GenerateRandomClothe(plusClotheImage, plusClothe);
        GenerateRandomClothe(mouthClotheImage, mouthClothe);
        GenerateRandomClothe(hautClotheImage, hautClothe);
        GenerateRandomClothe(basClotheImage, basClothe);
        GenerateRandomClothe(shoesClotheImage, shoesClothe);
    }

    private void GenerateRandomClothe(SpriteRenderer bodyPartImage, ClotheCollection bodyPartCollection)
    {
        if (bodyPartCollection.clotheList.Count <= 0)
        {
            return;
        }
        bodyPartImage.sprite = bodyPartCollection.ReturnRandomClothe();
    }
}
