using Assets.Scripts.Configs;
using Assets.Scripts.GameManager;
using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum Reward_popop_type
{
    Achive,
    Task,
    Gift
}

public class RewardPopup : MonoBehaviour
{
    public Image img_main;
    public Image img_ads;

    public Text txt_val;
    public Text txt_ads_val;

    Item_Type item_Type = Item_Type.coin;
    Reward_popop_type reward_Type = Reward_popop_type.Achive;

    int type = 0;

    int val = 0;

    public Sprite[] item_sp;

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

    public void OnClickReturn()
    {
        DialogManager.GetInstance().Close(null);


    }

    public void set_gift(int type,int val)
    {

        this.reward_Type = Reward_popop_type.Gift;

        this.item_Type = (Item_Type)type;

        this.type = type;

        img_main.sprite = item_sp[(int)item_Type];
        img_ads.sprite = item_sp[(int)item_Type];

        this.val = val;

        txt_val.text = "x" + val;
        txt_ads_val.text = "x" + val * 2;
    }

    public void set_Achive(int type)
    {

        this.reward_Type = Reward_popop_type.Achive;

        this.item_Type = Item_Type.coin;

        this.type = type;

        LocalData localData = AchiveData.GetInstance().Get(type);

        TAchive tAchive = Configs.TAchives[localData.key.ToString()];

        img_main.sprite = item_sp[(int)item_Type];
        img_ads.sprite = item_sp[(int)item_Type];

        this.val = tAchive.Item;

        txt_val.text = "x" + val;
        txt_ads_val.text = "x" + val * 2;
    }

    public void set_Task(int id)
    {
        item_sp = FindObjectOfType<MainScene>().itme_sp;

        this.reward_Type = Reward_popop_type.Task;

        this.item_Type = Item_Type.coin;

        this.type = id;

        LocalData data = TaskData.GetInstance().Get(this.type);

        TTask tTask = Configs.TTasks[data.key.ToString()];

        img_main.sprite = item_sp[(int)item_Type];
        img_ads.sprite = item_sp[(int)item_Type];

        this.val = tTask.Item;

        txt_val.text = "x" + val;
        txt_ads_val.text = "x" + val * 2;
    }

    public void get_item()
    {
        switch (reward_Type)
        {
            case Reward_popop_type.Achive:
                FireBaseManager.Instance.LogEvent("Achive_Get");

                if (!AchiveData.GetInstance().Finish(type))
                {
                    return;
                }

                GM.GetInstance().AddDiamond(val, true);
                FindObjectOfType<Achive>().Set_BindDataToUI();
                AudioManager.GetInstance().PlayEffect("sound_eff_achive");
                OnClickReturn();

                break;

            case Reward_popop_type.Task:
                FireBaseManager.Instance.LogEvent("Task_Get");

                if (!TaskData.GetInstance().Finish(type))
                {
                    return;
                }

                GM.GetInstance().AddDiamond(val, true);
                FindObjectOfType<Task>().Refresh();
                AudioManager.GetInstance().PlayEffect("sound_eff_task");
                OnClickReturn();

                break;
            case Reward_popop_type.Gift:
                FireBaseManager.Instance.LogEvent("Gift_Get");

                Get_Gift();
                AudioManager.GetInstance().PlayEffect("sound_eff_task");

                break;
            default:
                break;
        }




    }

    public void ads_item()
    {
        switch (reward_Type)
        {
            case Reward_popop_type.Achive:
                FireBaseManager.Instance.LogEvent("Achive_Ads");

                AdsControl.Instance.reward_Type = Reward_Type.Achive;

                AdsControl.Instance.achive_type = type;
                AdsControl.Instance.achive_val = val;
                AdsControl.Instance.ShowRewardedAd();

                break;
            case Reward_popop_type.Task:
                FireBaseManager.Instance.LogEvent("Task_Ads");

                AdsControl.Instance.reward_Type = Reward_Type.Task;

                AdsControl.Instance.achive_type = type;
                AdsControl.Instance.achive_val = val;
                AdsControl.Instance.ShowRewardedAd();
                break;

            case Reward_popop_type.Gift:

                FireBaseManager.Instance.LogEvent("Gift_Ads");

                val *= 2;
                AdsControl.Instance.reward_Type = Reward_Type.gift;

                AdsControl.Instance.ShowRewardedAd();
                break;
            default:
                break;
        }


        AudioManager.GetInstance().PlayEffect("sound_eff_achive");

    }

    public void Get_Gift()
    {
        FindObjectOfType<MainScene>().PlayitemAni((Item_Type)type);

        switch ((Item_Type)type)
        {
            case Item_Type.Boom:
                FireBaseManager.Instance.LogEvent("Gift_bomb");

                DataManager.Instance.state_Player.item_Localdata.Boom += val;
                break;
            case Item_Type.Hammer:
                FireBaseManager.Instance.LogEvent("Gift_cross_hammer");

                DataManager.Instance.state_Player.item_Localdata.Hammer += val;
                break;
            case Item_Type.Star:
                FireBaseManager.Instance.LogEvent("Gift_color_star");

                DataManager.Instance.state_Player.item_Localdata.Star += val;
                break;
            case Item_Type.Hint:
                FireBaseManager.Instance.LogEvent("Gift_magnigier");

                DataManager.Instance.state_Player.item_Localdata.Hint += val;
                break;
            case Item_Type.coin:
                FireBaseManager.Instance.LogEvent("Gift_coin");

                GM.GetInstance().AddDiamond(val);

                break;
            default:
                break;
        }

        DataManager.Instance.Save_Player_Data();
        OnClickReturn();

    }
}
