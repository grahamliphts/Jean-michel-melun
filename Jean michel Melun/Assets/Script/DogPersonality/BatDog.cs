using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatDog : MonoBehaviour
{
    private bool _initialize = false;
    private bool _found = false;
    private bool _ultiStart = false;

    private Dog _dog;

    [SerializeField]
    int _addForceStat = 0; // +1 / +3 / +5

    [SerializeField]
    float _addPerceptionStat = 20; // en °

    [SerializeField]
    uint _timeReloadUlti = 20; // en secondes

    [SerializeField]
    uint _timeUlti = 10; // en secondes

    [SerializeField]
    float _probabilityUlti = 0.4f; // en %

    [SerializeField]
    Sprite _hat;

    [SerializeField]
    Sprite _property;

    [SerializeField]
    Sprite _cape;

    [SerializeField]
    public ParticleSystem _batdogSmoke;

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
        _batdogSmoke.Play();
    }

    IEnumerator StopUlti()
    {
        yield return new WaitForSeconds(_timeUlti);
        _ultiStart = false;
        _batdogSmoke.Stop();
    }
}
