using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoggyBag : MonoBehaviour
{
    [SerializeField]
    int _numberDogs = 20;

    [SerializeField]
    RuntimeAnimatorController[] _shybaType;

    [SerializeField]
    RuntimeAnimatorController[] _bulldogType;

    [SerializeField]
    RuntimeAnimatorController[] _chihuahua;

    [SerializeField]
    public AudioClip[] _sourceSound; // 0 - big / 1 - medium / 2 - small / 3 - sniff

    [SerializeField]
    mapGenerator _mapGenerator;

    [SerializeField]
    int _numberObjects = 20;

    [SerializeField]
    GameObject[] _objects;

    [SerializeField]
    GameObject StartPoint;

    [SerializeField]
    GameObject Endpoint;

    List<Vector3> sideWalkList = new List<Vector3>();
    List<Vector3> freeSpaceList = new List<Vector3>();

    [SerializeField]
    GameObject batSmoke;

    void Start()
    {
        StartCoroutine("WaitMap");
    }

    IEnumerator WaitMap()
    {
        while(!_mapGenerator._finish)
        {
            yield return new WaitForSeconds(0.1f);
        }
        sideWalkList = _mapGenerator.sideWalkList;
        freeSpaceList = _mapGenerator.freeSpaceList;
        StartGenerator();
    }

    void StartGenerator()
    {
        RuntimeAnimatorController[][] _allDogs = new RuntimeAnimatorController[3][];
        _allDogs[0] = _shybaType;
        _allDogs[1] = _bulldogType;
        _allDogs[2] = _chihuahua;

        for (int i = 0; i < _numberDogs; i++)
        {
            GameObject newDog = Instantiate(Resources.Load("DogPrefab")) as GameObject;

            int pos = Random.Range(0, sideWalkList.Count);
            newDog.transform.position = sideWalkList[pos];
            sideWalkList.RemoveAt(pos);
            newDog.transform.rotation = new Quaternion();
            Dog scriptDog = newDog.GetComponent<Dog>();

            int typeDog = Random.Range(0, _allDogs.Length);

            switch (typeDog)
            {
                case 0: // Shiba
                    scriptDog.AddForce(6);
                    scriptDog.AddPerception(40);
                    scriptDog.woof.clip = _sourceSound[1];
                    scriptDog.background[0].clip = _sourceSound[3];
                    scriptDog.scoreValue = 3;
                    break;
                case 1: // Bulldog
                    scriptDog.AddForce(10);
                    scriptDog.AddPerception(20);
                    scriptDog.woof.clip = _sourceSound[0];
                    scriptDog.background[0].clip = _sourceSound[3];
                    scriptDog.scoreValue = 5;
                    break;
                case 2: // Chihuahua
                    scriptDog.AddForce(2);
                    scriptDog.AddPerception(60);
                    scriptDog.woof.clip = _sourceSound[1];
                    scriptDog.background[0].clip = _sourceSound[3];
                    scriptDog.scoreValue = 1;
                    break;
            }


            int caract = Random.Range(0, 6);
            newDog.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = _allDogs[typeDog][caract];
            ParticleSystem.MainModule settingsVision = scriptDog.vision.main;
            switch (caract)
            {
                case 0:
                    SuperDog superDog = newDog.AddComponent<SuperDog>();
                    superDog.Initialize(scriptDog);
                    settingsVision.startColor = new Color(0.6f, 0, 0, 0.8f);
                    scriptDog.scoreValue *= 5;
                    break;
                case 1:
                    BatDog batDog = newDog.AddComponent<BatDog>();
                    batDog.Initialize(scriptDog);
                    settingsVision.startColor = new Color(0.1f, 0.1f, 0.1f, 0.8f);
                    scriptDog.scoreValue *= 3;


                    GameObject smoke = Instantiate(batSmoke);
                    smoke.transform.SetParent(newDog.transform);
                    smoke.transform.localPosition = new Vector3();
                    batDog._batdogSmoke = smoke.GetComponent<ParticleSystem>();

                    break;
                case 2:
                    AquaDog aquaDog = newDog.AddComponent<AquaDog>();
                    aquaDog.Initialize(scriptDog);
                    settingsVision.startColor = new Color(0, 0, 0.6f, 0.8f);
                    scriptDog.scoreValue *= 1;
                    break;
                case 3:
                    ThorDog thorDog = newDog.AddComponent<ThorDog>();
                    thorDog.Initialize(scriptDog);
                    settingsVision.startColor = new Color(0.6f, 0, 0, 0.8f);
                    scriptDog.scoreValue *= 5;
                    break;
                case 4:
                    CaptainDog captainDog = newDog.AddComponent<CaptainDog>();
                    captainDog.Initialize(scriptDog);
                    settingsVision.startColor = new Color(0, 0, 0.6f, 0.8f);
                    scriptDog.scoreValue *= 3;
                    break;
                case 5:
                    LokiDog lokiDog = newDog.AddComponent<LokiDog>();
                    lokiDog.Initialize(scriptDog);
                    settingsVision.startColor = new Color(0, 0.6f, 0, 0.8f);
                    scriptDog.scoreValue *= 3;
                    break;
            }
        }

        foreach (Vector3 pos in sideWalkList)
            freeSpaceList.Add(pos);

        for (int i = 0; i < _numberObjects; i++)
        {
            int pos = Random.Range(0, freeSpaceList.Count);

            int typeObject = Random.Range(0, _objects.Length);
            GameObject newObject = Instantiate(_objects[typeObject]) as GameObject;
            newObject.transform.position = freeSpaceList[pos] + new Vector3(0, 0, -0.1f);
            newObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f));
            freeSpaceList.RemoveAt(pos);
        }

        addStartStopPoint();

        //Sprite sprite = newDog.AddComponent<SpriteRenderer>();
    }

    public int getDogOnMap()
    {
        return _numberDogs;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void addStartStopPoint()
    {
        StartPoint.transform.position =  _mapGenerator.EntryPoint;
        Endpoint.transform.position = _mapGenerator.ExitPoint;

    }
}
