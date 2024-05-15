using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayInputController : MonoBehaviour
{
    private MusicController _musicController;
    public int CorrectAnswerCount;
    public int WrongAnswerCount;
    public int AnswerCount;
    public static PlayInputController Instance;
    public float score;
    public TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    
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
    void Start()
    {
        gameObject.TryGetComponent(out _musicController);
    }

    // Update is called once per frame
    void Update()
    {
        if (_musicController.Isplayable)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) )
            {
                if (_musicController.IsUpArrow && _musicController.IsCorrectable)
                {
                    CorrectAnswerCount++;
                    _musicController.CorrectScore();
                    score++; 
                }
                else
                {
                    WrongAnswerCount++;
                    _musicController.WhiteAnswer();
                }

            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (_musicController.IsRightArrow && _musicController.IsCorrectable )
                {
                    CorrectAnswerCount++;
                    _musicController.CorrectScore();
                    score++; 
                }
                else
                {
                    WrongAnswerCount++;
                    _musicController.WhiteAnswer();
                }

            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) )
            {
                if (_musicController.IsLeftArrow && _musicController.IsCorrectable)
                {
                    CorrectAnswerCount++;
                    _musicController.CorrectScore();
                    score++; 
                }
                else
                {
                    WrongAnswerCount++;
                    _musicController.WhiteAnswer();
                }

            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (_musicController.IsDownArrow && _musicController.IsCorrectable)
                {
                    CorrectAnswerCount++;
                    _musicController.CorrectScore();
                    score++; 
                }
                else
                {
                    WrongAnswerCount++;
                    _musicController.WhiteAnswer();
                }

            }

            if (WrongAnswerCount > 0)
            {
                score = 100 * (float)CorrectAnswerCount / (float)(WrongAnswerCount + CorrectAnswerCount);
                scoreText.text = "% " + ((int)score).ToString();
            }
            
        }
    }
}
