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
        //switch (DataManager.Instance.state_Player.clear_Stage.Count)
        //{
        //    case 10:
        //        Achievements.BestStage10.Unlock();

        //        break;
        //    case 30:
        //        Achievements.BestStage30.Unlock();

        //        break;
        //    case 50:
        //        Achievements.BestStage50.Unlock();

        //        break;
        //    case 100:
        //        Achievements.BestStage100.Unlock();

        //        break;
        //    case 200:
        //        Achievements.BestStage200.Unlock();

        //        break;
        //    case 300:
        //        Achievements.BestStage300.Unlock();

        //        break;
        //    case 500:
        //        Achievements.BestStage500.Unlock();

        //        break;

        //    default:
        //        break;
        //}

        
        //if (DataManager.Instance.state_Player.Classic >= 10000)
        //{
        //    Achievements.BestClassic10000.Unlock();

        //}

        //if(DataManager.Instance.state_Player.Classic >= 5000)
        //{
        //    Achievements.BestClassic5000.Unlock();

        //}

        //if (DataManager.Instance.state_Player.Classic >= 1000)
        //{
        //    Achievements.BestClassic1000.Unlock();

        //}

    }

    public void Repart_LeaderBoard(int highScore = 0, int gameid = 0)
    {
        switch (gameid)
        {
            case 1:
                Social.ReportScore(highScore, GPGSIds.leaderboard_combine_puzzle_numbers, (result) =>
                {
                    Debug.Log(string.Format("ReportScore : {0}, {1}", highScore, result));
                });

                break;
            case 2:
                Social.ReportScore(highScore, GPGSIds.leaderboard_combine_number_2048, (result) =>
                {
                    Debug.Log(string.Format("ReportScore : {0}, {1}", highScore, result));
                });

                break;
            case 3:
                Social.ReportScore(highScore, GPGSIds.leaderboard_connect, (result) =>
                {
                    Debug.Log(string.Format("ReportScore : {0}, {1}", highScore, result));
                });

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
