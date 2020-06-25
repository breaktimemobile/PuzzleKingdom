using Assets.Scripts.Configs;
using Assets.Scripts.GameManager;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Utils;
using UnityEngine.EventSystems;

public class LevelUp : MonoBehaviour
{
	public Text m_txt_lv_value;

	public Text m_txt_awards;

    public Text m_txt_coin;

    public GameObject[] obj_trigger;

    private void Start()
	{
		this.InitUI();

        CloudOnceManager.Instance.Report_Achievements();

        foreach (var item in obj_trigger)
        {
            EventTrigger eventTrigger = item.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry entry_PointerDown = new EventTrigger.Entry();
            entry_PointerDown.eventID = EventTriggerType.PointerDown;
            entry_PointerDown.callback.AddListener((data) => { FindObjectOfType<MainScene>().Pointer_Down(item.transform); });
            eventTrigger.triggers.Add(entry_PointerDown);

            EventTrigger.Entry entry_PointerUp = new EventTrigger.Entry();
            entry_PointerUp.eventID = EventTriggerType.PointerUp;
            entry_PointerUp.callback.AddListener((data) => { FindObjectOfType<MainScene>().Pointer_Up(item.transform); });
            eventTrigger.triggers.Add(entry_PointerUp);

        }
    }

	private void Update()
	{
	}

	public void OnClickOK()
    {
        FireBaseManager.Instance.LogEvent("Level_Up_Ok");

        if (Configs.TPlayers.ContainsKey(GM.GetInstance().Lv.ToString()))
		{
			TPlayer tPlayer = Configs.TPlayers[GM.GetInstance().Lv.ToString()];
			GM.GetInstance().AddDiamond(tPlayer.Item, true);
		}
		if (GM.GetInstance().GameId == 2)
		{
            G2BoardGenerator.GetInstance().IsPuase = false;
		}
		DialogManager.GetInstance().Close(null);
	}

    public void Show_Ads()
    {

        AdsControl.Instance.reward_Type = Reward_Type.levelup;
        AdsControl.Instance.ShowRewardedAd();
    }

    public void OnClickAds()
    {
        FireBaseManager.Instance.LogEvent("Level_Up_Ads");

        if (Configs.TPlayers.ContainsKey(GM.GetInstance().Lv.ToString()))
        {
            TPlayer tPlayer = Configs.TPlayers[GM.GetInstance().Lv.ToString()];
            GM.GetInstance().AddDiamond(tPlayer.Item*2, true);
        }
        if (GM.GetInstance().GameId == 2)
        {
            G2BoardGenerator.GetInstance().IsPuase = false;
        }
        DialogManager.GetInstance().Close(null);
    }

    private void InitUI()
	{
        this.m_txt_lv_value.text =  GM.GetInstance().Lv.ToString();
		if (Configs.TPlayers.ContainsKey(GM.GetInstance().Lv.ToString()))
		{
			TPlayer tPlayer = Configs.TPlayers[GM.GetInstance().Lv.ToString()];
			this.m_txt_awards.text = string.Format("x{0}", tPlayer.Item);
            this.m_txt_coin.text = string.Format("x{0}", tPlayer.Item *2);

        }
	}
}
