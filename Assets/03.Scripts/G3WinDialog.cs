using Assets.Scripts.Configs;
using Assets.Scripts.GameManager;
using DG.Tweening;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/*
 * this class are attached to dialog that shown when level is clear
 * */
public class G3WinDialog : MonoBehaviour
{
    //all parameter will show or save when you pass the levels
	private int gameID = 3;

	private int m_level = 1;

	private int m_next;

	private int m_lv;

	private int m_award;

	private int m_exp;

	private bool isShowAward;

	public Text m_txt_coin;
    public Text m_txt_ads;

    public Transform[] m_star_tr;
    public GameObject[] obj_trigger;

    public int Level
	{
		get
		{
			return this.m_level;
		}
		set
		{
			this.m_level = value;
		}
	}

	public int Next
	{
		get
		{
			return this.m_next;
		}
		set
		{
			this.m_next = value;
		}
	}

	public int Lv
	{
		get
		{
			return this.m_lv;
		}
		set
		{
			this.m_lv = value;
		}
	}

	public int Award
	{
		get
		{
			return this.m_award;
		}
		set
		{
			this.m_award = value;
		}
	}

	public int Exp
	{
		get
		{
			return this.m_exp;
		}
		set
		{
			this.m_exp = value;
		}
	}

	private void Start()
	{
        AudioManager.GetInstance().PlayBgMusic("sound_eff_victory",true);

        GM.GetInstance().SetSavedGameID(0);
		if (this.isShowAward)
		{
			GM.GetInstance().AddDiamond(this.Award);
			GM.GetInstance().AddExp(this.Exp);
		}
        //AudioManager.GetInstance().PlayEffect("sound_eff_popup");
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

	public void Load(int score, int next, int lv, int award = 0, int exp = 0)
	{

        this.Level = score;
		this.Next = next;
		this.Lv = lv;
		this.Award = award;
		this.Exp = exp;
		this.m_txt_coin.text = "x" + award.ToString();
        this.m_txt_ads.text = "x" + award.ToString();

        if (score > 15)
		{
			GM.GetInstance().SetFirstFinishGame();
		}

	}

	public void OnClickNext()
	{
        FireBaseManager.Instance.LogEvent("Puzzle_Line_Result_Next");

        UnityEngine.Debug.Log("On next lv");
		GM.GetInstance().SetSavedGameID(this.gameID);
        G3BoardGenerator.GetInstance().DestroyMap();
        G3BoardGenerator.GetInstance().StartNewGame(Configs.TG00301[this.Next.ToString()].ID);
		GlobalEventHandle.EmitDoRefreshCheckPoint(this.Lv);
		DialogManager.GetInstance().Close(null);

        AudioManager.GetInstance().PlayBgMusic("sound_ingame", true);

        if (this.Level % 5 == 0)
		{
            //AdsManager.GetInstance().Play(AdsManager.AdType.Finish, null, null, 5, null);
            AdsControl.Instance.showAds();
		}
	}

	public void OnClickAgain()
    {
        FireBaseManager.Instance.LogEvent("Puzzle_Line_Result_Retry");

        //AdsControl.Instance.reward_Type = Reward_Type.again;

        //AdsControl.Instance.ShowRewardedAd();
        //GM.GetInstance().AddDiamond(5, true);

        GlobalEventHandle.EmitDoRefreshCheckPoint(this.Lv);
        this.OnClickNext();

        /*
        AdsManager.GetInstance().Play(AdsManager.AdType.MultiAwards, delegate
		{
			if (this.isShowAward)
			{
				GM.GetInstance().AddDiamond(this.Award, true);
			}
			GlobalEventHandle.EmitDoRefreshCheckPoint(this.Lv);
			this.OnClickNext();
		}, null, 5, null);
		*/

	}

	public void OnClickHome()
    {
        FireBaseManager.Instance.LogEvent("Puzzle_Line_Result_Main");
        AdsControl.Instance.ShowInterstitial();

        G3BoardGenerator.GetInstance().DestroyMap();
		GM.GetInstance().SetSavedGameID(0);
		GM.GetInstance().ResetToNewGame();
		GM.GetInstance().ResetConsumeCount();
		GlobalEventHandle.EmitClickPageButtonHandle("main", 0);
		GlobalEventHandle.EmitDoRefreshCheckPoint(this.Lv);
		DialogManager.GetInstance().Close(null);
	}
    
	public void IsShowAward(bool isShowAward)
	{
		this.isShowAward = isShowAward;
		
	}

    public void OnClickAds()
    {
        FireBaseManager.Instance.LogEvent("Puzzle_Line_Result_Ads");

        AdsControl.Instance.reward_Type = Reward_Type.stage;
        AdsControl.Instance.ShowRewardedAd();

        m_txt_ads.transform.parent.GetComponent<Button>().interactable = false;
        m_txt_ads.transform.parent.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
    }

    public void Reward()
    {
        GM.GetInstance().AddDiamond(this.Award, true);
        m_txt_ads.transform.parent.GetComponent<Button>().interactable = false;
    }

}
