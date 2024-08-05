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
    
    
    [FormerlySerializedAs("FoodParent")] public Transform TableFoodParent;
    public Transform FoodParent;
    public Transform OrderBillParent;
    public Transform CustomerAIObjectParent;
    public Transform CustomerRagdollAIObjectParent;
    public Transform OrderBoxParent;
    
    
    
    public int initialSize = 10;
    
    private Queue<GameObject> FoodTablePool = new Queue<GameObject>();
    private Queue<GameObject> FoodPool= new Queue<GameObject>();
    private Queue<GameObject> OrderBillPool= new Queue<GameObject>();
    private Queue<GameObject> CustomerAIPool= new Queue<GameObject>();
    private Queue<GameObject> CustomerRagdollAIPool= new Queue<GameObject>();
    private Queue<GameObject> OrderBoxPool= new Queue<GameObject>();
    
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
    void Start()
    {
        
        CreatePoolObject(FoodTablePool,TableFoodParent,FoodTableObject);
        CreatePoolObject(FoodPool,FoodParent,FoodObject);
        CreatePoolObject(OrderBillPool,OrderBillParent,OrderBill);
        CreatePoolObject(CustomerAIPool,CustomerAIObjectParent,CustomerAIObject);
        CreatePoolObject(OrderBoxPool,OrderBoxParent,OrderBoxObject);
        CreatePoolObject(CustomerRagdollAIPool,CustomerRagdollAIObjectParent,CustomerRagdollAIObject);
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
    
    public GameObject GetFromPoolForOrderBill()
    {
        if (OrderBillPool.Count == 0)
        {
            GameObject obj = Instantiate(OrderBill);
            obj.SetActive(false);
            OrderBillPool.Enqueue(obj);
        }

        GameObject pooledObject = OrderBillPool.Dequeue();
        pooledObject.SetActive(true);
        return pooledObject;
    }
    public GameObject GetFromPool()
    {
        if (FoodTablePool.Count == 0)
        {
            GameObject obj = Instantiate(FoodTableObject);
            obj.SetActive(false);
            FoodTablePool.Enqueue(obj);
        }

        GameObject pooledObject = FoodTablePool.Dequeue();
        pooledObject.SetActive(true);
        return pooledObject;
    }
    public GameObject GetFromPoolForFood()
    {
        if (FoodPool.Count == 0)
        {
            GameObject obj = Instantiate(FoodObject);
            obj.SetActive(false);
            FoodPool.Enqueue(obj);
        }

        GameObject pooledObject = FoodPool.Dequeue();
        pooledObject.SetActive(true);
        return pooledObject;
    }
    public GameObject GetCustomerAI()
    {
        if (CustomerAIPool.Count == 0)
        {
            GameObject obj = Instantiate(CustomerAIObject);
            obj.SetActive(false);
            CustomerAIPool.Enqueue(obj);
        }

        GameObject pooledObject = CustomerAIPool.Dequeue();
        pooledObject.SetActive(true);
        return pooledObject;
    }
    public GameObject GetCustomerRagdollAI()
    {
        if (CustomerRagdollAIPool.Count == 0)
        {
            GameObject obj = Instantiate(CustomerRagdollAIObject);
            obj.SetActive(false);
            CustomerRagdollAIPool.Enqueue(obj);
        }

        GameObject pooledObject = CustomerRagdollAIPool.Dequeue();
        pooledObject.SetActive(true);
        return pooledObject;
    }
    public GameObject GetFromPoolForOrderBox()
    {
        if (OrderBoxPool.Count == 0)
        {
            GameObject obj = Instantiate(OrderBoxObject);
            obj.SetActive(false);
            OrderBoxPool.Enqueue(obj);
        }

        GameObject pooledObject = OrderBoxPool.Dequeue();
        pooledObject.SetActive(true);
        return pooledObject;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.transform.SetParent(TableFoodParent);
        obj.SetActive(false);
        FoodTablePool.Enqueue(obj);
    }
    public void ReturnToPoolForFood(GameObject obj)
    {
        obj.transform.SetParent(FoodParent);
        obj.SetActive(false);
        FoodPool.Enqueue(obj);
    }
    public void ReturnToPoolForOrderBill(GameObject obj)
    {
        obj.transform.SetParent(OrderBillParent);
        obj.SetActive(false);
        OrderBillPool.Enqueue(obj);
    }
    public void ReturnToPoolForCustomerAI(GameObject obj)
    {
        obj.transform.SetParent(CustomerAIObjectParent);
        obj.SetActive(false);
        CustomerAIPool.Enqueue(obj);
    }
    public void ReturnToPoolForRagdollCustomerAI(GameObject obj)
    {
        obj.transform.SetParent(CustomerRagdollAIObjectParent);
        obj.SetActive(false);
        CustomerRagdollAIPool.Enqueue(obj);
    }
    public void ReturnToPoolForOrderBox(GameObject obj)
    {
        obj.transform.SetParent(OrderBoxParent);
        obj.SetActive(false);
        OrderBoxPool.Enqueue(obj);
    }
}
