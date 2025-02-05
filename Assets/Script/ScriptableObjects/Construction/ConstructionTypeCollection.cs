using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Asset/ConstructionType")]
public class ConstructionTypeCollection : ScriptableObject
{
    [System.Serializable]
    public class PrefabLimit
    {
        public GameObject prefab;
        public TypeOfConstruction type;
        public NameOfConstruction name;
        public int maxSpawn = int.MaxValue; // Par défaut, pas de limite
    }

    [System.Serializable]
    public class AffinityRule
    {
        public NameOfConstruction sourceName;  
        public NameOfConstruction targetName;  
        [Range(0, 1)] public float affinityChance;  
    }

    [SerializeField] public List<AffinityRule> affinityRules = new();
    [SerializeField] private List<PrefabLimit> prefabLimits = new();

    private Dictionary<TypeOfConstruction, List<PrefabLimit>> prefabsByType;
    private Dictionary<GameObject, int> prefabSpawnCounts = new();

    private void InitializePrefabs()
    {
        if (prefabsByType != null) return; 

        prefabsByType = new Dictionary<TypeOfConstruction, List<PrefabLimit>>();

        foreach (PrefabLimit prefabLimit in prefabLimits)
        {
            if (!prefabsByType.ContainsKey(prefabLimit.type))
            {
                prefabsByType[prefabLimit.type] = new List<PrefabLimit>();
            }
            prefabsByType[prefabLimit.type].Add(prefabLimit);
            prefabSpawnCounts[prefabLimit.prefab] = 0; 
        }
    }

    public GameObject GetPrefab(TypeOfConstruction typeToGet)
    {
        InitializePrefabs();

        if (prefabsByType.ContainsKey(typeToGet))
        {
            List<PrefabLimit> PrefabDispo = new List<PrefabLimit>();

            foreach (PrefabLimit prefabLimit in prefabsByType[typeToGet])
            {
                if (prefabSpawnCounts[prefabLimit.prefab] < prefabLimit.maxSpawn)
                {
                    PrefabDispo.Add(prefabLimit);
                }
            }

            if (PrefabDispo.Count > 0)
            {
                PrefabLimit selectedPrefabLimit = PrefabDispo[Random.Range(0, PrefabDispo.Count)];
                prefabSpawnCounts[selectedPrefabLimit.prefab]++; // Incrémenter le compteur de ce prefab
                return selectedPrefabLimit.prefab;
            }
        }

        return null; 
    }

    public void DecreaseSpawnCount(GameObject prefab)
    {
        if (prefabSpawnCounts.ContainsKey(prefab) && prefabSpawnCounts[prefab] > 0)
        {
            prefabSpawnCounts[prefab]--;
        }
    }

    public void ResetSpawnCounts()
    {
        if (prefabSpawnCounts != null)
        {
            List<GameObject> keys = new List<GameObject>(prefabSpawnCounts.Keys);
            foreach (GameObject key in keys)
            {
                prefabSpawnCounts[key] = 0;
            }
        }
    }

    public void ResetPrefabLibrary()
    {
        prefabsByType = null; 
        prefabSpawnCounts.Clear(); 
        InitializePrefabs(); 
    }

}