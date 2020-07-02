using Assets.Scripts.GameManager;
using Assets.Scripts.Utils;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Finish : MonoBehaviour
{
	private int m_gameID = 1;

	private int m_number = 1;

	private int m_type = 1;

    public Text txt_score_value;

    public GameObject obj_Next;
    public GameObject obj_gold;

    public GameObject Btn_conti;
    public GameObject Btn_Ok;


    public int GameID
	{
		get
		{
			return this.m_gameID;
		}
		set
		{
			this.m_gameID = value;
		}
	}

	public int Number
	{
		get
		{
			return this.m_number;
		}
		set
		{
			this.m_number = value;
		}
	}

	private void Start()
	{
        Debug.Log("Finish Start");


        obj_Next.SetActive(PlayerPrefs.GetInt("conti", 0) == 0);
        obj_gold.SetActive(PlayerPrefs.GetInt("conti", 0) != 0);

        if (AdsUtil.isRewardAdsLoaded() || AdsUtil.isRewardISAdsLoaded())
		{
			this.m_type = 1;
		}
		else
		{
			this.m_type = 2;
		}

        Debug.Log("Btn Start");

  
        Debug.Log("Teween Start");

        Sequence sequence = DOTween.Sequence();
		int type = this.m_type;
	
        Debug.Log("Teween 2 Start");

        sequence = DOTween.Sequence();
		sequence.AppendInterval(2f);
		GM.GetInstance().SetFirstFinishGame();

        Debug.Log("Finish End");

        EventTrigger eventTrigger = Btn_Ok.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry_PointerDown = new EventTrigger.Entry();
        entry_PointerDown.eventID = EventTriggerType.PointerDown;
        entry_PointerDown.callback.AddListener((data) => { FindObjectOfType<MainScene>().Pointer_Down(Btn_Ok.transform); });
        eventTrigger.triggers.Add(entry_PointerDown);

        EventTrigger.Entry entry_PointerUp = new EventTrigger.Entry();
        entry_PointerUp.eventID = EventTriggerType.PointerUp;
        entry_PointerUp.callback.AddListener((data) => { FindObjectOfType<MainScene>().Pointer_Up(Btn_Ok.transform); });
        eventTrigger.triggers.Add(entry_PointerUp);

        eventTrigger = Btn_conti.gameObject.AddComponent<EventTrigger>();

        entry_PointerDown = new EventTrigger.Entry();
        entry_PointerDown.eventID = EventTriggerType.PointerDown;
        entry_PointerDown.callback.AddListener((data) => { FindObjectOfType<MainScene>().Pointer_Down(Btn_conti.transform); });
        eventTrigger.triggers.Add(entry_PointerDown);

        entry_PointerUp = new EventTrigger.Entry();
        entry_PointerUp.eventID = EventTriggerType.PointerUp;
        entry_PointerUp.callback.AddListener((data) => { FindObjectOfType<MainScene>().Pointer_Up(Btn_conti.transform); });
        eventTrigger.triggers.Add(entry_PointerUp);
    }

    private void Update()
	{
	}

    
	public void Load(int gameId, int number)
	{
		GM.GetInstance().SetSavedGameID(0);
		GM.GetInstance().SaveScore(this.GameID, 0);
		this.GameID = gameId;
		this.Number = number;
		if (gameId == 1)
		{
            Debug.Log("이게 111111" + Game1DataLoader.GetInstance().Score.ToString());

            txt_score_value.text = Game1DataLoader.GetInstance().Score.ToString();

            return;
		}
		if (gameId == 2)
		{
            Debug.Log("이게 2222" + G2BoardGenerator.GetInstance().Score.ToString());

            txt_score_value.text = G2BoardGenerator.GetInstance().Score.ToString();
            return;
		}

		//this.m_g00201.setNum(number);
	}

	public void OnClickAds()
	{
		int gameID = this.GameID;
        if (gameID == 3)
            return;

        if (gameID == 1)
		{
           AdsControl.Instance.reward_Type = Reward_Type.game1Finish; 
        }
        else if(gameID == 2)
        {
            AdsControl.Instance.reward_Type = Reward_Type.game2Finish;
        }

        AdsControl.Instance.GameID = this.GameID;

        if (PlayerPrefs.GetInt("conti", 0) == 0)
        {
            AdsControl.Instance.ShowRewardedAd();
            PlayerPrefs.SetInt("conti", PlayerPrefs.GetInt("conti", 0) + 1);
        }
        else
        {

            if (gameID == 1)
            {
                FireBaseManager.Instance.LogEvent("Puzzle_Mix_Continue_Coin_Check");
            }
            else if (gameID == 2)
            {
                FireBaseManager.Instance.LogEvent("2048_Continue_Coin_Check");
            }

            if (DataManager.Instance.state_Player.LocalData_Diamond >= 100)
            {
                GM.GetInstance().ConsumeGEM(100);
                AdsControl.Instance.Ads_Reward();

                PlayerPrefs.SetInt("conti", PlayerPrefs.GetInt("conti", 0) + 1);

            }

        }

    }

	public void OnClickCoin()
	{
		if (!GM.GetInstance().isFullGEM(100))
		{
			ToastManager.Show("TXT_NO_50001", true);
			return;
		}
		int gameID = this.GameID;
		if (gameID == 1)
		{
			GM.GetInstance().SetSavedGameID(this.GameID);
            Game1DataLoader.GetInstance().FillLife(false);
            Game1DataLoader.GetInstance().DoFillLife();
			DialogManager.GetInstance().Close(null);
			return;
		}
		if (gameID != 2)
		{
			return;
		}
		GM.GetInstance().SetSavedGameID(this.GameID);
		DialogManager.GetInstance().Close(null);
		Action expr_85 = G2BoardGenerator.GetInstance().DoVedioRefresh;
		if (expr_85 == null)
		{
			return;
		}
		expr_85();
	}

	public void OnClickAgain()
    {
        this.ShowFinish();

  //      if (GM.GetInstance().IsFirstFinishGame())
		//{
		//	this.ShowFinish();
		//	return;
		//}

		//AdsManager.GetInstance().Play(AdsManager.AdType.Finish, delegate
		//{
		//	this.ShowFinish();
		//}, null, 5, null);
	}

	private void ShowFinish()
	{
        Debug.Log("끝났을때");
		DialogManager.GetInstance().Close(delegate
		{
			int gameID = this.GameID;
			if (gameID == 1)
			{
                FireBaseManager.Instance.LogEvent("Puzzle_Mix_Continue_No");
                CloudOnceManager.Instance.Repart_LeaderBoard(Game1DataLoader.GetInstance().Score,1);
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/G00102") as GameObject);
				gameObject.GetComponent<G1UIManager>().Load(Game1DataLoader.GetInstance().Score, Game1DataLoader.GetInstance().MaxScore);
				DialogManager.GetInstance().show(gameObject, true);
				return;
			}

			if (gameID == 2)
			{
                FireBaseManager.Instance.LogEvent("2048_Continue_No");
                CloudOnceManager.Instance.Repart_LeaderBoard(G2BoardGenerator.GetInstance().Score, 2);

                GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/G00203") as GameObject);
                gameObject2.GetComponent<G2UIManager>().Load(G2BoardGenerator.GetInstance().Score, G2BoardGenerator.GetInstance().MaxScore);
                DialogManager.GetInstance().show(gameObject2, true);
                return;
			}
       
		});
	}
}
