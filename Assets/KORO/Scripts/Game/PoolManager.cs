using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    public GameObject FoodTableObject;
    public GameObject FoodObject;
    public GameObject OrderBill;
    [FormerlySerializedAs("FoodParent")] public Transform TableFoodParent;
    public Transform FoodParent;
    public Transform OrderBillParent;
    public int initialSize = 10;
    
    private Queue<GameObject> FoodTablePool;
    private Queue<GameObject> FoodPool;
    private Queue<GameObject> OrderBillPool;
    
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
        FoodTrayPool();
        FoodPoolMethod();
        OrderBillMethod();
    }

    public void FoodTrayPool()
    {
        FoodTablePool = new Queue<GameObject>();
        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = Instantiate(FoodTableObject);
            obj.transform.SetParent(TableFoodParent);
            obj.SetActive(false);
            FoodTablePool.Enqueue(obj);
        }
    }

    public void FoodPoolMethod()
    {
        FoodPool = new Queue<GameObject>();
        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = Instantiate(FoodObject);
            obj.transform.SetParent(FoodParent);
            obj.SetActive(false);
            FoodPool.Enqueue(obj);
        }
    }
    public void OrderBillMethod()
    {
        OrderBillPool = new Queue<GameObject>();
        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = Instantiate(OrderBill);
            obj.transform.SetParent(OrderBillParent);
            obj.SetActive(false);
            OrderBillPool.Enqueue(obj);
        }
    }
    public GameObject GetFromPoolForOrderBill()
    {
        if (OrderBillPool.Count == 0)
        {
            GameObject obj = Instantiate(OrderBill);
            obj.SetActive(false);
            FoodTablePool.Enqueue(obj);
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
}
