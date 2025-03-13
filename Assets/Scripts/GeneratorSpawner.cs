using System.Collections;
using UnityEngine;

public class GeneratorSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // spawn something every n seconds
        // the object that is spawned has its own behavior
        StartCoroutine(SpawnSomething());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator SpawnSomething()
    {
        ///loop as long as the value is true
        while (true)
        {
            Vector3 pos = new Vector3 (Random.Range(-10,10), 2, Random.Range(-10, 10));
            Instantiate(prefab, pos, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
