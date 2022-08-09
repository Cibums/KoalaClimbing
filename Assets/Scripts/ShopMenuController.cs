using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenuController : MonoBehaviour
{
    public static ShopMenuController MC;

    public GameObject[] buttonLocks;
    public Button[] unlockedButtons;
    public int[] costs;
    public Text[] costText;

    GameController gameController;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void OnEnable()
    {
        MC = this;
    }

    void Start()
    {
        SetUpStore();
    }

    public void SetUpStore()
    {
        for (int i = 0; i < PersistantData.PD.allCharacters.Length; i++)
        {
            buttonLocks[i].SetActive(!PersistantData.PD.allCharacters[i]);
            unlockedButtons[i].interactable = PersistantData.PD.allCharacters[i];
            costText[i].text = costs[i].ToString();
        }
    }

    public void UnlockCharacter(int index)
    {
        if (costs[index] <= gameController.TotalLeaves)
        {
            PersistantData.PD.allCharacters[index] = true;
            PlayFabController.PFC.SetUserData(PersistantData.PD.CharacterDataToString());
            gameController.TotalLeaves -= costs[index];
            SetUpStore();
            gameController.SaveGame();
        }

        //Disclaimer
    }

    public void SetSelectedCharacter(int whichCharacter)
    {
        PersistantData.PD.selectedCharacter =  whichCharacter;
        gameController.selectedCharacterID = whichCharacter;
        gameController.gameObject.GetComponent<AudioSource>().clip = gameController.CharacterMusic[whichCharacter];
        gameController.SaveGame();
    }

}
