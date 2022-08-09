using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayFabController : MonoBehaviour
{
    public static PlayFabController PFC;

    public List<FriendInfo> _friends = null;

    public InputField DisplayNameInput;

    public string userEmail;
    public string userPassword;
    public string userName;
    public string displayName;

    private string myID;

    public GameObject LoginPanel;
    public GameObject AddLoginPanel;

    GameController gameController;

    private void OnEnable()
    {
        if (PlayFabController.PFC == null)
        {
            PlayFabController.PFC = this;
        }
        else
        {
            if (PlayFabController.PFC != this)
            {
                Destroy(this.gameObject);
            }
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    public void Start()
    {
        //Note: Setting title Id here can be skipped if you have set the value in Editor Extensions already.
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "D2D8F"; // Please change this value to your own titleId from PlayFab Game Manager
        }

        //var request = new LoginWithCustomIDRequest { CustomId = "GettingStartedGuide", CreateAccount = true };
        //PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);

        if (PlayerPrefs.HasKey("EMAIL"))
        {
            Debug.Log("Logging in via Email");

            userEmail = PlayerPrefs.GetString("EMAIL");
            userPassword = PlayerPrefs.GetString("PASSWORD");

            var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
            PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
        }
        else
        {
            //Don't find an account

            gameController.OpenPanel(gameController.NewDevicePanel.transform);

//            Debug.Log("Making new device account");

//#if UNITY_ANDROID
//            var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = ReturnMobileID(), CreateAccount = true };
//            PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnMobileLoginSuccess, OnMobileLoginFailure);
//#elif UNITY_IOS
//            var requestIOS = new LoginWithIOSDeviceIDRequest{ DeviceId = ReturnMobileID(), CreateAccount = true };
//            PlayFabClientAPI.LoginWithAndroidDeviceID(requestIOS, OnMobileLoginSuccess, OnMobileLoginFailure);
//#endif

            
        }

    }

    #region Login

    private void OnMobileLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        GetStatistics();

        myID = result.PlayFabId;

        GetPlayerProfile();
        GetPlayerData();
    }

    private void OnMobileLoginFailure(PlayFabError error)
    {


        //Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());

        gameController.SetDefaultPanel();
        gameController.OpenPanel(gameController.LoginPanel.transform);

        Debug.Log(SystemInfo.deviceName);
        ChangeDisplayName(SystemInfo.deviceName);

        gameController.LoadGame();
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");

        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);

        gameController.ConnectAccountButton.SetActive(false);
        AddLoginPanel.SetActive(false);
        LoginPanel.SetActive(false);
        GetStatistics();

        myID = result.PlayFabId;

        GetPlayerProfile();
        GetPlayerData();
    }

    public void ChangeDisplayNameToInputField()
    {
        ChangeDisplayName(DisplayNameInput.text);
    }

    public void ChangeDisplayName(string displayName)
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = displayName }, OnDisplayName, OnMobileLoginFailure);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");

        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);

        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = userName }, OnDisplayName, OnMobileLoginFailure);

        AddLoginPanel.SetActive(false);
        gameController.GameOver();

        OnClickLogin();
    }

    void OnDisplayName(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log(result.DisplayName + " is your new display name");
    }

    private void OnLoginFailure(PlayFabError error)
    {
        var registerRequest = new RegisterPlayFabUserRequest { Email = userEmail, Password = userPassword, Username = userName };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterFailure);

        gameController.LoadGame();

        //Debug.LogWarning("Something went wrong with your first API call.  :(");
        //Debug.LogError("Here's some debug information:");
        //Debug.LogError(error.GenerateErrorReport());
    }

    private void OnRegisterFailure(PlayFabError error)
    {
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());

        PlayerPrefs.DeleteKey("EMAIL");
        PlayerPrefs.DeleteKey("PASSWORD");

        userEmail = null;
        userPassword = null;

        Start();
    }

    public void GetUserEmail(string EmailIn)
    {
        userEmail = EmailIn;
    }

    public void GetUserPassword(string PasswordIn)
    {
        userPassword = PasswordIn;
    }

    public void GetUserName(string UsernameIn)
    {
        userName = UsernameIn;
    }

    public void OnClickLogin()
    {
        var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);

        gameController.GameOver();
    }

    public static string ReturnMobileID()
    {
        string deviceID = SystemInfo.deviceUniqueIdentifier;
        return deviceID;
    }

    public void OpenAddLoginPanel()
    {
        gameController.SetDefaultPanel();
        gameController.DefaultPanel.SetActive(false);
        LoginPanel.SetActive(false);
        AddLoginPanel.SetActive(true);
    }

    public void OpenLoginPanel()
    {
        gameController.SetDefaultPanel();
        gameController.DefaultPanel.SetActive(false);
        AddLoginPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }

    public void OnClickAddLogin()
    {
        var addLoginRequest = new AddUsernamePasswordRequest { Email = userEmail, Password = userPassword, Username = userName };
        PlayFabClientAPI.AddUsernamePassword(addLoginRequest, OnAddLoginSuccess, OnRegisterFailure);
    }

    private void OnAddLoginSuccess(AddUsernamePasswordResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");

        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);

        AddLoginPanel.SetActive(false);
        gameController.GameOver();

        gameController.ConnectAccountButton.SetActive(false);

        GetStatistics();
    }

    #endregion

    #region Statistics

    public void SetStats()
    {
        StartCloudUpdatePlayerStatistics();

        return;

        //PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        //{
        //    // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.
        //    Statistics = new List<StatisticUpdate> {
        //        new StatisticUpdate { StatisticName = "highScore", Value = gameController.HighScore },
        //        new StatisticUpdate { StatisticName = "totalLeaves", Value = gameController.TotalLeaves },
        //        new StatisticUpdate { StatisticName = "selectedCharacterID", Value = gameController.selectedCharacterID },
        //    }
        //},
        //result => { Debug.Log("User statistics updated"); },
        //error => { Debug.LogError(error.GenerateErrorReport()); });
    }

    void GetStatistics()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStatistics,
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }

    void OnGetStatistics(GetPlayerStatisticsResult result)
    {
        //Debug.Log("Received the following Statistics:");
        foreach (var eachStat in result.Statistics)
        {
            //Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);

            switch (eachStat.StatisticName)
            {
                case "highScore":
                    gameController.HighScore = eachStat.Value;
                    break;
                case "totalLeaves":
                    gameController.TotalLeaves = eachStat.Value;
                    break;
                case "selectedCharacterID":
                    gameController.selectedCharacterID = eachStat.Value;
                    break;
            }
        }
            
    }

    // Build the request object and access the API
    public void StartCloudUpdatePlayerStatistics()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "UpdatePlayerStatistics", // Arbitrary function name (must exist in your uploaded cloud.js file)
            FunctionParameter = new { highScore = gameController.HighScore, totalLeaves = gameController.TotalLeaves, selectedCharacterID = gameController.selectedCharacterID }, // The parameter provided to your function
            GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
        }, OnCloudUpdatePlayerStatistics, OnErrorShared);
    }
    // OnCloudHelloWorld defined in the next code block

    private static void OnCloudUpdatePlayerStatistics(ExecuteCloudScriptResult result)
    {
        // Cloud Script returns arbitrary results, so you have to evaluate them one step and one parameter at a time
        Debug.Log(PlayFab.PluginManager.GetPlugin<ISerializerPlugin>(PluginContract.PlayFab_Serializer).SerializeObject(result.FunctionResult));
        JsonObject jsonResult = (JsonObject)result.FunctionResult;
        object messageValue;
        jsonResult.TryGetValue("messageValue", out messageValue); // note how "messageValue" directly corresponds to the JSON values set in Cloud Script
        Debug.Log((string)messageValue);
    }

    private static void OnErrorShared(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    #endregion

    #region Leaderboard

    public void GetLeaderBoarder()
    {
        var requestLeaderboard = new GetLeaderboardRequest { StartPosition = 0, StatisticName = "highScore", MaxResultsCount = 10 };
        PlayFabClientAPI.GetLeaderboard(requestLeaderboard, OnGetLeaderboard, OnErrorLeaderboard);
    }

    void OnGetLeaderboard(GetLeaderboardResult result)
    {
        GetLeaderboard(result);
        gameController.UpdateLeaderboard();
    }

    public void GetLeaderboard(GetLeaderboardResult result)
    {

        List<LeaderboardHighScore> leaderboardHighScores = new List<LeaderboardHighScore>();

        int index = 1;

        foreach (PlayerLeaderboardEntry player in result.Leaderboard)
        {
            if (index <= 6)
            {
                LeaderboardHighScore lbh = new LeaderboardHighScore();
                lbh.name = player.DisplayName;
                lbh.score = player.StatValue;
                leaderboardHighScores.Add(lbh);
            }

            index++;
        }

        gameController.LeaderboardHighScores = leaderboardHighScores;
    }

    string GetLeaderboardFriends(GetLeaderboardResult result)
    {

        string returnstring = "";

        foreach (PlayerLeaderboardEntry player in result.Leaderboard)
        {
            returnstring += (player.Position + 1) + ". " + player.DisplayName + ": " + player.StatValue + "\n";
        }

        return returnstring;
    }

    void OnErrorLeaderboard(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    #endregion

    #region PlayerData

    public void GetPlayerData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myID,
            Keys = null
        }, UserDataSuccess, OnErrorLeaderboard);
    }

    void UserDataSuccess(GetUserDataResult result)
    {
        if (result.Data == null || !result.Data.ContainsKey("Characters"))
        {
            Debug.Log("Characters not set");
        }
        else
        {
            PersistantData.PD.CharacterStringToData(result.Data["Characters"].Value);
        }
    }

    public void SetUserData(string CharactersData)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                { "Characters", CharactersData }
            }
        }, SetDataSuccess, OnErrorLeaderboard);
    }

    void SetDataSuccess(UpdateUserDataResult result)
    {
        Debug.Log(result.DataVersion);
    }

    #endregion

    #region Friends

    void DisplayFriends(List<FriendInfo> friendsCache)
    {
        gameController.DisplayFriends();
        friendsCache.ForEach(f => Debug.Log(f.Username));
    }

    void DisplayPlayFabError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    void DisplayError(string error)
    {
        Debug.LogError(error);
    }

    public List<string> GetFriendNames()
    {
        List<string> friendNames = new List<string>();

        foreach (FriendInfo fi in _friends)
        {
            friendNames.Add(fi.TitleDisplayName);
        }

        return friendNames;
    }

    public void GetFriends()
    {
        PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest
        {
            IncludeSteamFriends = false,
            IncludeFacebookFriends = false
        }, result => {
            _friends = result.Friends;
            DisplayFriends(_friends); // triggers your UI
        }, DisplayPlayFabError);
    }

    public void AddFriendByDisplayName(string friendUsername)
    {
        AddFriend(FriendIdType.DisplayName, friendUsername);
    }

    public void RemoveFriendByUsername(string friendUsername)
    {
        FriendInfo friend = null;
        GetFriends();

        foreach (FriendInfo fi in _friends)
        {
            if (fi.Username == friendUsername)
            {
                friend = fi;
                break;
            }
        }

        if (friend == null)
        {
            return;
        }

        RemoveFriend(friend);
    }

    void AddFriend(FriendIdType idType, string friendId)
    {
        var request = new AddFriendRequest();
        switch (idType)
        {
            case FriendIdType.PlayFabId:
                request.FriendPlayFabId = friendId;
                break;
            case FriendIdType.Username:
                request.FriendUsername = friendId;
                break;
            case FriendIdType.Email:
                request.FriendEmail = friendId;
                break;
            case FriendIdType.DisplayName:
                request.FriendTitleDisplayName = friendId;
                break;
        }
        // Execute request and update friends when we are done
        PlayFabClientAPI.AddFriend(request, result => {
            Debug.Log("Friend added successfully!");
        }, DisplayPlayFabError);

        gameController.DisplayFriends();
    }

    // unlike AddFriend, RemoveFriend only takes a PlayFab ID
    // you can get this from the FriendInfo object under FriendPlayFabId
    void RemoveFriend(FriendInfo friendInfo)
    {
        PlayFabClientAPI.RemoveFriend(new RemoveFriendRequest
        {
            FriendPlayFabId = friendInfo.FriendPlayFabId
        }, result => {
            _friends.Remove(friendInfo);
        }, DisplayPlayFabError);
    }

    #endregion

    public void UpdateDisplayNameInputText()
    {
        DisplayNameInput.text = displayName;
    }

    public void GetPlayerProfile()
    {
        GetPlayerProfileRequest request = new GetPlayerProfileRequest();

        PlayFabClientAPI.GetPlayerProfile(request, OnGetProfileSuccess, OnGetProfileFailure);
    }

    void OnGetProfileSuccess(GetPlayerProfileResult result)
    {
        displayName = result.PlayerProfile.DisplayName;
        UpdateDisplayNameInputText();

        if (displayName == "" || displayName == null)
        {
            displayName = SystemInfo.deviceName;
        }
    }

    void OnGetProfileFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
}

public enum FriendIdType { PlayFabId, Username, Email, DisplayName };

[System.Serializable]
public class LeaderboardHighScore
{
    public string name;
    public int score;
}