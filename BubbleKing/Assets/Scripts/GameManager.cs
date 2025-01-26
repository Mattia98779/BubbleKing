using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject ui;
    public GameObject timerUi;

    public TextMeshProUGUI timer;
    
    public EndingScene endingScene;

    private float time = 0;
    public bool hasGameStarted = false;
    public bool hasGameEnded = false;
    public bool isEndingAnimationPlaying = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hasGameStarted && !hasGameEnded)
        {
            time += Time.deltaTime;
            timer.text = string.Format("{0:00}:{1:00}:{2:000}", Mathf.FloorToInt(time/60f), Mathf.FloorToInt(time%60f), Mathf.FloorToInt(time*1000f));
        }
        if ((Input.GetMouseButton(0) || Input.GetMouseButton(1)) && !hasGameStarted)
        {
            ui.SetActive(false);
            hasGameStarted = true;
        }
    }

    public void PlayerWin()
    {
        hasGameEnded = true;
        endingScene.StartEndingAnimation();
        timerUi.SetActive(false);
        Debug.Log("Player Win");
        
    }

    public void StartNewGame()
    {
        timerUi.SetActive(true);
        hasGameStarted = false;
        hasGameEnded = false;
        isEndingAnimationPlaying = false;
        time = 0;
        ui.SetActive(true);
        timer.text = string.Format("{0:00}:{1:00}:{2:000}", 0,0,0);

    }

    public void EndingAnimationEnded()
    {
        isEndingAnimationPlaying = true;
    }
}
