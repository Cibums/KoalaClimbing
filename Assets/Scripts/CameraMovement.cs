using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float StartY = -0.5f;

    GameController gameController;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void Update()
    {
        if (gameController.isClimbing)
        {
            transform.position += new Vector3(0, 1, 0) * Time.deltaTime * (gameController.Player.gameObject.GetComponent<PlayerController>().Speed + gameController.Player.gameObject.GetComponent<PlayerController>().speedAddition);
        }

        if (gameController.resetBackground)
        {
            transform.position = new Vector3(0, StartY, -10);
        }
    }
}
