using CloudOnce;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public enum itme
{
    coin
}

[Serializable]
public class State_Player
{
    public int LocalData_LanguageId = -1;                   //언어
    public int LocalData_guide_game01 = 0;                  //첫번째 게임 가이드
    public int LocalData_guide_game0102 = 0;                //첫번째 두번째 가이드
    public string LocalData_PreVedioTime = "nil";           //광고 타임
    public bool RemoveAds = false;                               //관고 유무
    public string LocalData_SignData = "-1";                //로그인 데이터
    public int LocalData_SerialLogin = 0;                   //로그인 유무
    public string LocalData_PreInsertTime = "nil";          //로컬 타임
    public string LocalData_LastLogin = "-1";               //마지막 로그인
    public int LocalData_NowStatus = 0;                     //지금 상태
    public string LocalData_PreSubscriptionTime = "-1";     //서브 타임
    public string LocalData_Timer = "-1";                   //로컬 타이머
    public int LocalData_guide_game02 = 0;                  //두번쨰 게임 가이드
    public int AdsCounter = 0;                              //광고 카운트
    public int LocalData_Music = 1;                         //뮤직 온오프
    public int LocalData_Effect = 1;                        //이펙트 온오프
    public int LocalData_Diamond = 0;                       //다이아몬드
    public int LocalData_Exp = 0;                           //경험치
    public int LocalData_Lv = 1;                            //레벨
    public int LocalData_GameId = 0;                        //게임 아이디
    public string LocalData_OldGame = "";                   //옛날 게임
    public string LocalData_OldLife = "";                   //옛날 라이프
    public float LocalData_OldPosX = 0;                     //옛날 포지션 x
    public float LocalData_OldPosY = 0;                     //옛날 포지션 y
    public string LocalData_OldScore = "0,0,0";             //옛날 스코어
    public string LocalData_Record_Score = "0,0,0";         //베스트 스코어
    public int LocalData_FirstGame = 0;                     //첫번째 게임
    public int LocalData_IsFirstFinish = 0;                 //첫번째 끝 게임
    public int LocalData_SkinID = 1;                        //스킨 아이디
    public string LocalData_InitTime = "-1";                //인스톨 타임
    public string LocalData_SkinData = "0,1";               //스킨 데이터
    public string LocalData_SkinFreeTime = "-1,-1";         //스킨 타임
    public int LocalData_FirstShare = 0;                    //첫번째 공유
    public string LocalData_Shop_Time = "-1";               //스킨 타임
    public string LocalData_Game_Time = "-1";               //스킨 타임
    public string LocalData_Main_Time = "-1";               //스킨 타임


    public List<LocalData_G003_Record_Score> localData_G003_Record_Scores = new List<LocalData_G003_Record_Score>(); //3게임 베스트 스코어
    public List<Achive_localdata> achive_Localdatas = new List<Achive_localdata>(); //업적
    public List<Task_localdata> task_Localdatas = new List<Task_localdata>(); //일일 퀘스트
    public Item_localdata item_Localdata = new Item_localdata();

}

[Serializable]
public class LocalData_G003_Record_Score
{
    public string _Id = "";                        //아이디
    public string val = "";                        //정보 
}

[Serializable]
public class Achive_localdata
{
    public string _Id = "";                        //아이디
    public string val = "";                        //정보 
}

[Serializable]
public class Task_localdata
{
    public string _Id = "";                        //아이디
    public string val = "";                        //정보 
}

[Serializable]
public class Item_localdata
{
    public int Boom = 0;                        //아이디
    public int Hammer = 0;                        //아이디
    public int Star = 0;                        //아이디
    public int Hint = 0;                        //아이디
}

public class DataManager : MonoBehaviour
{

    public static DataManager Instance;

    public State_Player state_Player = new State_Player();      //플레이어 정보

    private readonly string PlayerDataName = "/Player.dat";

    public List<Dictionary<string, object>> language_data;      //튜토리얼 블럭 정보

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        Get_Csv_Data();
        Get_Json_Data();

    }

    /// <summary>
    /// csv 데이터 가져오기
    /// </summary>
    void Get_Csv_Data()
    {
        language_data = CSVReader.Read("language");

    }

    /// <summary>
    /// json 데이터 가져오기
    /// </summary>
    public void Get_Json_Data()
    {

        if (PlayerPrefs.GetInt("Load",0).Equals(0))
        {
            CheckPlayer();
            PlayerPrefs.SetInt("Load", 1);
        }

        state_Player = Load_Player();

        if (state_Player.LocalData_LanguageId == -1)
        {
            state_Player.LocalData_LanguageId = (int)Application.systemLanguage;
            Save_Player_Data();
        }
    }

    /// <summary>
    /// 플레이어 데이터 저장
    /// </summary>
    public void Save_Player_Data()
    {
        Save(state_Player);

#if UNITY_ANDROID
        GoogleManager.Instance.isPopup = false;
        GoogleManager.Instance.Player_Data_Save();
#elif UNITY_IOS

        CloudOnceManager.Instance.isSave = false;

        string jsonStr = JsonUtility.ToJson(DataManager.Instance.state_Player);
        string aes = AESCrypto.AESEncrypt128(jsonStr);

        CloudVariables.Player_Data = aes;

        Cloud.OnCloudSaveComplete += CloudOnceManager.Instance.CloudeSave;

        Cloud.Storage.Save();
#endif

    }


    public bool CheckPlayer()
    {
        string GameInfoPath = Application.persistentDataPath + PlayerDataName;
        System.IO.File.Delete(GameInfoPath);

        Debug.Log("처음접속 데이터 삭제");
        return false;
    }
#region SaveData


    /// <summary>
    /// 집 정보 저장 하기
    /// </summary>
    /// <param name="SaveData"></param>
    public void Save(State_Player SaveData)
    {
        string GameInfoPath = Application.persistentDataPath + PlayerDataName;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(GameInfoPath);

        string jsonStr = JsonUtility.ToJson(SaveData);
        string aes = AESCrypto.AESEncrypt128(jsonStr);
        bf.Serialize(file, aes);
        file.Close();
    }

#endregion SaveData

#region LoadData


    /// <summary>
    /// 자기 정보 가져오기
    /// </summary>
    /// <returns></returns>
    public State_Player Load_Player()
    {
        string InfoPath = Application.persistentDataPath + PlayerDataName;
        State_Player playerInfoSave = new State_Player();

        if (File.Exists(InfoPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(InfoPath, FileMode.Open);
            var str = (string)bf.Deserialize(file);
            file.Close();

            if (!string.IsNullOrEmpty(str))
            {
                string aes = AESCrypto.AESDecrypt128(str);

                var data = JsonUtility.FromJson<State_Player>(aes);

                playerInfoSave = data;
            }
        }
        else
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(InfoPath);
            State_Player data = new State_Player();

            playerInfoSave = data;

            string jsonStr = JsonUtility.ToJson(playerInfoSave);
            string aes = AESCrypto.AESEncrypt128(jsonStr);

            bf.Serialize(file, aes);
            file.Close();
        }

        return playerInfoSave;
    }

#endregion LoadData
}
