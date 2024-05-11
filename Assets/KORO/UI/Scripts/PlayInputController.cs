using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayInputController : MonoBehaviour
{
    private MusicController _musicController;

    public int score;
    public TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.TryGetComponent(out _musicController);
    }

    // Update is called once per frame
    void Update()
    {
        if (_musicController.IsCorrectable)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) )
            {
                if (_musicController.IsUpArrow)
                {
                    _musicController.CorrectScore();
                    score++; 
                }
                else
                {
                    _musicController.WhiteAnswer();
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) )
            {
                if (_musicController.IsRightArrow)
                {
                    _musicController.CorrectScore();
                    score++; 
                }
                else
                {
                    _musicController.WhiteAnswer();
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) )
            {
                if (_musicController.IsLeftArrow)
                {
                    _musicController.CorrectScore();
                    score++; 
                }
                else
                {
                    _musicController.WhiteAnswer();
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (_musicController.IsDownArrow)
                {
                    _musicController.CorrectScore();
                    score++; 
                }
                else
                {
                    _musicController.WhiteAnswer();
                }
            }
            

            scoreText.text = score.ToString();
        }
    }
}
