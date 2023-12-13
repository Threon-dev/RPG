using UnityEngine;

namespace RPG.Core
{
    public class PersistantObjectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject persistantObjectPrefab;

        private static bool hasSpawned;
        private void Awake()
        {
            if (hasSpawned) return;
            hasSpawned = true;
            SpawnPersistentObjects();
        }

        private void SpawnPersistentObjects()
        {
            GameObject persistantObject = Instantiate(persistantObjectPrefab);
            DontDestroyOnLoad(persistantObject);
        }
    }

}