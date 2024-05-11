using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    public RectTransform CorrectAreaRect;
    public RectTransform AreaInducatorRect;
    public RectTransform DownArea;

    public Transform MusicControllerPanel;

    //public GameObject correctObject;
    public Sprite[] ArrowImages;

    public bool IsCorrectable;
    public bool Isplayable;
    public bool IsRightArrow,IsLeftArrow,IsUpArrow,IsDownArrow;
    public Vector2 AreaInducatorPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        AreaInducatorPosition = AreaInducatorRect.anchoredPosition;
        AreaInducatorRect.anchoredPosition = AreaInducatorPosition;
        AreaInducatorRect.gameObject.GetComponent<Image>().sprite = ReturnArrowSprite(Random.Range(0, 4));
    }

    // Update is called once per frame
    void Update()
    {
        if (Isplayable)
        {
            if (!MusicControllerPanel.gameObject.activeSelf)
            {
                MusicControllerPanel.gameObject.SetActive(true);
            }
            ControlInducatorTrnsform();
            InducatorMoveStart();
        }
        else
        {
            AreaInducatorRect.anchoredPosition = AreaInducatorPosition;
            MusicControllerPanel.gameObject.SetActive(false);

        }
        
    }

    public void InducatorMoveStart()
    {
        AreaInducatorRect.transform.localPosition += Vector3.down*Time.deltaTime*200f;
    }

    private void ControlInducatorTrnsform()
    {
        if (IsInsideFirstImage(AreaInducatorRect,DownArea))
        {
            AreaInducatorRect.anchoredPosition = AreaInducatorPosition;
            AreaInducatorRect.gameObject.GetComponent<Image>().sprite = ReturnArrowSprite(Random.Range(0, 4));
        }
        if (IsInsideFirstImage(AreaInducatorRect,CorrectAreaRect))
        {
            //correctObject.SetActive(true);
            IsCorrectable = true;
        }
        else
        {
            //correctObject.SetActive(false);
            IsCorrectable = false;
        }
    }
    Rect GetUIElementBounds(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        Rect bounds = new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);

        return bounds;
    }
    private bool IsInsideFirstImage(RectTransform skorRectTransform,RectTransform controlArea)
    {
        Rect scrollViewObject = GetUIElementBounds(controlArea);

        Vector2 ownerPosition = skorRectTransform.position;

        return scrollViewObject.Contains(ownerPosition);
    }

    private Sprite ReturnArrowSprite(int arrowNumber)
    {
        switch (arrowNumber)
        {
            case 0:
                IsUpArrow = true;
                IsRightArrow = false;
                IsLeftArrow = false;
                IsDownArrow = false;
                return ArrowImages[0];
                break;
            case 1:
                IsUpArrow = false;
                IsRightArrow = true;
                IsLeftArrow = false;
                IsDownArrow = false;
                return ArrowImages[1];

                break;
            case 2:
                IsUpArrow = false;
                IsRightArrow = false;
                IsLeftArrow = true;
                IsDownArrow = false;
                return ArrowImages[2];

                break;
            case 3:
                IsUpArrow = false;
                IsRightArrow = false;
                IsLeftArrow = false;
                IsDownArrow = true;
                return ArrowImages[3];

                break;
        }

        return null;
    }
}
