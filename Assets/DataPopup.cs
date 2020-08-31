using Assets.Scripts.GameManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using UnityEngine.EventSystems;
using CloudOnce;
using Assets.Scripts.Utils;
#if UNITY_IOS
using UnityEngine.iOS;
#endif
public class DataPopup : MonoBehaviour
{

    public GameObject rote;

    public GameObject Btn_Ok;

    public GameObject btn_google;
    public GameObject btn_ios;

    private void Start()
    {
        EventTrigger eventTrigger = Btn_Ok.GetComponent<EventTrigger>();

        if (eventTrigger == null)
        {
            eventTrigger = Btn_Ok.gameObject.AddComponent<EventTrigger>();

        }

        EventTrigger.Entry entry_PointerDown = new EventTrigger.Entry();
        entry_PointerDown.eventID = EventTriggerType.PointerDown;
        entry_PointerDown.callback.AddListener((data) => { FindObjectOfType<MainScene>().Pointer_Down(Btn_Ok.transform); });
        eventTrigger.triggers.Add(entry_PointerDown);

        EventTrigger.Entry entry_PointerUp = new EventTrigger.Entry();
        entry_PointerUp.eventID = EventTriggerType.PointerUp;
        entry_PointerUp.callback.AddListener((data) => { FindObjectOfType<MainScene>().Pointer_Up(Btn_Ok.transform); });
        eventTrigger.triggers.Add(entry_PointerUp);

    }

    private void Update()
    {
        if (rote != null)
        rote.transform.Rotate(new Vector3(0, 0, -3));
    }

    public void Exit()
    {
        FireBaseManager.Instance.LogEvent("Main_Exit_OK");

        Application.Quit();
    }

    public void OnClickClose()
    {
        DialogManager.GetInstance().Close(null);
    }

    public void OnClickSave()
    {


        DialogManager.GetInstance().Close(null);


#if UNITY_ANDROID

        GoogleManager.Instance.isPopup = true;
        GoogleManager.Instance.Player_Data_Save();

#elif UNITY_IOS

        CloudOnceManager.Instance.isSave = false;

        CloudOnceManager.Instance.Saving();

        string jsonStr = JsonUtility.ToJson(DataManager.Instance.state_Player);
        string aes = AESCrypto.AESEncrypt128(jsonStr);

        CloudVariables.Player_Data = aes;

        Cloud.OnCloudSaveComplete += CloudOnceManager.Instance.CloudeSave;

        Cloud.Storage.Save();
#endif
    }


    public void Ser_Restore(bool success)
    {
        transform.Find("txt_load").GetComponent<LanguageComponent>().SetText(success ? "TXT_NO_50136" : "TXT_NO_50137");

    }
    public void Set_Google()
    {


#if UNITY_ANDROID

        transform.Find("txt_load").GetComponent<LanguageComponent>().SetText("TXT_NO_50131");
        btn_google.SetActive(true);
        btn_ios.SetActive(false);

#elif UNITY_IOS

        transform.Find("txt_load").GetComponent<LanguageComponent>().SetText("TXT_NO_50134");
        btn_google.SetActive(false);
        btn_ios.SetActive(true);

#endif


    }


    public void OnClickLoad()
    {

        DialogManager.GetInstance().Close(null);
#if UNITY_ANDROID
        GoogleManager.Instance.isPopup = true;
        GoogleManager.Instance.Player_Data_Load();

#elif UNITY_IOS

        CloudOnceManager.Instance.isSave = false;

        CloudOnceManager.Instance.Loading();

        Cloud.OnCloudLoadComplete += CloudOnceManager.Instance.CloudeLoad;

        Cloud.Storage.Load();
#endif


    }

    public void Google_Login()
    {
        CloudOnceManager.Instance.Login();
        DialogManager.GetInstance().Close(null);

    }

    public void Google_Logout()
    {
        CloudOnceManager.Instance.Logout();
        DialogManager.GetInstance().Close(null);

        DataManager.Instance.state_Player = new State_Player();
        DataManager.Instance.Save_Player_Data();

        Main obj = FindObjectOfType(typeof(Main)) as Main;
        obj.Reload();
    }

    public void OnClickFacebook()
    {
        FireBaseManager.Instance.LogEvent("Review");


#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.block.puzzle.puzzlego.number.puzzledom.Kingdom");
#elif UNITY_IOS
         Device.RequestStoreReview());
#endif
        DialogManager.GetInstance().Close(null);

    }


   
}
