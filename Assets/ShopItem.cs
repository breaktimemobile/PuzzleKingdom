using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Shop_itme_type
{
    ads,
    package,
    gift,
    gold,
    item
}

public class ShopItem : MonoBehaviour
{
    public Button btn_info;
    public Button btn_buy;
    public Text txt_coin_price;
    public Text txt_price;
    public Image img_price;
    public Image img_item;
    public Text txt_val;
    public Text txt_timer;

    public Sprite[] price_icon;
    public GameObject txt_item;

    Dictionary<string, object> data;

    private void Update()
    {
        if (txt_timer == null)
        {
            return;
        }

        TimeSpan LateTime = GiftTime - DateTime.Now;

        if (LateTime.TotalSeconds <= 0)
        {
            btn_buy.interactable = true;
            txt_timer.gameObject.SetActive(false);

        }
        else
        {
            btn_buy.interactable = false;

            int diffMiniute = LateTime.Minutes; //30
            int diffSecond = LateTime.Seconds; //0

            txt_timer.text = string.Format("{0:00}:{1:00}", diffMiniute, diffSecond);

        }
    }

    public void Set(Dictionary<string, object> data)
    {
        this.data = data;

        if ((int)data["num"] == 0 && DataManager.Instance.state_Player.RemoveAds)
        {
            gameObject.SetActive(false);
        }

        btn_info.gameObject.SetActive(false);
        txt_coin_price.gameObject.SetActive(false);
        txt_price.gameObject.SetActive(false);
        img_price.gameObject.SetActive(false);

        switch ((Shop_itme_type)(int)data["shop_type"])
        {
            case Shop_itme_type.ads:
                btn_info.gameObject.SetActive(true);

                btn_buy.onClick.AddListener(() =>
                {
                    IAPManager.Instance.data = data;
                    IAPManager.Instance.OnBtnPurchaseClicked(data["skuid"].ToString());
                });

                btn_info.onClick.AddListener(() =>
                {
                    GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/shop_review_ads") as GameObject);
                    DialogManager.GetInstance().show(obj);
                    obj.GetComponentInChildren<Button>().onClick.AddListener(() => DialogManager.GetInstance().Close(null));
                });
              
                break;
            case Shop_itme_type.package:
                btn_info.gameObject.SetActive(true);
                Text[] texts = txt_item.GetComponentsInChildren<Text>();
                for (int i = 0; i < 4; i++)
                {
                    texts[i].text = "x" + data["item_" + i].ToString();
                }

                btn_buy.onClick.AddListener(() =>
                {
                    Debug.Log((int)data["shop_type"]);
                    IAPManager.Instance.data = data;
                    IAPManager.Instance.OnBtnPurchaseClicked(data["skuid"].ToString());
                });

                btn_info.onClick.AddListener(() =>
                {
                    GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/shop_review_package") as GameObject);
                    DialogManager.GetInstance().show(obj);
                    obj.GetComponentInChildren<Button>().onClick.AddListener(() => DialogManager.GetInstance().Close(null));

                });
                

                break;
            case Shop_itme_type.gift:
                FindObjectOfType<Shop>().ads_item = this;
                btn_buy.onClick.AddListener(() => FindObjectOfType<Shop>().Buy(data));

                Set_Timer();

                break;
            case Shop_itme_type.gold:
                txt_val.text = "x" + data["item_4"].ToString();

                btn_buy.onClick.AddListener(() =>
                {
                    Debug.Log((int)data["shop_type"]);

                    IAPManager.Instance.data = data;
                    IAPManager.Instance.OnBtnPurchaseClicked(data["skuid"].ToString());
                });
                break;

            case Shop_itme_type.item:

                btn_buy.onClick.AddListener(() => FindObjectOfType<Shop>().Buy(data));

                btn_info.gameObject.SetActive(true);

                for (int i = 0; i < 5; i++)
                {
                    if ((int)data["item_" + i] != 0)
                    {
                        txt_val.text = "x" + data["item_" + i].ToString();
                        break;
                    }

                }

                btn_info.onClick.AddListener(() =>
                {
                    GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/shop_review_item") as GameObject);
                    DialogManager.GetInstance().show(obj);
                    for (int i = 0; i < 5; i++)
                    {
                        if ((int)data["item_" + i] != 0)
                        {
                            obj.GetComponent<ReviewItem>().Set(i);
                            break;
                        }

                    }

                });

                break;
            default:
                break;
        }

        Set_Price((int)data["price_type"]);


    }

    public void Set_Timer()
    {
        if (DataManager.Instance.state_Player.LocalData_Shop_Time == "-1")
        {
            btn_buy.interactable = true;
            txt_timer.gameObject.SetActive(false);

        }
        else
        {
            btn_buy.interactable = false;

            txt_timer.gameObject.SetActive(true);

            GiftTime = DateTime.Parse(DataManager.Instance.state_Player.LocalData_Shop_Time);
        }
    }

    DateTime GiftTime;

    public void Set_Price(int type)
    {
        Debug.Log("Item " + data["name"] + "   "+ type + "" + data["price"]);

        //현금
        if (type == 0)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                txt_price.gameObject.SetActive(true);
                txt_coin_price.text = data["price"].ToString();

            }
            else
            {
                txt_price.gameObject.SetActive(true);

                int idx = 0;

#if UNITY_ANDROID

                idx = IAPManager.Instance.Android_sProductIds.IndexOf(data["skuid"].ToString());


#elif UNITY_IOS

                idx = IAPManager.Instance.Ios_sProductIds.IndexOf(data["skuid"].ToString());

#endif

                txt_price.text = IAPManager.Instance.price[idx];
            }
     
        }
        //코인
        else if(type == 1)
        {
            img_price.sprite = price_icon[0];
            txt_coin_price.text = data["price"].ToString();
            img_price.gameObject.SetActive(true);
            txt_coin_price.gameObject.SetActive(true);

        }
        //선물
        else
        {
            img_price.sprite = price_icon[1];

            if (txt_coin_price.GetComponent<LanguageComponent>() == null)
            {
                txt_coin_price.gameObject.AddComponent<LanguageComponent>();
            }
            
            txt_coin_price.GetComponent<LanguageComponent>().SetText("TXT_NO_20018");

            img_price.gameObject.SetActive(true);
            txt_coin_price.gameObject.SetActive(true);

        }
    }
}
