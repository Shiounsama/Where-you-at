using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Asset/ConstructionType")]
public class ConstructionTypeCollection : ScriptableObject
{
    [SerializeField] private List<GameObject> prefabs = new();

    // Utilisation d'un dictionnaire pour classer les prefabs par type lors de l'initialisation
    private Dictionary<TypeOfConstruction, List<GameObject>> prefabsByType;

    public GameObject GetPrefab(TypeOfConstruction typeToGet)
    {
        //Random.InitState(seed.Instance.SeedValue);
        // Initialisation du dictionnaire
        prefabsByType = new Dictionary<TypeOfConstruction, List<GameObject>>();

        foreach (GameObject prefab in prefabs)
        {
            // Vérifie si le prefab possède le composant ConstructionType
            ConstructionType constructionType = prefab.GetComponent<ConstructionType>();
            if (constructionType != null)
            {
                TypeOfConstruction type = constructionType.prefabBuildingType;

                // Ajoute le prefab dans la liste correspondant à son type
                if (!prefabsByType.ContainsKey(type))
                {
                    prefabsByType[type] = new List<GameObject>();
                }
                prefabsByType[type].Add(prefab);
            }
            else
            {
                Debug.LogWarning($"Prefab {prefab.name} n'a pas de composant ConstructionType attaché.");
            }
        }
        // Vérifie si le type demandé existe dans le dictionnaire
        if (prefabsByType.ContainsKey(typeToGet))
        {
            List<GameObject> list = prefabsByType[typeToGet];

            // Vérifie que la liste n'est pas vide avant de retourner un prefab aléatoire
            if (list.Count > 0)
            {
                return list[Random.Range(0, list.Count)];
            }
            else
            {
                Debug.LogWarning($"Aucun prefab disponible pour le type {typeToGet}.");
            }
        }
        else
        {
            Debug.LogWarning($"Type de construction {typeToGet} non trouvé dans le dictionnaire.");
        }

        // Retourne null si aucun prefab n'est trouvé
        return null;
    }
}
