using UnityEngine;

public class DragObjectWithMouse : MonoBehaviour
{
    public Transform objectToMove; // Hareket ettirilecek obje
    private bool isDragging = false;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // E tuşuna basıldığında hareket modunu aktif et/deaktif et
        if (Input.GetKeyDown(KeyCode.H))
        {
            isDragging = !isDragging;
        }
        if (Input.GetKeyUp(KeyCode.H))
        {
            isDragging = false;
        }
        

        // Hareket modu aktifse, fareyi takip et ve objeyi taşı
        if (isDragging)
        {
            MoveObjectToMousePosition();
        }
    }

    void MoveObjectToMousePosition()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            objectToMove.position = hit.point; // Objenin pozisyonunu fare ile tıklanan noktaya taşı
        }
    }
}