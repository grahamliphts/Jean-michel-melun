﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreManager : MonoBehaviour {

    [SerializeField]
    DoggyBag _bag;
    [SerializeField]
    Player _player;
    [SerializeField]
    Text Score;
    [SerializeField]
    Text timer;

    [SerializeField]
    GameObject tutoPanel;
    [SerializeField]
    GameObject endGamePanel;
    [SerializeField]
    Text finalScore;
    [SerializeField]
    float maxtime = 300;

    private int maxDog = 0;
    private int score;
    bool timeLaunched = false;
    private float timeRemover = 0;
	// Use this for initialization
	void Start () {
        maxDog = _bag.getDogOnMap();
        tutoPanel.SetActive(true);
        endGamePanel.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        string val = "";
        val += _player.getDogInHands();
        val += '/';
        val += maxDog;
        Score.text = val;

        float timeValue = maxtime - (Time.time - timeRemover);
        string timeVal = "";
        timeVal += (int)(timeValue / 60);
        timeVal += ':';
        timeVal += (int)timeValue % 60;
        timer.text = timeVal;

        if (!timeLaunched)
            timeRemover = Time.time;

        if (timeValue <= 0)
        {
            endGame();
            timer.text = 0.ToString();
        }
           
	}

    public void showTuto()
    {
       
        tutoPanel.SetActive(true);
    }
    public void hideTuto()
    {
        if (!timeLaunched)
        {
            timeLaunched = true;
        }
        tutoPanel.SetActive(false);

    }
    public void endGame()
    {
        finalScore.text = score.ToString();
        endGamePanel.SetActive(true);
       
    }
    public void updateFinalScore(int val)
    {
        score = val;
    } 
}
