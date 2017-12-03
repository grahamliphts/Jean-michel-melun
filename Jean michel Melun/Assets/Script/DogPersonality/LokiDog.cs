using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LokiDog : MonoBehaviour
{
    private bool _initialize = false;
    private bool _found = false;
    private bool _ultiStart = false;

    private Dog _dog;

    [SerializeField]
    int _addForceStat = 0; // +1 / +3 / +5

    [SerializeField]
    float _addPerceptionStat = 40; // en °

    [SerializeField]
    uint _timeReloadUlti = 60; // en secondes

    [SerializeField]
    uint _timeUlti = 30; // en secondes

    [SerializeField]
    float _probabilityUlti = 0.1f; // en %

    [SerializeField]
    Sprite _hat;

    [SerializeField]
    Sprite _property;

    [SerializeField]
    Sprite _cape;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_initialize && !_found && _dog.tag == "Dog")
        {
            _found = true;
            transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
        }


        if (_initialize && _found && !_ultiStart)
        {
            _ultiStart = true;
            StartCoroutine("TriggerUlti");
        }
    }

    public void Initialize(Dog dog)
    {
        _dog = dog;
        _initialize = true;

        _dog.AddForce(_addForceStat);
        _dog.AddPerception(_addPerceptionStat);
    }

    void DogFound()
    {
        _found = true;
        transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
    }

    IEnumerator TriggerUlti()
    {
        yield return new WaitForSeconds(_timeReloadUlti);

        float randomValue = Random.Range(0, 100) / 100.0f;

        if (randomValue < _probabilityUlti)
        {
            StartUlti();
            StartCoroutine("StopUlti");
        }
        else
            _ultiStart = false;

    }

    void StartUlti()
    {
        _dog.gameObject.tag = "LostDog";
        _found = false;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        _dog.gameObject.GetComponent<CircleCollider2D>().enabled = false;
        _dog.gameObject.transform.SetParent(null);
    }

    IEnumerator StopUlti()
    {
        yield return new WaitForSeconds(_timeUlti);
        _ultiStart = false;
        _dog.gameObject.SetActive(true);
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
        _dog.gameObject.GetComponent<CircleCollider2D>().enabled = true;
    }
}
