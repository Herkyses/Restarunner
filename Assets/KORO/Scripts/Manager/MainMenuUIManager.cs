using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
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
