using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class OpenCloseController : MonoBehaviour,IInterectableObject
{
    [SerializeField] private string[] texts = new [] {"Check Order "};
    [SerializeField] private string[] textsButtons = new [] {"E"};
    
    public Tween OpenCloseTween;
    public Sequence OpenCloseSequence;
    public Transform OpenTransform;
    public Transform CloseTransform;
    public Transform Parent;
    public bool CanMove = true;
    public void InterectableObjectRun()
    {
        if (CanMove)
        {
            CanMove = false;
            if (OpenCloseSequence != null)
            {
                OpenCloseSequence.Kill();
            }
            PlaceController.RestaurantIsOpen = !PlaceController.RestaurantIsOpen;
            if (PlaceController.RestaurantIsOpen)
            {
                OpenCloseSequence = DOTween.Sequence();
                OpenCloseSequence.Append(Parent.DORotate(new Vector3(-90, 0, 0), 0.2f,RotateMode.LocalAxisAdd)).
                    Append(Parent.DORotate(new Vector3(0, 180, 0), 0.2f,RotateMode.LocalAxisAdd)).
                    Append(Parent.DORotate(new Vector3(-90, 0, 0), 0.2f,RotateMode.LocalAxisAdd));
                // Y ekseni etrafında döndür
                //OpenCloseSequence.Append(transform.DORotate(new Vector3(0, 90, 0), 0.2f));

            
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
       
    }

    public void ShowOutline(bool active)
    {
        
    }

    public Outline GetOutlineComponent()
    {
        return null;
    }

    public string GetInterectableText()
    {
        if (PlaceController.RestaurantIsOpen)
        {
            return "Close Restaurant";
        }
        else
        {
            return "Open Restaurant";

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
        if (PlaceController.RestaurantIsOpen)
        {
            texts[0] = "Close Restaurant";
            return texts;
        }
        else
        {
            texts[0] = "Open Restaurant";
            return texts;

        }
    }
    public string[] GetInterectableButtons()
    {
        return null;
    }
}
