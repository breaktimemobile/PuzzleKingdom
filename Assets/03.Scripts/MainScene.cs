using Assets.Scripts.Configs;
using Assets.Scripts.GameManager;
using Assets.Scripts.Utils;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class colors
{
    public List<int> color_num = new List<int>();
}

/*
 * This scrips will show information for UI in main scene
 */
public class MainScene : MonoBehaviour
{

    private Dictionary<string, GameObject> nodes = new Dictionary<string, GameObject>();

    private string curBtnType = "main";

    public GameObject txt_lv;

    public GameObject img_exp;

    public GameObject icon_gem;

    public Text txt_ads_timer;

    public GameObject txt_diamond;

    public GameObject panel_cotent;

    public GameObject panel_handle;

    public GameObject panel_top;

    public GameObject panel_maxTop;

    public GameObject gamelist;

    public GameObject content;

    public PageView gameview;

    public GameList gameContral;

    public Button btn_ads;
    public Text txt_timer;

    public Dictionary<string, Vector2> m_nodesPositions = new Dictionary<string, Vector2>();

    public GameObject m_splash;

    public Sprite[] itme_sp;

    private string[] block_colors = { "#955113", "#FDD602", "#51FBDF", "#C61AFF", "#FD81C4",
        "#FB2B07", "#E01EC6", "#F78C00", "#F5600F", "#1BC3A3" , "#32C31B" , "#0C64FD" , "#428DFF" , "#999999"  };

    //[System.Serializable]
    public List<colors> color_num = new List<colors>();

    public int music_sound;

    public Transform[] icons;

    public void Init()
    {

        this.LoadData();
        this.InitUI();
        this.InitEvent();
        //this.RunSplash();
        Set_Timer();
        Open_Icon();
    }

    public void Pointer_Down(Transform isbtn)
    {
        isbtn.DOKill();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(isbtn.DOScale(0.7f, 0.1f));
        sequence.Append(isbtn.DOScale(0.8f, 0.1f));
        sequence.Append(isbtn.DOScale(0.7f, 0.1f));
        sequence.Append(isbtn.DOScale(0.75f, 0.1f));
        sequence.Append(isbtn.DOScale(0.7f, 0.1f));
        sequence.Append(isbtn.DOScale(0.75f, 0.1f));
        sequence.Append(isbtn.DOScale(0.7f, 0.1f));

    }

    public void Pointer_Up(Transform isbtn)
    {
        isbtn.DOKill();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(isbtn.DOScale(1.0f, 0.1f));
        sequence.Append(isbtn.DOScale(1.1f, 0.1f));
        sequence.Append(isbtn.DOScale(1.0f, 0.1f));
        sequence.Append(isbtn.DOScale(1.05f, 0.1f));
        sequence.Append(isbtn.DOScale(1.0f, 0.1f));
        sequence.Append(isbtn.DOScale(1.05f, 0.1f));
        sequence.Append(isbtn.DOScale(1.0f, 0.1f));
    }

    private void OnDestroy()
    {
        this.DestroyEvent();
    }

    public void Close_Icon()
    {
        foreach (var item in icons)
        {
            item.DOScale(0, 0.5f).SetEase(Ease.OutBack);
        }
    }

