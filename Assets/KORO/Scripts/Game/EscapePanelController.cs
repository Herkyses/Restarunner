using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapePanelController : MonoBehaviour
{
    public GameObject escapePanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (escapePanel.activeSelf)
            {
                ResumeGame(); 
            }
            else
            {
                PauseGame(); 
            }
        }
    }

    public void PauseGame()
    {
        escapePanel.SetActive(true); 
        Time.timeScale = 0f;         
        Cursor.visible = true;       
        Cursor.lockState = CursorLockMode.None; 
    }

    public void ResumeGame()
    {
        escapePanel.SetActive(false); 
        Time.timeScale = 1f;          
        Cursor.visible = false;       
        Cursor.lockState = CursorLockMode.Locked; 
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit(); // Oyundan çıkış
    }
}
