using UnityEngine;

public class PNJClothe : MonoBehaviour
{
    [Header("CollectionOfClotheAppliedToOurPNJ")]
    [SerializeField] private ClotheData headClothe;
    [SerializeField] private ClotheData expressionClothe;
    [SerializeField] private ClotheData chestClothe;
    [SerializeField] private ClotheData legsClothe;
    [SerializeField] private ClotheData feetsClothe;

    [Header("BodyPartOfOurPNJ")]
    [SerializeField] private SpriteRenderer headClotheImage;
    [SerializeField] private SpriteRenderer expressionClotheImage;
    [SerializeField] private SpriteRenderer chestClotheImage;
    [SerializeField] private SpriteRenderer legsClotheImage;
    [SerializeField] private SpriteRenderer feetsClotheImage;

    private void Start()
    {
        GenerateRandomClothe(headClotheImage, headClothe);
    }

    private void GenerateRandomClothe(SpriteRenderer bodyPartImage, ClotheData bodyPartCollection)
    {
        bodyPartImage.sprite = bodyPartCollection.clotheList[Random.Range(0, bodyPartCollection.clotheList.Count)].sprite;
    }
}
