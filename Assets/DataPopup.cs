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

    bool isSave = false;

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

        isSave = false;

        StartCoroutine(Co_Saving());


        Cloud.OnCloudSaveComplete += CloudeSave;

        Cloud.Storage.Save();
#endif
    }


    public void CloudeSave(bool success)
    {

        Debug.Log(success ? "저장 성공" : "저장 실패");


        isSave = true;

        Cloud.OnCloudSaveComplete -= CloudeSave;

    }

    IEnumerator Co_Saving()
    {
        GameObject obj = null;

        obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/data_saveing") as GameObject);
        DialogManager.GetInstance().show(obj, false);

        yield return new WaitForSeconds(2f);

        while (true)
        {
            if (isSave)
            {
                DialogManager.GetInstance().Close(null);

                obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/data_save_confirm") as GameObject);
                DialogManager.GetInstance().show(obj, false);

                yield return null;

            }
        }

    }

    public void CloudeLoad(bool success)
    {
        Cloud.OnCloudLoadComplete -= CloudeLoad;

        if (!success)
            return;

        isSave = true;

        string str = CloudVariables.Player_Data;

        Debug.Log(success ? "로드 성공 " + str : "로드 실패");

        if (str != "")
        {

            var aes = AESCrypto.AESDecrypt128(str);
            var data = JsonUtility.FromJson<State_Player>(aes);

            DataManager.Instance.state_Player = data;

            DataManager.Instance.Save_Player_Data();

            Language.GetInstance().Set((SystemLanguage)DataManager.Instance.state_Player.LocalData_LanguageId);

            Main obj = FindObjectOfType(typeof(Main)) as Main;
            obj.Reload();

        }


    }

    public void OnClickLoad()
    {

        DialogManager.GetInstance().Close(null);
#if UNITY_ANDROID
        GoogleManager.Instance.Player_Data_Load();

#elif UNITY_IOS

        isSave = false;

        StartCoroutine(Co_Saving());

        Cloud.OnCloudLoadComplete += CloudeLoad;

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


    IEnumerator Co_Loading()
    {
        GameObject obj = null;

        obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/data_loading") as GameObject);
        DialogManager.GetInstance().show(obj, false);

        yield return new WaitForSeconds(2f);


        while (true)
        {
            if (isSave)
            {
                DialogManager.GetInstance().Close(null);

                obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/data_load_confirm") as GameObject);
                DialogManager.GetInstance().show(obj, false); yield return null;

            }
        }

 
    }
}
