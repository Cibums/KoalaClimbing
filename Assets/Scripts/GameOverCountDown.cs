using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverCountDown : MonoBehaviour
{
    public int seconds = 3;
    public float time;
    public bool Counting = false;
    public Slider timePercentageSlider;

    GameController gameController;

    private void Awake()
    {
        time = seconds;
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void OnEnable()
    {
        time = seconds;
    }

    void Update()
    {
        timePercentageSlider.value = time / seconds;

        if (!Counting)
        {
            time = seconds;
            return;
        }

        time -= Time.deltaTime;
        if (time < 0)
        {
            gameController.GameOver();
        }
    }
}
