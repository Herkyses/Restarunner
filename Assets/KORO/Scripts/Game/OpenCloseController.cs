using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class OpenCloseController : MonoBehaviour,IInterectableObject
{
    [SerializeField] private string[] texts = new [] {"Check Order "};
    [SerializeField] private string[] textsButtons = new [] {"E"};
    [SerializeField] private Outline outline;
    
    public Tween OpenCloseTween;
    public Sequence OpenCloseSequence;
    public Transform OpenTransform;
    public Transform CloseTransform;
    public Transform Parent;
    public bool CanMove = true;

    public static Action RestaurantOpened;
    public void InterectableObjectRun()
    {
        
        if (CanMove)
        {
            CanMove = false;
            if (OpenCloseSequence != null)
            {
                OpenCloseSequence.Kill();
            }
            ControllerManager.Instance.PlaceController.IsRestaurantOpen = !ControllerManager.Instance.PlaceController.IsRestaurantOpen;
            if (ControllerManager.Instance.PlaceController.IsRestaurantOpen)
            {
                Debug.Log("restaurantope" + PlayerPrefsManager.Instance.LoadPlayerTutorialStep());
               
                RestaurantOpened?.Invoke();

                OpenCloseSequence = DOTween.Sequence();
                OpenCloseSequence.Append(Parent.DORotate(new Vector3(-90, 0, 0), 0.2f,RotateMode.LocalAxisAdd)).
                    Append(Parent.DORotate(new Vector3(0, 180, 0), 0.2f,RotateMode.LocalAxisAdd)).
                    Append(Parent.DORotate(new Vector3(-90, 0, 0), 0.2f,RotateMode.LocalAxisAdd));
                // Y ekseni etrafında döndür
                //OpenCloseSequence.Append(transform.DORotate(new Vector3(0, 90, 0), 0.2f));
                Debug.Log("animbasladi" + OpenCloseSequence);

            
                OpenCloseSequence.OnComplete(() =>
                {
                    CanMove = true;
                });            
            }
            else
            {
                OpenCloseSequence = DOTween.Sequence();
                OpenCloseSequence.Append(Parent.DORotate(new Vector3(90, 0, 0), 0.2f,RotateMode.LocalAxisAdd)).
                    Append(Parent.DORotate(new Vector3(0, 180, 0), 0.2f,RotateMode.LocalAxisAdd)).
                    Append(Parent.DORotate(new Vector3(90, 0, 0), 0.2f,RotateMode.LocalAxisAdd));
                // Y ekseni etrafında döndür
                //OpenCloseSequence.Append(transform.DORotate(new Vector3(0, 90, 0), 0.2f));

            
                OpenCloseSequence.OnComplete(() =>
                {
                    CanMove = true;
                });            

            }
        }
        
    }

    private void Start()
    {
        textsButtons = new []{"E"};
        texts = new []{"E"};
        outline = GetComponent<Outline>();
    }

    public void ShowOutline(bool active)
    {
        outline.enabled = active;
    }

    public Outline GetOutlineComponent()
    {
        return outline;
    }

    public string GetInterectableText()
    {
        if (ControllerManager.Instance.PlaceController.IsRestaurantOpen)
        {
            return "";
        }
        else
        {
            return "";

        }
    }

    public void Move()
    {
        
    }
    public void Open()
    {
        
    }
    public string[] GetInterectableTexts()
    {
        if (ControllerManager.Instance.PlaceController.IsRestaurantOpen)
        {
            texts[0] = "Key_Close_Restaurant";
            return texts;
        }
        else
        {
            texts[0] = "Key_Open_Restaurant";
            return texts;

        }
    }
    public string[] GetInterectableButtons()
    {
        return textsButtons;
    }
    public Enums.PlayerStateType GetStateType()
    {
        return Enums.PlayerStateType.Free;
    }
}
