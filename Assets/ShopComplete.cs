using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopComplete : MonoBehaviour
{
    public GameObject item_1_bg;
    public GameObject item_2_bg;

    public GameObject[] item;

    public Text txt_item;

    public Text[] txt_item_val;

    public Sprite[] sp_item;

    private string[] package_name = { "TXT_NO_50079", "TXT_NO_50080", "TXT_NO_50081", "TXT_NO_50082", "TXT_NO_50083" };

    private string[] item_name = { "TXT_NO_50098", "TXT_NO_50099", "TXT_NO_50100", "TXT_NO_50096" };

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

    public void Onclick_Return()
    {
        DialogManager.GetInstance().Close(null);

    }

    public void Set_Item(Dictionary<string, object> data)
    {


        foreach (var item in item)
        {
            item.SetActive(false);
        }

        foreach (var item in txt_item_val)
        {
            item.gameObject.SetActive(false);
        }

        item_1_bg.SetActive(false);
        item_2_bg.SetActive(false);

        switch ((Shop_itme_type)(int)data["shop_type"])
        {
            case Shop_itme_type.ads:
                break;
            case Shop_itme_type.package:
                item_2_bg.SetActive(true);

                int int_num = (int)data["num"];

                txt_item.GetComponent<LanguageComponent>().SetText(package_name[int_num-1]);

                for (int i = 0; i < 4; i++)
                {
                    int int_item = (int)data["item_" + i];

                    if (int_item > 0)
                    {

                        item[i].SetActive(true);
                        txt_item_val[i].gameObject.SetActive(true);
                        txt_item_val[i].text = "x" + int_item;

                    }
                }

                break;
            case Shop_itme_type.gift:
                break;
            case Shop_itme_type.gold:
                break;
            case Shop_itme_type.item:

                item_1_bg.SetActive(true);

                for (int i = 0; i < 4; i++)
                {
                    int int_item = (int)data["item_" + i];
               
                    if (int_item > 0)
                    {

                        item[i].GetComponent<Image>().sprite = sp_item[i];
                        txt_item.GetComponent<LanguageComponent>().SetText(item_name[i]);
                        item[i].SetActive(true);
                        txt_item_val[i].gameObject.SetActive(true);
                        txt_item_val[i].text = "x" + int_item;

                    }
                }
               


                break;
            default:
                break;
        }

    }

}
