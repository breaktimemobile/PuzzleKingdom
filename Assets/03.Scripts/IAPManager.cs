using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using Assets.Scripts.Configs;
using Assets.Scripts.GameManager;
using Assets.Scripts.Utils;

[System.Serializable]
public struct ProductPurchase
{
    public string productNameApple;
    public string productNameGooglePlay;
}

// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
public class IAPManager : MonoBehaviour, IStoreListener
{
    public static IAPManager Instance;
    private IStoreController is_controller = null;
    private IExtensionProvider is_extensions = null;

    [HideInInspector]
    public List<string> sProductIds = new List<string>(new string[] { "noads_199" ,"sweet_pack_399", "abundant_pack_999", "soft_pack_2399", "refreshing_pack_5999",
                                      "rich_pack_9999", "coin_299","coin_499","coin_999","coin_1999","coin_4999"});


    public List<string> price = new List<string>();

    private List<Dictionary<string, object>> global_data;      //튜토리얼 블럭 정보


    private void Awake()
    {
        Instance = this;
        global_data = CSVReader.Read("global");
    }

    void Start()
    {
        InitializePurchasing();
    }

    private bool IsInitialized()
    {
        return (is_controller != null && is_extensions != null);
    }

    public void InitializePurchasing()
    {
        Debug.Log("iap시작" + sProductIds.Count);
        if (IsInitialized())
            return;


        var module = StandardPurchasingModule.Instance();

        ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);

        foreach (var item in sProductIds)
        {
            builder.AddProduct(item, ProductType.Consumable, new IDs
            {
                { item, AppleAppStore.Name },
                { item, GooglePlay.Name },
            });
        }




        UnityPurchasing.Initialize(this, builder);


    }
    
    void IStoreListener.OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("초기화 성공");

        is_controller = controller;
        Debug.Log("is_controller");

        is_extensions = extensions;
        Debug.Log("is_extensions");


        foreach (var item in controller.products.all)
        {
            //Debug.Log("price " + item.metadata.localizedPrice.ToString());

                Find_Sing(item.metadata.isoCurrencyCode.ToString());

                price.Add(sign + " " + item.metadata.localizedPrice.ToString());   
        }

    }

    void IStoreListener.OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("초기화 실패");

    }

    public Dictionary<string, object> data;

    public void OnBtnPurchaseClicked(string index)
    {

        Debug.Log("결제 클릭   " + index);

        Debug.Log("안드로이드 시작   ");

        if (is_controller != null)
        {
            // Fetch the currency Product reference from Unity Purchasing
            Product product = is_controller.products.WithID(index);
            if (product != null && product.availableToPurchase)
            {
                is_controller.InitiatePurchase(product);
            }

        }
    }

    
    
    PurchaseProcessingResult IStoreListener.ProcessPurchase(PurchaseEventArgs e)
    {
        Debug.Log(e.purchasedProduct.definition.id);
        FireBaseManager.Instance.LogEvent("Shop_" + e.purchasedProduct.definition.id);

        Debug.Log((int)data["shop_type"]);

        Debug.Log("구매 완료");

       FindObjectOfType<Shop>().Buy(data);

        return PurchaseProcessingResult.Complete;
    }

    void IStoreListener.OnPurchaseFailed(Product i, PurchaseFailureReason error)
    {
        if (!error.Equals(PurchaseFailureReason.UserCancelled))
        {
            Debug.Log("구매 실패");

        }
    }

    string sign = "";

    /// <summary>
    /// 통화 기호 찾기
    /// </summary>
    public void Find_Sing(string code)
    {
        foreach (var item in global_data)
        {
            if (code.Equals(item["code"]))
            {
                sign = item["sign"].ToString();
                break;
            }

            sign = "$";

        }

    }
}
