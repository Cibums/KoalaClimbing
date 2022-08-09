using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantData : MonoBehaviour
{
    public static PersistantData PD;

    public bool[] allCharacters;
    public int selectedCharacter = 0;

    private void OnEnable()
    {
        PersistantData.PD = this;
    }

    public void CharacterStringToData(string characterIn)
    {
        for (int i = 0; i < characterIn.Length; i++)
        {
            if (int.Parse(characterIn[i].ToString()) > 0)
            {
                allCharacters[i] = true;
            }
            else
            {
                allCharacters[i] = false;
            }
        }
    }

    public string CharacterDataToString()
    {
        string toString = "";
        for (int i = 0; i < allCharacters.Length; i++)
        {
            if (allCharacters[i] == true)
            {
                toString += "1";
            }
            else
            {
                toString += "0";
            }
        }

        return toString;
    }

    void Update()
    {
        
    }

}
