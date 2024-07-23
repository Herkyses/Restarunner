using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

public class MapManager : MonoBehaviour
{
    private string filePath;
    private MapData mapData;

    [FormerlySerializedAs("treePrefab")] public GameObject tableSetPrefab;
    public GameObject[] tableSets;
    public GameObject rockPrefab;
    public GameObject orderBoxPf;
    public ShopItemData[] orderBoxShopItemDatas;
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

    public void SetTableTransformFromMapData()
    {
        GameObject[] tableObjects = GameObject.FindGameObjectsWithTag("TableSet");
        if (tableObjects.Length > 0)
        {
            foreach (GameObject table in tableObjects)
            {
                MapObject tableSet = new MapObject();
                tableSet.type = "TableSet";
                tableSet.posX = table.transform.position.x;
                tableSet.posY = table.transform.position.y;
                tableSet.posZ = table.transform.position.z;
                tableSet.rotX = table.transform.rotation.eulerAngles.x;
                tableSet.rotY = table.transform.rotation.eulerAngles.y;
                tableSet.rotZ = table.transform.rotation.eulerAngles.z;
                tableSet.tableID = table.GetComponent<TableSet>().tableTypeID;
                mapData.objects.Add(tableSet);
            }
        }
        
    }
    public void SetOrderBoxTransformFromMapData()
    {
        GameObject[] orderBoxObjects = GameObject.FindGameObjectsWithTag("OrderBox");
        if (orderBoxObjects.Length > 0)
        {
            foreach (GameObject orderBoxObject in orderBoxObjects)
            {
                MapObject orderBox = new MapObject();
                orderBox.type = "OrderBox";
                orderBox.posX = orderBoxObject.transform.position.x;
                orderBox.posY = orderBoxObject.transform.position.y;
                orderBox.posZ = orderBoxObject.transform.position.z;
                orderBox.rotX = orderBoxObject.transform.rotation.eulerAngles.x;
                orderBox.rotY = orderBoxObject.transform.rotation.eulerAngles.y;
                orderBox.rotZ = orderBoxObject.transform.rotation.eulerAngles.z;
                orderBox.shopItemData = orderBoxObject.gameObject.GetComponent<OrderBox>().GetShopItemData();
                mapData.objects.Add(orderBox);
            }
        }
        
    }
    public void SaveMap()
    {
        // Mevcut sahnede bulunan objelerin bilgilerini alıp kaydedelim
        mapData.objects.Clear(); // Önceki verileri temizleyelim

        SetTableTransformFromMapData();
        SetOrderBoxTransformFromMapData();
        /*GameObject[] rockObjects = GameObject.FindGameObjectsWithTag("Rock");
        foreach (GameObject rock in rockObjects)
        {
            MapObject rockObject = new MapObject();
            rockObject.type = "Rock";
            rockObject.posX = rock.transform.position.x;
            rockObject.posY = rock.transform.position.y;
            rockObject.posZ = rock.transform.position.z;
            rockObject.rotX = rock.transform.rotation.eulerAngles.x;
            rockObject.rotY = rock.transform.rotation.eulerAngles.y;
            rockObject.rotZ = rock.transform.rotation.eulerAngles.z;
            mapData.objects.Add(rockObject);
        }*/

        // JSON olarak kaydetme
        string json = JsonUtility.ToJson(mapData);
        File.WriteAllText(filePath, json);

        Debug.Log("Map saved.");
    }

    public void LoadMap()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            mapData = JsonUtility.FromJson<MapData>(json);

            // Yükleme işlemi
            foreach (MapObject mapObject in mapData.objects)
            {
                GameObject prefab = GetPrefabByType(mapObject.type,mapObject.tableID);
                if (mapObject.type == "TableSet")
                {
                    if (prefab != null)
                    {
                        Vector3 position = new Vector3(mapObject.posX, mapObject.posY, mapObject.posZ);
                        Quaternion rotation = Quaternion.Euler(mapObject.rotX, mapObject.rotY, mapObject.rotZ);
                        var table = Instantiate(prefab, position, rotation);
                        table.transform.SetParent(TableController.Instance.TableTransform);
                    }
                }
                if (mapObject.type == "OrderBox")
                {
                    if (prefab != null)
                    {
                        Vector3 position = new Vector3(mapObject.posX, mapObject.posY, mapObject.posZ);
                        Quaternion rotation = Quaternion.Euler(mapObject.rotX, mapObject.rotY, mapObject.rotZ);
                        var orderBox = Instantiate(prefab, position, rotation);
                        orderBox.gameObject.GetComponent<OrderBox>().SetShopItemData(mapObject.shopItemData); 
                    }
                }
                
            }

            Debug.Log("Map loaded.");
        }
        else
        {
            Debug.LogWarning("Map file not found.");
        }
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

    private GameObject GetPrefabByType(string type,int tableID = -1)
    {
        if (type == "TableSet")
        {
            return tableSets[tableID];
        }

        if (type == "OrderBox")
        {
            return orderBoxPf;
        }
        return null;
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
