using Assets.Scripts.GameManager;
using Assets.Scripts.Utils;
using DG.Tweening;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_IOS
using UnityEngine.iOS;
#endif
/*
 * This script will show information for UI when game over (Merge Block Game)
 */
public class G1UIManager : MonoBehaviour
{
    //add 5 diamond if user get first sharing
	[Serializable]
	private sealed class CommonClass
	{
		public static readonly G1UIManager.CommonClass _commonClass = new G1UIManager.CommonClass();

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
    //score text
	public Text m_txt_score_value;
    //best score text
	public Text m_txt_record_value;

    public Text m_txt_title;

    public Text txt_subtitle;

    private double m_needGEM;

	private int gameID = 1;

	public Transform[] m_tips_component;

	public string[] m_tips_content;

    public GameObject[] obj_base;

    public GameObject[] obj_best;

    public GameObject[] obj_trigger;

    private void Start()
	{
		this.m_needGEM = 100.0 * Math.Pow(2.0, (double)GM.GetInstance().ConsumeCount);
		GM.GetInstance().SetSavedGameID(0);
		GM.GetInstance().SaveScore(this.gameID, 0);
        Game1DataLoader.GetInstance().FinishCount = 0;
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

	public void OnClickDiamond()
	{
		if (!GM.GetInstance().isFullGEM(Convert.ToInt32(this.m_needGEM)))
		{
			ToastManager.Show("TXT_NO_50001", true);
			return;
		}
		GM.GetInstance().SetSavedGameID(this.gameID);
		GM.GetInstance().ConsumeGEM(Convert.ToInt32(this.m_needGEM));
		GM.GetInstance().AddConsumeCount();
        Game1DataLoader.GetInstance().FillLife(false);
        Game1DataLoader.GetInstance().DoFillLife();
		DialogManager.GetInstance().Close(null);
	}

	public void OnClickAds()
	{
		AdsManager.GetInstance().Play(AdsManager.AdType.ResetLife, delegate
		{
			GM.GetInstance().SetSavedGameID(this.gameID);
            Game1DataLoader.GetInstance().FillLife(false);
            Game1DataLoader.GetInstance().DoFillLife();
			DialogManager.GetInstance().Close(null);
		}, null, 5, null);
	}

	public void OnClickHome()
    {
        FireBaseManager.Instance.LogEvent("Puzzle_Mix_Result_Main");

        AdsControl.Instance.ShowInterstitial();
        Game1DataLoader.GetInstance().Score = 0;
		GM.GetInstance().SaveScore(this.gameID, 0);
		GM.GetInstance().SetSavedGameID(0);
		GM.GetInstance().ResetToNewGame();
		GM.GetInstance().ResetConsumeCount();
		DialogManager.GetInstance().Close(null);
		GlobalEventHandle.EmitClickPageButtonHandle("main", 0);
	}

	public void OnClickAgain()
    {
        FireBaseManager.Instance.LogEvent("Puzzle_Mix_Result_Retry");

        AudioManager.GetInstance().PlayBgMusic("sound_ingame", true);

        PlayerPrefs.SetInt("conti", 0);

        GM.GetInstance().SaveScore(this.gameID, 0);
		GM.GetInstance().SetSavedGameID(0);
		GM.GetInstance().ResetToNewGame();
		GM.GetInstance().ResetConsumeCount();
        Game1DataLoader.GetInstance().Score = 0;
        Game1DataLoader.GetInstance().StartNewGame();
		DialogManager.GetInstance().Close(null);

	}

	public void OnClickShare()
    {
        FireBaseManager.Instance.LogEvent("Puzzle_Mix_Result_Review");

#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.block.puzzle.puzzlego.number.puzzledom.Kingdom");
#elif UNITY_IOS
         Device.RequestStoreReview());
#endif
    }

}
