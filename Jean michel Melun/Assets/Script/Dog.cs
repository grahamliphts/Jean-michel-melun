﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour {

    [SerializeField]
    private int force;
    private Collider2D _collider;
    private Vector3 lastInteract;

    private int lastInterest = 0;
    private float lastDistance = -1;

    private Vector2 CurrentDirection = new Vector2(0, 0);
    bool newInterract = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        /*
        Vector2 direction = lastInteract - this.transform.position;
        direction.Normalize();
        //Debug.Log(CurrentDirection + " " + direction);

        GetComponent<Rigidbody2D>().AddForce(direction * force);

        newInterract = false;*/
        lastInterest = 0;
        lastDistance = -1;

        if(lastInteract != new Vector3())
        {
            Debug.DrawRay(transform.position, lastInteract - transform.position, GetComponent<SpriteRenderer>().color);

            Vector3 diff = lastInteract - transform.position;
            diff.Normalize();

            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Object")
        {        
            newInterract = true;
            int interest = other.gameObject.GetComponent<InteractibleObject>().getInterest();
            //Debug.Log("Test Interest " + other.gameObject.name + " " + interest);
            if (lastInterest <= interest)
            {
                Vector2 direction = lastInteract - this.transform.position;
                float Distance = direction.magnitude;
                //Debug.Log(Distance);
                if (lastInterest == interest)
                {                                   
                    if (lastDistance != -1 && lastDistance > Distance)
                    {
                        lastInterest = interest;
                        lastInteract = other.gameObject.transform.position;
                        lastDistance = Distance;

                    }
                }
                else
                {
                    lastInterest = interest;
                    lastInteract = other.gameObject.transform.position;
                    lastDistance = Distance;
                }

            }

        }
    }

    public Vector3 getLastInteract()
    {
        Vector3 temp = lastInteract;
        lastInteract = new Vector3(0, 0, 0);
        return temp;
    }
    public int getForce()
    {
        return force;
    }
}
