using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public int countdownminutes = 2;
    private float countdownSeconds;
    private Text timeText;

    [SerializeField]
    private GameManager gameManager;
    void Start()
    {
            timeText = GetComponent<Text>();
            countdownSeconds = countdownminutes * 100;
        //countdownSeconds = countdownminutes*10;
    }

    // Update is called once per frame
    void Update()
    {
        countdownSeconds -= Time.deltaTime;
        var span = new TimeSpan(0,0,(int)countdownSeconds);
        timeText.text = span.ToString(@"mm\:ss");

        if(countdownSeconds <= 0)
        {
            gameManager.GameOver();
            countdownSeconds = countdownminutes * 0;
        }
    }
}