    public void Open_Icon()
    {
        foreach (var item in icons)
        {
            item.localScale = Vector3.zero;

            item.DOScale(1, 0.5f).SetEase(Ease.OutBounce);
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {

            GameList.Instance.Pause_Return();

            GlobalTimer.GetInstance().TrackMiniTime();
            GlobalTimer.GetInstance().TrackTotalTime();
            AdsManager.GetInstance().SaveInsertTime();
            UnityEngine.Debug.Log("game pause ................");
            return;
        }
        AdsUtil.LoadRewardAds();
        AdsManager.GetInstance().PlayTransiformGroundAds();
        UnityEngine.Debug.Log("game continue .............");
    }

    private void OnApplicationQuit()
    {
        GlobalTimer.GetInstance().TrackMiniTime();
        GlobalTimer.GetInstance().TrackTotalTime();
        UnityEngine.Debug.Log("game quit .............");
    }

    public void OnClickStartGame(int id = 0)
    {
        Debug.Log("id " + id);
        PlayerPrefs.SetInt("conti", 0);

        int num = (new int[]
        {
            2,
            1,
            3,
            0
        })[this.gameview.PageIdx];

        PlayerPrefs.SetInt("MyGame", gameview.PageIdx);

        GM.GetInstance().GameId = num;
        if (num == 0)
        {
            return;
        }
        if (num == 1)
        {

            //GlobalEventHandle.EmitClickPageButtonHandle("G00106", 0);
            //return;

            if (DataManager.Instance.state_Player.LocalData_guide_game01 == 0)
            {
                PlayerPrefs.SetInt("BoardSize", 5);

                GlobalEventHandle.EmitClickPageButtonHandle("G00103", 0);
                return;
            }


        }
        else if (num == 3 && id == 0)
        {
            FireBaseManager.Instance.LogEvent("Puzzle_Line_Check_Star");

            Close_Icon();
            GlobalEventHandle.EmitClickPageButtonHandle("G003", 0);
            //GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/G003") as GameObject);
            //DialogManager.GetInstance().show(obj, false);
            return;
        }


        this.gamelist.SetActive(false);
        this.content.SetActive(true);
        this.gameContral.gameObject.SetActive(true);
        this.gameContral.LoadGame(num, id, true);
        AudioManager.GetInstance().PlayBgMusic("sound_ingame", true);
    }

    public void Game1Play()
    {


        this.gamelist.SetActive(false);
        this.content.SetActive(true);
        this.gameContral.gameObject.SetActive(true);
        this.gameContral.LoadGame(1, 0, true);
        AudioManager.GetInstance().PlayBgMusic("sound_ingame", true);
    }

    public Color Get_Block_Color(int num)
    {
        Debug.Log(num);
        Color color = Color.white;

        int color_pick = 0;


        for (int i = 0; i < color_num.Count; i++)
        {
            if (color_num[i].color_num.Exists(x => x == num))
            {
                color_pick = i;
                break;
            }
        }

        Debug.Log(block_colors[color_pick]);

        ColorUtility.TryParseHtmlString(block_colors[color_pick], out color);
        return color;
    }

    public void onClickBottom(string type)
    {
        Debug.Log("type " + type);

        string text = type;
        if (text.Equals("shop02"))
        {
            type = "shop";
        }

        if (!this.curBtnType.Equals(type))
        {
            this.curBtnType = type;
            foreach (KeyValuePair<string, GameObject> current in this.nodes)
            {
                if (current.Value != null)
                {
                    current.Value.SetActive(false);
                }
            }
            if (this.nodes[type] == null)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>
                {
                    {
                        "shop",
                        "shop"
                    },
                    {
                        "main",
                        "gamelist"
                    },
                    {
                        "achive",
                        "achive"
                    },
                    {
                        "task",
                        "task"
                    },
                    {
                        "skin",
                        "skin"
                    },
                    {
                        "activity",
                        "activity"
                    },
                    {
                        "setting",
                        "setting"
                    },
                    {
                        "G00103",
                        "G00103"
                    },
                    {
                        "G003",
                        "G003"
                    },
                    {
                        "G00106",
                        "G00106"
                    }
                };
                GameObject gameObject = Resources.Load("Prefabs/" + dictionary[type]) as GameObject;
                if (gameObject != null)
                {
                    GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                    gameObject2.transform.SetParent(this.panel_cotent.transform, false);
                    gameObject2.name = type;
                    this.nodes[type] = gameObject2;
                }
            }
            else
            {
                this.nodes[type].SetActive(true);
            }

            AudioManager.GetInstance().PlayEffect("sound_eff_button");
            return;
        }
        if (!type.Equals("main"))
        {
            return;
        }
        if (!this.nodes[type].activeSelf)
        {
            return;
        }
        GlobalEventHandle.EmitDoGoHome();
    }

    public void OnClickSetting()
    {
        switch (PlayerPrefs.GetInt("MyGame", 0))
        {
            case 1:

                break;
            case 0:

                if (G2BoardGenerator.GetInstance() != null)
                {
                    G2BoardGenerator.GetInstance().IsPuase = true;

                }

                break;
            case 2:
                break;
        }

        GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/setting") as GameObject);
        DialogManager.GetInstance().show(obj, false);


    }

    public void Main_shop()
    {
        GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/shop") as GameObject);
        DialogManager.GetInstance().show(obj, false);
    }

    public void Main_task()
    {
        GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/task") as GameObject);
        DialogManager.GetInstance().show(obj, false);
    }

