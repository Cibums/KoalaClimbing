using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    

    public AudioClip[] CharacterMusic;
    public AudioSource backgroundMusicSource;
    public AudioClip[] sounds;

    public List<LeaderboardHighScore> LeaderboardHighScores;

    public int saveVersion = 0;
    public int Leaves = 0;
    public int TotalLeavesInGame = 0;
    public int TotalLeaves = 0;
    public int HighScore = 0;
    public int selectedCharacterID = 0;

    [Range(0, 1)]
    public float MusicVolume;
    public float RewardMultiplier = 1;

    public bool rewardedAdWatched = false;
    public bool SoundFXOn = true;
    public bool MusicOn = true;
    public bool isClimbing = false;
    public bool hasLost = false;
    [HideInInspector]
    public bool resetBackground = false;

    private bool continueClicked = false;
    private bool endGame2XClicked = false;

    public InputField FriendUsernameInputField;

    public Button MusicToggleButton;
    public Button SoundFXToggleButton;
    public Button ScoreTabPanelButton;
    public Button LeaderboardTabPanelButton;
    public Button ShopTabPanelButton;
    public Button ProfileTabPanelButton;

    public Text heightText;
    public Text leafText;
    public Text ResultText;
    public Text TotalLeavesText;
    public Text DebugPanel;

    public Animator[] CharacterAnimators;
    public float[] CharacterHitboxHeights;

    public Sprite Music;
    public Sprite NoMusic;
    public Sprite SoundFX;
    public Sprite NoSoundFX;
    public Sprite buttonSprite;
    public Sprite tabOpenSprite;
    public Sprite UnlockedSprite;

    public Transform FriendPanelParent;
    public Transform Player;
    public Transform Root;
    public Transform scoreboardParent;

    public GameObject FriendPanelPrefab;
    public GameObject GameOverPanel;
    public GameObject ConnectAccountButton;
    public GameObject ContinuePanel;
    public GameObject ShopPanel;
    public GameObject DefaultPanel;
    public GameObject SettingsPanel;
    public GameObject ScoreTabPanel;
    public GameObject LeaderboardTabPanel;
    public GameObject ShopTabPanel;
    public GameObject ProfileTabPanel;
    public GameObject ScoreboardPartPrefab;
    public GameObject DeleteOnClimb;
    public GameObject HitPanel;
    public GameObject LoginPanel;
    public GameObject NewDevicePanel;

    AdController ads;

    private void Awake()
    {
        Debug.Log("Shop System Video: " + @"https://www.youtube.com/watch?v=5pqemuPpyjQ");

        ads = GetComponent<AdController>();
    }

    public void AddFriend()
    {
        PlayFabController.PFC.AddFriendByDisplayName(FriendUsernameInputField.text);
        PlayFabController.PFC.GetFriends();
    }

    public void DisplayFriends()
    {
        foreach (Transform child in FriendPanelParent)
        {
            Destroy(child.gameObject);
        }

        foreach (string name in PlayFabController.PFC.GetFriendNames())
        {
            Transform tf = Instantiate(FriendPanelPrefab, FriendPanelParent).transform;
            tf.GetComponentInChildren<Text>().text = name;
        }
    }

    public void SelectScoreTab()
    {
        ScoreTabPanel.SetActive(true);
        LeaderboardTabPanel.SetActive(false);
        ShopTabPanel.SetActive(false);
        ProfileTabPanel.SetActive(false);

        ScoreTabPanelButton.image.sprite = tabOpenSprite;
        LeaderboardTabPanelButton.image.sprite = buttonSprite;
        ShopTabPanelButton.image.sprite = buttonSprite;
        ProfileTabPanelButton.image.sprite = buttonSprite;
    }

    public void SelectProfileTab()
    {
        ScoreTabPanel.SetActive(false);
        LeaderboardTabPanel.SetActive(false);
        ShopTabPanel.SetActive(false);
        ProfileTabPanel.SetActive(true);

        ScoreTabPanelButton.image.sprite = buttonSprite;
        LeaderboardTabPanelButton.image.sprite = buttonSprite;
        ShopTabPanelButton.image.sprite = buttonSprite;
        ProfileTabPanelButton.image.sprite = tabOpenSprite;

        PlayFabController.PFC.GetFriends();
        PlayFabController.PFC.GetPlayerProfile();
    }

    public void SelectLeaderboardTab()
    {
        ScoreTabPanel.SetActive(false);
        LeaderboardTabPanel.SetActive(true);
        ShopTabPanel.SetActive(false);
        ProfileTabPanel.SetActive(false);

        ScoreTabPanelButton.image.sprite = buttonSprite;
        LeaderboardTabPanelButton.image.sprite = tabOpenSprite;
        ShopTabPanelButton.image.sprite = buttonSprite;
        ProfileTabPanelButton.image.sprite = buttonSprite;

        PlayFabController.PFC.GetLeaderBoarder();
    }

    public void SelectShopTab()
    {
        ShopMenuController.MC.SetUpStore();

        ScoreTabPanel.SetActive(false);
        LeaderboardTabPanel.SetActive(false);
        ShopTabPanel.SetActive(true);
        ProfileTabPanel.SetActive(false);

        ScoreTabPanelButton.image.sprite = buttonSprite;
        LeaderboardTabPanelButton.image.sprite = buttonSprite;
        ShopTabPanelButton.image.sprite = tabOpenSprite;
        ProfileTabPanelButton.image.sprite = buttonSprite;
    }

    void Start()
    {

        Debug.LogWarning("www.freevector.com");
        //LoadGame();

        if (MusicOn)
        {
            MusicToggleButton.image.sprite = Music;
        }
        else
        {
            MusicToggleButton.image.sprite = Music;
        }

        if (SoundFX)
        {
            SoundFXToggleButton.image.sprite = SoundFX;
        }
        else
        {
            SoundFXToggleButton.image.sprite = NoSoundFX;
        }

        ClimbAgain();
    }

    public void ResetGame()
    {
        PlayerPrefs.DeleteKey("savedOnce" + saveVersion.ToString());

        PlayerPrefsSetBool("tutorialWatched", false);
        PlayerPrefs.SetInt("totalLeaves", 0);

        PlayerPrefs.Save();
        SceneManager.LoadScene("intro");
    }


    public void SaveGame()
    {
        PlayerPrefsSetBool("savedOnce" + saveVersion.ToString(), true);

        PlayerPrefs.SetInt("totalLeaves", TotalLeaves);
        PlayerPrefs.SetInt("highScore", HighScore);

        PlayerPrefs.SetInt("selectedCharacterID", selectedCharacterID);

        PlayFabController.PFC.SetStats();
        PlayerPrefs.SetString("characters", PersistantData.PD.CharacterDataToString());

        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("savedOnce" + saveVersion.ToString()))
        {
            TotalLeaves = PlayerPrefs.GetInt("totalLeaves");
            HighScore = PlayerPrefs.GetInt("highScore");

            selectedCharacterID = PlayerPrefs.GetInt("selectedCharacterID"); ;

            PersistantData.PD.CharacterStringToData(PlayerPrefs.GetString("characters"));
        }
        else
        {
            print("No game loaded, created new game");
            SaveGame();
        }

    }

    void PlayerPrefsSetBool(string key, bool value)
    {
        if (value)
        {
            PlayerPrefs.SetInt(key, 1);
        }
        else
        {
            PlayerPrefs.SetInt(key, 0);
        }
    }

    bool PlayerPrefsGetBool(string key)
    {
        if (PlayerPrefs.GetInt(key) > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdateLeaderboard()
    {

        foreach (Transform child in scoreboardParent)
        {
            Destroy(child.gameObject);
        }

        if (LeaderboardHighScores.Count < 0)
        {
            return;
        }

        foreach (LeaderboardHighScore lbh in LeaderboardHighScores)
        {
            Transform tf = Instantiate(ScoreboardPartPrefab, scoreboardParent).transform;

            tf.Find("NameText").gameObject.GetComponent<Text>().text = lbh.name + ":";
            tf.Find("ScoreText").gameObject.GetComponent<Text>().text = (lbh.score).ToString();
        }
    }

    private void Update()
    {
        

        TotalLeavesText.text = TotalLeaves.ToString();

        if (gameObject.GetComponent<AudioSource>().clip != CharacterMusic[selectedCharacterID])
        {
            gameObject.GetComponent<AudioSource>().clip = CharacterMusic[selectedCharacterID];
        }

        if (!gameObject.GetComponent<AudioSource>().isPlaying)
        {
            gameObject.GetComponent<AudioSource>().Play();
        }

        UpdateLeaderboard();
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.AltGr))
        {
            PlayerPrefs.DeleteKey("EMAIL");
            PlayerPrefs.DeleteKey("USERNAME");
            PlayerPrefs.DeleteKey("PASSWORD");

            SceneManager.LoadScene("intro");
        }

        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    WinAndCollect();
        //}

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    ResetGame();
        //}

        if (MusicOn)
        {
            backgroundMusicSource.volume = MusicVolume;
        }
        else
        {
            backgroundMusicSource.volume = 0;
        }

        if (Player.position.y > 0)
        {
            heightText.text = Mathf.RoundToInt(Player.position.y).ToString() + "m";
        }
        else
        {
            heightText.text = "0m";
        }


        DebugPanel.text = "Leaves: " + Leaves + "; TotalLeaves: " + TotalLeaves + "; Is Climbing: " + isClimbing + "; Has Lost: " + hasLost + "; Right: " + Player.gameObject.GetComponent<PlayerController>().right;

        foreach (Animator animator in CharacterAnimators)
        {
            if (animator.gameObject.activeSelf)
            {
                animator.SetBool("isClimbing", isClimbing);
            }

            
        }

        leafText.text = "LEAVES:\n" + Leaves.ToString();

        #region Rewards

        if (rewardedAdWatched)
        {

            #region Continue

            if (continueClicked)
            {
                hasLost = false;
                GameOverPanel.SetActive(false);
                ContinuePanel.SetActive(false);
                SettingsPanel.SetActive(false);
                ShopPanel.SetActive(false);
                DefaultPanel.SetActive(true);
                isClimbing = false;
                Player.gameObject.GetComponent<PlayerController>().right = true;

                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("TreePart"))
                {
                    if (obj.GetComponent<Branch>())
                    {
                        if (Vector2.Distance(obj.transform.position, Player.position) < 7)
                        {
                            Destroy(obj);
                        }
                    }
                }

                continueClicked = false;
                rewardedAdWatched = false;
                return;
            }

            #endregion

            #region End Game With Double Points

            if (endGame2XClicked)
            {
                RewardMultiplier = 2;

                GameOver();

                endGame2XClicked = false;
                rewardedAdWatched = false;
                return;
            }

            #endregion

            rewardedAdWatched = false;
        }

        #endregion
    }

    public void Shop()
    {
        DefaultPanel.SetActive(false);
        GameOverPanel.SetActive(false);
        ShopPanel.SetActive(true);
        PlaySound("Leaf", 1);
    }

    public void CloseShop()
    {
        DefaultPanel.SetActive(false);
        GameOverPanel.SetActive(true);
        ShopPanel.SetActive(false);
        PlaySound("Leaf", 1);
    }

    public void SetDefaultPanel()
    {
        NewDevicePanel.SetActive(false);
        DefaultPanel.SetActive(true);
        GameOverPanel.SetActive(false);
        ShopPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        PlaySound("Leaf", 1);
    }

    public void Continue()
    {
        ads.ShowRewardedAd();
        continueClicked = true;
    }

    public void EndGame2X()
    {
        ads.ShowRewardedAd();
        endGame2XClicked = true;
    }

    public void ClimbAgain()
    {
        foreach (Animator a in CharacterAnimators)
        {
            a.gameObject.SetActive(false);
        }

        if (!PlayerPrefs.HasKey("selectedCharacterID"))
        {
            selectedCharacterID = PersistantData.PD.selectedCharacter;
        }
        else
        {
            selectedCharacterID = PlayerPrefs.GetInt("selectedCharacterID");
        }

        

        CharacterAnimators[selectedCharacterID].gameObject.SetActive(true);
        Player.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(Player.gameObject.GetComponent<BoxCollider2D>().size.x, CharacterHitboxHeights[selectedCharacterID]);

        Player.gameObject.GetComponent<PlayerController>().speedAddition = 0;

        RewardMultiplier = 1;

        DeleteOnClimb.SetActive(true);

        SetDefaultPanel();

        Leaves = 0;
        Player.position = new Vector3(0.5f,-2,0);
        ContinuePanel.SetActive(false);
        SettingsPanel.SetActive(false);
        ShopPanel.SetActive(false);

        isClimbing = false;
        hasLost = false;
        DefaultPanel.SetActive(true);
        GameOverPanel.SetActive(false);

        foreach (GameObject treePart in GameObject.FindGameObjectsWithTag("TreePart"))
        {
            Destroy(treePart);
        }

        Root.gameObject.GetComponent<AddTreePart>().ResetPart();

        resetBackground = true;
    }

    public void WinAndCollect()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("TreePart"))
        {
            if (go.GetComponent<Collectable>())
            {

                go.GetComponent<Collectable>().Collect();
            }
        }

        Win();
    }



    public void Climb()
    {
        DeleteOnClimb.SetActive(false);
        Player.gameObject.GetComponent<PlayerController>().right = true;
        isClimbing = true;
        resetBackground = false;
    }

    public void Instagram()
    {
        Application.OpenURL(@"https://www.instagram.com/crylath/");
    }

    public void FaceBook()
    {
        Application.OpenURL(@"https://www.facebook.com/crylath/");
    }

    public void OpenPanel(Transform panel)
    {
        SetDefaultPanel();
        panel.gameObject.SetActive(true);
        DefaultPanel.SetActive(false);
    }

    public void Win()
    {
        print("Won");

        hasLost = true;
        isClimbing = false;
        GameOver();
    }

    public void Lose()
    {

        
        print("Lost");

        Player.gameObject.GetComponent<PlayerController>().right = true;

        if (Player.transform.position.y > HighScore)
        {
            HighScore = Mathf.RoundToInt(Player.transform.position.y);
        }

        hasLost = true;
        isClimbing = false;
        ContinuePanel.SetActive(true);
        DefaultPanel.SetActive(false);
    }

    public void GameOver()
    {

        TotalLeavesInGame = Mathf.RoundToInt(Leaves * RewardMultiplier);
        TotalLeaves += TotalLeavesInGame;

        SelectScoreTab();

        isClimbing = false;
        ContinuePanel.SetActive(false);
        GameOverPanel.SetActive(true);

        PlaySound("Leaf", 1);

        int height = Mathf.RoundToInt(Player.position.y);

        ResultText.text = "HEIGHT:\n" + height.ToString();

        DefaultPanel.SetActive(false);

        SaveGame();

        PlayFabController.PFC.GetLeaderBoarder();

    }

    public void OpenSettingsPanel()
    {
        SettingsPanel.SetActive(true);
        DefaultPanel.SetActive(false);

        PlaySound("Leaf", 1);
    }    

    public void ToggleSoundFX()
    {
        SoundFXOn = !SoundFXOn;

        if (SoundFXOn)
        {
            SoundFXToggleButton.image.sprite = SoundFX;
        }
        else
        {
            SoundFXToggleButton.image.sprite = NoSoundFX;
        }
    }

    public void ToggleMusic()
    {
        MusicOn = !MusicOn;

        if (MusicOn)
        {
            MusicToggleButton.image.sprite = Music;
        }
        else
        {
            MusicToggleButton.image.sprite = NoMusic;
        }
    }

    public void PlaySound(string soundname)
    {
        if (SoundFXOn == true)
        {
            foreach (AudioClip clip in sounds)
            {
                if (clip.name == soundname)
                {
                    AudioSource source = gameObject.AddComponent<AudioSource>();
                    source.loop = false;
                    source.volume = 1;
                    source.clip = clip;
                    source.Play();
                    DestroyAfter(source, clip.length);
                    return;
                }
            }

            print("Sound isn't in GameController");
            return;
        }
    }

    public void PlaySound(string soundname, float volume)
    {
        if (SoundFXOn == true)
        {
            foreach (AudioClip clip in sounds)
            {
                if (clip.name == soundname)
                {
                    AudioSource source = gameObject.AddComponent<AudioSource>();
                    //print(clip.length);
                    source.loop = false;
                    source.volume = volume;
                    source.clip = clip;
                    source.Play();
                    DestroyAfter(source, clip.length);
                    return;
                }
            }

            print("Sound isn't in GameController");
            return;
        }
        
    }


    public void DestroyAfter(Object obj, float seconds)
    {
        StartCoroutine(DestroyAfterCoroutine(obj,seconds));
    }

    public IEnumerator DestroyAfterCoroutine(Object obj, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(obj);
    }
}
