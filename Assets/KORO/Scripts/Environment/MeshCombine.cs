using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Tüm çocuk objelerin mesh'lerini al
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            // MeshRenderer bileşeninin varlığını kontrol edin
            MeshRenderer meshRenderer = meshFilters[i].GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                Debug.LogWarning($"MeshRenderer bileşeni bulunamadı: {meshFilters[i].gameObject.name}");
                continue;
            }

            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false); // Orijinal mesh'leri kapat
        }

        // Yeni bir mesh oluştur ve birleştirilen mesh'leri içine yerleştir
        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine);

        // Yeni mesh'i mevcut GameObject'e ekleyin
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = combinedMesh;

        // Yeni mesh renderer ekleyin ve birleştirilmiş mesh'e ilk materyali uygulayın
        MeshRenderer newMeshRenderer = gameObject.AddComponent<MeshRenderer>();
        if (meshFilters.Length > 0 && meshFilters[0].GetComponent<MeshRenderer>() != null)
        {
            newMeshRenderer.sharedMaterial = meshFilters[0].GetComponent<MeshRenderer>().sharedMaterial;
        }
        else
        {
            Debug.LogWarning("Materyal atanamadı. Lütfen sahnede en az bir MeshRenderer olduğundan emin olun.");
        }

        // Objeyi etkinleştir
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