    public void Main_achive()
    {
        GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/achive") as GameObject);
        DialogManager.GetInstance().show(obj, false);
    }
    public void OnclickShop()
    {
        switch (PlayerPrefs.GetInt("MyGame", 0))
        {
            case 1:

                break;
            case 0:

                if (G2BoardGenerator.GetInstance() != null)
                {
                    G2BoardGenerator.GetInstance().IsPuase = true;

                }

                break;
            case 2:
                break;
        }


        GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/shop") as GameObject);
        DialogManager.GetInstance().show(obj, false);
    }

    public void OnClickExit()
    {
        FireBaseManager.Instance.LogEvent("Main_Exit");

        GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/exit") as GameObject);
        DialogManager.GetInstance().show(obj, false);

    }

    public void OnClickExpButton(int value)
    {
        PlayerPrefs.DeleteAll();
    }

    private void Update()
    {

        TimeSpan LateTime = GiftTime - DateTime.Now;

        if (LateTime.TotalSeconds <= 0)
        {
            btn_ads.interactable = true;
            txt_timer.GetComponent<LanguageComponent>().SetText("TXT_NO_20018");

        }
        else
        {
            btn_ads.interactable = false;

            int diffMiniute = LateTime.Minutes; //30
            int diffSecond = LateTime.Seconds; //0

            txt_timer.text = string.Format("{0:00}:{1:00}", diffMiniute, diffSecond);

        }
    }

    public void Set_Timer()
    {
        if (DataManager.Instance.state_Player.LocalData_Main_Time == "-1")
        {
            btn_ads.interactable = true;
            txt_timer.GetComponent<LanguageComponent>().SetText("TXT_NO_20018");

        }
        else
        {
            btn_ads.interactable = false;

            txt_timer.gameObject.SetActive(true);

            GiftTime = DateTime.Parse(DataManager.Instance.state_Player.LocalData_Main_Time);
        }
    }

    DateTime GiftTime;

    public void onClickAds()
    {
        FireBaseManager.Instance.LogEvent("Main_Ads");
        AdsControl.Instance.reward_Type = Reward_Type.stimulate;
        AdsControl.Instance.ShowRewardedAd();

    }

    public void ClearLocalData()
    {
        PlayerPrefs.DeleteAll();
        GM.GetInstance().Init();
        this.InitUI();
    }

    private void LoadData()
    {
        nodes.Clear();

        this.nodes.Add("shop", null);
        this.nodes.Add("main", base.transform.Find("panel_content/gamelist").gameObject);
        this.nodes.Add("achive", null);
        this.nodes.Add("task", null);
        this.nodes.Add("skin", null);
        this.nodes.Add("activity", null);
        this.nodes.Add("setting", null);
        this.nodes.Add("G00103", null);
        this.nodes.Add("G003", null);
        this.nodes.Add("G00106", null);

    }

    private void InitUI()
    {
        if (AdsManager.GetInstance().IsWatch)
        {
            this.txt_ads_timer.GetComponent<LanguageComponent>().SetText("TXT_NO_20018");
        }
        this.LoadDiamondUI();
        this.LoadExpUI();
        this.PlayGENTipAni();

    }

    private void InitEvent()
    {

        if (this.gameview.OnClickHandle == null)
        {
            Debug.Log("핸들러 추가");
            GlobalEventHandle.GetDiamondHandle += new Action<int, bool>(this.OnGetDiamond);
            GlobalEventHandle.ConsumeDiamondHandle += new Action<int>(this.OnConsumeGEM);
            GlobalEventHandle.AddExpHandle += new Action<bool>(this.OnAddExp);
            GlobalEventHandle.DoClickBottom += new Action(this.OnDoClickBottomButton);
            GlobalEventHandle.OnClickPageButtonHandle += new Action<string, int>(this.OnClickPageButton);
            GlobalEventHandle.DoUseProps += new Action<bool, List<GameObject>>(this.OnDoUseProps);
            GlobalEventHandle.AdsHandle += new Action<string, bool>(this.OnRefreshAdsTimer);
            GoodsManager.GetInstance().ShowSubscriptionHanle += new Action<int, int>(this.ShowSubscriptionAwards);
            this.gameview.OnClickHandle = new Action<int>(this.OnClickStartGame);
        }

    }

