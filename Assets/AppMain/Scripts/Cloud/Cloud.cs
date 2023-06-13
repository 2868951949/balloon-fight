using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public GameObject thunderPrefab;

    private void Start()
    {
        StartCoroutine(GenerateThunder());
    }

    IEnumerator GenerateThunder()
    {
        int count = 2;

        float randomTime = Random.Range(10f, 15.0f);

        while (count > 0)
        {
            yield return new WaitForSeconds(randomTime);

            GameObject go = Instantiate(thunderPrefab, transform.position, Quaternion.identity);
            Vector2 velocity = new Vector2(Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f));
            go.GetComponent<Rigidbody2D>().velocity = velocity;

            count--;
        }
    }
}
