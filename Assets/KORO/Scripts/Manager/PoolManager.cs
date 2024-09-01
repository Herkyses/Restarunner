using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;
    public static Action IsPoolManagerInitiliazed;
    
    
    public GameObject FoodTableObject;
    public GameObject FoodObject;
    public GameObject OrderBill;
    public GameObject CustomerAIObject;
    public GameObject CustomerRagdollAIObject;
    public GameObject OrderBoxObject;
    public GameObject FoodIngredientObject;
    
    
    [FormerlySerializedAs("FoodParent")] public Transform TableFoodParent;
    public Transform FoodParent;
    public Transform OrderBillParent;
    public Transform CustomerAIObjectParent;
    public Transform CustomerRagdollAIObjectParent;
    public Transform OrderBoxParent;
    public Transform FoodIngredientParent;
    
    
    
    public int initialSize = 10;
    
    private Queue<GameObject> FoodTablePool = new Queue<GameObject>();
    private Queue<GameObject> FoodPool= new Queue<GameObject>();
    private Queue<GameObject> OrderBillPool= new Queue<GameObject>();
    private Queue<GameObject> CustomerAIPool= new Queue<GameObject>();
    private Queue<GameObject> CustomerRagdollAIPool= new Queue<GameObject>();
    private Queue<GameObject> OrderBoxPool= new Queue<GameObject>();
    private Queue<GameObject> FoodIngredientPool = new Queue<GameObject>();
    
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

    }
    public void Initiliaze()
    {
        
        CreatePoolObject(FoodTablePool,TableFoodParent,FoodTableObject);
        CreatePoolObject(FoodPool,FoodParent,FoodObject);
        CreatePoolObject(OrderBillPool,OrderBillParent,OrderBill);
        CreatePoolObject(CustomerAIPool,CustomerAIObjectParent,CustomerAIObject);
        CreatePoolObject(OrderBoxPool,OrderBoxParent,OrderBoxObject);
        CreatePoolObject(CustomerRagdollAIPool,CustomerRagdollAIObjectParent,CustomerRagdollAIObject);
        CreatePoolObject(FoodIngredientPool,FoodIngredientParent,FoodIngredientObject);
        IsPoolManagerInitiliazed?.Invoke();
    }

   

    public void CreatePoolObject(Queue<GameObject> poolList,Transform parent,GameObject poolObject)
    {
        poolList = new Queue<GameObject>();
        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = Instantiate(poolObject);
            obj.transform.SetParent(parent);
            obj.SetActive(false);
            poolList.Enqueue(obj);
        }
    }
    public GameObject GetFromPool(Queue<GameObject> pool, GameObject prefab)
    {
        if (pool.Count == 0)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }

        GameObject pooledObject = pool.Dequeue();
        pooledObject.SetActive(true);
        return pooledObject;
    }
    public GameObject GetFromPoolForOrderBill()
    {
        return GetFromPool(OrderBillPool, OrderBill);
    }
    public GameObject GetFromPoolForFoodIngredient()
    {
        return GetFromPool(FoodIngredientPool, FoodIngredientObject);
    }
    public GameObject GetFromPoolForFoodTable()
    {
        return GetFromPool(FoodTablePool, FoodTableObject);
    }
    public GameObject GetFromPoolForFood()
    {
        return GetFromPool(FoodPool, FoodObject);
    }
    public GameObject GetCustomerAI()
    {
        return GetFromPool(CustomerAIPool, CustomerAIObject);
    }
    public GameObject GetCustomerRagdollAI()
    {
        return GetFromPool(CustomerRagdollAIPool, CustomerRagdollAIObject);
    }
    public GameObject GetFromPoolForOrderBox()
    {
        return GetFromPool(OrderBoxPool, OrderBoxObject);
    }

    public void ReturnToPoolForTrays(GameObject obj)
    {
        ReturnToPool(obj,FoodTablePool,TableFoodParent);
    }
    public void ReturnToPoolForFood(GameObject obj)
    {
        ReturnToPool(obj,FoodPool,FoodParent);
    }
    public void ReturnToPoolForOrderBill(GameObject obj)
    {
        ReturnToPool(obj,OrderBillPool,OrderBillParent);
    }
    public void ReturnToPoolForCustomerAI(GameObject obj)
    {
        ReturnToPool(obj,CustomerAIPool,CustomerAIObjectParent);
    }
    public void ReturnToPoolForRagdollCustomerAI(GameObject obj)
    {
        ReturnToPool(obj,CustomerRagdollAIPool,CustomerRagdollAIObjectParent);
    }
    public void ReturnToPoolForOrderBox(GameObject obj)
    {
        ReturnToPool(obj,OrderBoxPool,OrderBoxParent);
    }
    public void ReturnToPoolForFoodIngredient(GameObject obj)
    {
        ReturnToPool(obj,FoodIngredientPool,FoodIngredientParent);
    }

    public void ReturnToPool(GameObject obj,Queue<GameObject> pool, Transform parentTransform)
    {
        obj.transform.SetParent(parentTransform);
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