    private void DestroyEvent()
    {
        GlobalEventHandle.GetDiamondHandle -= new Action<int, bool>(this.OnGetDiamond);
        GlobalEventHandle.ConsumeDiamondHandle -= new Action<int>(this.OnConsumeGEM);
        GlobalEventHandle.AddExpHandle -= new Action<bool>(this.OnAddExp);
        GlobalEventHandle.DoClickBottom -= new Action(this.OnDoClickBottomButton);
        GlobalEventHandle.DoUseProps -= new Action<bool, List<GameObject>>(this.OnDoUseProps);
        GlobalEventHandle.AdsHandle -= new Action<string, bool>(this.OnRefreshAdsTimer);
        GoodsManager.GetInstance().ShowSubscriptionHanle -= new Action<int, int>(this.ShowSubscriptionAwards);
    }

    private void LoadDiamondUI()
    {
        this.txt_diamond.GetComponent<OverlayNumber>().SetStartNumber(GM.GetInstance().Diamond);
    }

    private void LoadExpUI()
    {
        this.txt_lv.GetComponent<Text>().text = GM.GetInstance().Lv.ToString();
        if (Configs.TPlayers.ContainsKey(GM.GetInstance().Lv.ToString()))
        {
            TPlayer tPlayer = Configs.TPlayers[GM.GetInstance().Lv.ToString()];
            this.img_exp.GetComponent<Image>().fillAmount = (((float)GM.GetInstance().Exp / (float)tPlayer.Exp >= 1f) ? 1f : ((float)GM.GetInstance().Exp / (float)tPlayer.Exp));
        }

        Game2Manager.GetInstance()?.set_lv();
        Game1Manager.GetInstance()?.set_lv();
        G3BoardManager.GetInstance()?.set_lv();

        FindObjectOfType<G3BoardManager>()?.set_lv();

    }

    //다이아몬드 초기화
    private void OnGetDiamond(int num, bool isPlayAni)
    {
        if (isPlayAni)
        {
            int toNum = GM.GetInstance().Diamond;
            this.PlayGEMAni(delegate
            {
                this.txt_diamond.GetComponent<OverlayNumber>().setNum(toNum);
                Game2Manager.GetInstance()?.txt_gold.GetComponent<OverlayNumber>().setNum(toNum);
                Game1Manager.GetInstance()?.txt_gold.GetComponent<OverlayNumber>().setNum(toNum);
                FindObjectOfType<Shop>()?.txt_gold.GetComponent<OverlayNumber>().setNum(toNum);
                FindObjectOfType<G3BoardManager>()?.txt_gold.GetComponent<OverlayNumber>().setNum(toNum);
                FindObjectOfType<Achive>()?.txt_gold.GetComponent<OverlayNumber>().setNum(toNum);
                FindObjectOfType<Task>()?.txt_gold.GetComponent<OverlayNumber>().setNum(toNum);


            }, Item_Type.Boom);
        }
        else
        {

            this.txt_diamond.GetComponent<OverlayNumber>().setNum(GM.GetInstance().Diamond);
            Game2Manager.GetInstance()?.txt_gold.GetComponent<OverlayNumber>().setNum(GM.GetInstance().Diamond);
            Game1Manager.GetInstance()?.txt_gold.GetComponent<OverlayNumber>().setNum(GM.GetInstance().Diamond);
            FindObjectOfType<Shop>()?.txt_gold.GetComponent<OverlayNumber>().setNum(GM.GetInstance().Diamond);
            FindObjectOfType<G3BoardManager>()?.txt_gold.GetComponent<OverlayNumber>().setNum(GM.GetInstance().Diamond);
            FindObjectOfType<Achive>()?.txt_gold.GetComponent<OverlayNumber>().setNum(GM.GetInstance().Diamond);
            FindObjectOfType<Task>()?.txt_gold.GetComponent<OverlayNumber>().setNum(GM.GetInstance().Diamond);


        }
        AudioManager.GetInstance().PlayEffect("sound_eff_coin");
    }

