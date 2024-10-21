using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameVfxManager : MonoBehaviour
{
    public static GameVfxManager Instance;
    [System.Serializable]
    public class VFXPool
    {
        public GameObject vfxPrefab;
        public int poolSize;
        public Queue<GameObject> vfxQueue;
    }

    public List<VFXPool> vfxPools;
    private Dictionary<GameObject, VFXPool> vfxPoolDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        vfxPoolDictionary = new Dictionary<GameObject, VFXPool>();

        foreach (VFXPool pool in vfxPools)
        {
            pool.vfxQueue = new Queue<GameObject>();
            
            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject obj = Instantiate(pool.vfxPrefab);
                obj.SetActive(false);
                obj.transform.SetParent(transform);
                pool.vfxQueue.Enqueue(obj);
            }

            vfxPoolDictionary.Add(pool.vfxPrefab, pool);
        }
    }

    public GameObject SpawnVFX(GameObject vfxPrefab, Vector3 position, Quaternion rotation)
    {
        if (!vfxPoolDictionary.ContainsKey(vfxPrefab))
        {
            Debug.LogWarning("VFX prefab is not in the pool.");
            return null;
        }

        VFXPool pool = vfxPoolDictionary[vfxPrefab];
        
        if (pool.vfxQueue.Count > 0)
        {
            GameObject vfxToSpawn = pool.vfxQueue.Dequeue();
            vfxToSpawn.SetActive(true);
            vfxToSpawn.transform.position = position;
            vfxToSpawn.transform.rotation = rotation;
            StartCoroutine(DelayDeactiveVfx(vfxToSpawn));
            return vfxToSpawn;
        }
        else
        {
            GameObject newVFX = Instantiate(vfxPrefab, position, rotation);
            newVFX.SetActive(true);
            StartCoroutine(DelayDeactiveVfx(newVFX));
            return newVFX;
        }
    }

    public IEnumerator DelayDeactiveVfx(GameObject vfxPrefab)
    {
        yield return new WaitForSeconds(0.5f);
        ReturnVFX(vfxPools[1].vfxPrefab,vfxPrefab);
    }

    public void ReturnVFX(GameObject vfxPrefab, GameObject vfxToReturn)
    {
        if (!vfxPoolDictionary.ContainsKey(vfxPrefab))
        {
            Debug.LogWarning("VFX prefab is not in the pool.");
            return;
        }

        vfxToReturn.SetActive(false);
        vfxPoolDictionary[vfxPrefab].vfxQueue.Enqueue(vfxToReturn);
    }
}
