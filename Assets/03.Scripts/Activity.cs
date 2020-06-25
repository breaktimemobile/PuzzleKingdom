using Assets.Scripts.Configs;
using Assets.Scripts.GameManager;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Activity : MonoBehaviour
{

    private sealed class DisplayClass
    {
        public Activity _this;

        public int id;

        internal void OnClickVideo()
        {
            this._this.OnClickButton(this.id);
            DialogManager.GetInstance().Close(null);
        }
    }


    [Serializable]
    private sealed class AnimClass
    {
        public static readonly Activity.AnimClass _animFunc = new Activity.AnimClass();

        public static Action _action;

        internal void _OnClickVideo()
        {
            DialogManager.GetInstance().Close(null);
        }
    }

    [SerializeField]
    public List<GameObject> m_items = new List<GameObject>();

    public GameObject btn_return;
    public GameObject btn_ads;

    private void Start()
    {
        base_width = base_width != 0 ? base_width : btn_return.GetComponent<RectTransform>().sizeDelta.x;
        base_hight = base_hight != 0 ? base_hight : btn_return.GetComponent<RectTransform>().sizeDelta.y;

        width = btn_return.GetComponent<RectTransform>().sizeDelta.x;
        hight = btn_return.GetComponent<RectTransform>().sizeDelta.y;


        DOTween.To(() => width, x => width = x, width * 0.9f, 1)
              .SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

        DOTween.To(() => hight, x => hight = x, hight * 0.9f, 1)
         .SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

        EventTrigger eventTrigger = btn_return.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry_PointerDown = new EventTrigger.Entry();
        entry_PointerDown.eventID = EventTriggerType.PointerDown;
        entry_PointerDown.callback.AddListener((data) => { FindObjectOfType<MainScene>().Pointer_Down(btn_return.transform); });
        eventTrigger.triggers.Add(entry_PointerDown);

        EventTrigger.Entry entry_PointerUp = new EventTrigger.Entry();
        entry_PointerUp.eventID = EventTriggerType.PointerUp;
        entry_PointerUp.callback.AddListener((data) => { FindObjectOfType<MainScene>().Pointer_Up(btn_return.transform); });
        eventTrigger.triggers.Add(entry_PointerUp);



        eventTrigger = btn_ads.gameObject.AddComponent<EventTrigger>();

        entry_PointerDown = new EventTrigger.Entry();
        entry_PointerDown.eventID = EventTriggerType.PointerDown;
        entry_PointerDown.callback.AddListener((data) => { FindObjectOfType<MainScene>().Pointer_Down(btn_ads.transform); });
        eventTrigger.triggers.Add(entry_PointerDown);

        entry_PointerUp = new EventTrigger.Entry();
        entry_PointerUp.eventID = EventTriggerType.PointerUp;
        entry_PointerUp.callback.AddListener((data) => { FindObjectOfType<MainScene>().Pointer_Up(btn_ads.transform); });
        eventTrigger.triggers.Add(entry_PointerUp);

        this.InitUI();


    }

    bool play_anim = false;

    float base_width = 0;
    float base_hight = 0;

    float width = 0;
    float hight = 0;

    private void Update()
    {
        if (play_anim)
        {
            btn_return.GetComponent<RectTransform>().sizeDelta = new Vector2(width, hight);
            btn_ads.GetComponent<RectTransform>().sizeDelta = new Vector2(width, hight);


        }
    }

    private void OnEnable()
    {
    }

    public void OnClickButton(int id)
    {
        if (!Configs.TActivitys.ContainsKey(id.ToString()))
        {
            return;
        }
        LoginData.GetInstance().SetSignInData(id, 2);
        TActivity tActivity = Configs.TActivitys[id.ToString()];


        FindObjectOfType<MainScene>().PlayitemAni((Item_Type)tActivity.Item_type);

        switch ((Item_Type)tActivity.Item_type)
        {
            case Item_Type.Boom:
                DataManager.Instance.state_Player.item_Localdata.Boom += tActivity.Item;
                break;
            case Item_Type.Hammer:
                DataManager.Instance.state_Player.item_Localdata.Hammer += tActivity.Item;
                break;
            case Item_Type.Star:
                DataManager.Instance.state_Player.item_Localdata.Star += tActivity.Item;
                break;
            case Item_Type.Hint:
                DataManager.Instance.state_Player.item_Localdata.Hint += tActivity.Item;
                break;
            case Item_Type.coin:
                GM.GetInstance().AddDiamond(tActivity.Item, true);

                break;
            default:
                break;
        }

        this.InitUI();
    }

    public void OnClickNo()
    {
        int[] _loginData = LoginData.GetInstance().GetSignInData();
        int num = 0;
        bool flag = false;
        int[] array = _loginData;
        for (int i = 0; i < array.Length; i++)
        {
            int _loginIndex = array[i];
            num++;
            if (_loginIndex == 1)
            {
                flag = true;
                break;
            }
        }
        if (!flag)
        {
            return;
        }
        this.OnClickButton(num);
        DialogManager.GetInstance().Close(null);
    }

    public void OnClickVideo()
    {
        int[] _loginData = LoginData.GetInstance().GetSignInData();
        int num = 0;
        bool flag = false;
        int[] array = _loginData;
        for (int i = 0; i < array.Length; i++)
        {
            int _loginIndex = array[i];
            num++;
            if (_loginIndex == 1)
            {
                flag = true;
                break;
            }
        }
        if (!flag)
        {
            return;
        }

        if (!Configs.TActivitys.ContainsKey(num.ToString()))
        {
            return;
        }

        LoginData.GetInstance().SetSignInData(num, 2);
        TActivity tActivity = Configs.TActivitys[num.ToString()];


        FindObjectOfType<MainScene>().PlayitemAni((Item_Type)tActivity.Item_type);

        switch ((Item_Type)tActivity.Item_type)
        {
            case Item_Type.Boom:
                DataManager.Instance.state_Player.item_Localdata.Boom += tActivity.Item * 2;
                break;
            case Item_Type.Hammer:
                DataManager.Instance.state_Player.item_Localdata.Hammer += tActivity.Item * 2;
                break;
            case Item_Type.Star:
                DataManager.Instance.state_Player.item_Localdata.Star += tActivity.Item * 2;
                break;
            case Item_Type.Hint:
                DataManager.Instance.state_Player.item_Localdata.Hint += tActivity.Item * 2;
                break;
            case Item_Type.coin:
                GM.GetInstance().AddDiamond(tActivity.Item * 2, true);

                break;
            default:
                break;
        }

        this.InitUI();
        DialogManager.GetInstance().Close(null);
    }

    public void OnClickAds()
    {
        AdsControl.Instance.reward_Type = Reward_Type.Daily;
        AdsControl.Instance.ShowRewardedAd();
    }

    public void OnClickReturn()
    {
        DialogManager.GetInstance().Close(null);
    }

    private void InitUI()
    {
        int serialLoginCount = LoginData.GetInstance().GetSerialLoginCount();
        int[] signInData = LoginData.GetInstance().GetSignInData();
        for (int i = 0; i < signInData.Length; i++)
        {
            this.ShowLogin(this.m_items[i], serialLoginCount, signInData[i], i + 1);
        }
    }

    private void ShowLogin(GameObject obj, int count, int status, int id)
    {

        TActivity tActivity = Configs.TActivitys[id.ToString()];
        Image icon = obj.transform.Find("img_icon").GetComponent<Image>();
        Text _component = obj.transform.Find("img_icon/txt_value").GetComponent<Text>();
        Transform transform = obj.transform.Find("txt_desc01");
        Transform transform2 = obj.transform.Find("img_item_bg");
        Transform transform3 = obj.transform.Find("img_finish");
        Transform transform4 = obj.transform.Find("button/txt");
        _component.text = string.Format("{0}", tActivity.Item.ToString());
        transform.GetComponent<LanguageComponent>().SetText(tActivity.Desc);

        icon.sprite = FindObjectOfType<MainScene>().itme_sp[tActivity.Item_type];

        switch (status)
        {
            case 0:
                transform3.gameObject.SetActive(false);
                transform4.GetComponent<LanguageComponent>().SetText("TXT_NO_20001");
                return;
            case 1:
                transform3.gameObject.SetActive(false);
                transform4.GetComponent<LanguageComponent>().SetText("TXT_NO_20001");
                return;
            case 2:
                transform3.gameObject.SetActive(true);
                transform4.GetComponent<LanguageComponent>().SetText("TXT_NO_50006");
                return;
            default:
                return;
        }
    }


}
