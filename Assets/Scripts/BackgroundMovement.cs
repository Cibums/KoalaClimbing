using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    [Range(0f,1f)]
    public float depth;
    public float minStartY;
    public float maxStartY;

    public float minStartX;
    public float maxStartX;


    GameController gameController;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void Update()
    {
        if (gameController.isClimbing)
        {
            transform.position += new Vector3(0, 1, 0) * Time.deltaTime * (gameController.Player.gameObject.GetComponent<PlayerController>().Speed + gameController.Player.gameObject.GetComponent<PlayerController>().speedAddition) / (1/depth);
        }

        if(gameController.resetBackground)
        {
            if (minStartX >= maxStartX)
            {
                transform.position = new Vector2(maxStartX, Random.Range(minStartY, maxStartY));
            }
            else
            {
                transform.position = new Vector2(UnityEngine.Random.Range(minStartX, maxStartX), UnityEngine.Random.Range(minStartY, maxStartY));
            }

            
        }

    }
}
