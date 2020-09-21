using Assets.Scripts.GameManager;
using Assets.Scripts.Utils;
using DG.Tweening;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_IOS
using UnityEngine.iOS;
#endif
using UnityEngine.UI;
/*
 * this class will show information of game UI in game over
 */
public class G2UIManager : MonoBehaviour
{
    //common function, first sharing will get 5 diamonds
	[Serializable]
	private sealed class CommonClass
	{
		public static readonly G2UIManager.CommonClass _commonClass = new G2UIManager.CommonClass();

		public static Action _action;

		internal void OnClickShare()
		{
			if (GM.GetInstance().isFirstShare())
			{
				GM.GetInstance().ResetFirstShare(1);
				GM.GetInstance().AddDiamond(5, true);
			}
		}
	}
    //score value text
	public Text m_txt_score_value;
    //best score value text
	public Text m_txt_record_value;
    //button play again
	public Button m_btn_again;
    //button quit game play
	public Button m_btn_main;
    //game ID (default 2 for block 2048 shooter)
	private int gameID = 2;

	public Image m_img_circle;

	public LanguageComponent m_txt_tips;

	public Transform[] m_tips_component;

	public string[] m_tips_content;

    public Text m_txt_title;    

    public Text txt_subtitle;

    public GameObject[] obj_base;

    public GameObject[] obj_best;

    public GameObject[] obj_trigger;


    private void Start()
	{
		GM.GetInstance().SetSavedGameID(0);
		GM.GetInstance().SaveScore(this.gameID, 0);
        G2BoardGenerator.GetInstance().FinishCount = 0;
        AudioManager.GetInstance().PlayBgMusic("sound_gameover",true);

        
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

	private void OnDestroy()
	{
		DOTween.Kill(this.m_img_circle, false);
	}

	public void Load(int score, int maxScore)
	{
		this.m_txt_score_value.text = string.Format((score < 1000) ? "{0}" : "{0:0,00}", score);
		this.m_txt_record_value.text = string.Format((maxScore < 1000) ? "{0}" : "{0:0,00}", maxScore);

        Set_Best(score == maxScore);

    }

    public void Set_Best(bool bset)
    {

        foreach (var item in obj_best)
        {
            item.SetActive(bset);
        }

        foreach (var item in obj_base)
        {
            item.SetActive(!bset);

        }

        if (bset)
        {
            m_txt_title.GetComponent<LanguageComponent>().SetText("TXT_NO_50130");
            txt_subtitle.GetComponent<LanguageComponent>().SetText("TXT_NO_50129");


            GameObject eff_levelup = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/effect/eff_levelup") as GameObject);
            //gameObject.transform.SetParent(base.transform, false);
            UnityEngine.Object.Destroy(eff_levelup, 5f);
        }
        else
        {
            m_txt_title.GetComponent<LanguageComponent>().SetText("TXT_NO_40053");
            txt_subtitle.GetComponent<LanguageComponent>().SetText("TXT_NO_50121");


        }
    }

    public void OnClickAgain()
    {
        FireBaseManager.Instance.LogEvent("2048_Result_Retry");

        PlayerPrefs.SetInt("conti", 0);

        AudioManager.GetInstance().PlayBgMusic("sound_ingame", true);

        GM.GetInstance().SetSavedGameID(this.gameID);
        G2BoardGenerator.GetInstance().StartNewGame();
		DialogManager.GetInstance().Close(null);
	}

	public void OnClickAds()
	{
		AdsManager.GetInstance().Play(AdsManager.AdType.UseProp, delegate
		{
			GM.GetInstance().SetSavedGameID(this.gameID);
			DialogManager.GetInstance().Close(null);
			Action _action = G2BoardGenerator.GetInstance().DoVedioRefresh;
			if (_action == null)
			{
				return;
			}
            _action();
		}, null, 5, null);
	}

	public void OnClickHome()
    {
        FireBaseManager.Instance.LogEvent("2048_Result_Main");

        
        AdsControl.Instance.ShowInterstitial();

        G2BoardGenerator.GetInstance().Score = 0;
		GM.GetInstance().SaveScore(this.gameID, 0);
		GM.GetInstance().SetSavedGameID(0);
		GM.GetInstance().ResetToNewGame();
		GM.GetInstance().ResetConsumeCount();
		DialogManager.GetInstance().Close(null);
		GlobalEventHandle.EmitClickPageButtonHandle("main", 0);
	}

	public void OnClickRate()
	{
        FireBaseManager.Instance.LogEvent("2048_Result_Review");

#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.block.puzzle.puzzlego.number.puzzledom.Kingdom");
#elif UNITY_IOS
         Device.RequestStoreReview();
#endif
    }

    private void PlayTips()
	{
		int idx = 0;
		this.m_img_circle.gameObject.SetActive(true);
		Sequence _sequence = DOTween.Sequence();
        _sequence.Append(this.m_img_circle.DOFade(0.3f, 1f));
        _sequence.Append(this.m_img_circle.DOFade(1f, 1f));
        _sequence.Append(this.m_img_circle.DOFade(0.3f, 1f));
        _sequence.Append(this.m_img_circle.DOFade(1f, 1f));
        _sequence.SetLoops(-1);
        _sequence.SetTarget(this.m_img_circle);
		Sequence _sequence2 = DOTween.Sequence();
        int idx1 = 0;
        _sequence2.AppendCallback(delegate
		{
			//int idx;
			this.m_img_circle.transform.localPosition = this.m_tips_component[idx1].localPosition;
			this.m_txt_tips.SetText(this.m_tips_content[idx1]);
			idx1++;
			idx = idx1;
			idx1 %= 3;
		});
        _sequence2.AppendInterval(4f);
        _sequence2.SetLoops(-1);
        _sequence2.SetTarget(this.m_img_circle);
	}
}
