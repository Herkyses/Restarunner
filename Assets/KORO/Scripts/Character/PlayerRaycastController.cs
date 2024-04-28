using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycastController : MonoBehaviour
{
    public GameObject TargetPrice;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SendRaycastCoroutine());
    }

    // Update is called once per frame
    

    public IEnumerator SendRaycastCoroutine()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(0.5f);
            // Kameranın oyun ekranındaki orta noktasını bul
            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);

            // Ekran ortasından bir ray oluştur (kamera tarafından)
            Ray ray = Camera.main.ScreenPointToRay(screenCenter);
            
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit,10f))
            {
                if (hit.collider.gameObject.GetComponent<Place>())
                {
                    var place = hit.collider.gameObject.GetComponent<Place>();
                    //place.ShowPlacePrice();
                }
                // Raycast sonucunu işle
                Debug.Log("Raycast isabet etti: " + hit.collider.gameObject.name);
                Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.green, 2f);

            }
            else
            {
                // Raycast hiçbir şeye isabet etmedi
                Debug.Log("Raycast hiçbir şeye isabet etmedi.");
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2f);

            }
        }
    }
}
