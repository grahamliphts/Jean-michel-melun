using System.Collections;
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

        CalculateForce();
    }

    void CalculateForce()
    {
        Vector3 directionToApply = new Vector3(0, 0, 0);
        foreach (Dog dog in _leftDogs)
        {
            Vector3 lastInteract = dog.getLastInteract();
            if (lastInteract != new Vector3(0,0,0))
            {
                directionToApply += (lastInteract - transform.position) * dog.getForce();
                //Debug.Log(lastInteract);
            }
        }

        foreach (Dog dog in _rightDogs)
        {
            Vector3 lastInteract = dog.getLastInteract();
            if (lastInteract != new Vector3(0, 0, 0))
            {
                directionToApply += (lastInteract - transform.position) * dog.getForce();
                //Debug.Log(lastInteract);
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

            Debug.Log("Left : " + _leftDogs.Count);
            Debug.Log("Right : " + _rightDogs.Count);
        }
    }

    void SwitchDogRightToLeft()
    {
        if (_rightDogs.Count != 0)
        {
            _rightDogs[0].gameObject.name = "Left Dog";
            _leftDogs.Add(_rightDogs[0]);
            _rightDogs.RemoveAt(0);

            Debug.Log("Left : " + _leftDogs.Count);
            Debug.Log("Right : " + _rightDogs.Count);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "LostDog")
        {
            _leftDogs.Add(other.GetComponent<Dog>());
            Debug.Log("Left : " + _leftDogs.Count);
            Debug.Log("Right : " + _rightDogs.Count);

            other.tag = "Dog";
            other.gameObject.transform.SetParent(this.transform);
            other.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }
    }
}
