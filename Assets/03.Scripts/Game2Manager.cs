using Assets.Scripts.Configs;
using Assets.Scripts.GameManager;
using Assets.Scripts.Utils;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Game2Manager : MonoBehaviour
{
    private static Game2Manager m_instance;

    private void Awake()
    {
        Game2Manager.m_instance = this;
    }


    public static Game2Manager GetInstance()
    {
        return Game2Manager.m_instance;
    }

    private struct PathRDM
	{
		public List<Node> paths;

		public int index;
	}
  
	[Serializable]
	private sealed class CommonClass
	{
		public static readonly Game2Manager.CommonClass _commonClass = new Game2Manager.CommonClass();

		public static Action _action_1;

		public static Action _action_2;

		public static Comparison<Game2Manager.PathRDM> _comparison;

		internal void OnClickReturn1()
		{
			GM.GetInstance().SaveScore(2, 0);
			GM.GetInstance().SetSavedGameID(0);
			GM.GetInstance().ResetToNewGame();
			GM.GetInstance().ResetConsumeCount();
            G2BoardGenerator.GetInstance().Score = 0;
            G2BoardGenerator.GetInstance().StartNewGame();
		}

		internal void OnClickReturn2()
		{
            G2BoardGenerator.GetInstance().IsPuase = false;
		}

		internal int FindPath(Game2Manager.PathRDM a, Game2Manager.PathRDM b)
		{
			return -1 * a.paths.Count.CompareTo(b.paths.Count);
		}
	}
    
	public GameObject m_map;

	public GameObject m_mask;

	private List<G2Block> blocks = new List<G2Block>();

	public GameObject m_objEndPos;

	[SerializeField]
	public List<GameObject> bloodPosList = new List<GameObject>();

	private List<GameObject> bloodList = new List<GameObject>
	{
		null,
		null,
		null,
		null,
		null
	};

    public Text txt_lv;

    public Image img_exp;

	private List<GameObject> m_tranformObjs = new List<GameObject>();

	public GameObject m_img_coin;

	public Text m_txt_tips;

	public GameObject txt_score;

    public Text txt_gold;

    public Button btn_ads;

    public Text txt_timer;

    public float m_downSpeed = 10f;

	public int m_backLength = 50;

	public int m_deleteBackLength = 100;

	public float m_upSpeed = 500f;

	public bool m_isBack;

	public float m_backTotal;

	public Image m_img_finger;

    public Transform start_pos;

    private int m_dobleTotal;

	private Vector3 m_saveFingerPos = Vector3.zero;

	private int m_step;

	private string[] m_data_tiptxts = new string[]
	{
		"TXT_NO_50011",
		"TXT_NO_50033",
		"TXT_NO_50034",
		"TXT_NO_50035"
	};

	private void Start()
	{
		this.LoadUI();
		this.LoadBlock();
		this.InitEvent();
        AudioManager.GetInstance().PlayBgMusic("sound_ingame", true);

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

        if (!G2BoardGenerator.GetInstance().IsStart)
		{
			return;
		}
		if (G2BoardGenerator.GetInstance().IsPuase)
		{
			return;
		}
		if (G2BoardGenerator.GetInstance().IsGameOver)
		{
			return;
		}
		if (!G2BoardGenerator.GetInstance().IsFinishGuide)
		{
			return;
		}
		if (this.m_isBack)
		{
			return;
		}
		this.DownMap();
	}

    public void OnclickReturn()
    {
        GameList.Instance.Pause_Return();
    }

    public void OnclickSetting()
    {
        GameList.Instance.Setting_Return();  
    }

    public void OnclickShop()
    {
        GameList.Instance.Shop_Return();

    }

    public void set_lv()
    {
        this.txt_lv.GetComponent<Text>().text = GM.GetInstance().Lv.ToString();
        if (Configs.TPlayers.ContainsKey(GM.GetInstance().Lv.ToString()))
        {
            TPlayer tPlayer = Configs.TPlayers[GM.GetInstance().Lv.ToString()];
            this.img_exp.GetComponent<Image>().fillAmount = (((float)GM.GetInstance().Exp / (float)tPlayer.Exp >= 1f) ? 1f : ((float)GM.GetInstance().Exp / (float)tPlayer.Exp));
        }
    }

    private void OnDestroy()
	{
		this.RemoveEvent();
	}

	private void InitEvent()
	{
        G2BoardGenerator.GetInstance().DoRefreshHandle += new Action(this.RefreshMap);
        G2BoardGenerator.GetInstance().DoDeleteHandle += new Action<List<int>, int>(this.Delete);
        G2BoardGenerator.GetInstance().DoDropHandle += new Action<List<sDropData>, List<int>>(this.Drop);
        G2BoardGenerator expr_47 = G2BoardGenerator.GetInstance();
		expr_47.DoClickBlock = (Action<G2Block>)Delegate.Combine(expr_47.DoClickBlock, new Action<G2Block>(this.OnClickBlock));
        G2BoardGenerator.GetInstance().DoNullDelete += new Action(this.OnNullDelete);
        G2BoardGenerator.GetInstance().DoCompMaxNumber += new Action<int>(this.PlayNewNumber);
        G2BoardGenerator expr_99 = G2BoardGenerator.GetInstance();
		expr_99.DoVedioRefresh = (Action)Delegate.Combine(expr_99.DoVedioRefresh, new Action(this.UseVedioRefresh));
        G2BoardGenerator.GetInstance().OnClickReturnHandle = new Action<GameList>(this.OnClickReturn);
        G2BoardGenerator.GetInstance().OnRandomCoinHandle = new Action(this.OnRandomCoin);
	}

	private void RemoveEvent()
	{
	}

	public void OnClickProp(int type)
	{
		if (G2BoardGenerator.GetInstance().IsPlaying)
		{
			return;
		}
		if (!G2BoardGenerator.GetInstance().IsStart)
		{
			return;
		}
		int value = Constant.COMMON_CONFIG_PROP[type - 1];
		if (!GM.GetInstance().isFullGEM(value))
		{
			ToastManager.Show("TXT_NO_50001", true);
			return;
		}
		if (G2BoardGenerator.GetInstance().CurPropId != 0)
		{
            G2BoardGenerator.GetInstance().CurPropId = 0;
            G2BoardGenerator.GetInstance().IsPuase = false;
			this.ControlPropsPannel(true);
			return;
		}
        G2BoardGenerator.GetInstance().CurPropId = type;
        G2BoardGenerator.GetInstance().IsPuase = true;
		this.ControlPropsPannel(false);
	}

	public void OnClickBox(int idx)
	{
		if (!G2BoardGenerator.GetInstance().IsStart)
		{
            G2BoardGenerator.GetInstance().IsStart = true;
		}
		if (G2BoardGenerator.GetInstance().IsPuase)
		{
			return;
		}
		if (G2BoardGenerator.GetInstance().IsPlaying)
		{
			return;
		}
		if (!G2BoardGenerator.GetInstance().IsFinishGuide)
		{
			switch (this.m_step)
			{
			case 0:
				idx = 2;
				break;
			case 1:
				idx = 2;
				break;
			case 2:
				idx = 3;
				break;
			}
			this.m_step++;
			if (this.m_step >= 4)
			{
                G2BoardGenerator.GetInstance().FinishGuide();
				this.m_txt_tips.GetComponent<LanguageComponent>().SetText(this.m_data_tiptxts[0]);
				//AppsflyerUtils.TrackTutorialCompletion(2, 1);
			}
		}
		this.m_dobleTotal = 0;

        Debug.Log("½ÃÀÛ");

        int num = G2BoardGenerator.GetInstance().AddBock(idx);

		if (num == -1)
		{
			return;
		}

        Debug.Log("³Ñ¾î°¨");

        int number = G2BoardGenerator.GetInstance().GetNumber(num, idx);
        G2Block block = G2BoardGenerator.GetInstance().CreateBlock(number, G2BoardGenerator.GetInstance().GetIndex(num, idx), this.m_map);
		Vector3 localPosition = block.transform.localPosition;
		block.transform.position = this.bloodPosList[idx].transform.position;
		block.transform.SetParent(this.m_map.transform, true);
		Vector3 localPosition2 = block.transform.localPosition;
		float duration = (localPosition.y - localPosition2.y) / this.m_upSpeed;
        Debug.Log("go " + localPosition + "  " + duration);
        block.GetComponent<G2Block>().img_panel.SetActive(true);

        block.transform.DOLocalMove(localPosition, duration, false).OnStart(delegate
		{
            G2BoardGenerator.GetInstance().AddLife();
			this.bloodList[2].GetComponent<G2Block>().setNum(G2BoardGenerator.GetInstance().GetBlood());

        }).OnComplete(delegate
		{
			this.blocks[block.Index] = block;
			this.BackMap(1);
            G2BoardGenerator.GetInstance().IsPlaying = false;
            G2BoardGenerator.GetInstance().Delete(block.Index);
			if (block.Index == G2BoardGenerator.GetInstance().CoinIndex)
			{
				GM.GetInstance().AddDiamond(1, true);
                G2BoardGenerator.GetInstance().CoinIndex = -1;
				this.m_img_coin.gameObject.SetActive(false);
			}
			TaskData.GetInstance().Add(100102, 1, true);
		});
        G2BoardGenerator.GetInstance().IsPlaying = true;
		this.HideTxtTips();
		this.StopFingerAni();
		AudioManager.GetInstance().PlayEffect("sound_eff_click_1");
	}


    public void Set_Timer()
    {
        if (DataManager.Instance.state_Player.LocalData_Game_Time == "-1")
        {
            btn_ads.interactable = true;
            txt_timer.GetComponent<LanguageComponent>().SetText("TXT_NO_20018");

        }
        else
        {
            btn_ads.interactable = false;

            txt_timer.gameObject.SetActive(true);

            GiftTime = DateTime.Parse(DataManager.Instance.state_Player.LocalData_Game_Time);
        }
    }

    DateTime GiftTime;

    public void onClickAds()
    {
        AdsControl.Instance.reward_Type = Reward_Type.game;
        AdsControl.Instance.ShowRewardedAd();
        
    }

    public void OnTouchStartBox(BaseEventData eventData)
	{
		if (G2BoardGenerator.GetInstance().IsPuase)
		{
			return;
		}
		if (G2BoardGenerator.GetInstance().IsPlaying)
		{
			return;
		}

		PointerEventData pointerEventData = (PointerEventData)eventData;
		int num = -1;
		for (int i = 0; i < this.bloodPosList.Count; i++)
		{
			GameObject expr_33 = this.bloodPosList[i];
			Vector2 point;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(expr_33.GetComponent<RectTransform>(), pointerEventData.pressPosition, pointerEventData.pressEventCamera, out point);
			if (expr_33.GetComponent<RectTransform>().rect.Contains(point))
			{
				num = i;
				break;
			}
		}

        //this.bloodList[2].GetComponent<G2Block>().img_panel.SetActive(true);
        this.bloodList[2].transform.SetParent(this.bloodPosList[num].transform, false);
        this.bloodList[2].transform.localPosition = Vector3.zero;

        if (num == -1)
		{
			return;
		}



    }

	public void OnTouchMoveBox(BaseEventData eventData)
	{
		PointerEventData pointerEventData = (PointerEventData)eventData;
		int num = -1;
		for (int i = 0; i < this.bloodPosList.Count; i++)
		{
			GameObject expr_19 = this.bloodPosList[i];
			Vector2 point;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(expr_19.GetComponent<RectTransform>(), pointerEventData.position, pointerEventData.pressEventCamera, out point);
			if (expr_19.GetComponent<RectTransform>().rect.Contains(point))
			{
				num = i;
				break;
			}
		}
		if (num == -1)
		{
			return;
		}
		this.bloodList[2].transform.SetParent(this.bloodPosList[num].transform, false);
	}

	public void onTouchEndBox(BaseEventData eventData)
	{

        PointerEventData pointerEventData = (PointerEventData)eventData;
		int num = -1;
		for (int i = 0; i < this.bloodPosList.Count; i++)
		{
			GameObject expr_19 = this.bloodPosList[i];
			Vector2 point;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(expr_19.GetComponent<RectTransform>(), pointerEventData.position, pointerEventData.pressEventCamera, out point);
			if (expr_19.GetComponent<RectTransform>().rect.Contains(point))
			{
				num = i;
				break;
			}
		}


        if (num == -1)
		{
			return;
		}


        this.OnClickBox(num);

        this.bloodList[2].transform.SetParent(this.bloodPosList[2].transform, false);
        this.bloodList[2].transform.position = start_pos.position;
        //this.bloodList[2].GetComponent<G2Block>().img_panel.SetActive(false);

    }

    public void OnTouchMap(BaseEventData eventData)
	{
		PointerEventData pointerEventData = (PointerEventData)eventData;
		Vector2 vector;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_map.GetComponent<RectTransform>(), pointerEventData.pressPosition, pointerEventData.pressEventCamera, out vector);
		int num = (int)Math.Floor((double)((this.m_map.GetComponent<RectTransform>().sizeDelta.x / 2f + vector.x) / 160f));
        Debug.Log("Ãþ¼ö~~"+num);
        if (num < 0 || num >= 6)
		{
			return;
		}
		this.OnClickBox(num);
	}

	public void OnClickReturn(GameList obj)
	{
		if (G2BoardGenerator.GetInstance().IsPlaying && G2BoardGenerator.GetInstance().IsPuase)
		{
			return;
		}

        G2BoardGenerator.GetInstance().IsPuase = true;
		int arg_79_0 = G2BoardGenerator.GetInstance().Score;
		Action arg_79_1 = delegate
		{
            G2BoardGenerator.GetInstance().Score = 0;
			GM.GetInstance().SaveScore(2, 0);
			GM.GetInstance().SetSavedGameID(0);
			GM.GetInstance().ResetToNewGame();
			GM.GetInstance().ResetConsumeCount();
			GlobalEventHandle.EmitDoGoHome();
			GlobalEventHandle.EmitClickPageButtonHandle("main", 0);
			//obj.HideTopBtn();
		};
		Action arg_79_2;
		if ((arg_79_2 = Game2Manager.CommonClass._action_1) == null)
		{
			arg_79_2 = (Game2Manager.CommonClass._action_1= new Action(Game2Manager.CommonClass._commonClass.OnClickReturn1));
		}
		Action arg_79_3;
		if ((arg_79_3 = Game2Manager.CommonClass._action_2) == null)
		{
			arg_79_3 = (Game2Manager.CommonClass._action_2= new Action(Game2Manager.CommonClass._commonClass.OnClickReturn2));
		}
		Utils.ShowPause(arg_79_0, arg_79_1, arg_79_2, arg_79_3);
	}

	private void LoadUI()
	{
        
		this.RefreshScore(false);
		this.InitLife();
		this.ShowTxtTips();
		RectTransform component = this.m_map.GetComponent<RectTransform>();
		RectTransform arg_5E_0 = this.m_mask.GetComponent<RectTransform>();
		component.anchoredPosition = GM.GetInstance().GetSavedPos();
		arg_5E_0.anchoredPosition = GM.GetInstance().GetSavedPos() + new Vector2(0f, component.sizeDelta.y);
		this.m_saveFingerPos = this.m_img_finger.transform.localPosition;
		if (!G2BoardGenerator.GetInstance().IsFinishGuide)
		{
			this.PlayFingerAni();
			this.m_txt_tips.GetComponent<LanguageComponent>().SetText(this.m_data_tiptxts[this.m_step]);
		}
        Set_Timer();
        set_lv();
        txt_gold.GetComponent<OverlayNumber>().SetStartNumber(DataManager.Instance.state_Player.LocalData_Diamond);

    }

    private void RefreshScore(bool isAni = true)
	{
		if (isAni)
		{
			this.txt_score.GetComponent<OverlayNumber>().setNum(G2BoardGenerator.GetInstance().Score);
			return;
		}
		this.txt_score.GetComponent<OverlayNumber>().Reset();
		this.txt_score.GetComponent<OverlayNumber>().setNum(G2BoardGenerator.GetInstance().Score);
	}

	private void LoadBlock()
	{
		for (int i = 0; i < 7; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				int number = G2BoardGenerator.GetInstance().GetNumber(i, j);
				if (number == 0)
				{
					this.blocks.Add(null);
				}
				else
				{

                    int index = G2BoardGenerator.GetInstance().GetIndex(i, j);
                    G2Block item = G2BoardGenerator.GetInstance().CreateBlock(number, index, this.m_map);
                    item.img_panel.SetActive(true);

                    this.blocks.Add(item);
				}
			}
		}
	}

	private void InitLife()
	{
		for (int i = 0; i < 5; i++)
		{
			int num = G2BoardGenerator.GetInstance().BloodList[i];
			if (num != 0)
			{
				this.CreateNewLife(num, this.bloodPosList[i], i).transform.position = start_pos.position;
            }
        }
	}

	private void RefreshMap()
	{
		foreach (G2Block current in this.blocks)
		{
			if (!(current == null))
			{
                G2BoardGenerator.GetInstance().FreeBlock(current.gameObject);
			}
		}
		this.blocks.Clear();
		for (int i = 0; i < this.bloodList.Count; i++)
		{
			GameObject gameObject = this.bloodList[i];
			if (!(gameObject == null))
			{
				this.bloodList[i] = null;
                G2BoardGenerator.GetInstance().FreeBlock(gameObject);
			}
		}
		this.LoadBlock();
		this.LoadUI();
		this.OnRandomCoin();
	}

	private void DownMap()
	{
		float y = this.m_downSpeed * Time.deltaTime;
		RectTransform component = this.m_map.GetComponent<RectTransform>();
		RectTransform arg_40_0 = this.m_mask.GetComponent<RectTransform>();
		component.anchoredPosition -= new Vector2(0f, y);
		arg_40_0.anchoredPosition -= new Vector2(0f, y);
		GM.GetInstance().SaveGame("", "", component.anchoredPosition.x, component.anchoredPosition.y);
		this.GameOver();
	}

	private void BackMap(int type)
	{
		float num = (float)((type == 1) ? this.m_backLength : this.m_deleteBackLength);
		switch (type)
		{
		case 1:
			num = (float)this.m_backLength;
			break;
		case 2:
			num = (float)this.m_deleteBackLength;
			break;
		case 3:
			num = 770f;
			break;
		}
		this.m_backTotal += num;
		if (this.m_map.transform.localPosition.y + this.m_backTotal > 0f)
		{
			this.m_backTotal = 0f - this.m_map.transform.localPosition.y;
		}
		this.m_isBack = true;
		Vector3 byValue = new Vector3(0f, this.m_backTotal, 0f);
		this.m_map.transform.DOKill(false);
		this.m_map.transform.DOBlendableLocalMoveBy(byValue, 0.1f, false).OnComplete(delegate
		{
			this.m_isBack = false;
			this.m_backTotal = 0f;
		});
		this.m_mask.transform.DOKill(false);
		this.m_mask.transform.DOBlendableLocalMoveBy(byValue, 0.1f, false);
	}

	private void OnClickBlock(G2Block block)
	{
	}

	private void OnNullDelete()
	{
		this.ShowGuide();
		this.PlayFingerAni();
	}

	private void Delete(List<int> list, int index)
	{
		AudioManager.GetInstance().PlayEffect("sound_eff_clear_2");
		List<Game2Manager.PathRDM> list2 = this.FindPath(list, index);
		if (list2.Count < 0)
		{
			return;
		}
		this.m_dobleTotal++;
		int count = list2[0].paths.Count;
		Sequence sequence = DOTween.Sequence();
		foreach (Game2Manager.PathRDM current in list2)
		{
            G2Block g = this.blocks[current.index];
			g.ShowScore();
			this.blocks[current.index] = null;
			int row = G2BoardGenerator.GetInstance().GetRow(current.index);
			int col = G2BoardGenerator.GetInstance().GetCol(current.index);
			if (current.paths.Count >= 2)
			{
				Vector2 b = new Vector2((float)current.paths[0].x, (float)current.paths[0].y);
				Vector2 vector = new Vector2((float)current.paths[1].x, (float)current.paths[1].y) - b;
				int index2 = G2BoardGenerator.GetInstance().GetIndex(row + (int)vector.y, col + (int)vector.x);
				Tween t = g.DelayMove(index2, (float)(count - current.paths.Count) * 0.1f);
				sequence.Insert(0f, t);
			}
		}
		sequence.OnComplete(delegate
		{
			//this.PlayDoubleAni();
			this.BackMap(2);
			this.blocks[index].setNum(G2BoardGenerator.GetInstance().GetNumber(index));
            G2BoardGenerator.GetInstance().Down();
		});
        G2BoardGenerator.GetInstance().IsPlaying = true;
		this.RefreshScore(true);
	}

	private List<Game2Manager.PathRDM> FindPath(List<int> list, int index)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 7;
		int num4 = 5;
		foreach (int current in list)
		{
			int row = G2BoardGenerator.GetInstance().GetRow(current);
			int col = G2BoardGenerator.GetInstance().GetCol(current);
			if (row > num)
			{
				num = row;
			}
			if (row < num3)
			{
				num3 = row;
			}
			if (col > num2)
			{
				num2 = col;
			}
			if (col < num4)
			{
				num4 = col;
			}
		}
		int row2 = Math.Abs(num - num3) + 1;
        Assets.Scripts.Utils.Grid grid = new Assets.Scripts.Utils.Grid(Math.Abs(num2 - num4) + 1, row2);
		List<sGridRMD> list2 = new List<sGridRMD>();
		int py = 0;
		int px = 0;
		int i = num3;
		int num5 = 0;
		while (i < num + 1)
		{
			int j = num4;
			int num6 = 0;
			while (j < num2 + 1)
			{
				int index2 = G2BoardGenerator.GetInstance().GetIndex(i, j);
				grid.getNode(num6, num5).isWalk = list.Contains(index2);
				if (index2 == index)
				{
					py = num5;
					px = num6;
				}
				else if (list.Contains(index2))
				{
					list2.Add(new sGridRMD(i, j, num5, num6));
				}
				j++;
				num6++;
			}
			i++;
			num5++;
		}
		AStar aStar = new AStar(grid, DiagonalMovement.NEVER, HeuristicType.MANHATTAN);
		List<Game2Manager.PathRDM> list3 = new List<Game2Manager.PathRDM>();
		foreach (sGridRMD current2 in list2)
		{
			int index3 = G2BoardGenerator.GetInstance().GetIndex(current2.gameRow, current2.gameCol);
			if (index3 != index)
			{
                G2BoardGenerator.GetInstance().GetRow(index3);
                G2BoardGenerator.GetInstance().GetCol(index3);
				list3.Add(new Game2Manager.PathRDM
				{
					paths = aStar.Find(new AVec2(current2.gridCol, current2.gridRow), new AVec2(px, py)),
					index = index3
				});
			}
		}
		List<Game2Manager.PathRDM> arg_20D_0 = list3;
		Comparison<Game2Manager.PathRDM> arg_20D_1;
		if ((arg_20D_1 = Game2Manager.CommonClass._comparison) == null)
		{
			arg_20D_1 = (Game2Manager.CommonClass._comparison = new Comparison<Game2Manager.PathRDM>(Game2Manager.CommonClass._commonClass.FindPath));
		}
		arg_20D_0.Sort(arg_20D_1);
		return list3;
	}

	private void Drop(List<sDropData> dropList, List<int> newList)
	{
		Sequence sequence = DOTween.Sequence();
		foreach (sDropData current in dropList)
		{
            G2Block g = this.blocks[current.srcIdx];
			this.blocks[current.srcIdx] = null;
			this.blocks[current.dstIdx] = g;
			g.Index = current.dstIdx;
			Tween t = g.Move(current.dstIdx);
			sequence.Insert(0f, t);
		}
		sequence.OnComplete(delegate
		{
            G2BoardGenerator.GetInstance().IsPlaying = false;
            G2BoardGenerator.GetInstance().AutoDelete();
			this.ShowGuide();
			this.PlayFingerAni();
		});
		if (G2BoardGenerator.GetInstance().CoinIndex > 0)
		{
			int row = G2BoardGenerator.GetInstance().GetRow(G2BoardGenerator.GetInstance().CoinIndex);
			int col = G2BoardGenerator.GetInstance().GetCol(G2BoardGenerator.GetInstance().CoinIndex);
			for (int i = 6; i > row; i--)
			{
				if (G2BoardGenerator.GetInstance().GetNumber(i, col) == 0)
				{
					Vector3 endValue = new Vector3((float)col * 160f + 55f - 375f, (float)i * 160f + 55f - 440f, 0f);
                    this.m_img_coin.transform.DOLocalMove(endValue, 0.3f, false);
                    G2BoardGenerator.GetInstance().CoinIndex = G2BoardGenerator.GetInstance().GetIndex(i, col);
					break;
				}
			}
		}
        G2BoardGenerator.GetInstance().IsPlaying = true;
	}

    public void video()
    {
        Debug.Log("±¤°í ºÃÀ½");
        bool flag = false;
        if (this.m_mask.transform.localPosition.y <= 0f)
        {
            flag = true;
        }
        if (!flag)
        {
            float num = this.m_objEndPos.transform.position.y + 0.55f;
            int idx = 0;
            for (int i = 0; i < this.blocks.Count; i++)
            {
                G2Block g = this.blocks[i];
                if (!(g == null) && g.transform.position.y <= num)
                {
                    flag = true;
                    idx = i;
                    break;
                }
            }
            if (!flag)
            {
                return;
            }
            foreach (int current in G2BoardGenerator.GetInstance().Use(idx, 2))
            {
                if (current < this.blocks.Count)
                {
                    G2Block g2 = this.blocks[current];
                    this.blocks[current] = null;
                    if (!(g2 == null))
                    {
                        Debug.Log("name :" + g2.transform.position + "   " + g2.GetCurrentColor());
                        ParticlesControl.GetInstance().PlayExplodeEffic(g2.transform.position, g2.GetCurrentColor());
                        G2BoardGenerator.GetInstance().FreeBlock(g2.gameObject);
                    }
                }
            }
            this.BackMap(3);
            G2BoardGenerator.GetInstance().IsStart = true;
            G2BoardGenerator.GetInstance().IsGameOver = false;
        }
    }

	public void UseVedioRefresh()
	{
		bool flag = false;
		if (this.m_mask.transform.localPosition.y <= 0f)
		{
			flag = true;
		}
		if (!flag)
		{
			float num = this.m_objEndPos.transform.position.y + 0.55f;
			int idx = 0;
			for (int i = 0; i < this.blocks.Count; i++)
			{
                G2Block g = this.blocks[i];
				if (!(g == null) && g.transform.position.y <= num)
				{
					flag = true;
					idx = i;
					break;
				}
			}
			if (!flag)
			{
				return;
			}
			foreach (int current in G2BoardGenerator.GetInstance().Use(idx, 2))
			{
				if (current < this.blocks.Count)
				{
                    G2Block g2 = this.blocks[current];
					this.blocks[current] = null;
					if (!(g2 == null))
					{
						ParticlesControl.GetInstance().PlayExplodeEffic(g2.transform.position, g2.GetCurrentColor());
                        G2BoardGenerator.GetInstance().FreeBlock(g2.gameObject);
					}
				}
			}
		}
		this.BackMap(3);
        G2BoardGenerator.GetInstance().IsStart = true;
        G2BoardGenerator.GetInstance().IsGameOver = false;
	}

	private void ShowTxtTips()
	{
		this.m_txt_tips.gameObject.SetActive(true);
		if (DOTween.IsTweening(this.m_txt_tips, false))
		{
			return;
		}
		Sequence expr_25 = DOTween.Sequence();
		expr_25.Append(this.m_txt_tips.transform.DOScale(0.95f, 1f));
		expr_25.Append(this.m_txt_tips.transform.DOScale(1f, 1f));
		expr_25.SetLoops(-1);
		expr_25.SetTarget(this.m_txt_tips);
	}

	private void HideTxtTips()
	{
		this.m_txt_tips.gameObject.SetActive(false);
	}

	private void PlayDoubleAni()
	{
		//if (this.m_dobleTotal < 2)
		//{
		//	return;
		//}
		//this.m_img_double.transform.Find("txt").GetComponent<Text>().text = string.Format("{0}", this.m_dobleTotal);
		//DOTween.Kill(this.m_img_double, false);
		//Sequence arg_92_0 = DOTween.Sequence();
		//this.m_img_double.gameObject.SetActive(true);
		//this.m_img_double.DOKill(false);
		//this.m_img_double.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
		//arg_92_0.Append(this.m_img_double.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack));
		//arg_92_0.AppendInterval(0.5f);
		//arg_92_0.OnComplete(delegate
		//{
		//	this.m_img_double.gameObject.SetActive(false);
		//});
		//UnityEngine.Debug.Log("Double:" + this.m_dobleTotal);
	}

	private void PlayNewNumber(int number)
	{
		GameObject original = Resources.Load("Prefabs/G00204") as GameObject;
		GameObject node = UnityEngine.Object.Instantiate<GameObject>(original, base.transform, false);
		node.transform.GetComponentInChildren<G2Block>().setNum(number);
        node.transform.localScale = new Vector3(1f, 0f, 0f);
		Sequence expr_72 = DOTween.Sequence();
		expr_72.Append(node.transform.DOScaleY(1f, 0.1f));
		expr_72.AppendInterval(0.5f);
		expr_72.OnComplete(delegate
		{
			UnityEngine.Object.Destroy(node);
		});
		AudioManager.GetInstance().PlayEffect("sound_eff_newNum");
	}

	private void OnRandomCoin()
	{
		if (G2BoardGenerator.GetInstance().CoinIndex > 0)
		{
			int num = G2BoardGenerator.GetInstance().CoinIndex / 5;
			int num2 = G2BoardGenerator.GetInstance().CoinIndex % 5;
			this.m_img_coin.transform.localPosition = new Vector3((float)num2 * 160f + 55f - 375f, (float)num * 160f + 55f - 440f, 0f);
            this.m_img_coin.gameObject.SetActive(true);
			return;
		}
		this.m_img_coin.gameObject.SetActive(false);
	}

	private void GameOver()
	{
		if (G2BoardGenerator.GetInstance().IsPlaying)
		{
			return;
		}
		bool flag = false;
		if (this.m_mask.transform.localPosition.y <= 0f)
		{
			flag = true;
		}
		if (!flag)
		{
			float num = this.m_objEndPos.transform.position.y + 0.55f;
			foreach (G2Block current in this.blocks)
			{
				if (!(current == null) && current.transform.position.y <= num)
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			return;
		}
        G2BoardGenerator.GetInstance().IsStart = false;
        G2BoardGenerator.GetInstance().IsGameOver = true;
		if (G2BoardGenerator.GetInstance().FinishCount < 1000)
		{
            Debug.Log("ÇÇ´Ï½¬~~~ " + G2BoardGenerator.GetInstance().FinishCount);
            G2BoardGenerator.GetInstance().FinishCount++;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/finish") as GameObject);
			gameObject.GetComponent<Finish>().Load(2, G2BoardGenerator.GetInstance().GetMapMaxNumber());
			DialogManager.GetInstance().show(gameObject, true);
			return;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/G00203") as GameObject);
		gameObject2.GetComponent<G2UIManager>().Load(G2BoardGenerator.GetInstance().Score, G2BoardGenerator.GetInstance().MaxScore);
		DialogManager.GetInstance().show(gameObject2, true);

    }

    public void ControlPropsPannel(bool isDel)
	{
		if (isDel)
		{
			for (int i = 0; i < this.m_tranformObjs.Count; i++)
			{
				if (i < this.m_tranformObjs.Count - 1)
				{
					this.m_tranformObjs[i].transform.SetParent(this.m_map.transform, true);
					this.m_tranformObjs[i].GetComponent<G2Block>().RemoveClick();
				}
				else
				{
					//this.m_tranformObjs[i].transform.SetParent(this.m_pannel_props.transform, true);
				}
			}
			this.m_tranformObjs.Clear();
			GlobalEventHandle.EmitDoUseProps(true, this.m_tranformObjs);
			return;
		}
		foreach (G2Block current in this.blocks)
		{
			if (!(current == null))
			{
				current.AddClickListener();
				this.m_tranformObjs.Add(current.gameObject);
			}
		}
		this.m_tranformObjs.Add(base.transform.Find(string.Format("img_pros/item{0}", G2BoardGenerator.GetInstance().CurPropId)).gameObject);
		GlobalEventHandle.EmitDoUseProps(false, this.m_tranformObjs);
	}

	private void ShowGuide()
	{
		if (G2BoardGenerator.GetInstance().IsFinishGuide)
		{
			return;
		}
		this.m_txt_tips.gameObject.SetActive(true);
		this.m_txt_tips.GetComponent<LanguageComponent>().SetText(this.m_data_tiptxts[this.m_step]);
		switch (this.m_step)
		{
		case 1:
			break;
		case 2:
			this.m_saveFingerPos += new Vector3(110f, 0f, 0f);
			return;
		case 3:
		{
			Vector3 vector = this.bloodPosList[this.bloodPosList.Count - 1].transform.position;
			vector = base.transform.InverseTransformPoint(vector);
			this.m_saveFingerPos = vector + new Vector3(70f, -70f, 0f);
			break;
		}
		default:
			return;
		}
	}

	private void PlayFingerAni()
	{
		if (G2BoardGenerator.GetInstance().IsFinishGuide)
		{
			return;
		}
		this.m_img_finger.gameObject.SetActive(true);
		this.m_img_finger.transform.localPosition = this.m_saveFingerPos;
		DOTween.Kill(this.m_img_finger, false);
		Sequence expr_46 = DOTween.Sequence();
		expr_46.Append(this.m_img_finger.transform.DOBlendableLocalMoveBy(new Vector3(0f, -10f, 0f), 0.5f, false));
		expr_46.Append(this.m_img_finger.transform.DOBlendableLocalMoveBy(new Vector3(0f, 10f, 0f), 0.3f, false));
		expr_46.SetLoops(-1);
		expr_46.SetTarget(this.m_img_finger);
	}

	private void StopFingerAni()
	{
		DOTween.Kill(this.m_img_finger, false);
		this.m_img_finger.gameObject.SetActive(false);
	}

	private G2Block CreateNewLife(int number, GameObject parent, int idx)
	{
        G2Block g = G2BoardGenerator.GetInstance().CreateBlock(number, 0, parent);
		this.bloodList[idx] = g.gameObject;
        g.GetComponent<G2Block>().img_panel.SetActive(false);


        return g;
	}
}
