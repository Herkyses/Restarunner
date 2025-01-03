using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubbish : MonoBehaviour,IInterectableObject
{
    
    [SerializeField] private string[] texts = new [] {"Take OrderBox"};
    [SerializeField] private string[] textsButtons = new [] {"E"};
    public bool ForRubbishLevel;
    
    public event Action OnDestroyed;

    private Outline _rubbishOutline;
    public void InterectableObjectRun()
    {
        
        //if (Player.Instance.CanCleanRubbish)
        //{
            //gameObject.SetActive(false);
            transform.SetParent(null);
            
            //Player.Instance.StartClean();
            
            if (PlayerPrefsManager.Instance.LoadPlaceRubbishLevel() == 0)
            {
                RubbishManager.CheckedRubbishesForTutorial?.Invoke();
            }
            
            RubbishManager.Instance.CheckRubbishRate();
            Debug.Log("vfxobject:" + GameVfxManager.Instance.vfxPools[1].vfxPrefab);
            GameVfxManager.Instance.SpawnVFX(GameVfxManager.Instance.vfxPools[1].vfxPrefab, transform.position, transform.rotation);
            OnDestroyed?.Invoke();
            //RubbishManager.Instance.ReturnRubbish(gameObject);
            

        //}
    }

    private void Start()
    {
        texts = new [] {"Key_Clean"};
        textsButtons = new [] {"E"};
        _rubbishOutline = GetComponent<Outline>();
    }

    public void ShowOutline(bool active)
    {
        _rubbishOutline.enabled = active;
    }

    public Outline GetOutlineComponent()
    {
        return _rubbishOutline;
    }

    public string GetInterectableText()
    {
        if (Player.Instance.CanCleanRubbish)
        {
            return "Key_Clean";
        }
        return null;

    }

    public void Move()
    {
        
    }
    public void Open()
    {
        
    }
    public string[] GetInterectableTexts()
    {
        //if (Player.Instance.CanCleanRubbish)
        //{
            return texts;
        //}
        //return null;
    }
    public string[] GetInterectableButtons()
    {
        //if (Player.Instance.CanCleanRubbish)
        //{
            return textsButtons;
        //}
        //return null;
    }
    public Enums.PlayerStateType GetStateType()
    {
        return Enums.PlayerStateType.Cleaner;
    }
}
