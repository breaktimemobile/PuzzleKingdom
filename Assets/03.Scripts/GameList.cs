using Assets.Scripts.Configs;
using Assets.Scripts.GameManager;
using Assets.Scripts.Utils;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class GameList : MonoBehaviour
{
    public static GameList Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
    }


    [Serializable]
    private sealed class __c
    {
        public static readonly GameList.__c __9 = new GameList.__c();

        public static Action __9__20_1;

        internal void _Update_b__20_1()
        {
            Utils.ExitGame();
        }
    }

    public GameObject gamelist;

    public GameObject content;

    public Dictionary<int, GameObject> m_gameDict = new Dictionary<int, GameObject>();

    public Text m_txt_maxScore01;

    public Text m_txt_maxScore02;

    public Text m_txt_maxScore03;

    private void Start()
    {
      
    }

    public void init()
    {
        this.Init();
        this.InitEvent();
        if (GM.GetInstance().isFirstGame())
        {
            GM.GetInstance().SetFristGame();

        }
    }

    private void Update()
    {
        Utils.BackListener(base.gameObject, delegate
        {
            switch (PlayerPrefs.GetInt("MyGame", 0))
            {
                case 1:
                    if (Game1DataLoader.GetInstance() != null)
                    {
                        if (!Game1DataLoader.GetInstance().IsPlaying)
                        {
                            switch (Game1DataLoader.GetInstance().CurPropId)
                            {
                                case 1:
                                    Debug.Log("아이템 1");
                                    Game1DataLoader.GetInstance().CurPropId = 0;
                                    Game1Manager.GetInstance().ControlPropsPannel(true);
                                    return;

                                case 2:
                                    Debug.Log("아이템 2");
                                    Game1DataLoader.GetInstance().CurPropId = 0;
                                    Game1Manager.GetInstance().ControlPropsPannel(true);
                                    return;

                                case 3:
                                    Debug.Log("아이템 3");
                                    Game1DataLoader.GetInstance().CurPropId = 0;
                                    Game1Manager.GetInstance().ControlPropsPannel(true);
                                    return;
                            }
                        }
                    }
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }

            if (GM.GetInstance().GameId != 0)
            {

                this.OnClickReturn();
                return;
            }

            Action arg_39_0;
            if ((arg_39_0 = GameList.__c.__9__20_1) == null)
            {
                arg_39_0 = (GameList.__c.__9__20_1 = new Action(GameList.__c.__9._Update_b__20_1));
            }

            Utils.ShowConfirmOrCancel(arg_39_0, null, "TXT_NO_50025", true);
        });
    }

    public void Pause_Return()
    {
        switch (PlayerPrefs.GetInt("MyGame", 0))
        {
            case 1:
                if (Game1DataLoader.GetInstance() != null)
                {
                    if (!Game1DataLoader.GetInstance().IsPlaying)
                    {
                        switch (Game1DataLoader.GetInstance().CurPropId)
                        {
                            case 1:
                                Debug.Log("아이템 1");
                                Game1DataLoader.GetInstance().CurPropId = 0;
                                Game1Manager.GetInstance().ControlPropsPannel(true);
                                return;

                            case 2:
                                Debug.Log("아이템 2");
                                Game1DataLoader.GetInstance().CurPropId = 0;
                                Game1Manager.GetInstance().ControlPropsPannel(true);
                                return;

                            case 3:
                                Debug.Log("아이템 3");
                                Game1DataLoader.GetInstance().CurPropId = 0;
                                Game1Manager.GetInstance().ControlPropsPannel(true);
                                return;
                        }
                    }
                }
                break;
            case 2:
                break;
            case 3:
                break;
        }

        if (GM.GetInstance().GameId != 0)
        {
            this.OnClickReturn();
            return;
        }

        Action arg_39_0;
        if ((arg_39_0 = GameList.__c.__9__20_1) == null)
        {
            arg_39_0 = (GameList.__c.__9__20_1 = new Action(GameList.__c.__9._Update_b__20_1));
        }

        //Utils.ShowConfirmOrCancel(arg_39_0, null, "TXT_NO_50025", true);
    }

    public void Setting_Return()
    {
        switch (PlayerPrefs.GetInt("MyGame", 0))
        {
            case 1:
                if (Game1DataLoader.GetInstance() != null)
                {
                    if (!Game1DataLoader.GetInstance().IsPlaying)
                    {
                        switch (Game1DataLoader.GetInstance().CurPropId)
                        {
                            case 1:
                                Debug.Log("아이템 1");
                                Game1DataLoader.GetInstance().CurPropId = 0;
                                Game1Manager.GetInstance().ControlPropsPannel(true);
                                return;

                            case 2:
                                Debug.Log("아이템 2");
                                Game1DataLoader.GetInstance().CurPropId = 0;
                                Game1Manager.GetInstance().ControlPropsPannel(true);
                                return;

                            case 3:
                                Debug.Log("아이템 3");
                                Game1DataLoader.GetInstance().CurPropId = 0;
                                Game1Manager.GetInstance().ControlPropsPannel(true);
                                return;
                        }
                    }
                }
                break;
            case 2:
                break;
            case 3:
                break;
        }

        if (GM.GetInstance().GameId != 0)
        {

            FindObjectOfType<MainScene>().OnClickSetting();
            return;
        }

        Action arg_39_0;
        if ((arg_39_0 = GameList.__c.__9__20_1) == null)
        {
            arg_39_0 = (GameList.__c.__9__20_1 = new Action(GameList.__c.__9._Update_b__20_1));
        }

        Utils.ShowConfirmOrCancel(arg_39_0, null, "TXT_NO_50025", true);
    }


    public void Shop_Return()
    {
        switch (PlayerPrefs.GetInt("MyGame", 0))
        {
            case 1:
                if (Game1DataLoader.GetInstance() != null)
                {
                    if (!Game1DataLoader.GetInstance().IsPlaying)
                    {
                        switch (Game1DataLoader.GetInstance().CurPropId)
                        {
                            case 1:
                                Debug.Log("아이템 1");
                                Game1DataLoader.GetInstance().CurPropId = 0;
                                Game1Manager.GetInstance().ControlPropsPannel(true);
                                return;

                            case 2:
                                Debug.Log("아이템 2");
                                Game1DataLoader.GetInstance().CurPropId = 0;
                                Game1Manager.GetInstance().ControlPropsPannel(true);
                                return;

                            case 3:
                                Debug.Log("아이템 3");
                                Game1DataLoader.GetInstance().CurPropId = 0;
                                Game1Manager.GetInstance().ControlPropsPannel(true);
                                return;
                        }
                    }
                }
                break;
            case 2:
                break;
            case 3:
                break;
        }

        if (GM.GetInstance().GameId != 0)
        {

            FindObjectOfType<MainScene>().OnclickShop();
            return;
        }

        Action arg_39_0;
        if ((arg_39_0 = GameList.__c.__9__20_1) == null)
        {
            arg_39_0 = (GameList.__c.__9__20_1 = new Action(GameList.__c.__9._Update_b__20_1));
        }

        Utils.ShowConfirmOrCancel(arg_39_0, null, "TXT_NO_50025", true);
    }

    private void OnEnable()
    {
        //this.PlayRecordAni();
    }

    private void OnDestroy()
    {
        GlobalEventHandle.DoGoHome -= new Action(this.Init);
    }

    public void OnClickStartGame(int id)
    {
        GM.GetInstance().GameId = id;

        AudioManager.GetInstance().PlayEffect("sound_eff_button");
        this.gamelist.SetActive(false);
        this.content.SetActive(true);
        this.LoadGame(id, 0, true);
    }

    public void OnClickAds()
    {
        //AdsManager.GetInstance().Play(AdsManager.AdType.Stimulate, null, null, 5, null);

        //AdsControl.Instance.ShowRewardedAd();
        //GM.GetInstance().AddDiamond(5, true);
    }

    public void OnClickReturn()
    {
        if (GM.GetInstance().GameId == 0)
        {
            GlobalEventHandle.EmitDoGoHome();
            GlobalEventHandle.EmitClickPageButtonHandle("main", 0);
            return;
        }

        if (FindObjectOfType<Pause>() != null)
            return;

        switch (GM.GetInstance().GameId)
        {
            case 1:
                {

                    Action<GameList> expr_36 = Game1DataLoader.GetInstance().OnClickReturnHandle;
                    if (expr_36 == null)
                    {
                        return;
                    }
                    expr_36(this);
                    return;
                }
            case 2:
                {

                    Action<GameList> expr_4C = G2BoardGenerator.GetInstance().OnClickReturnHandle;
                    if (expr_4C == null)
                    {
                        return;
                    }
                    expr_4C(this);
                    return;
                }
            case 3:
                {

                    Action<GameList> expr_62 = G3BoardGenerator.GetInstance().OnClickReturnHandle;
                    if (expr_62 == null)
                    {
                        return;
                    }
                    expr_62(this);
                    return;
                }
            default:
                return;
        }


    }

    public void LoadGame(int id, int value = 0, bool isPageIn = true)
    {
        //this.PlayRecordAni();
        if (AdsManager.GetInstance().IsWatch)
        {
            //this.m_videoTimer.GetComponent<LanguageComponent>().SetText("TXT_NO_20018");
        }
        if (AdsManager.GetInstance().IsWatch)
        {
        }

        if (id == 0)
        {
            return;
        }
        foreach (KeyValuePair<int, GameObject> current in this.m_gameDict)
        {
            current.Value.SetActive(false);
        }
        if (this.m_gameDict.ContainsKey(id))
        {
            this.m_gameDict[id].SetActive(true);
            switch (id)
            {
                case 1:
                    FireBaseManager.Instance.LogEvent("Puzzle_Mix_Start");
                    Game1DataLoader.GetInstance().StartNewGame();
                    break;
                case 2:
                    FireBaseManager.Instance.LogEvent("2048_Start");
                    G2BoardGenerator.GetInstance().StartNewGame();
                    break;
                case 3:
                    FireBaseManager.Instance.LogEvent("Puzzle_Line_Start");

                    G3BoardGenerator.GetInstance().StartNewGame(value);
                    break;
            }
        }
        else
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>
            {
                {
                    1,
                    "Prefabs/G001"
                },
                {
                    2,
                    "Prefabs/G002"
                },
                {
                    3,
                    "Prefabs/G00301"
                }
            };
            if (!dictionary.ContainsKey(id))
            {
                return;
            }
            GameObject gameObject = Resources.Load(dictionary[id]) as GameObject;
            gameObject = UnityEngine.Object.Instantiate<GameObject>(gameObject);
            gameObject.transform.SetParent(this.content.transform, false);
            if (id == 3 && value != 0)
            {
                G3BoardGenerator.GetInstance().StartNewGame(value);
            }
            this.m_gameDict.Add(id, gameObject);
        }
        if (isPageIn)
        {
            this.PlayGameIn();
        }
        //AppsflyerUtils.TrackPlayGame(id);
    }
    
    private void Init()
    {
        bool flag = GM.GetInstance().isSavedGame();
        this.gamelist.SetActive(!flag);
        this.content.SetActive(flag);
        this.LoadGame(GM.GetInstance().GetSavedGameID(), GM.GetInstance().GetScore(3), false);
        this.RefreshMaxScore(new string[]
        {
            GM.GetInstance().GetScoreRecord(1).ToString(),
            GM.GetInstance().GetScoreRecord(2).ToString(),
            GM.GetInstance().GetScoreRecord(3).ToString()
        });
    }

    private void InitEvent()
    {

        if (GlobalEventHandle.Action_Bool())
        {
            GlobalEventHandle.DoGoHome += new Action(this.Init);
            //GlobalEventHandle.OnRefreshAchiveHandle = (Action<int>)Delegate.Combine(GlobalEventHandle.OnRefreshAchiveHandle, new Action<int>(this.RefreshRecord));
            GlobalEventHandle.OnRefreshMaxScoreHandle = (Action<string[]>)Delegate.Combine(GlobalEventHandle.OnRefreshMaxScoreHandle, new Action<string[]>(this.RefreshMaxScore));

        }

    }

    private void PlayGameIn()
    {
        if (this.content == null)
        {
            return;
        }
        this.content.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        Sequence expr_38 = DOTween.Sequence();
        expr_38.Append(this.content.transform.DOScale(1.1f, 0.3f));
        expr_38.Append(this.content.transform.DOScale(1f, 0.1f));
    }
    
    public void RefreshMaxScore(string[] array)
    {
        this.m_txt_maxScore01.text = array[0];
        this.m_txt_maxScore02.text = array[1];
        if (Configs.TG00301.ContainsKey(array[2]))
        {
            this.m_txt_maxScore03.text = string.Format("{0}/300", Configs.TG00301[array[2]].Level);
            return;
        }
        this.m_txt_maxScore03.text = string.Format("{0}/300", 0);
    }
    
}
