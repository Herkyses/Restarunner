using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class OpenCloseController : MonoBehaviour,IInterectableObject
{
    public Tween OpenCloseTween;
    public Sequence OpenCloseSequence;
    public Transform OpenTransform;
    public Transform CloseTransform;
    public Transform Parent;
    public void InterectableObjectRun()
    {
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
            });            

        }
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
}
