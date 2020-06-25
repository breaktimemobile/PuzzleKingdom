using Assets.Scripts.GameManager;
using Assets.Scripts.Utils;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    [SerializeField]
    public List<GameObject> contents = new List<GameObject>();

    [SerializeField]
    public List<Toggle> toggles = new List<Toggle>();

    private List<Dictionary<string, object>> shop_data;      //Æ©Åä¸®¾ó ºí·° Á¤º¸

    public ScrollRect top_view;
    public ScrollRect btm_view;

    public ShopItem ads_item;

    public GameObject img_top_left;
    public GameObject img_top_right;

    public GameObject img_btm_left;
    public GameObject img_btm_right;

    public Image img_top_1;
    public Image img_top_2;

    public Image img_btm_1;
    public Image img_btm_2;

    public Sprite[] icon;

    public Text txt_gold;

    public GameObject img_network;

    private void Start()
    {
        FireBaseManager.Instance.LogEvent("Shop_Open");

        shop_data = CSVReader.Read("shop");


        top_view.onValueChanged.AddListener(Set_top);
        btm_view.onValueChanged.AddListener(Set_btm);

        foreach (var item in toggles)
        {
            item.onValueChanged.AddListener(delegate
            {
                Change_Content(item);
            });


        }

        int i = 0;

        foreach (var item in transform.GetComponentsInChildren<ShopItem>(true))
        {
            item.Set(shop_data[i]);
            i++;
        }

        txt_gold.GetComponent<OverlayNumber>().SetStartNumber(DataManager.Instance.state_Player.LocalData_Diamond);

    }

    public void OnclickSetting()
    {
        GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/setting") as GameObject);
        DialogManager.GetInstance().show(obj, false);
    }


    private void OnEnable()
    {
        toggles[0].isOn = true;
        toggles[1].isOn = false;
        toggles[2].isOn = false;

        Change_Content(toggles[0]);

        txt_gold.GetComponent<OverlayNumber>().SetStartNumber(DataManager.Instance.state_Player.LocalData_Diamond);

    }

    private void Update()
    {
        //Utils.BackListener(base.gameObject, delegate
        //{
        //    this.OnClickReturn();
        //});
    }

    public void OnClickReturn()
    {
        DialogManager.GetInstance().Close(null);
        //GlobalEventHandle.EmitClickPageButtonHandle("main", 0);
    }

    public void Change_Content(Toggle ison)
    {

        foreach (var item in contents)
        {
            item.SetActive(false);
        }

        img_network.SetActive(false);

        if (Application.internetReachability == NetworkReachability.NotReachable
            && (toggles.IndexOf(ison) == 0 || toggles.IndexOf(ison) == 1))
        {
            img_network.SetActive(true);
        }

        contents[toggles.IndexOf(ison)].SetActive(true);

        top_view.horizontalNormalizedPosition = 0;
        btm_view.horizontalNormalizedPosition = 0;

        Set_top(Vector2.zero);
        Set_btm(Vector2.zero);
    }

    private void Set_top(Vector2 val)
    {

        if (Mathf.Round(val.x * 100) * 0.01f <= 0)
        {
            img_top_left.SetActive(false);
            img_top_right.SetActive(true);

            img_top_1.sprite = icon[1];
            img_top_2.sprite = icon[0];



        }
        else if (Mathf.Round(val.x * 100) * 0.01f >= 1)
        {
            img_top_left.SetActive(true);
            img_top_right.SetActive(false);

            img_top_1.sprite = icon[0];
            img_top_2.sprite = icon[1];
        }
    }

    private void Set_btm(Vector2 val)
    {

        if (Mathf.Round(val.x * 100) * 0.01f <= 0)
        {
            img_btm_left.SetActive(false);
            img_btm_right.SetActive(true);

            img_btm_1.sprite = icon[1];
            img_btm_2.sprite = icon[0];
        }
        else if (Mathf.Round(val.x * 100) * 0.01f >= 1)
        {
            img_btm_left.SetActive(true);
            img_btm_right.SetActive(false);

            img_btm_1.sprite = icon[0];
            img_btm_2.sprite = icon[1];
        }
    }

    public void Buy(Dictionary<string, object> data)
    {


        Debug.Log((int)data["shop_type"]);

        switch ((Shop_itme_type)(int)data["shop_type"])
        {
            case Shop_itme_type.ads:

                DataManager.Instance.state_Player.RemoveAds = true;
                transform.GetComponentsInChildren<ShopItem>(true)[0].gameObject.SetActive(false);
                AdsControl.Instance.BannerShow(false);
                break;
            case Shop_itme_type.package:

                FindObjectOfType<MainScene>().PlayPackageAni();

                for (int i = 0; i < 5; i++)
                {
                    int item = (int)data["item_" + i];
                    if (item == 0)
                        continue;


                    switch ((Item_Type)i)
                    {
                        case Item_Type.Boom:
                            DataManager.Instance.state_Player.item_Localdata.Boom += item;
                            break;
                        case Item_Type.Hammer:
                            DataManager.Instance.state_Player.item_Localdata.Hammer += item;
                            break;
                        case Item_Type.Star:
                            DataManager.Instance.state_Player.item_Localdata.Star += item;
                            break;
                        case Item_Type.Hint:
                            DataManager.Instance.state_Player.item_Localdata.Hint += item;
                            break;
                        case Item_Type.coin:
                            GM.GetInstance().AddDiamond(item);

                            break;
                        default:
                            break;
                    }
                }

                GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/shop_complet") as GameObject);
                obj.GetComponent<ShopComplete>().Set_Item(data);
                DialogManager.GetInstance().show(obj);


                break;
            case Shop_itme_type.gift:

                AdsControl.Instance.reward_Type = Reward_Type.shop_ads;

                AdsControl.Instance.ShowRewardedAd();

                
                break;
            case Shop_itme_type.gold:

                Debug.Log("°ñµå~~ " + (int)data["item_4"]);

                GM.GetInstance().AddDiamond((int)data["item_4"]);


                break;
            case Shop_itme_type.item:

                if (DataManager.Instance.state_Player.LocalData_Diamond >= (int)data["price"])
                {
                    GM.GetInstance().ConsumeGEM((int)data["price"]);

                    for (int i = 0; i < 5; i++)
                    {
                        int item = (int)data["item_" + i];

                        if (item == 0)
                            continue;

                        FindObjectOfType<MainScene>().PlayitemAni((Item_Type)i);



                        switch ((Item_Type)i)
                        {
                            case Item_Type.Boom:
                                FireBaseManager.Instance.LogEvent("Shop_bomb");

                                DataManager.Instance.state_Player.item_Localdata.Boom += item;
                                break;
                            case Item_Type.Hammer:
                                FireBaseManager.Instance.LogEvent("Shop_cross_hammer");

                                DataManager.Instance.state_Player.item_Localdata.Hammer += item;
                                break;
                            case Item_Type.Star:
                                FireBaseManager.Instance.LogEvent("Shop_color_star");

                                DataManager.Instance.state_Player.item_Localdata.Star += item;
                                break;
                            case Item_Type.Hint:
                                FireBaseManager.Instance.LogEvent("Shop_magnigier");

                                DataManager.Instance.state_Player.item_Localdata.Hint += item;
                                break;
                            case Item_Type.coin:

                                GM.GetInstance().AddDiamond(item);

                                break;
                            default:
                                break;
                        }

                    }

                    obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/shop_complet") as GameObject);
                    obj.GetComponent<ShopComplete>().Set_Item(data);
                    DialogManager.GetInstance().show(obj);


                }



                break;
            default:
                break;
        }

        Game1Manager.GetInstance()?.Set_Txt_Item();
        G3BoardManager.GetInstance()?.Set_Item_Txt();
        DataManager.Instance.Save_Player_Data();

    }
}

public enum Item_Type
{
    Boom,
    Hammer,
    Star,
    Hint,
    coin
}
