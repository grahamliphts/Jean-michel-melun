﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour {

    [SerializeField]
    private int _force = 0;
    [SerializeField]
    float _perception = 0f;

    [SerializeField]
    public AudioSource[] background;
    [SerializeField]
    public AudioSource woof;
    [SerializeField]
    public int scoreValue = 1;

    [SerializeField]
    public ParticleSystem vision;

    private Collider2D _collider;
    private Vector3 _lastInteract;

    private int _lastInterest = 0;
    private float _lastDistance = -1;

    private Vector2 _CurrentDirection = new Vector2(0, 0);
    bool _newInterract = false;
    bool _haveMaster = false;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        /*
        Vector2 direction = lastInteract - this.transform.position;
        direction.Normalize();
        //Debug.Log(CurrentDirection + " " + direction);

        GetComponent<Rigidbody2D>().AddForce(direction * force);

        newInterract = false;*/
        _lastInterest = 0;
        _lastDistance = -1;

        if (_lastInteract != new Vector3())
        {
            Debug.DrawRay(transform.position, _lastInteract - transform.position,Color.white);
        }
        else
        {
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(false);
            vision.gameObject.SetActive(false);
        }

        if (woof.volume == 0.01f)
            woof.volume = 0;

    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Object")
        {
            Vector3 targetDir = other.gameObject.transform.position - transform.position;
            float angle = Vector3.Angle(targetDir, transform.GetChild(0).up);

            if (angle < _perception)
            {
                _newInterract = true;
                int interest = other.gameObject.GetComponent<InteractibleObject>().getInterest();

                //Debug.Log("Test Interest " + other.gameObject.name + " " + interest);
                if (_lastInterest <= interest)
                {
                    Vector2 direction = _lastInteract - transform.position;
                    float Distance = direction.magnitude;
                    //Debug.Log(Distance);
                    if (_lastInterest == interest)
                    {
                        if (_lastDistance != -1 && _lastDistance > Distance)
                        {
                            _lastInterest = interest;
                            _lastInteract = other.gameObject.transform.position;
                            _lastDistance = Distance;

                        }
                    }
                    else
                    {
                        _lastInterest = interest;
                        _lastInteract = other.gameObject.transform.position;
                        _lastDistance = Distance;
                    }
                    bark(true);
                    vision.gameObject.SetActive(true);

                    if (Distance > 12f)
                    {
                        transform.GetChild(2).gameObject.SetActive(true);
                        transform.GetChild(3).gameObject.SetActive(false);
                    }
                    else
                    {
                        transform.GetChild(2).gameObject.SetActive(false);
                        transform.GetChild(3).gameObject.SetActive(true);
                    }
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Object")
        {
            _lastInteract = new Vector3(0, 0, 0);
            vision.gameObject.SetActive(false);
            bark(false);
        }
    }

    public Vector3 GetLastInteract()
    {
        return _lastInteract;
    }

    public void ResetLastInteract()
    {
        _lastInteract = new Vector3(0, 0, 0);
    }

    public int GetForce()
    {
        return _force;
    }

    public float Perception()
    {
        return _perception;
    }

    public void AddForce(int add)
    {
        _force += add;
    }

    public void AddPerception(float add)
    {
        _perception += add;
    }
    public void  playBackgroundLoop()
    {
        foreach(AudioSource sound in background)
        if(!sound.isPlaying)
                sound.Play();
        if (!woof.isPlaying)
        {
            woof.Play();
            woof.volume = 0;
        }
           
    }
    public void bark(bool state)
    {
        if(_haveMaster)
        {
            if (state)
            {
                if(woof.volume != 1)
                {
                    StopCoroutine(fadeout());
                    StartCoroutine(fadein());
                }          

            }
            else
            {
                if(woof.volume != 0)
                {
                    StopCoroutine(fadein());
                    StartCoroutine(fadeout());
                }
               
            }
        }
       
    }
    public void haveMaster(bool state)
    {
        _haveMaster = state;
    }

    IEnumerator fadeout()
    {
        woof.volume -= 0.01f;
        yield return new WaitForSeconds(0.01f);
        if (woof.volume > 0)
        {
            StartCoroutine(fadeout());

        }
        else
            woof.volume = 0;

    }
    IEnumerator fadein()
    {
        woof.volume += 0.01f;
        yield return new WaitForSeconds(0.01f);
        if (woof.volume != 1)
        {
            StartCoroutine(fadein());
        }
        else
            woof.volume = 1;
    }

    public void Evade()
    {
        transform.SetParent(null);
        GetComponent<LineRenderer>().enabled = false;
        GetComponent<LineRenderer>().startWidth = 0f;
        gameObject.GetComponent<LineRenderer>().endWidth = 0f;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        Vector3 diff;
        if (_lastInteract == new Vector3(0, 0, 0))
        {
            _lastInteract = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), 0);
            diff = _lastInteract;
        }
        else
            diff = _lastInteract - transform.position;

        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.GetChild(0).localRotation = Quaternion.Euler(0f, 0f, rot_z - 90);

        GetComponent<Rigidbody2D>().AddForce(diff * 200);

        StartCoroutine("WaitEvade");
    }

    IEnumerator WaitEvade()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    public int getValue()
    {
        return scoreValue;
    }

}
