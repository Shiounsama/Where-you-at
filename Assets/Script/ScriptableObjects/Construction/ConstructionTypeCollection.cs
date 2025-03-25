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
        public int maxSpawn = int.MaxValue;
    }

    [System.Serializable]
    public class AffinityRule
    {
        public NameOfConstruction sourceName;  
        public NameOfConstruction targetName;  
        [Range(0, 1)] public float affinityChance;  
    }

    [System.Serializable]
    public class ExclusionRule
    {
        public NameOfConstruction sourceName;  
        public NameOfConstruction excludedName; 
    }

    [SerializeField] public List<ExclusionRule> exclusionRules = new();
    [SerializeField] public List<AffinityRule> affinityRules = new();
    [SerializeField] private List<PrefabLimit> prefabLimits = new();

    private Dictionary<TypeOfConstruction, List<PrefabLimit>> prefabsByType;
    private Dictionary<NameOfConstruction, List<PrefabLimit>> prefabsByName;

    private Dictionary<GameObject, int> prefabSpawnCounts = new();

    private void InitializePrefabs()
    {
        if (prefabsByType != null) return;
        if (prefabsByName != null) return;

        prefabsByType = new Dictionary<TypeOfConstruction, List<PrefabLimit>>();
        prefabsByName = new Dictionary<NameOfConstruction, List<PrefabLimit>>();

        foreach (PrefabLimit prefabLimit in prefabLimits)
        {
            if (!prefabsByType.ContainsKey(prefabLimit.type))
            {
                prefabsByType[prefabLimit.type] = new List<PrefabLimit>();
            }
            if (!prefabsByName.ContainsKey(prefabLimit.name))
            {
                prefabsByName[prefabLimit.name] = new List<PrefabLimit>();
            }
            prefabsByType[prefabLimit.type].Add(prefabLimit);
            prefabsByName[prefabLimit.name].Add(prefabLimit);
            prefabSpawnCounts[prefabLimit.prefab] = 0; 
        }
    }

    public bool CanSpawn(NameOfConstruction nameToSpawn)
    {
        InitializePrefabs();

        foreach (var exclusionRule in exclusionRules)
        {
            if (IsPrefabSpawned(exclusionRule.sourceName) && exclusionRule.excludedName == nameToSpawn)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsPrefabSpawned(NameOfConstruction name)
    {
        if (prefabsByName.ContainsKey(name))
        {
            foreach (var prefabLimit in prefabsByName[name])
            {
                if (prefabSpawnCounts[prefabLimit.prefab] > 0)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public GameObject GetPrefab(TypeOfConstruction typeToGet)
    {
        InitializePrefabs();

        if (prefabsByType.ContainsKey(typeToGet))
        {
            List<PrefabLimit> PrefabDispo = new List<PrefabLimit>();

            foreach (PrefabLimit prefabLimit in prefabsByType[typeToGet])
            {
                if (prefabSpawnCounts[prefabLimit.prefab] < prefabLimit.maxSpawn && CanSpawn(prefabLimit.name))
                {
                    PrefabDispo.Add(prefabLimit);
                }
            }

            if (PrefabDispo.Count > 0)
            {
                PrefabLimit selectedPrefabLimit = PrefabDispo[Random.Range(0, PrefabDispo.Count)];
                prefabSpawnCounts[selectedPrefabLimit.prefab]++;
                return selectedPrefabLimit.prefab;
            }
        }

        return null;
    }

    public GameObject GetPrefabByName(NameOfConstruction nameToGet)
    {
        InitializePrefabs();

        if (prefabsByName.ContainsKey(nameToGet) && CanSpawn(nameToGet))
        {
            List<PrefabLimit> PrefabDispo = new List<PrefabLimit>();

            foreach (PrefabLimit prefabLimit in prefabsByName[nameToGet])
            {
                if (prefabSpawnCounts[prefabLimit.prefab] < prefabLimit.maxSpawn)
                {
                    PrefabDispo.Add(prefabLimit);
                }
            }

            if (PrefabDispo.Count > 0)
            {
                PrefabLimit selectedPrefabLimit = PrefabDispo[0];
                prefabSpawnCounts[selectedPrefabLimit.prefab]++;
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
        prefabsByName = null;
        prefabSpawnCounts.Clear(); 
        InitializePrefabs(); 
    }

}