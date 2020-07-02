using Assets.Scripts.GameManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using UnityEngine.EventSystems;
using CloudOnce;
using Assets.Scripts.Utils;

public class DataPopup : MonoBehaviour
{

    public GameObject rote;

    public GameObject Btn_Ok;


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
        GoogleManager.Instance.Player_Data_Save();

#elif UNITY_IOS

        GoogleManager.Instance.isSave = false;

        GoogleManager.Instance.Saving();

        string jsonStr = JsonUtility.ToJson(DataManager.Instance.state_Player);
        string aes = AESCrypto.AESEncrypt128(jsonStr);

        CloudVariables.Player_Data = aes;

        Cloud.OnCloudSaveComplete += GoogleManager.Instance.CloudeSave;

        Cloud.Storage.Save();
#endif
    }


 
    public void OnClickLoad()
    {

        DialogManager.GetInstance().Close(null);
#if UNITY_ANDROID
        GoogleManager.Instance.Player_Data_Load();

#elif UNITY_IOS

        GoogleManager.Instance.isSave = false;

        GoogleManager.Instance.Loading();

        Cloud.OnCloudLoadComplete += GoogleManager.Instance.CloudeLoad;

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


        Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
        DialogManager.GetInstance().Close(null);

    }


   
}
