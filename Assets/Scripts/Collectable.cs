using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public int minLeaves = 0;
    public int maxLeaves = 100;
    public GameObject CounterPrefab;
    GameController gameController;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Collect();
        }
    }

    public void Collect()
    {
        gameController.PlaySound("Leaf", 0.5f);
        int rnd = Random.Range(minLeaves, maxLeaves);

        GameObject counter = Instantiate(CounterPrefab, transform.position, Quaternion.identity);

        if (Mathf.RoundToInt(rnd * (transform.position.y / 100)) > 0)
        {
            gameController.Leaves += Mathf.RoundToInt(rnd * (transform.position.y / 100));
            counter.GetComponent<TextMesh>().text = Mathf.RoundToInt(rnd * (transform.position.y / 100)).ToString();
        }
        else
        {
            gameController.Leaves++;
            counter.GetComponent<TextMesh>().text = "1";
        }

        Destroy(gameObject);
    }
}
