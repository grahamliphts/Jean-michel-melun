using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITITLE : MonoBehaviour {

    [SerializeField]
    Text main;
    [SerializeField]
    Text reduced;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        reduced.fontSize = main.cachedTextGenerator.fontSizeUsedForBestFit - 2;
	}
}
