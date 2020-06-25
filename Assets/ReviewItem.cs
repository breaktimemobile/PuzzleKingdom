using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ReviewItem : MonoBehaviour
{
    public Image img_item;
    public Text txt_item;
    public Text txt_config;

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

    public void Set(int type)
    {
        img_item.sprite = FindObjectOfType<MainScene>().itme_sp[type];

        switch ((Item_Type)type)
        {
            case Item_Type.Boom:
                txt_item.GetComponent<LanguageComponent>().SetText("TXT_NO_50098");
                this.txt_config.GetComponent<LanguageComponent>().SetText("TXT_NO_50039");

                break;
            case Item_Type.Hammer:
                txt_item.GetComponent<LanguageComponent>().SetText("TXT_NO_50099");

                this.txt_config.GetComponent<LanguageComponent>().SetText("TXT_NO_50040");

                break;
            case Item_Type.Star:
                txt_item.GetComponent<LanguageComponent>().SetText("TXT_NO_50100");

                this.txt_config.GetComponent<LanguageComponent>().SetText("TXT_NO_50041");

                break;
            case Item_Type.Hint:
                txt_item.GetComponent<LanguageComponent>().SetText("TXT_NO_50096");

                this.txt_config.GetComponent<LanguageComponent>().SetText("TXT_NO_50069");

                break;
            case Item_Type.coin:
                txt_item.GetComponent<LanguageComponent>().SetText("TXT_NO_20004");

                this.txt_config.GetComponent<LanguageComponent>().SetText("TXT_NO_20004");

                break;
            default:
                break;
        }

    }

    public void Return()
    {
        DialogManager.GetInstance().Close(null);
    }

}
