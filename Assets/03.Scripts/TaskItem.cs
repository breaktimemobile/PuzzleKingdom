using Assets.Scripts.Configs;
using Assets.Scripts.GameManager;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TaskItem : MonoBehaviour
{
	
	public Image img_pregress;

	public Text txt_desc;

	public Button button;

	public Text btn_txt;

	public Image txt_progress;

    public Text txt_src;

    
    public Text txt_awards;

	public int id;

    public Sprite[] btn;
    public Color[] color;


    float base_width = 0;
    float base_hight = 0;

    float width = 0;
    float hight = 0;

    bool play_anim = true;

    private void Start()
	{
		GlobalEventHandle.OnRefreshTaskHandle = (Action<int>)Delegate.Combine(GlobalEventHandle.OnRefreshTaskHandle, new Action<int>(this.DoRefresh));
		this.BindDataToUI();
	}

	private void Update()
	{
        if (play_anim)
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(width, hight);

    }

    private void OnDestroy()
	{
		GlobalEventHandle.OnRefreshTaskHandle = (Action<int>)Delegate.Remove(GlobalEventHandle.OnRefreshTaskHandle, new Action<int>(this.DoRefresh));
	}

	private void OnEnable()
	{
		this.BindDataToUI();
	}

	public void BindDataToUI()
	{
		if (this.id == 0)
		{
			return;
		}
		if (!Configs.TTasks.ContainsKey(this.id.ToString()))
		{
			return;
		}

        base_width = base_width != 0 ? base_width : button.GetComponent<RectTransform>().sizeDelta.x;
        base_hight = base_hight != 0 ? base_hight : button.GetComponent<RectTransform>().sizeDelta.y;

        width = base_width != 0 ? base_width : button.GetComponent<RectTransform>().sizeDelta.x;
        hight = base_hight != 0 ? base_hight : button.GetComponent<RectTransform>().sizeDelta.y;

        DOTween.Kill(button.gameObject);

        button.transform.localScale = Vector3.one;

        EventTrigger eventTrigger = button.gameObject.AddComponent<EventTrigger>();


        LocalData localData = TaskData.GetInstance().Get(this.id);
		TTask tTask = Configs.TTasks[this.id.ToString()];
		this.txt_awards.text = string.Format("{0}", tTask.Item);
		this.txt_desc.GetComponent<LanguageComponent>().SetText(tTask.Desc);

        float num = (float)localData.value / (float)tTask.Value;
        num = ((num >= 1f) ? 1f : num);
        this.img_pregress.fillAmount = num;

        int num2 = (localData.value > tTask.Value) ? tTask.Value : localData.value;
		this.txt_src.text = string.Format("{0}/{1}", num2, tTask.Value);

        Debug.Log(width * 0.9f);

        DOTween.To(() => width, x => width = x, width * 0.9f, 1)
           .SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

        DOTween.To(() => hight, x => hight = x, hight * 0.9f, 1)
         .SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

        if (localData.status == -1)
		{
            //바로가기
            button.GetComponent<Image>().sprite = btn[1];

            foreach (var item in btn_txt.GetComponents<Outline>())
            {
                item.effectColor = color[1];
            }

            this.btn_txt.GetComponent<LanguageComponent>().SetText("TXT_NO_20002");


            EventTrigger.Entry entry_PointerDown = new EventTrigger.Entry();
            entry_PointerDown.eventID = EventTriggerType.PointerDown;
            entry_PointerDown.callback.AddListener((data) => { FindObjectOfType<MainScene>().Pointer_Down(button.transform); });
            eventTrigger.triggers.Add(entry_PointerDown);

            EventTrigger.Entry entry_PointerUp = new EventTrigger.Entry();
            entry_PointerUp.eventID = EventTriggerType.PointerUp;
            entry_PointerUp.callback.AddListener((data) => { FindObjectOfType<MainScene>().Pointer_Up(button.transform); });
            eventTrigger.triggers.Add(entry_PointerUp);

            return;
		}
		if (localData.status == -2)
		{
            //받기
            button.GetComponent<Image>().sprite = btn[0];

            foreach (var item in btn_txt.GetComponents<Outline>())
            {
                item.effectColor = color[0];
            }
            this.btn_txt.GetComponent<LanguageComponent>().SetText("TXT_NO_20001");


            EventTrigger.Entry entry_PointerDown = new EventTrigger.Entry();
            entry_PointerDown.eventID = EventTriggerType.PointerDown;
            entry_PointerDown.callback.AddListener((data) => { FindObjectOfType<MainScene>().Pointer_Down(button.transform); });
            eventTrigger.triggers.Add(entry_PointerDown);

            EventTrigger.Entry entry_PointerUp = new EventTrigger.Entry();
            entry_PointerUp.eventID = EventTriggerType.PointerUp;
            entry_PointerUp.callback.AddListener((data) => { FindObjectOfType<MainScene>().Pointer_Up(button.transform); });
            eventTrigger.triggers.Add(entry_PointerUp);

            return;
		}
		if (localData.status == -3)
		{
            //완료
            button.GetComponent<Image>().sprite = btn[0];
            button.interactable = false;

            foreach (var item in btn_txt.GetComponents<Outline>())
            {
                item.effectColor = color[0];
            }
            this.btn_txt.GetComponent<LanguageComponent>().SetText("TXT_NO_20004");
		}
	}

	public void OnClickButton()
	{
		if (this.id == 0)
		{
			return;
		}
		if (!Configs.TTasks.ContainsKey(this.id.ToString()))
		{
			return;
		}

        LocalData data = TaskData.GetInstance().Get(this.id);

        if (data.status == -3)
        {
            return;
        }

        if (data.status == -1)
        {
            DialogManager.GetInstance().Close(null);
            return;
        }
      
        GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/reward") as GameObject);
        obj.GetComponent<RewardPopup>().set_Task(this.id);
        DialogManager.GetInstance().show(obj);

        AudioManager.GetInstance().PlayEffect("sound_eff_task");

    }

    private void DoRefresh(int id)
	{
		if (this.id != id)
		{
			return;
		}
		this.BindDataToUI();
	}
}
