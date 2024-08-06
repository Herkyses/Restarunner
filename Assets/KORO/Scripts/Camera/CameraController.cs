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
    public Transform PlayerTakedObjectTransformParent;
    public GameObject CleanTool;
    public GameObject FightTool;
    public GameObject CleanToolChild;
    public GameObject FightToolChild;
    public bool CleanToolChildMove = true;
    public bool FightToolChildMove = true;
    public Sequence CleanToolSequence;
    public Sequence FightToolSequence;

    /*public float CamTransformXDifference = 0;
    public float CamTransformYDifference = 0;
    public float CamTransformZDifference = 0;
    public float CamTransformAngle = 0;*/
    private Transform CamLookHeight = null;

    private Quaternion rotation;
    private Vector3 position;
    
    public Vector3 CamPosition = new Vector3();
    public Vector3 CleanToolFirstRotation = new Vector3();
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
        var localRotforClean = CleanToolChild.transform.localRotation.eulerAngles;
        CleanToolFirstRotation = new Vector3(localRotforClean.x, localRotforClean.y, localRotforClean.z);
        _canFollow = true;
        GameSceneCanvas.Instance.CanMove = true;
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

    public void MoveCleanTool()
    {
        if (CleanToolChildMove)
        {
            CleanToolChildMove = false;
            if (CleanToolSequence != null)
            {
                CleanToolSequence.Kill();
            }

            var toolChild = CleanToolChild.transform.localRotation.eulerAngles;
            CleanToolSequence = DOTween.Sequence();
            CleanToolSequence.Append(CleanToolChild.transform.DOLocalRotate(new Vector3(40f,toolChild.y , toolChild.z), 0.2f)).
                Append(CleanToolChild.transform.DOLocalRotate(new Vector3(20f,toolChild.y , toolChild.z), 0.2f)).
                Append(CleanToolChild.transform.DOLocalRotate(new Vector3(40f,toolChild.y , toolChild.z), 0.2f)).
                Append(CleanToolChild.transform.DOLocalRotate(CleanToolFirstRotation,0.2f));
            
            CleanToolSequence.OnComplete(() =>
            {
                CleanToolChildMove = true;
            });      
        }
    }
    public void MoveFightTool()
    {
        if (FightToolChildMove)
        {
            var firstRot = FightToolChild.transform.localRotation.eulerAngles;

            FightToolChildMove = false;
            if (FightToolSequence != null)
            {
                FightToolSequence.Kill();
            }

            var toolChild = FightToolChild.transform.localRotation.eulerAngles;
            FightToolSequence = DOTween.Sequence();
            FightToolSequence.Append(FightToolChild.transform.DOLocalRotate(new Vector3(toolChild.x,toolChild.y , 50f), 0.1f)).
                Append(FightToolChild.transform.DOLocalRotate(new Vector3(toolChild.x,toolChild.y , toolChild.z), 0.1f)).
                Append(FightToolChild.transform.DOLocalRotate(firstRot,0.1f));
            
            FightToolSequence.OnComplete(() =>
            {
                FightToolChildMove = true;
            });      
        }
    }
    private void LookTarget()
    {
        transform.DOLookAt(PlayerTransform.position, 0.2f);
        //transform.LookAt(PlayerTransform);
    }
    void LateUpdate()
    {
        if (GameSceneCanvas.Instance.CanMove)
        {
            if (PlayerTransform != null)
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
        
        
        
    }

    public void CleanToolActive(bool active)
    {
        var playerState = Player.Instance.PlayerStateType;
        if (playerState == Enums.PlayerStateType.Free || playerState == Enums.PlayerStateType.Cleaner|| playerState == Enums.PlayerStateType.Fight)
        {

            CleanTool.SetActive(active);
            if (active)
            {
                Player.Instance.PlayerStateType = Enums.PlayerStateType.Cleaner;
                FightTool.SetActive(false);
                Player.Instance.CanFight = false;
            }
            else
            {
                Player.Instance.PlayerStateType = Enums.PlayerStateType.Free;

            }
        }
        
    }
    public void FightToolActive(bool active)
    {
        var playerState = Player.Instance.PlayerStateType;
        if (playerState == Enums.PlayerStateType.Free || playerState == Enums.PlayerStateType.Fight|| playerState == Enums.PlayerStateType.Cleaner|| playerState == Enums.PlayerStateType.GiveFood)
        {

            FightTool.SetActive(active);
            if (active)
            {
                Player.Instance.PlayerStateType = Enums.PlayerStateType.Fight;
                CleanTool.SetActive(false);
                Player.Instance.CanCleanRubbish = false;
                if (PlayerOrderController.Instance.TakedFood)
                {
                    PlayerOrderController.Instance.FoodTable.gameObject.SetActive(false);
                }
            }
            else
            {
                if (PlayerOrderController.Instance.TakedFood)
                {
                    PlayerOrderController.Instance.FoodTable.gameObject.SetActive(true);
                    Player.Instance.PlayerStateType = Enums.PlayerStateType.GiveFood;

                }
                else
                {
                    Player.Instance.PlayerStateType = Enums.PlayerStateType.Free;

                }
            }
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
