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
    [SerializeField]
    GameObject nextToshow;

    private string notice;
    private int iterator = 0;
    private int maxiter;

    int up = 0;

    float speed = 0.02f;
	// Use this for initialization
	void Start () {
        notice = _maintext.text;
        maxiter = notice.Length;
        _maintext.text = " ";
        StartCoroutine(WriteNotice());
        nextToshow.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.Space))
        {
            speed = 0.001f;
        }
        else
            speed = 0.02f;

        if (iterator >= maxiter)
        {

                sound.Stop();
            StartCoroutine(waitForDogPred());
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
        yield return new WaitForSeconds(speed);
        
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
        
        yield return new WaitForSeconds(speed * 10);
        StartCoroutine(WriteLine());

    }

    IEnumerator waitForDogPred()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            yield return new WaitForSeconds(0.1f);
        else
            yield return new WaitForSeconds(2f);
        nextToshow.SetActive(true);
        this.gameObject.SetActive(false);
    }

}
