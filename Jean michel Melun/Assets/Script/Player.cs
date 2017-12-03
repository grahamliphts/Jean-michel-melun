using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private List<Dog> _leftDogs;
    private List<Dog> _rightDogs;
    private Vector2 _directionPlayer;
    private Rigidbody2D _rigidbody;

    private int _forceLeft = 0;
    private int _forceRight = 0;

    [SerializeField]
    GameObject leftArmRoot;
    [SerializeField]
    Transform Lefthandroot;
    [SerializeField]
    GameObject rightArmRoot;
    [SerializeField]
    Transform Righthandroot;

    [SerializeField]
    Image[] _jauge;

    [SerializeField]
    int _maxForcePlayer = 15;

    [SerializeField]
    int _forcePlayer = 30;


    float _distPlayer = .9f;

    private int dogAmount = 0;
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
            _rigidbody.AddForce(new Vector2(0, _forcePlayer));
        }
        if (Input.GetKey(KeyCode.S))
        {
            _rigidbody.AddForce(new Vector2(0, -_forcePlayer));
        }
        if (Input.GetKey(KeyCode.Q))
        {
            _rigidbody.AddForce(new Vector2(-_forcePlayer, 0));
        }
        if (Input.GetKey(KeyCode.D))
        {
            _rigidbody.AddForce(new Vector2(_forcePlayer, 0));
        }


        if (Input.GetMouseButtonUp(0))
        {
            SwitchDogRightToLeft();
        }
        if (Input.GetMouseButtonUp(1))
        {
            SwitchDogLeftToRight();
        }
        if (Input.GetMouseButtonUp(2))
        {
            if(_leftDogs.Count != 0 || _rightDogs.Count != 0)
            {
                int choice = -1;
                if(_leftDogs.Count != 0 && _rightDogs.Count != 0)
                {
                    choice = Random.Range(0, 2);
                }

                if (choice == -1 && _rightDogs.Count != 0 || choice == 0)
                {
                    int i = Random.Range(0, _rightDogs.Count);
                    _rightDogs[i].Evade();
                    _rightDogs.RemoveAt(i);
                }
                else if (choice == -1 && _leftDogs.Count != 0 || choice == 1)
                {
                    int i = Random.Range(0, _leftDogs.Count);
                    _leftDogs[i].Evade();
                    _leftDogs.RemoveAt(i);
                }
            }
        }

        CheckDogs();
        CalculateForce();
    }

    void CalculateForce()
    {
        Vector3 directionToApply = new Vector3(0, 0, 0);
        int i = 0;
        Vector3 leftDirection = new Vector3(0, 0, 0);
        Vector3 rightDirecton = new Vector3(0, 0, 0);
        
        _forceLeft = 0;
        _forceRight = 0;
        dogAmount = 0;
        foreach (Dog dog in _leftDogs)
        {
            dogAmount++;
            Vector3 lastInteract = dog.GetLastInteract();
            if (lastInteract != new Vector3(0,0,0) && (lastInteract.x < transform.position.x))
            {
                float angle = Mathf.PI / (_leftDogs.Count + 1);
                float x = _distPlayer * Mathf.Cos(angle * (i + 1) + Mathf.PI / 2);
                float y = _distPlayer * Mathf.Sin(angle * (i + 1) + Mathf.PI / 2);

                dog.transform.localPosition = new Vector3(x + ((lastInteract.x - (x + transform.position.x)) * 0.1f), y + ((lastInteract.y - (y + transform.position.y)) * 0.2f), 0);

                Vector3 diff = lastInteract - dog.transform.position;
                diff.Normalize();

                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                dog.transform.GetChild(0).localRotation = Quaternion.Euler(0f, 0f, rot_z - 90);

                directionToApply += (lastInteract - dog.transform.position) * dog.GetForce();
                _forceLeft += dog.GetForce();
            }
            else
            {
                dog.ResetLastInteract();
                float angle = Mathf.PI / (_leftDogs.Count + 1);
                float x = _distPlayer * Mathf.Cos(angle * (i + 1) + Mathf.PI / 2);
                float y = _distPlayer * Mathf.Sin(angle * (i + 1) + Mathf.PI / 2);

                dog.transform.localPosition = new Vector3(x, y, 0);

                Vector3 diff = dog.transform.position - transform.position;
                diff.Normalize();

                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                dog.transform.GetChild(0).localRotation = Quaternion.Euler(0f, 0f, rot_z - 90);
            }
            i++;
            leftDirection += dog.transform.localPosition;

            if (dog.gameObject.GetComponent<LineRenderer>() != null)
            {
                dog.gameObject.GetComponent<LineRenderer>().SetPosition(0, Lefthandroot.position);
                dog.gameObject.GetComponent<LineRenderer>().SetPosition(1, dog.transform.position);
            }
        }

        i = 0;
        foreach (Dog dog in _rightDogs)
        {
            dogAmount++;
            Vector3 lastInteract = dog.GetLastInteract();
            if (lastInteract != new Vector3(0, 0, 0) && (lastInteract.x > transform.position.x))
            {
                float angle = Mathf.PI / (_rightDogs.Count + 1);
                float x = _distPlayer * Mathf.Cos(angle * (i + 1) + Mathf.PI + Mathf.PI / 2);
                float y = _distPlayer * Mathf.Sin(angle * (i + 1) + Mathf.PI + Mathf.PI / 2);

                dog.transform.localPosition = new Vector3(x + ((lastInteract.x - (x + transform.position.x)) * 0.1f), y + ((lastInteract.y - (y + transform.position.y)) * 0.2f), 0);

                Vector3 diff = lastInteract - dog.transform.position;
                diff.Normalize();

                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                dog.transform.GetChild(0).localRotation = Quaternion.Euler(0f, 0f, rot_z - 90);

                directionToApply += (lastInteract - dog.transform.position) * dog.GetForce();
                _forceRight += dog.GetForce();
            }
            else
            {
                dog.ResetLastInteract();
                float angle = Mathf.PI / (_rightDogs.Count + 1);
                float x = _distPlayer * Mathf.Cos(angle * (i + 1) + Mathf.PI + Mathf.PI / 2);
                float y = _distPlayer * Mathf.Sin(angle * (i + 1) + Mathf.PI + Mathf.PI / 2);

                dog.transform.localPosition = new Vector3(x, y, 0);

                Vector3 diff = dog.transform.position - transform.position;
                diff.Normalize();

                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                dog.transform.GetChild(0).localRotation = Quaternion.Euler(0f, 0f, rot_z - 90);
            }
            rightDirecton += dog.transform.localPosition;
            if (dog.gameObject.GetComponent<LineRenderer>() != null)
            {
                dog.gameObject.GetComponent<LineRenderer>().SetPosition(0, Righthandroot.position);
                dog.gameObject.GetComponent<LineRenderer>().SetPosition(1, dog.transform.position);
            }
            i++;

        }


        // POSITION ARMS
        leftDirection = leftDirection / _leftDogs.Count;
        rightDirecton = rightDirecton / _rightDogs.Count;
        leftDirection.Normalize();
        rightDirecton.Normalize();
        
        float Lrot_z = Mathf.Atan2(leftDirection.y, leftDirection.x) * Mathf.Rad2Deg;
        leftArmRoot.transform.localRotation = Quaternion.Euler(0f, 0f, Lrot_z + (leftDirection == new Vector3(0,0,0) ? 0 : -90));
        
        float Rrot_z = Mathf.Atan2(rightDirecton.y, rightDirecton.x) * Mathf.Rad2Deg;
        rightArmRoot.transform.localRotation = Quaternion.Euler(0f, 0f, Rrot_z + (rightDirecton == new Vector3(0, 0, 0) ? 0 : -90));
        //----------

        // JAUGES FORCE
        float forceLeft = _forceLeft * 1.0f / _maxForcePlayer;
        float forceRight = _forceRight * 1.0f / _maxForcePlayer;

        _jauge[0].color = Color.HSVToRGB((120 - forceLeft * 120) / 255.0f, 1, 1);
        _jauge[0].fillAmount = forceLeft;
        _jauge[1].color = Color.HSVToRGB((120 - forceRight * 120) / 255.0f, 1, 1);
        _jauge[1].fillAmount = forceRight;

        if (forceLeft >= 1.0f)
        {
            int j = Random.Range(0, _leftDogs.Count);
            _leftDogs[j].Evade();
            _leftDogs.RemoveAt(j);
        }
        if (forceRight >= 1.0f)
        {
            int j = Random.Range(0, _rightDogs.Count);
            _rightDogs[j].Evade();
            _rightDogs.RemoveAt(j);
        }


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
        for (int i = 0; i < _leftDogs.Count; i++)
        {
            float angle = Mathf.PI / (_leftDogs.Count + 1);
            float x = _distPlayer * Mathf.Cos(angle * (i + 1) + Mathf.PI / 2);
            float y = _distPlayer * Mathf.Sin(angle * (i + 1) + Mathf.PI / 2);

            _leftDogs[i].transform.localPosition = new Vector3(x, y, 0);

            if (_leftDogs[i].GetLastInteract() == new Vector3(0, 0, 0))
            {
                Vector3 diff = _leftDogs[i].transform.position - transform.position;
                diff.Normalize();

                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                _leftDogs[i].transform.GetChild(0).localRotation = Quaternion.Euler(0f, 0f, rot_z - 90);
            }

          }

        for (int i = 0; i < _rightDogs.Count; i++)
        {
            float angle = Mathf.PI / (_rightDogs.Count + 1);
            float x = _distPlayer * Mathf.Cos(angle * (i + 1) + Mathf.PI + Mathf.PI / 2);
            float y = _distPlayer * Mathf.Sin(angle * (i + 1) + Mathf.PI + Mathf.PI / 2);

            _rightDogs[i].transform.localPosition = new Vector3(x, y, 0);

            if (_rightDogs[i].GetLastInteract() == new Vector3(0, 0, 0))
            {
                Vector3 diff = _rightDogs[i].transform.position - transform.position;
                diff.Normalize();

                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                _rightDogs[i].transform.GetChild(0).localRotation = Quaternion.Euler(0f, 0f, rot_z - 90);
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
            other.gameObject.GetComponent<Dog>().playBackgroundLoop();
            other.gameObject.GetComponent<Dog>().haveMaster(true);
           // other.gameObject.AddComponent<LineRenderer>();

            other.gameObject.GetComponent<LineRenderer>().SetPosition(0, transform.position);
            other.gameObject.GetComponent<LineRenderer>().SetPosition(1, other.transform.position);
            other.gameObject.GetComponent<LineRenderer>().startWidth = .01f;
            other.gameObject.GetComponent<LineRenderer>().endWidth = .01f;
        }

    }
    public int getDogInHands()
    {
        return dogAmount;
    }
}
