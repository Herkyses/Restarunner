using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    
    public void ChangeSceneForSample()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        MapManager.Instance.ResetMap();
        SceneManager.LoadScene("SampleScene");
    }
}
