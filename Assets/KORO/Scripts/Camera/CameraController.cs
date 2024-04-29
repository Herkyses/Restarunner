using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
//using DG.Tweening;

using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    public Transform PlayerTransform;
    
    /*public float CamTransformXDifference = 0;
    public float CamTransformYDifference = 0;
    public float CamTransformZDifference = 0;
    public float CamTransformAngle = 0;*/
    private Transform CamLookHeight = null;

    private Quaternion rotation;
    private Vector3 position;
    
    public Vector3 CamPosition = new Vector3();
    public float CamSpeed = 2f;
    public float currentRotationAngle;
    public float currentRotationAngleY;
    
    public float distance = 5.0f; // Kameranın karakterden uzaklığı
    public float height = 2.0f; // Kameranın yüksekliği
    public float rotationSpeed = 5.0f;
    public float damping = 1.0f;
    public float MouseX;
    public float MouseY;
    public float MinAxisY;
    public float MaxAxisY;
    //public Player RelatedPlayer;
    private float mouseX;
    private float mouseY;
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    private bool _canFollow;
    
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

    private void Start()
    {
        _canFollow = true;
    }

    public void CanFollowController(bool zort)
    {
        _canFollow = zort;

        if (!zort)
        {
            distance = 4f;
            height = 2f;
            //transform.DOMove(transform.position + Vector3.up * 0.5f + (PlayerTransform.forward * 3f), 0.5f).OnComplete(LookTarget);
            //transform.position += (Vector3.up*0.5f+(PlayerTransform.forward*3f));
        }
        else
        {
            distance = -0.26f;
            height = 1.74f;

        }
        
    }
    private void LookTarget()
    {
        transform.DOLookAt(PlayerTransform.position, 0.2f);
        //transform.LookAt(PlayerTransform);
    }
    void LateUpdate()
    {
        
        if (PlayerTransform != null && _canFollow )
        {
            mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            currentRotationAngle += mouseX;
            rotationY -= mouseY;
            rotationY = Mathf.Clamp(rotationY, MinAxisY, MaxAxisY); // Kameranın aşırı yukarı ya da aşağı dönmesini engelle

            rotation = Quaternion.Euler(rotationY, currentRotationAngle, 0);
            position = PlayerTransform.position - (rotation * Vector3.forward * distance) + (Vector3.up * height);

            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * damping);
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * damping);
            
            //PlayerTransform.Rotate(Vector3.up * mouseX); // Karakterin yatay dönüşü
            //PlayerTransform.rotation = Quaternion.Euler(new Vector3(PlayerTransform.rotation.x,PlayerTransform.rotation.y+mouseX,PlayerTransform.rotation.z));
            PlayerTransform.rotation = Quaternion.Euler(new Vector3(0,Quaternion.LookRotation(transform.forward).eulerAngles.y,0));
        }
        else if(!_canFollow)
        {
            mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            currentRotationAngle += mouseX;
            rotationY -= mouseY;
            rotationY = Mathf.Clamp(rotationY, -90f, 90f); // Kameranın aşırı yukarı ya da aşağı dönmesini engelle

            rotation = Quaternion.Euler(rotationY, currentRotationAngle, 0);
            position = PlayerTransform.position - (rotation * Vector3.forward * distance) + (Vector3.up * height);

            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * damping);
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * damping);
        }
        
        
    }

    

    /*public float GetRotation()
    {
        if (RelatedPlayer.Object.InputAuthority)
        {
            return currentRotationAngle;
        }

        return 0;
    }*/
    
}
