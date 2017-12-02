﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private List<Dog> _leftDogs;
    private List<Dog> _rightDogs;
    private Vector2 _directionPlayer;
    private Rigidbody2D _rigidbody;

    // Use this for initialization
    void Start ()
    {
        _leftDogs = new List<Dog>();
        _rightDogs = new List<Dog>();
        
        _rigidbody = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            _rigidbody.AddForce(new Vector2(0, 5));
        }
        if (Input.GetKey(KeyCode.S))
        {
            _rigidbody.AddForce(new Vector2(0, -5));
        }
        if (Input.GetKey(KeyCode.Q))
        {
            _rigidbody.AddForce(new Vector2(-5, 0));
        }
        if (Input.GetKey(KeyCode.D))
        {
            _rigidbody.AddForce(new Vector2(5, 0));
        }


        if (Input.GetKeyDown(KeyCode.A))
        {
            SwitchDogRightToLeft();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchDogLeftToRight();
        }

        CheckDogs();
        CalculateForce();
    }

    void CalculateForce()
    {
        Vector3 directionToApply = new Vector3(0, 0, 0);
        foreach (Dog dog in _leftDogs)
        {
            Vector3 lastInteract = dog.GetLastInteract();
            if (lastInteract != new Vector3(0,0,0))
            {
                if(lastInteract.x < transform.position.x)
                {
                    directionToApply += (lastInteract - dog.transform.position) * dog.GetForce();

                    Vector3 diff = lastInteract - dog.transform.position;
                    diff.Normalize();

                    float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                    dog.transform.localRotation = Quaternion.Euler(0f, 0f, rot_z);
                }
                else
                {
                    dog.ResetLastInteract();
                    Vector3 diff = dog.transform.position - transform.position;
                    diff.Normalize();

                    float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                    dog.transform.localRotation = Quaternion.Euler(0f, 0f, rot_z);
                }
                //Debug.Log(lastInteract);
            }
            else
            {
                Vector3 diff = dog.transform.position - transform.position;
                diff.Normalize();

                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                dog.transform.localRotation = Quaternion.Euler(0f, 0f, rot_z);
            }
        }

        foreach (Dog dog in _rightDogs)
        {
            Vector3 lastInteract = dog.GetLastInteract();
            if (lastInteract != new Vector3(0, 0, 0))
            {
                if (lastInteract.x > transform.position.x)
                {
                    directionToApply += (lastInteract - dog.transform.position) * dog.GetForce();
                    //Debug.Log(lastInteract);
                    Vector3 diff = lastInteract - dog.transform.position;
                    diff.Normalize();

                    float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                    dog.transform.localRotation = Quaternion.Euler(0f, 0f, rot_z);
                }
                else
                {
                    dog.ResetLastInteract();
                    Vector3 diff = dog.transform.position - transform.position;
                    diff.Normalize();

                    float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                    dog.transform.localRotation = Quaternion.Euler(0f, 0f, rot_z);
                }
            }
            else
            {
                Vector3 diff = dog.transform.position - transform.position;
                diff.Normalize();

                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                dog.transform.localRotation = Quaternion.Euler(0f, 0f, rot_z);
            }
        }
        //Debug.Log("DirApply " + directionToApply);

        _rigidbody.AddForce(directionToApply);
    }

    void SwitchDogLeftToRight()
    {
        if(_leftDogs.Count != 0)
        {
            _leftDogs[0].gameObject.name = "Right Dog";
            _rightDogs.Add(_leftDogs[0]);
            _leftDogs.RemoveAt(0);

            //Debug.Log("Left : " + _leftDogs.Count);
            //Debug.Log("Right : " + _rightDogs.Count);


            CalculatePositionDogs();
        }
    }

    void SwitchDogRightToLeft()
    {
        if (_rightDogs.Count != 0)
        {
            _rightDogs[0].gameObject.name = "Left Dog";
            _leftDogs.Add(_rightDogs[0]);
            _rightDogs.RemoveAt(0);

            //Debug.Log("Left : " + _leftDogs.Count);
            //Debug.Log("Right : " + _rightDogs.Count);
            CalculatePositionDogs();
        }
    }

    void CalculatePositionDogs()
    {
        float distPlayer = 0.5f;
        for (int i = 0; i < _leftDogs.Count; i++)
        {
            float angle = Mathf.PI / (_leftDogs.Count + 1);
            float x = distPlayer * Mathf.Cos(angle * (i + 1) + Mathf.PI / 2);
            float y = distPlayer * Mathf.Sin(angle * (i + 1) + Mathf.PI / 2);

            _leftDogs[i].transform.localPosition = new Vector3(x, y, 0);

            if (_leftDogs[i].GetLastInteract() == new Vector3(0, 0, 0))
            {
                Vector3 diff = _leftDogs[i].transform.position - transform.position;
                diff.Normalize();

                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                _leftDogs[i].transform.localRotation = Quaternion.Euler(0f, 0f, rot_z);
            }
        }

        for (int i = 0; i < _rightDogs.Count; i++)
        {
            float angle = Mathf.PI / (_rightDogs.Count + 1);
            float x = distPlayer * Mathf.Cos(angle * (i + 1) + Mathf.PI + Mathf.PI / 2);
            float y = distPlayer * Mathf.Sin(angle * (i + 1) + Mathf.PI + Mathf.PI / 2);

            _rightDogs[i].transform.localPosition = new Vector3(x, y, 0);

            if (_rightDogs[i].GetLastInteract() == new Vector3(0, 0, 0))
            {
                Vector3 diff = _rightDogs[i].transform.position - transform.position;
                diff.Normalize();

                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                _rightDogs[i].transform.localRotation = Quaternion.Euler(0f, 0f, rot_z);
            }
        }
    }

    void CheckDogs()
    {
        for (int i = 0; i < _leftDogs.Count; i++)
        {
            if(_leftDogs[i].tag == "LostDog")
            {
                _leftDogs.RemoveAt(i);
                CalculatePositionDogs();
            }
        }

        for (int i = 0; i < _rightDogs.Count; i++)
        {
            if (_rightDogs[i].tag == "LostDog")
            {
                _rightDogs.RemoveAt(i);
                CalculatePositionDogs();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "LostDog")
        {
            _leftDogs.Add(other.GetComponent<Dog>());
            //Debug.Log("Left : " + _leftDogs.Count);
            //Debug.Log("Right : " + _rightDogs.Count);

            other.tag = "Dog";
            other.gameObject.transform.SetParent(this.transform);
            other.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            CalculatePositionDogs();
        }
    }
}
