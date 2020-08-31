using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Assets.Scripts.Utils;
using CloudOnce;

#if UNITY_IOS
using UnityEngine.SocialPlatforms.GameCenter;
#endif

public class GoogleManager : MonoBehaviour
{

    public static GoogleManager Instance;

    public string save_data;

    public string _IDtoken = null;

    //인증코드 받기
    public string _authCode = null;

    // Start is called before the first frame update
    void Awake()
    {
        //PlayerPrefs.DeleteAll();

        if (Instance == null)
        {
            Instance = this;

        }

        //구글 서비스 활성화
#if UNITY_ANDROID

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
          .EnableSavedGames()
          .Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

#elif UNITY_IOS
 
        GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
 
#endif

    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            PlayerPrefs.SetInt("Login", Social.localUser.authenticated ? 1 : 0);
        }
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Login", Social.localUser.authenticated ? 1 : 0);

    }


    public void ShowSaveSelectUI()
    {
#if UNITY_ANDROID

        if (!Social.localUser.authenticated)
        {
        }
        else
        {

            uint maxNumToDisplay = 5;
            bool allowCreateNew = true;
            bool allowDelete = true;

            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.ShowSelectSavedGameUI("Select saved game",
                maxNumToDisplay,
                allowCreateNew,
                allowDelete,
                OnSavedGameSelected);
        }

#else


#endif

    }

    public void ShowLoadSelectUI()
    {

#if UNITY_ANDROID

        if (!Social.localUser.authenticated)
        {
        }
        else
        {

            uint maxNumToDisplay = 5;
            bool allowCreateNew = false;
            bool allowDelete = false;

            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.ShowSelectSavedGameUI("Select saved game",
                maxNumToDisplay,
                allowCreateNew,
                allowDelete,
                OnLoadGameSelected);
        }

#else


#endif

    }

    public void OnSavedGameSelected(SelectUIStatus status, ISavedGameMetadata game)
    {

        switch (status)
        {
            case SelectUIStatus.SavedGameSelected:

                Player_Data_Save();

                break;
            case SelectUIStatus.UserClosedUI:
                break;
            case SelectUIStatus.InternalError:
                break;
            case SelectUIStatus.TimeoutError:
                break;
            case SelectUIStatus.AuthenticationError:
                break;
            case SelectUIStatus.BadInputError:
                break;
            default:
                break;
        }
    }

    public void OnLoadGameSelected(SelectUIStatus status, ISavedGameMetadata game)
    {

        switch (status)
        {
            case SelectUIStatus.SavedGameSelected:

                //open the data.
                Player_Data_Load();
                break;
            case SelectUIStatus.UserClosedUI:
                break;
            case SelectUIStatus.InternalError:
                break;
            case SelectUIStatus.TimeoutError:
                break;
            case SelectUIStatus.AuthenticationError:
                break;
            case SelectUIStatus.BadInputError:
                break;

            default:
                break;
        }
    }

    public bool isPopup = false;


    #region Google Cloud Save


    public void Player_Data_Save()
    {

        FireBaseManager.Instance.LogEvent("Setting_Data_Save");

        string jsonStr = JsonUtility.ToJson(DataManager.Instance.state_Player);
        string aes = AESCrypto.AESEncrypt128(jsonStr);

        StartCoroutine(Save(aes));
    }

    IEnumerator Save(string _data)
    {

        GameObject obj = null;
#if UNITY_EDITOR

        if (isPopup)
        {
            obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/data_saveing") as GameObject);
            DialogManager.GetInstance().show(obj, false);

            yield return new WaitForSeconds(2f);
            DialogManager.GetInstance().Close(null);

            obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/data_save_confirm") as GameObject);
            DialogManager.GetInstance().show(obj, false);
        }

#endif


        yield return new WaitForSeconds(0.1f);

        if (isPopup)
        {
            obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/data_saveing") as GameObject);
            DialogManager.GetInstance().show(obj, false);
        }

        string id = Social.localUser.id;
        string filename = string.Format("{0}Bolck", id);
        save_data = _data;

        OpenSaveGame(filename, true);
    }

    public void OpenSaveGame(string _fileName, bool _saved)
    {

#if UNITY_ANDROID

        ISavedGameClient savedGame = PlayGamesPlatform.Instance.SavedGame;

        //요청
        if (_saved)
        {

            //save
            savedGame.OpenWithAutomaticConflictResolution(_fileName, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGamePendedTOsave);
        }
        else
        {
            //load
            savedGame.OpenWithAutomaticConflictResolution(_fileName, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGamePendedTOLoad);
        }
#endif

    }

    public void OnSavedGamePendedTOsave(SavedGameRequestStatus _states, ISavedGameMetadata _data)
    {

        if (_states == SavedGameRequestStatus.Success)
        {

            byte[] b = Encoding.UTF8.GetBytes(save_data);
            //ToastManager.instance.ShowToast(b);

            SaveGame(_data, b, DateTime.Now.TimeOfDay);

        }
        else
        {
            Debug.Log("Save Fail");

        }
    }

    public void SaveGame(ISavedGameMetadata _data, byte[] _byte, TimeSpan _playTime)
    {
#if UNITY_ANDROID

        ISavedGameClient savedGame = PlayGamesPlatform.Instance.SavedGame;
        SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();

        builder = builder.WithUpdatedPlayedTime(_playTime).WithUpdatedDescription("Saved at " + DateTime.Now);

        SavedGameMetadataUpdate updateData = builder.Build();
        savedGame.CommitUpdate(_data, updateData, _byte, OnSacedGameWritten);
#endif

    }

    //세이브 저장 여부
    public void OnSacedGameWritten(SavedGameRequestStatus _state, ISavedGameMetadata _data)
    {
        if (_state == SavedGameRequestStatus.Success)
        {
            if (isPopup)
            {
                Debug.Log("save Complete");
                DialogManager.GetInstance().Close(null);

                GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/data_save_confirm") as GameObject);
                DialogManager.GetInstance().show(obj, false);
            }
        }
        else
        {
            Debug.Log("Save Fail");

        }
    }

    #endregion

    #region Google Cloud Load

    #region old

    public void Player_Data_Load()
    {
        FireBaseManager.Instance.LogEvent("Setting_Data_Down");

        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        GameObject obj = null;

#if UNITY_EDITOR
        if (isPopup)
        {
            obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/data_loading") as GameObject);
            DialogManager.GetInstance().show(obj, false);

            yield return new WaitForSeconds(2f);

            DialogManager.GetInstance().Close(null);

            obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/data_load_confirm") as GameObject);
            DialogManager.GetInstance().show(obj, false);
        }
#endif

        yield return new WaitForSeconds(0.1f);

        if (isPopup)
        {
            obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/data_loading") as GameObject);
            DialogManager.GetInstance().show(obj, false);
        }
        string id = Social.localUser.id;
        string filename = string.Format("{0}Bolck", id);

        OpenSaveGame(filename, false);

    }

    public void OnSavedGamePendedTOLoad(SavedGameRequestStatus _states, ISavedGameMetadata _data)
    {
        if (_states == SavedGameRequestStatus.Success)
        {
            LoadGameData(_data);
        }
        else
        {
            Debug.Log("Load Fail");

        }
    }

    //세이브 저장 여부
    public void OnSacedGameRead(SavedGameRequestStatus _state, byte[] _byte)
    {
        if (_state == SavedGameRequestStatus.Success)
        {
            Debug.Log("save Complete");


            string data = Encoding.Default.GetString(_byte);
            Player_Data_Load(data);

        }
        else
        {
            Debug.Log("load Fail");

        }
    }

    public void LoadGameData(ISavedGameMetadata _data)
    {
#if UNITY_ANDROID

        ISavedGameClient savedGame = PlayGamesPlatform.Instance.SavedGame;

        savedGame.ReadBinaryData(_data, OnSacedGameRead);
#endif
    }

    #endregion


    public void Player_Data_Load(string str)
    {
        DialogManager.GetInstance().Close(null);

        if (isPopup)
        {
            GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/data_load_confirm") as GameObject);
            DialogManager.GetInstance().show(obj, false);
        }


        string aes = AESCrypto.AESDecrypt128(str);

        var data = JsonUtility.FromJson<State_Player>(aes);

        DataManager.Instance.state_Player = data;

        DataManager.Instance.Save_Player_Data();

        Language.GetInstance().Set((SystemLanguage)DataManager.Instance.state_Player.LocalData_LanguageId);

        Reload();
    }

    public void Reload()
    {
        Main obj = FindObjectOfType(typeof(Main)) as Main;
        obj.Reload();
    }

    #endregion

}