    private void OnConsumeGEM(int num)
    {
        this.txt_diamond.GetComponent<OverlayNumber>().setNum(GM.GetInstance().Diamond);
        Game2Manager.GetInstance()?.txt_gold.GetComponent<OverlayNumber>().setNum(GM.GetInstance().Diamond);
        Game1Manager.GetInstance()?.txt_gold.GetComponent<OverlayNumber>().setNum(GM.GetInstance().Diamond);
        FindObjectOfType<Shop>()?.txt_gold.GetComponent<OverlayNumber>().setNum(GM.GetInstance().Diamond);
        FindObjectOfType<G3BoardManager>()?.txt_gold.GetComponent<OverlayNumber>().setNum(GM.GetInstance().Diamond);
        FindObjectOfType<Achive>()?.txt_gold.GetComponent<OverlayNumber>().setNum(GM.GetInstance().Diamond);
        FindObjectOfType<Task>()?.txt_gold.GetComponent<OverlayNumber>().setNum(GM.GetInstance().Diamond);

        //AudioManager.GetInstance().PlayEffect("sound_eff_coin");
    }

    private void OnAddExp(bool isLevelUp)
    {
        this.LoadExpUI();

        if (Game1Manager.GetInstance() != null)
        {
            Game1Manager.GetInstance().LoadExpUI();
        }

        if (!isLevelUp)
        {
            return;
        }
        if (GM.GetInstance().GameId == 2)
        {
            G2BoardGenerator.GetInstance().IsPuase = true;
        }
        this.ShowLevelUp();
    }

    private void OnDoClickBottomButton()
    {
        this.onClickBottom("main");
    }

    private void OnClickPageButton(string name, int value)
    {
        Debug.Log("name " + name + " val  " + value);
        if (name.Equals("Game01"))
        {
            this.curBtnType = "main";
            Game1Play();
            return;
        }
        if (name.Equals("Game03"))
        {
            this.curBtnType = "main";
            this.OnClickStartGame(value);
            return;
        }
        this.onClickBottom(name);
    }

    private void OnDoUseProps(bool isDel, List<GameObject> objs)
    {
        this.panel_handle.SetActive(!isDel);
        if (isDel)
        {
            return;
        }
        using (List<GameObject>.Enumerator enumerator = objs.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                enumerator.Current.transform.SetParent(this.panel_handle.transform, true);
            }
        }
        Transform transform = this.panel_handle.transform.Find("txt_tips");
        Text component = transform.GetComponent<Text>();
        if (transform == null)
        {
            return;
        }
        DOTween.Kill(transform, false);
        Sequence expr_86 = DOTween.Sequence();
        expr_86.Append(component.DOFade(1f, 0f));
        expr_86.Append(component.DOFade(0.5f, 1f));
        expr_86.Append(component.DOFade(1f, 1f));
        expr_86.SetLoops(-1);
        expr_86.SetTarget(transform);
        string[] array = new string[]
        {
            "TXT_NO_50039",
            "TXT_NO_50040",
            "TXT_NO_50041"
        };

        if (GM.GetInstance().GameId == 1)
        {
            component.GetComponent<LanguageComponent>().SetText(array[Game1DataLoader.GetInstance().CurPropId - 1]);
        }

