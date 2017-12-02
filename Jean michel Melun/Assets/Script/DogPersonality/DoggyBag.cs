using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoggyBag : MonoBehaviour
{
    [SerializeField]
    int _numberDogs = 6;
    
    // Use this for initialization
    void Start ()
    {
        for(int i = 0; i < _numberDogs; i++)
        {
            GameObject newDog = Instantiate(Resources.Load("DogPrefab")) as GameObject;
            newDog.transform.position = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), 0);
            newDog.transform.rotation = new Quaternion();

            newDog.GetComponent<Dog>().AddForce(1);
            newDog.GetComponent<Dog>().AddPerception(20);

            int caract = Random.Range(0, 6);
            switch (caract)
            {
                case 0:
                    SuperDog superDog = newDog.AddComponent<SuperDog>();
                    superDog.Initialize(newDog.GetComponent<Dog>());
                    newDog.GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                case 1:
                    BatDog batDog = newDog.AddComponent<BatDog>();
                    batDog.Initialize(newDog.GetComponent<Dog>());
                    newDog.GetComponent<SpriteRenderer>().color = Color.black;
                    break;
                case 2:
                    AquaDog aquaDog = newDog.AddComponent<AquaDog>();
                    aquaDog.Initialize(newDog.GetComponent<Dog>());
                    newDog.GetComponent<SpriteRenderer>().color = Color.blue;
                    break;
                case 3:
                    ThorDog thorDog = newDog.AddComponent<ThorDog>();
                    thorDog.Initialize(newDog.GetComponent<Dog>());
                    newDog.GetComponent<SpriteRenderer>().color = Color.yellow;
                    break;
                case 4:
                    CaptainDog captainDog = newDog.AddComponent<CaptainDog>();
                    captainDog.Initialize(newDog.GetComponent<Dog>());
                    newDog.GetComponent<SpriteRenderer>().color = Color.white;
                    break;
                case 5:
                    LokiDog lokiDog = newDog.AddComponent<LokiDog>();
                    lokiDog.Initialize(newDog.GetComponent<Dog>());
                    newDog.GetComponent<SpriteRenderer>().color = Color.green;
                    break;
            }
        }
        //Sprite sprite = newDog.AddComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
