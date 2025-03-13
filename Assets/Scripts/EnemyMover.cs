using System.Collections;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    private Vector3 randomPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        randomPosition = new Vector3(Random.Range(-10, 10), transform.position.y, Random.Range(-10, 10));


    }

    // Update is called once per frame
    
    
    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            randomPosition,
            2 * Time.deltaTime);

        

        if (Vector3.Distance(transform.position,randomPosition) <= 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
