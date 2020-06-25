using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CloudOnce;

public class CloudOnceManager : MonoBehaviour
{
    public static CloudOnceManager Instance;

    private void Awake()
    {
        Instance = this;
        Cloud.OnInitializeComplete += CloudOnceInitializeComplete;
        Cloud.Initialize(true, false);
    }

    public void DoAutoLogin()
    {

        //구글 로그인이 되어있지 않다면
        if (PlayerPrefs.GetInt("Login", 0).Equals(1))
        {
            Cloud.SignIn(true, authenticateCallBck);
        }

    }


    private void Start()
    {
        FireBaseManager.Instance.FirebaseNullLogin();
        DoAutoLogin();
    }

    public void Login()
    {
        Cloud.SignIn(true, authenticateCallBck);

    }

    public void Logout()
    {
        Cloud.SignOut();

    }

    public void OnClickGoogle()
    {

        if (!Social.localUser.authenticated)
        {
            GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/Google_Login") as GameObject);
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
}
