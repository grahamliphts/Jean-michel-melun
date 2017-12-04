using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleObject : MonoBehaviour {

    [SerializeField]
    private int interest;

    public bool animated = true;

    private float maxRotate = 0.30f;
    private bool left = true;
	// Use this for initialization
	void Start () {
        if (Random.Range(0, 2) == 0)
            left = false;
        else
            left = true;
        transform.rotation = Quaternion.Euler(0,0,Random.Range(0,30));
        StartCoroutine(rotate());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int getInterest()
    {
        return interest;
    }

    IEnumerator rotate()
    {
        if (left)
            transform.Rotate(0, 0, 2);
        else
            transform.Rotate(0, 0, -2);

        yield return new WaitForSeconds(0.01f);
        
        if(Mathf.Abs(transform.rotation.z) > maxRotate)
        {
            left = !left;
        }
        StartCoroutine(rotate());
    }
}
