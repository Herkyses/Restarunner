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
            if (Input.GetKeyDown(KeyCode.UpArrow) && _musicController.IsUpArrow)
            {
                score++;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && _musicController.IsRightArrow)
            {
                score++;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && _musicController.IsLeftArrow)
            {
                score++;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && _musicController.IsDownArrow)
            {
                score++;
            }

            scoreText.text = score.ToString();
        }
    }
}
