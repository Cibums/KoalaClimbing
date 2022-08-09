using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviour
{
    public Vector3 offset;
    public GameObject LeafBunchPrefab;
    GameController gameController;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void Start()
    {
        Instantiate(LeafBunchPrefab, transform.position + offset, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!collision.gameObject.GetComponent<PlayerController>().invincable)
            {
                gameController.PlaySound("hit", 1);
                gameController.Lose();
                Destroy(gameObject);
            }
        }
    }
}
