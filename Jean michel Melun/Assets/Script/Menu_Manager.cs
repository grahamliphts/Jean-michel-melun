using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu_Manager : MonoBehaviour {


    [SerializeField]
    GameObject[] imageList;

    private int iterator = 00;
	// Use this for initialization
	void Start () {

        for (int i = 1; i < imageList.Length; i++)
            imageList[i].SetActive(false);


		
	}

    public void StartLevel()
    {
        SceneManager.LoadScene("TestFinal", LoadSceneMode.Single);
    }
	
    public void NextImage()
    {
        imageList[iterator].SetActive(false);
        if(iterator < imageList.Length -1)
            iterator++;
        imageList[iterator].SetActive(true);
    }
    public void PreviousImage()
    {
        imageList[iterator].SetActive(false);
        if (iterator > 0)
            iterator--;
        imageList[iterator].SetActive(true);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
