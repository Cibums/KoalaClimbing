using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    public Logo[] logos;
    public float introLength;
    public string SceneName;

    private int i = 0;
    private float seconds = 0;

    private void Start()
    {
        foreach (Logo logo in logos)
        {
            logo.LogoObject.SetActive(false);
        }
    }

    void Update()
    {
        i++;
        seconds = i * Time.deltaTime;

        foreach (Logo logo in logos)
        {
            if (seconds >= logo.startSeconds)
            {
                logo.LogoObject.SetActive(true);
            }
        }

        if (seconds >= introLength)
        {
            SceneManager.LoadScene(SceneName);
        }
    }
}

[System.Serializable]
public class Logo
{
    public float startSeconds;
    public GameObject LogoObject;
}
