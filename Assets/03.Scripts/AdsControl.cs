using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SocialPlatforms;
using GoogleMobileAds.Api;
using System;
using UnityEngine.UI;
using Assets.Scripts.GameManager;

public enum Reward_Type
{
    game1Finish,
    game2Finish,
    again,
    block,
    skin,
    stimulate,
    Achive,
    Task,
    shop_ads,
    levelup,
    stage,
    game,
    coin,
    Daily,
    gift
}

public class AdsControl : MonoBehaviour
{


    protected AdsControl()
    {
    }

    public Reward_Type reward_Type = Reward_Type.game1Finish;
    public int GameID;
    public int SkinID;
    public G1Block block;
    public int awardValue;

    public int achive_type =0;
    public int achive_val =0;
    private static AdsControl _instance;
    InterstitialAd interstitial;
    RewardedAd rewardedAd;
    BannerView bannerBtmView;

    public static AdsControl Instance { get { return _instance; } }

    void Awake()
    {

        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        DontDestroyOnLoad(gameObject); //Already done by CBManager
    }

    private void Start()
    {
        Set_Ads();

    }

    public void Set_Ads()
    {

#if UNITY_ANDROID
        string appId = "ca-app-pub-4682698622407711~3805835069";
#elif UNITY_IOS
        string appId = "ca-app-pub-4682698622407711~1898065790";
#else
        string appId = "unexpected_platform";
#endif

        //MobileAds.SetiOSAppPauseOnBackground(true);

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });


        this.RequestRewardedAd();
        this.RequestInterstitial();
        this.RequestBtmBanner();

    }

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
              .AddKeyword("game")
              .SetGender(Gender.Male)
              .SetBirthday(new DateTime(1985, 1, 1))
              .TagForChildDirectedTreatment(false)
              .AddExtra("color_bg", "9B30FF")
              .Build();
    }
    
    #region Banner

    private void RequestBtmBanner()
    {
        // These ad units are configured to always serve test ads.

#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-4682698622407711/3319298641";
#elif UNITY_IOS
        string adUnitId = "ca-app-pub-4682698622407711/4141085753";
#else
        string adUnitId = "unexpected_platform";
#endif

        this.bannerBtmView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Center);
        //this.bannerBtmView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Center);
        //this.bannerBtmView = new BannerView(adUnitId, AdSize.MediumRectangle, AdPosition.Center);

        // Register for ad events.
        this.bannerBtmView.OnAdLoaded += this.HandleAdLoaded;
        this.bannerBtmView.OnAdFailedToLoad += this.HandleAdFailedToLoad;
        this.bannerBtmView.OnAdOpening += this.HandleAdOpened;
        this.bannerBtmView.OnAdClosed += this.HandleAdClosed;
        this.bannerBtmView.OnAdLeavingApplication += this.HandleAdLeftApplication;

        // Load a banner ad.
        this.bannerBtmView.LoadAd(this.CreateAdRequest());
    }

    public void BannerShow(bool Top)
    {
        if (DataManager.Instance.state_Player.RemoveAds)
        {
            bannerBtmView.Hide();
            return;

        }

        bannerBtmView.Show();

    }

    #endregion

    #region Reward

    private void RequestRewardedAd()
    {

#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-4682698622407711/4927345041";  
#elif UNITY_IOS
        string adUnitId = "ca-app-pub-4682698622407711/3949514063";
#else
        string adUnitId = "unexpected_platform";
#endif
        // Create new rewarded ad instance.
        this.rewardedAd = new RewardedAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = this.CreateAdRequest();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    public void ShowRewardedAd()
    {
#if UNITY_EDITOR
        Ads_Reward();
#endif
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();

            DataManager.Instance.state_Player.AdsCounter += 1;
            DataManager.Instance.Save_Player_Data();
        }
        else
        {
            MonoBehaviour.print("Rewarded ad is not ready yet");
            RequestRewardedAd();

        }
    }

    #endregion

    #region interstitial

    private void RequestInterstitial()
    {
        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-4682698622407711/8866590053";
#elif UNITY_IOS
        string adUnitId = "ca-app-pub-4682698622407711/9201840742";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up interstitial ad before creating a new one.
        if (this.interstitial != null)
        {
            this.interstitial.Destroy();
        }

        // Create an interstitial.
        this.interstitial = new InterstitialAd(adUnitId);

        // Register for ad events.
        this.interstitial.OnAdLoaded += this.HandleInterstitialLoaded;
        this.interstitial.OnAdFailedToLoad += this.HandleInterstitialFailedToLoad;
        this.interstitial.OnAdOpening += this.HandleInterstitialOpened;
        this.interstitial.OnAdClosed += this.HandleInterstitialClosed;
        this.interstitial.OnAdLeavingApplication += this.HandleInterstitialLeftApplication;

        // Load an interstitial ad.
        this.interstitial.LoadAd(this.CreateAdRequest());
    }

    public void ShowInterstitial()
    {
        FireBaseManager.Instance.LogEvent("Reward_Start");

        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();


        }
        else
        {
            MonoBehaviour.print("Interstitial is not ready yet");
        }
    }

    #endregion

    #region Banner callback handlers

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
        Debug.Log("배너 성공");
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args.Message);
        Debug.Log("배너 실패");

    }

    public void HandleAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeftApplication event received");
    }

    #endregion

    #region Interstitial callback handlers

    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialLoaded event received");
    }

    public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
            "HandleInterstitialFailedToLoad event received with message: " + args.Message);
    }

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialOpened event received");
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialClosed event received");
        RequestInterstitial();
    }

    public void HandleInterstitialLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialLeftApplication event received");
    }

    #endregion

    #region RewardedAd callback handlers

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        Debug.Log(
            "HandleRewardedAdFailedToLoad event received with message: " + args.Message);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        Debug.Log(
            "HandleRewardedAdFailedToShow event received with message: " + args.Message);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdClosed event received");
        RequestRewardedAd();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        //string type = args.Type;
        //double amount = args.Amount;
        //MonoBehaviour.print(
        //    "HandleRewardedAdRewarded event received for "
        //                + amount.ToString() + " " + type);

        //광고 끝나고!!!

        Ads_Reward();
    }

    public bool Check_Rewaed()
    {
        if (this.rewardedAd.IsLoaded())
        {
            Debug.Log("광고 로드");
            return true;
        }
        else
        {
            Debug.Log("광고 로드 안대ㅔㅁ///");
            RequestRewardedAd();
            return false;

        }
    }
    #endregion

    public void RateMyGame()
    {
#if UNITY_EDITOR
        Application.OpenURL("https://itunes.apple.com/us/app/color-flow-puzzle/id1436566275?ls=1&mt=8");
#elif UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.ponygames.MagicBlockPuzzle");
#elif UNITY_IOS
        Application.OpenURL("https://itunes.apple.com/us/app/color-flow-puzzle/id1436566275?ls=1&mt=8");
#else
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.ponygames.MagicBlockPuzzle");
#endif


    }

    public void Ads_Reward()
    {
        FireBaseManager.Instance.LogEvent("Reward_End");

        switch (reward_Type)
        {
            case Reward_Type.game1Finish:
                Debug.Log("첫번째 이어하기");
                FireBaseManager.Instance.LogEvent("Puzzle_Mix_Continue_Ads");

                GM.GetInstance().SetSavedGameID(GameID);
                Game1DataLoader.GetInstance().FillLife(false);
                Game1DataLoader.GetInstance().DoFillLife();
                DialogManager.GetInstance().Close(null);

                break;

            case Reward_Type.game2Finish:
                Debug.Log("두번째 이어하기");
                FireBaseManager.Instance.LogEvent("2048_Continue_Ads");

                GM.GetInstance().SetSavedGameID(GameID);
                DialogManager.GetInstance().Close(null);
                Game2Manager.GetInstance().video();

                break;

            case Reward_Type.again:

                break;

            case Reward_Type.block:

                Game1DataLoader.GetInstance().IsPlaying = true;
                Game1Manager.GetInstance().ControlPropsPannel(true);
                Game1Manager.GetInstance().UseProps(block);
                Game1Manager.GetInstance().m_markTips = true;

                break;
            case Reward_Type.skin:
                GM.GetInstance().SetSkinData(this.SkinID, 2);
                GM.GetInstance().SetSkinFreeTime(this.SkinID, DateTime.Now);
                //AppsflyerUtils.TrackBuySkin(this.SkinID, 1);
                Action<int> expr_38 = SkinUnLock.Instance.OnUnlockSuccess;
                if (expr_38 != null)
                {
                    expr_38(this.SkinID);
                }
                DialogManager.GetInstance().Close(null);
                break;
            case Reward_Type.stimulate:

                int rans = UnityEngine.Random.Range(10, 100);
                GM.GetInstance().AddDiamond(rans);
                TaskData.GetInstance().Add(100105, 1, true);

                DateTime GiftTime = DateTime.Now.AddMinutes(10);
                DataManager.Instance.state_Player.LocalData_Main_Time = GiftTime.ToString();
                DataManager.Instance.Save_Player_Data();

                FindObjectOfType<MainScene>().Set_Timer();

                DotManager.GetInstance().CheckAds();

                break;
            case Reward_Type.Achive:

                if (!AchiveData.GetInstance().Finish(achive_type))
                {
                    return;
                }

                GM.GetInstance().AddDiamond(achive_val * 2, true);
                FindObjectOfType<Achive>().Set_BindDataToUI();
                DialogManager.GetInstance().Close(null);

                break;
            case Reward_Type.Task:
                if (!TaskData.GetInstance().Finish(achive_type))
                {
                    return;
                }

                GM.GetInstance().AddDiamond(achive_val *2, true);
                FindObjectOfType<Task>().Refresh();
                DialogManager.GetInstance().Close(null);

                break;

            case Reward_Type.shop_ads:
                
                GiftTime = DateTime.Now.AddMinutes(10);
                DataManager.Instance.state_Player.LocalData_Shop_Time = GiftTime.ToString();
                DataManager.Instance.Save_Player_Data();

                FindObjectOfType<Shop>().ads_item.Set_Timer();

                int ran = UnityEngine.Random.Range(10,100);
                GM.GetInstance().AddDiamond(ran);

                FireBaseManager.Instance.LogEvent("Shop_Ads");

                break;
            case Reward_Type.levelup:

                FindObjectOfType<LevelUp>().OnClickAds();


                break;
            case Reward_Type.stage:

                FindObjectOfType<G3WinDialog>().Reward();


                break;
            case Reward_Type.game:

                GiftTime = DateTime.Now.AddMinutes(10);
                DataManager.Instance.state_Player.LocalData_Game_Time = GiftTime.ToString();
                DataManager.Instance.Save_Player_Data();

                Game2Manager.GetInstance()?.Set_Timer();
                Game1Manager.GetInstance()?.Set_Timer();
                G3BoardManager.GetInstance()?.Set_Timer();

                ran = UnityEngine.Random.Range(10, 100);
                GM.GetInstance().AddDiamond(ran);

                break;
            case Reward_Type.coin:
                ran = UnityEngine.Random.Range(10, 100);
                GM.GetInstance().AddDiamond(ran);
                break;
            case Reward_Type.Daily:

                FindObjectOfType<Activity>().OnClickAds();


                break;
            case Reward_Type.gift:
                FindObjectOfType<RewardPopup>().Get_Gift();

                break;

            default:
                break;
        }
    }
}

