using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour {

    [SerializeField]
    private int _force = 0;
    [SerializeField]
    float _perception = 0f;

    private Collider2D _collider;
    private Vector3 _lastInteract;

    private int _lastInterest = 0;
    private float _lastDistance = -1;

    private Vector2 _CurrentDirection = new Vector2(0, 0);
    bool _newInterract = false;

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

        if(_lastInteract != new Vector3())
        {
            Debug.DrawRay(transform.position, _lastInteract - transform.position, GetComponent<SpriteRenderer>().color);
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Object")
        {

            Vector3 targetDir = other.gameObject.transform.position - transform.position;
            float angle = Vector3.Angle(targetDir, transform.right);

            if (angle < _perception)
            {
                _newInterract = true;
                int interest = other.gameObject.GetComponent<InteractibleObject>().getInterest();

                //Debug.Log("Test Interest " + other.gameObject.name + " " + interest);
                if (_lastInterest <= interest)
                {
                    Vector2 direction = _lastInteract - this.transform.position;
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

                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Object")
        {
            _lastInteract = new Vector3(0, 0, 0);
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
}