        if (GM.GetInstance().GameId == 2)
        {
            component.GetComponent<LanguageComponent>().SetText("TXT_NO_50041");
        }
    }

    private void OnRefreshAdsTimer(string timer, bool isWatch)
    {
        this.txt_ads_timer.text = timer;
        if (AdsManager.GetInstance().IsWatch)
        {
            this.txt_ads_timer.GetComponent<LanguageComponent>().SetText("TXT_NO_20018");
        }

    }

    private void ShowSubscriptionAwards(int day, int value)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/subscriptionAwards") as GameObject);
        gameObject.GetComponent<SubscriptionDialog>().Load(day, value);
        DialogManager.GetInstance().show(gameObject, false);
        AudioManager.GetInstance().PlayEffect("sound_eff_button");
    }

    public void PlayGEMAni(Action callback, Item_Type item_Type)
    {
        GameObject asset = Resources.Load("Prefabs/effect/eff_gem") as GameObject;
        System.Random rd = new System.Random();
        Sequence arg_50_0 = DOTween.Sequence();
        Sequence actions = DOTween.Sequence();
        arg_50_0.AppendCallback(delegate
        {
            GameObject prefab = UnityEngine.Object.Instantiate<GameObject>(asset);
            prefab.transform.SetParent(this.panel_maxTop.transform, false);
            prefab.transform.localPosition = new Vector3((float)rd.Next(-80, 80), (float)rd.Next(0, 80), 0f);
            prefab.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            Vector3 position = this.icon_gem.transform.position;
            Tweener t = prefab.transform.DOMove(position, 0.8f, false).SetEase(Ease.InBack).OnComplete(delegate
            {
                UnityEngine.Object.Destroy(prefab);
            });
            actions.Insert(0f, t);
        }).AppendInterval(0.01f).SetLoops(10);
        actions.OnComplete(delegate
        {
            Action expr_06 = callback;
            if (expr_06 == null)
            {
                return;
            }
            expr_06();
        });
    }

    public void PlayitemAni(Item_Type item_Type)
    {
        AudioManager.GetInstance().PlayEffect("sound_eff_item_get");


        GameObject asset = Resources.Load("Prefabs/effect/eff_gem") as GameObject;
        System.Random rd = new System.Random();
        Sequence arg_50_0 = DOTween.Sequence();
        Sequence actions = DOTween.Sequence();
        arg_50_0.AppendCallback(delegate
        {
            Debug.Log("아이템 소환");

            GameObject prefab = UnityEngine.Object.Instantiate<GameObject>(asset);
            prefab.GetComponentInChildren<Image>().sprite = itme_sp[(int)item_Type];
            prefab.transform.SetParent(this.panel_maxTop.transform, false);
            prefab.transform.localPosition = new Vector3((float)rd.Next(-80, 80), (float)rd.Next(0, 80), 0f);
            prefab.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            Vector3 position = this.icon_gem.transform.position;
            Tweener t = prefab.transform.DOMove(position, 0.8f, false).SetEase(Ease.InBack).OnComplete(delegate
            {
                UnityEngine.Object.Destroy(prefab);
            });
            actions.Insert(0f, t);
        }).AppendInterval(0.01f).SetLoops(10);

    }

    public void PlayPackageAni()
    {
        GameObject asset = Resources.Load("Prefabs/effect/eff_gem") as GameObject;
        System.Random rd = new System.Random();
        Sequence arg_50_0 = DOTween.Sequence();
        Sequence actions = DOTween.Sequence();
        int i = 0;
        arg_50_0.AppendCallback(delegate
        {
            Debug.Log("아이템 소환");

            GameObject prefab = UnityEngine.Object.Instantiate<GameObject>(asset);
            prefab.GetComponentInChildren<Image>().sprite = itme_sp[(int)i];
            prefab.transform.SetParent(this.panel_maxTop.transform, false);
            prefab.transform.localPosition = new Vector3((float)rd.Next(-80, 80), (float)rd.Next(0, 80), 0f);
            prefab.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            Vector3 position = this.icon_gem.transform.position;
            Tweener t = prefab.transform.DOMove(position, 0.8f, false).SetEase(Ease.InBack).OnComplete(delegate
            {
                UnityEngine.Object.Destroy(prefab);
            });
            actions.Insert(0f, t);

            i += 1;

            if (i == 4)
                i = 0;



        }).AppendInterval(0.01f).SetLoops(10);

    }

    private void PlayGENTipAni()
    {
        //Sequence expr_05 = DOTween.Sequence();
        //expr_05.Append(this.icon_gem.transform.DOScale(1.1f, 1f).SetEase(Ease.Linear));
        //expr_05.Append(this.icon_gem.transform.DOScale(1f, 1f).SetEase(Ease.Linear));
        //expr_05.SetLoops(-1);
    }

    private void ShowLevelUp()
    {

        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/LevelUp") as GameObject);
        DialogManager.GetInstance().show(gameObject, false);
        gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/effect/eff_levelup") as GameObject);
        gameObject.transform.SetParent(base.transform, false);
        UnityEngine.Object.Destroy(gameObject, 5f);
        AudioManager.GetInstance().PlayEffect("sound_eff_levelUp");

        if (GM.GetInstance().Lv == 2)
        {
            gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/review") as GameObject);
            DialogManager.GetInstance().show(gameObject, false);
        }

    }

    private void RunSplash()
    {
        AudioManager.GetInstance().audio_bg.mute = true;
        this.m_splash.SetActive(true);
        this.m_splash.GetComponent<Image>().DOFade(0f, 0.5f).OnComplete(delegate
        {
            AudioManager.GetInstance().audio_bg.mute = false;
            this.m_splash.SetActive(false);
        });
    }

}
