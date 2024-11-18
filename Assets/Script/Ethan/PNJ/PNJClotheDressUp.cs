using UnityEngine;

public class PNJClothe : MonoBehaviour
{
    [Header("CollectionOfClotheAppliedToOurPNJ")]
    [SerializeField] private ClotheCollection headClothe;
    [SerializeField] private ClotheCollection expressionClothe;
    [SerializeField] private ClotheCollection chestClothe;
    [SerializeField] private ClotheCollection legsClothe;
    [SerializeField] private ClotheCollection feetsClothe;

    [Header("BodyPartOfOurPNJ")]
    [SerializeField] private SpriteRenderer headClotheImage;
    [SerializeField] private SpriteRenderer expressionClotheImage;
    [SerializeField] private SpriteRenderer chestClotheImage;
    [SerializeField] private SpriteRenderer legsClotheImage;
    [SerializeField] private SpriteRenderer feetsClotheImage;

    private void Start()
    {
        GenerateRandomClothe(headClotheImage, headClothe);
        GenerateRandomClothe(expressionClotheImage, expressionClothe);
        GenerateRandomClothe(chestClotheImage, chestClothe);
        GenerateRandomClothe(legsClotheImage, legsClothe);
        GenerateRandomClothe(feetsClotheImage, feetsClothe);
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
