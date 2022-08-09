using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTreePart : MonoBehaviour
{
    public GameObject TreePartPrefab;
    public GameObject TreeBranchRight;
    public GameObject TreeBranchLeft;

    public bool isRoot = false;
    public int yDistance;

    private bool spawnedOnTop = false;
    private Transform Player;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void ResetPart()
    {
        spawnedOnTop = false;
    }

    void Update()
    {
        if (Player.position.y > transform.position.y + yDistance)
        {
            if (!isRoot)
            {
                Destroy(gameObject);
            }
        }

        if (Player.position.y > transform.position.y - yDistance)
        {
            if (spawnedOnTop == false)
            {
                spawnedOnTop = true;
                Instantiate(TreePartPrefab, transform.position + new Vector3(0,3, 0), Quaternion.identity);

                if (!isRoot)
                {
                    int rnd = Random.Range(0,101);

                    float x = 1.5f;

                    if (rnd > 50)
                    {
                        Instantiate(TreeBranchRight, transform.position + new Vector3(x,0,0), Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(TreeBranchLeft, transform.position + new Vector3(-x, 0, 0), Quaternion.identity);
                    }
                }
            }
        }
    }
}
