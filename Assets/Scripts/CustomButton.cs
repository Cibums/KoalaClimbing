using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerDownHandler
{
    public PlayerController playerController;

    GameController gameController;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameController.hasLost == false && gameController.isClimbing == false)
        {
            gameController.Climb();
            return;
        }

        playerController.right = !playerController.right;
        gameController.PlaySound("SwitchNew", 0.3f);
    }
}
