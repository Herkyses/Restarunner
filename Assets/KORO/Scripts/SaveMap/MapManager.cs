using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class MapManager : MonoBehaviour
{
    private string filePath;
    private MapData mapData;

    [Inject] private DiContainer _container;
    [FormerlySerializedAs("treePrefab")] public GameObject tableSetPrefab;
    public GameObject rockPrefab;
    public GameObject orderBoxPf;
    public ShopItemData[] tableShopItemDatas;
    public ShopItemData[] decorationShopItemDatas;
    public static MapManager Instance;
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
        filePath = Application.persistentDataPath + "/mapData.json";
        mapData = new MapData();
    }

    private void Update()
    {
        /*// Tuşlara atama işlemleri burada gerçekleştirilecek.
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveMap();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            LoadMap();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            ResetMap();
        }*/
    }
    
    private void CollectObjects(string tag, string type, System.Action<GameObject, MapObject> additionalDataAction = null)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            MapObject mapObject = new MapObject
            {
                type = type,
                posX = obj.transform.position.x,
                posY = obj.transform.position.y,
                posZ = obj.transform.position.z,
                rotX = obj.transform.rotation.eulerAngles.x,
                rotY = obj.transform.rotation.eulerAngles.y,
                rotZ = obj.transform.rotation.eulerAngles.z
            };
            additionalDataAction?.Invoke(obj, mapObject);  // Ek veri işlemesi (tableID, shopItemData gibi)
            mapData.objects.Add(mapObject);
        }
    }
    public void SaveMap()
    {
        mapData.objects.Clear(); // Önceki verileri temizleyelim

        CollectObjects("TableSet", "TableSet", (obj, mapObject) =>
        {
            mapObject.tableID = obj.GetComponent<TableSet>().tableTypeID;
        });
        CollectObjects("OrderBox", "OrderBox", (obj, mapObject) =>
        {
            mapObject.shopItemData = obj.GetComponent<OrderBox>().GetShopItemData();
        });
        CollectObjects("Decoration", "Decoration", (obj, mapObject) =>
        {
            mapObject.decorationID = obj.GetComponent<DecorationObject>().decorationID;
        });

        // JSON olarak kaydetme
        string json = JsonUtility.ToJson(mapData);
        File.WriteAllText(filePath, json);
        Debug.Log("Map saved.");
    }
    
    public void LoadMap()
    {
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("Map file not found.");
            return;
        }

        string json = File.ReadAllText(filePath);
        mapData = JsonUtility.FromJson<MapData>(json);

        // Yükleme işlemi
        foreach (MapObject mapObject in mapData.objects)
        {
            GameObject prefab = GetPrefabByType(mapObject.type, mapObject.tableID, mapObject.decorationID);
            if (prefab != null)
            {
                Vector3 position = new Vector3(mapObject.posX, mapObject.posY, mapObject.posZ);
                Quaternion rotation = Quaternion.Euler(mapObject.rotX, mapObject.rotY, mapObject.rotZ);
                GameObject instantiatedObj = Instantiate(prefab, position, rotation);

                if (mapObject.type == "TableSet")
                {
                    instantiatedObj.transform.SetParent(ControllerManager.Instance.Tablecontroller.TableTransform);
                }
                else if (mapObject.type == "OrderBox")
                {
                    instantiatedObj.GetComponent<OrderBox>().SetShopItemData(mapObject.shopItemData);
                }
                else if (mapObject.type == "Decoration")
                {
                    instantiatedObj.GetComponent<DecorationObject>().decorationID = mapObject.decorationID;
                }
            }
        }
        Debug.Log("Map loaded.");
    }

    public void ResetMap()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Map file deleted.");
        }
        else
        {
            Debug.LogWarning("No map file to delete.");
        }
    }
    
    
    private GameObject GetPrefabByType(string type, int tableID = -1, int decorationID = -1)
    {
        switch (type)
        {
            case "TableSet": return tableShopItemDatas[tableID].ItemObject;
            case "OrderBox": return orderBoxPf;
            case "Decoration": return decorationShopItemDatas[decorationID].ItemObject;
            default: return null;
        }
    }
}

[System.Serializable]
public class MapData
{
    public List<MapObject> objects = new List<MapObject>();
}

[System.Serializable]
public class Wrapper
{
    public List<MapObject> objects;
    public Wrapper(List<MapObject> objects)
    {
        this.objects = objects;
    }
}
