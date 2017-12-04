using System.Collections;
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

    private int maxDog = 0;
    private int score;
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

        string timeVal = "";
        timeVal += (int)(Time.time / 60);
        timeVal += ':';
        timeVal += (int)Time.time % 60;
        timer.text = timeVal;
	}

    public void showTuto()
    {
        tutoPanel.SetActive(true);
    }
    public void hideTuto()
    {
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
