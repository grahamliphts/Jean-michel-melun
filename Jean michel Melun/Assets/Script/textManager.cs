using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textManager : MonoBehaviour {

    [SerializeField]
    Text _maintext;
    [SerializeField]
    GameObject furry;
    [SerializeField]
    AudioSource sound;

    private string notice;
    private int iterator = 0;
    private int maxiter;

    int up = 0;
	// Use this for initialization
	void Start () {
        notice = _maintext.text;
        maxiter = notice.Length;
        _maintext.text = " ";
        StartCoroutine(WriteNotice());
	}
	
	// Update is called once per frame
	void Update () {

        if (iterator >= maxiter)
        {

                sound.Stop();
        }

    }
    IEnumerator WriteLine()
    {
        if (!sound.isPlaying)
        {
            //Debug.Log("playSound");
            sound.Play();
        }
            

        if (iterator < maxiter)
            _maintext.text += notice[iterator];
        else
            StopAllCoroutines();
        iterator++;
        if(up < 4)
        {
            furry.GetComponent<RectTransform>().offsetMin = new Vector2(furry.GetComponent<RectTransform>().offsetMin.x, furry.GetComponent<RectTransform>().offsetMin.y + 2);
            furry.GetComponent<RectTransform>().offsetMax = new Vector2(furry.GetComponent<RectTransform>().offsetMax.x, furry.GetComponent<RectTransform>().offsetMax.y + 2);
        }
        else
        {
            furry.GetComponent<RectTransform>().offsetMin = new Vector2(furry.GetComponent<RectTransform>().offsetMin.x , furry.GetComponent<RectTransform>().offsetMin.y -2);
            furry.GetComponent<RectTransform>().offsetMax = new Vector2(furry.GetComponent<RectTransform>().offsetMax.x , furry.GetComponent<RectTransform>().offsetMax.y -2);
        }
        up++;
        if (up >= 8)
            up = 0;
        yield return new WaitForSeconds(0.02f);
        
        if(iterator < maxiter && notice[iterator].CompareTo('\n') == 0)
        {
            if (sound.isPlaying)
            {
                sound.Stop();
            }
            StartCoroutine(WriteNotice());
        }
        else
        {
           
                
            StartCoroutine(WriteLine());
        }
    }
    IEnumerator WriteNotice()
    {
        
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(WriteLine());

    }

}
