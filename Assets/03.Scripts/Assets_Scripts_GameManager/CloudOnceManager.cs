using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CloudOnce;
using Assets.Scripts.Utils;
using GooglePlayGames;

public class CloudOnceManager : MonoBehaviour
{
    public static CloudOnceManager Instance;

    public bool isPopup = false;

    private void Awake()
    {
        Instance = this;
        Cloud.OnInitializeComplete += CloudOnceInitializeComplete;
        Cloud.Initialize(true, false, false);
    }

    public void DoAutoLogin()
    {
#if UNITY_ANDROID
        Social.localUser.Authenticate((authenticateCallBck));

#elif UNITY_IOS
                Cloud.SignIn(true, authenticateCallBck);

#endif
    }


    private void Start()
    {
#if !UNITY_EDITOR
        FireBaseManager.Instance.FirebaseNullLogin();
        DoAutoLogin();
#endif
    }

    public void Login()
    {
#if UNITY_ANDROID
        Social.localUser.Authenticate((authenticateCallBck));

#elif UNITY_IOS
                Cloud.SignIn(true, authenticateCallBck);

#endif

    }

    public void Logout()
    {

#if UNITY_ANDROID
        ((PlayGamesPlatform)Social.Active).SignOut();

#elif UNITY_IOS
        Cloud.SignOut();

#endif

    }

    public void OnClickGoogle()
    {

        if (!Social.localUser.authenticated)
        {
            GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/Google_Login") as GameObject);
            obj.GetComponent<DataPopup>().Set_Google();
            DialogManager.GetInstance().show(obj, false);
        }
        else
        {
#if UNITY_ANDROID

            GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/Google_Logout") as GameObject);
            DialogManager.GetInstance().show(obj, false);

#elif UNITY_IOS
           Show_Achievements();
#endif
        }

    }


    void authenticateCallBck(bool sucess)
    {
        if (sucess)
        {
            PlayerPrefs.SetInt("Login", 1);
            Debug.Log("로그인 성공 " + PlayerPrefs.GetInt("Login", 0));

#if UNITY_ANDROID
            GoogleManager.Instance.Player_Data_Load();
#elif UNITY_IOS
        
        isSave = false;

        Cloud.OnCloudLoadComplete += CloudeLoad;
        isPopup = false;

        Cloud.Storage.Load();
#endif

        }
        else
        {
            PlayerPrefs.SetInt("Login", 0);
            Debug.Log("로그인 실패 " + PlayerPrefs.GetInt("Login", 0));

        }
    }

    public void CloudOnceInitializeComplete()
    {
        Cloud.OnInitializeComplete -= CloudOnceInitializeComplete;

        // Do anything that requires CloudOnce to be initialized,
        // for example disabling your loading screen
    }

    public void Report_Achievements()
    {
        switch (DataManager.Instance.state_Player.LocalData_Lv)
        {
            case 5:
                Achievements.Levelup5.Unlock();

                break;
            case 10:
                Achievements.Levelup10.Unlock();

                break;
            case 15:
                Achievements.Levelup15.Unlock();

                break;
            case 20:
                Achievements.Levelup20.Unlock();

                break;
            case 25:
                Achievements.Levelup25.Unlock();

                break;

            default:
                break;
        }


        switch (DataManager.Instance.state_Player.AdsCounter)
        {
            case 5:
                Achievements.Useitem5.Unlock();

                break;
            case 10:
                Achievements.Useitem10.Unlock();

                break;
            case 15:
                Achievements.Useitem15.Unlock();

                break;
            case 20:
                Achievements.Useitem20.Unlock();

                break;
            case 25:
                Achievements.Useitem25.Unlock();

                break;

            default:
                break;
        }

    }

    public void Repart_LeaderBoard(int highScore = 0, int gameid = 0)
    {
        switch (gameid)
        {
            case 1:
                Leaderboards.Combinepuzzlenumbers.SubmitScore(highScore);


                break;
            case 2:
                Leaderboards.Combinenumber2048.SubmitScore(highScore);


                break;
            case 3:
                Leaderboards.Connect.SubmitScore(highScore);


                break;
            default:
                break;
        }
    }

    public void Show_Achievements()
    {
        if (!Social.localUser.authenticated)
        {
            OnClickGoogle();
        }
        else
        {
            Cloud.Achievements.ShowOverlay();
        }
    }

    public void Show_Leaderboards()
    {
        if (!Social.localUser.authenticated)
        {
            OnClickGoogle();
        }
        else
        {
            Cloud.Leaderboards.ShowOverlay();
        }

    }


    public bool isSave = false;

    public void CloudeSave(bool success)
    {

        Debug.Log(success ? "저장 성공" : "저장 실패");


        isSave = true;

        Cloud.OnCloudSaveComplete -= CloudeSave;


    }

    public void Saving()
    {
        StartCoroutine("Co_Saving");

    }

    IEnumerator Co_Saving()
    {
        Debug.Log("Co_Saving");

        GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/data_saveing") as GameObject);
        DialogManager.GetInstance().show(obj, false);

        yield return new WaitForSeconds(2f);

        DialogManager.GetInstance().Close(null);

        obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/data_save_confirm") as GameObject);
        DialogManager.GetInstance().show(obj, false);


    }

    public void CloudeLoad(bool success)
    {
        Cloud.OnCloudLoadComplete -= CloudeLoad;

        Debug.Log(success ? "로드 성공 " : "로드 실패");

        if (!success)
            return;


        string str = CloudVariables.Player_Data;


        if (str != "")
        {
            if(!isPopup)
            DialogManager.GetInstance().Close(null);

            var aes = AESCrypto.AESDecrypt128(str);
            var data = JsonUtility.FromJson<State_Player>(aes);

            DataManager.Instance.state_Player = data;

            DataManager.Instance.Save_Player_Data();

            Language.GetInstance().Set((SystemLanguage)DataManager.Instance.state_Player.LocalData_LanguageId);

            Main obj = FindObjectOfType(typeof(Main)) as Main;
            obj.Reload();

        }

        isSave = true;

    }


    public void Loading()
    {
        StartCoroutine("Co_Loading");

    }

    IEnumerator Co_Loading()
    {
        isPopup = true;

        GameObject obj = null;

        obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/data_loading") as GameObject);
        DialogManager.GetInstance().show(obj, false);

        yield return new WaitForSeconds(2f);



        while (true)
        {
            Debug.Log(isSave);

            if (isSave)
            {
                DialogManager.GetInstance().Close(null);

                obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/data_load_confirm") as GameObject);
                DialogManager.GetInstance().show(obj, false);

                break;


            }

            yield return new WaitForSeconds(0.1f);

        }


    }
}
