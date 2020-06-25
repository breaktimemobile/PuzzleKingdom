using Assets.Scripts.GameManager;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
	public Text m_lb_title;

	public Text m_lb_score;

	public Text m_lb_score_value;

	public Button m_btn_home;

	public Button m_btn_refresh;

	public Button m_btn_continue;

	public Action OnClickHomeHandle;

	public Action OnClickRefreshHandle;

	public Action OnClickContinueHandle;

    public GameObject[] obj_trigger;

    private void Start()
    {
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

	public void SetScore(int score)
	{
		//this.m_lb_score_value.text = string.Format((score < 1000) ? "{0}" : "{0:0,00}", score);
	}

	public void SetTitle(string id)
	{
		//this.m_lb_score.GetComponent<LanguageComponent>().SetText(id);
	}

	public void OnClickHome()
	{
        switch (GM.GetInstance().GameId)
        {
            case 1:
                FireBaseManager.Instance.LogEvent("Puzzle_Mix_Pause_Main");

                break;
            case 2:
                FireBaseManager.Instance.LogEvent("2048_Pause_Main");

                break;
            case 3:
                FireBaseManager.Instance.LogEvent("Puzzle_Line_Pause_Main");

                break;
            default:
                break;
        }

        Action _action = this.OnClickHomeHandle;
		if (_action != null)
		{
            _action();
		}
		DialogManager.GetInstance().Close(null);
	}

	public void OnClickRefresh()
	{
		if (GM.GetInstance().IsRandomStatus(50))
		{
			AdsManager.GetInstance().Play(AdsManager.AdType.Refresh, delegate
			{
				Action _action = this.OnClickRefreshHandle;
				if (_action == null)
				{
					return;
				}
                _action();
			}, null, 5, null);
		}
		else
		{
			Action _action2 = this.OnClickRefreshHandle;
			if (_action2 != null)
			{
                _action2();
			}
		}

        switch (GM.GetInstance().GameId)
        {
            case 1:
                FireBaseManager.Instance.LogEvent("Puzzle_Mix_Pause_Retry");

                break;
            case 2:
                FireBaseManager.Instance.LogEvent("2048_Pause_Retry");

                break;
            case 3:
                FireBaseManager.Instance.LogEvent("Puzzle_Line_Pause_Retry");

                break;
            default:
                break;
        }
        DialogManager.GetInstance().Close(null);
	}

	public void OnClickContinue()
	{
		if (GM.GetInstance().IsRandomStatus(50))
		{
			AdsManager.GetInstance().Play(AdsManager.AdType.Continue, delegate
			{
				Action _action = this.OnClickContinueHandle;
				if (_action == null)
				{
					return;
				}
                _action();
			}, null, 5, null);
		}
		else
		{
			Action _action2 = this.OnClickContinueHandle;
			if (_action2 != null)
			{
                _action2();
			}
		}

        switch (GM.GetInstance().GameId)
        {
            case 1:
                FireBaseManager.Instance.LogEvent("Puzzle_Mix_Pause_Continue");

                break;
            case 2:
                FireBaseManager.Instance.LogEvent("2048_Pause_Continue");

                break;
            case 3:
                FireBaseManager.Instance.LogEvent("Puzzle_Line_Pause_Continue");

                break;
            default:
                break;
        }
        DialogManager.GetInstance().Close(null);
	}
}
