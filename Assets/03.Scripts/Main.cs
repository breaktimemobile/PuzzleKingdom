using Assets.Scripts.Configs;
using Assets.Scripts.GameManager;
using Assets.Scripts.Utils;
using System;
using UnityEngine;

public class Main : MonoBehaviour
{
	private void Awake()
    {


	}

	private void Start()
    {

        this.InitializeGameConfig();
        Configs.LoadConfig();
        GM.GetInstance().Init();
        GoodsManager.Initialize();
        AchiveData.Initialize();
        TaskData.Initialize();
        LoginData.Initialize();
        GlobalTimer.Initialize();
        AdsManager.Initialize();
        DotManager.GetInstance().Check();
        GameList.Instance.init();
        GetComponent<MainScene>().Init();
        Utils.ShowLoginRewards();

    }

    private void Update()
	{
		GlobalTimer.GetInstance().Update();
	}

	private void InitializeGameConfig()
	{
		Application.targetFrameRate = 60;
		Input.multiTouchEnabled = false;
	}

    public void Reload()
    {
        this.InitializeGameConfig();
        Configs.LoadConfig();
        GM.GetInstance().Init();
        GoodsManager.Initialize();
        AchiveData.Initialize();
        TaskData.Initialize();
        LoginData.Initialize();
        GlobalTimer.Initialize();
        AdsManager.Initialize();
        DotManager.GetInstance().Check();
        GameList.Instance.init();
        GetComponent<MainScene>().Init();
        Utils.ShowLoginRewards();

    }
}
