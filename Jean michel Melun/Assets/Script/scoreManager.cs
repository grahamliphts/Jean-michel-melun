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

    private int maxDog = 0;
	// Use this for initialization
	void Start () {
        maxDog = _bag.getDogOnMap();
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
}
