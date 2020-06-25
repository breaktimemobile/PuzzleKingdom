using Assets.Scripts.Configs;
using Assets.Scripts.GameManager;
using Assets.Scripts.Utils;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/*
 * This scripts will generate the board of game 1(Merge block)
 */
public class G1BoardManager : MonoBehaviour
{
	private struct TransformControl
	{
		public Transform parent;

		public Transform self;

		public TransformControl(Transform parent, Transform self)
		{
			this.parent = parent;
			this.self = self;
		}
	}
    //size of row (default 5)
	public const int CONSTANT_MAP_ROW = 5;
    //size of column (default 5)
	public const int CONSTANT_MAP_COL = 5;
    //width size of cell (default 120)
	public const int CONSTANT_CELL_WIDTH = 120;
    //heigh size of cell (default 120)
	public const int CONSTANT_CELL_HEIGHT = 120;
    //button return
	public GameObject m_btn_return;
    //button skip levels
	public GameObject m_btn_skip;
    //tip texts
	public Text m_tips01;
    //finger tutorial for first playing
	public Image m_img_finger;
    //tip texts
	public string[] m_tipTxt;
    //this object will hold the board
	public GameObject gameBox;
    //bottom block that will be drag to the board
	public GameObject bloodBox;
    //mask object
	public GameObject m_mask;
    //blood list contain 5 block
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
    //list block in the board
	private List<G1Block> m_blocks = new List<G1Block>();

	private int m_step;
    //life of blood block (1,2 or -1)
	private int[] m_lifes = new int[]
	{
		1,
		1,
		1,
		2,
		-1
	};
    //array is used to save map data
	private int[][] m_nextMap;

	private int[] m_maps;

	private bool m_isPuase;

	private Vector3 m_saveFingerPos;

	private List<G1BoardManager.TransformControl> m_transformList;

	private Vector3 m_fingerRect;

	private Vector3 m_fingerRect2;

    public List<Text> txt_diamonds = new List<Text>();

    public Text txt_gold;

    public void Set_Txt_Item()
    {
        txt_diamonds[0].text = DataManager.Instance.state_Player.item_Localdata.Boom == 0 ? "+" : DataManager.Instance.state_Player.item_Localdata.Boom.ToString();
        txt_diamonds[1].text = DataManager.Instance.state_Player.item_Localdata.Hammer == 0 ? "+" : DataManager.Instance.state_Player.item_Localdata.Hammer.ToString();
        txt_diamonds[2].text = DataManager.Instance.state_Player.item_Localdata.Star == 0 ? "+" : DataManager.Instance.state_Player.item_Localdata.Star.ToString();
    }

    private void Start()
	{
		this.LoadUI();
        Set_Txt_Item();
        txt_gold.GetComponent<OverlayNumber>().SetStartNumber(DataManager.Instance.state_Player.LocalData_Diamond);

    }

    private void Update()
	{
		Utils.BackListener(base.gameObject, delegate
		{
			this.OnclickReturn();
		});
	}

	private void OnDestroy()
	{
		DOTween.Kill(this.m_img_finger, false);
	}

	private void InitEvent()
	{
	}

	private void RemoveEvent()
	{
	}

	public void OnClickSkip()
	{
		this.StartGame();
	}

	public void StartGame()
	{
        DataManager.Instance.state_Player.LocalData_guide_game01 = 1;
        DataManager.Instance.Save_Player_Data();

		GlobalEventHandle.EmitClickPageButtonHandle("Game01", 0);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void OnTouchScreen()
	{
		if (this.m_step != 0)
		{
			return;
		}
		this.m_step++;
		this.PlayFingerMoveAni(1);
		this.bloodList[0].GetComponent<G1BlockEvent>().DisableDrag = false;
	}
    public void OnclickReturn()
    {
        //GameList.Instance.Pause_Return();
    }

    public void OnclickSetting()
    {
        //GameList.Instance.Setting_Return();
    }

    public void OnclickShop()
    {
        //GameList.Instance.Shop_Return();

    }

    private void LoadUI()
	{
		this.LoadData();
		this.InitMap();
		this.InitLife();
		this.InPageAni();

    }

    private void LoadData()
	{
		this.m_saveFingerPos = this.m_img_finger.transform.localPosition;
		this.m_maps = this.m_nextMap[this.m_step];
	}

	private void InitMap()
	{
		for (int i = 0; i < this.m_maps.Length; i++)
		{
			int num = this.m_maps[i];
			if (num == 0)
			{
				this.m_blocks.Add(null);
			}
			else
			{
                G1Block g = this.CreateBlock(num, i, this.gameBox);
				this.SetPosition(g, i);
				this.m_blocks.Add(g);
				if (i != 1)
				{
					g.RemoveClick();
				}
			}
		}
	}

	private void InitLife()
	{
		for (int i = 0; i < this.m_lifes.Length; i++)
		{
			int number = this.m_lifes[i];
            G1BlockEvent g = this.CreateNewLife(number, this.bloodBox, i);
			g.transform.localPosition = this.bloodPosList[i].transform.localPosition;
			g.transform.localScale = this.bloodPosList[i].transform.localScale;
			this.bloodList[i] = g.gameObject;
		}
	}

	private void InPageAni()
	{
		this.SetTipsText("TXT_NO_50043");
		this.m_img_finger.gameObject.SetActive(false);
		Vector3 localPosition = this.gameBox.transform.localPosition;
		Vector3 localPosition2 = this.bloodBox.transform.localPosition;
		this.gameBox.transform.localPosition = new Vector3(localPosition.x - 500f, localPosition.y, localPosition.z);
		this.bloodBox.transform.localPosition = new Vector3(localPosition2.x + 500f, localPosition2.y, localPosition2.z);
		this.gameBox.transform.DOLocalMove(localPosition, 1f, false).SetEase(Ease.OutBack);
		this.bloodBox.transform.DOLocalMove(localPosition2, 1f, false).SetEase(Ease.OutBack).OnComplete(delegate
		{
			this.OnTouchScreen();
			this.m_isPuase = false;
		});
	}

	public void OnClick()
	{
		if (this.m_isPuase)
		{
			return;
		}
		this.m_step++;
		this.m_isPuase = true;
		this.StopFingerAni();
		switch (this.m_step)
		{
		case 2:
			this.MoveBloodToMap(this.m_blocks[1], 1);
			break;
		case 4:
			this.MoveBloodToMap(this.m_blocks[0], 0);
			break;
		case 5:
			this.MoveBloodToMap(this.m_blocks[2], 2);
			break;
		case 7:
			this.MoveBloodToMap(this.m_blocks[1], 1);
			break;
		case 9:
			this.MoveBloodToMap(this.m_blocks[1], 1);
			break;
		case 11:
			this.MoveBloodToMap(this.m_blocks[1], 1);
			break;
		}
		AudioManager.GetInstance().PlayEffect("sound_eff_click_1");
	}

	private void MoveBloodToMap(G1Block toObj, int idx)
	{
		int blood = this.GetBlood();
		this.m_maps[idx] += blood;
		int number = this.m_maps[idx];
		GameObject obj = this.bloodList[0];
		obj.transform.SetParent(base.transform);
		Sequence _sequence = DOTween.Sequence();
        _sequence.Append(obj.transform.DOMove(toObj.transform.position, 0.2f, false));
        _sequence.InsertCallback(0.2f, delegate
		{
			toObj.setNum(number);
			switch (this.m_step)
			{
			case 5:
			case 7:
			case 9:
			case 11:
				this.Delete();
				break;
			case 6:
			case 8:
			case 10:
				break;
			default:
				return;
			}
		});
        _sequence.AppendCallback(delegate
		{
			obj.GetComponent<G1BlockEvent>().FadeOut(toObj.GetCurrentColor());
		});
        _sequence.Append(obj.transform.DOScale(1.5f, 0.5f));
        _sequence.OnComplete(delegate
		{
			UnityEngine.Object.Destroy(obj);
			int step = this.m_step;
			if (step == 2)
			{
				this.Drop();
				return;
			}
			if (step != 4)
			{
				return;
			}
			this.SetTipsText("TXT_NO_50046");
			this.PlayFingerMoveAni(2);
			this.m_isPuase = false;
		});
		for (int i = 0; i < 5; i++)
		{
			if (i + 1 < 5)
			{
				this.bloodList[i] = this.bloodList[i + 1];
				if (this.bloodList[i + 1] != null)
				{
					this.bloodList[i + 1].transform.DOKill(false);
					this.bloodList[i + 1].transform.DOLocalMove(this.bloodPosList[i].transform.localPosition, 0.2f, false);
					this.bloodList[i + 1].transform.DOScale(this.bloodPosList[i].transform.localScale, 0.2f);
					this.bloodList[i + 1].GetComponent<G1BlockEvent>().DisableDrag = false;
				}
			}
			else
			{
				this.bloodList[i] = null;
			}
		}
	}

	private void Delete()
	{
		Sequence sequence = DOTween.Sequence();
		switch (this.m_step)
		{
		case 5:
			for (int i = 0; i < this.m_maps.Length; i++)
			{
				if (i != 2)
				{
                    G1Block block = this.m_blocks[i];
					sequence.Insert((float)i * 0.3f, block.transform.DOLocalMove(this.GetToPosition(i + 1), 0.3f, false).OnComplete(delegate
					{
						UnityEngine.Object.Destroy(block.gameObject);
					}));
				}
			}
			sequence.OnComplete(delegate
			{
				this.m_maps[2] = this.m_maps[2] + 1;
				this.m_blocks[2].setNum(this.m_maps[2]);
				this.AddNewLife();
				this.Drop();
			});
			return;
		case 6:
		case 8:
		case 10:
			break;
		case 7:
		case 9:
		case 11:
			for (int j = 0; j < this.m_maps.Length; j++)
			{
				if (j != 1)
				{
                    G1Block block = this.m_blocks[j];
					sequence.Insert(0f, block.transform.DOLocalMove(this.GetToPosition(1), 0.3f, false).OnComplete(delegate
					{
						UnityEngine.Object.Destroy(block.gameObject);
					}));
				}
			}
			sequence.OnComplete(delegate
			{
				this.m_maps[1] = this.m_maps[1] + 1;
				this.m_blocks[1].setNum(this.m_maps[1]);
				this.AddNewLife();
				this.Drop();
			});
			break;
		default:
			return;
		}
	}

	private void Drop()
	{
		this.m_step++;
		Dictionary<int, int> dictionary = new Dictionary<int, int>
		{
			{
				3,
				1
			},
			{
				6,
				2
			},
			{
				8,
				3
			},
			{
				10,
				4
			},
			{
				12,
				5
			}
		};

		int[] array = this.m_nextMap[dictionary[this.m_step]];
		Sequence sequence = DOTween.Sequence();
		for (int i = 0; i < array.Length; i++)
		{
			int num = array[i];
			if (num != 0)
			{
                G1Block g = this.CreateBlock(num, i, this.gameBox);
				this.SetPosition(g, 1, i);
				this.m_blocks[i] = g;
				sequence.Insert(0f, g.transform.DOLocalMove(this.GetToPosition(i), 0.3f, false));
				this.m_maps[i] = num;
			}
		}
		sequence.OnComplete(delegate
		{
			int step = this.m_step;
			if (step != 3)
			{
				switch (step)
				{
				case 6:
				{
					this.SetTipsText("TXT_NO_50047");
					DOTween.Kill(this.m_img_finger, false);
					Sequence _sequence = DOTween.Sequence();
                            _sequence.AppendCallback(delegate
					{
						this.m_img_finger.transform.localPosition = this.m_mask.transform.InverseTransformPoint(this.m_blocks[1].transform.position) + this.m_fingerRect2;
						this.m_img_finger.gameObject.SetActive(true);
					});
                            _sequence.Append(this.m_img_finger.transform.DOBlendableLocalMoveBy(new Vector3(0f, -10f, 0f), 0.5f, false));
                            _sequence.Append(this.m_img_finger.transform.DOBlendableLocalMoveBy(new Vector3(0f, 10f, 0f), 0.5f, false));
                            _sequence.SetLoops(-1);
                            _sequence.SetTarget(this.m_img_finger);
					this.m_isPuase = false;
					return;
				}
				case 8:
				{
					this.m_transformList.Add(new G1BoardManager.TransformControl(this.m_blocks[1].transform.parent, this.m_blocks[1].transform));
					this.m_transformList.Add(new G1BoardManager.TransformControl(this.m_img_finger.transform.parent, this.m_img_finger.transform));
					this.ToMask(this.m_transformList, "TXT_NO_50049", false, new Vector3(0f, -404f, 0f));
					DOTween.Kill(this.m_img_finger, false);
					Sequence _sequence = DOTween.Sequence();
                            _sequence.AppendCallback(delegate
					{
						this.m_img_finger.transform.localPosition = this.m_mask.transform.InverseTransformPoint(this.m_blocks[1].transform.position) + this.m_fingerRect2;
						this.m_img_finger.gameObject.SetActive(true);
					});
                            _sequence.Append(this.m_img_finger.transform.DOBlendableLocalMoveBy(new Vector3(0f, -10f, 0f), 0.5f, false));
                            _sequence.Append(this.m_img_finger.transform.DOBlendableLocalMoveBy(new Vector3(0f, 10f, 0f), 0.5f, false));
                            _sequence.SetLoops(-1);
                            _sequence.SetTarget(this.m_img_finger);
					this.m_isPuase = false;
					return;
				}
				case 10:
				{
					this.m_transformList.Add(new G1BoardManager.TransformControl(this.m_blocks[1].transform.parent, this.m_blocks[1].transform));
					this.m_transformList.Add(new G1BoardManager.TransformControl(this.m_img_finger.transform.parent, this.m_img_finger.transform));
					this.ToMask(this.m_transformList, "TXT_NO_50050", false, new Vector3(0f, -404f, 0f));
					DOTween.Kill(this.m_img_finger, false);
					Sequence _sequence = DOTween.Sequence();
                            _sequence.AppendCallback(delegate
					{
						this.m_img_finger.transform.localPosition = this.m_mask.transform.InverseTransformPoint(this.m_blocks[1].transform.position) + this.m_fingerRect2;
						this.m_img_finger.gameObject.SetActive(true);
					});
                            _sequence.Append(this.m_img_finger.transform.DOBlendableLocalMoveBy(new Vector3(0f, -10f, 0f), 0.5f, false));
                            _sequence.Append(this.m_img_finger.transform.DOBlendableLocalMoveBy(new Vector3(0f, 10f, 0f), 0.5f, false));
                            _sequence.SetLoops(-1);
                            _sequence.SetTarget(this.m_img_finger);
					this.m_isPuase = false;
					return;
				}
				case 12:
				{
					Sequence _sequence = DOTween.Sequence();
                            _sequence.AppendInterval(0.5f);
                            _sequence.AppendCallback(delegate
					{
                        this.StartGame();
                    });
					return;
				}
				}
				this.m_isPuase = false;
				return;
			}
			this.SetTipsText("TXT_NO_50045");
			this.PlayFingerMoveAni(0);
			this.m_isPuase = false;
		});
	}

	private void AddNewLife()
	{
		int num = 0;
		int i = 0;
		while (i < 5)
		{
			if (this.m_lifes[i] == 0)
			{
				num = i;
				int step = this.m_step;
				if (step == 5)
				{
					this.m_lifes[i] = -2;
					break;
				}
				this.m_lifes[i] = 1;
				break;
			}
			else
			{
				i++;
			}
		}
		int number = this.m_lifes[num];
        G1BlockEvent g = this.CreateNewLife(number, this.bloodBox, num);
		g.transform.localPosition = this.bloodPosList[num].transform.localPosition;
		g.transform.localScale = num == 0 ? new Vector3(1.3f, 1.3f, 1.3f) : new Vector3(1f, 1f, 1f);
		this.bloodList[num] = g.gameObject;
		g.transform.DOScale(this.bloodPosList[num].transform.localScale, 0.3f);
	}

	private void PlayFingerAni()
	{
		this.m_img_finger.gameObject.SetActive(true);
		this.m_img_finger.transform.localPosition = this.m_saveFingerPos;
		DOTween.Kill(this.m_img_finger, false);
		Sequence _sequence = DOTween.Sequence();
        _sequence.Append(this.m_img_finger.transform.DOBlendableLocalMoveBy(new Vector3(0f, -10f, 0f), 0.5f, false));
        _sequence.Append(this.m_img_finger.transform.DOBlendableLocalMoveBy(new Vector3(0f, 10f, 0f), 0.3f, false));
        _sequence.SetLoops(-1);
        _sequence.SetTarget(this.m_img_finger);
	}

	private int GetBlood()
	{
		int result = this.m_lifes[0];
		for (int i = 0; i < this.m_lifes.Length; i++)
		{
			if (i + 1 < this.m_lifes.Length)
			{
				this.m_lifes[i] = this.m_lifes[i + 1];
			}
			else
			{
				this.m_lifes[i] = 0;
			}
		}
		return result;
	}

	private void SetPosition(G1Block block, int index)
	{
		int row = index / 5;
		int col = index % 5;
		this.SetPosition(block, row, col);
	}

	private void SetPosition(G1Block block, int row, int col)
	{
        Debug.Log(col + "       "+row);
		block.transform.localPosition = new Vector3((float)(col * 160 - 160), (float)(row * 160 ), 0f);
    }

	private Vector3 GetToPosition(int index)
	{
		int num = 0;
		return new Vector3((float)(index * 160 - 160), (float)(num * 160), 0f);
    }

	private G1Block CreateBlock(int number, int idx, GameObject parent)
	{
		GameObject _object = this.CreateBlock();
        _object.SetActive(true);
        _object.GetComponent<G1Block>().Init(number, idx);
        _object.GetComponent<G1Block>().RemoveClick();
        _object.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnClick));
        _object.transform.SetParent(parent.transform, false);
		return _object.GetComponent<G1Block>();
	}

	private GameObject CreateBlock()
	{
		return UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/G00101") as GameObject);
	}

	private G1BlockEvent CreateNewLife(int number, GameObject parent, int idx)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/G00105") as GameObject);
		gameObject.transform.SetParent(parent.transform, false);
        G1BlockEvent _block = gameObject.GetComponent<G1BlockEvent>();
        _block.Init(number, idx);
        _block.OnDownHandle = new Action<GameObject, PointerEventData>(this.onBegainDragLife);
        _block.OnDragHandle = new Action<GameObject, PointerEventData>(this.OnDragLife);
        _block.OnUpHandle = new Action<GameObject, PointerEventData>(this.OnEndDragLife);
		this.bloodList[idx] = gameObject.gameObject;
		return _block;
	}

	private void onBegainDragLife(GameObject obj, PointerEventData eventData)
	{
		if (obj != this.bloodList[0])
		{
			return;
		}
		obj.transform.DOKill(false);
		obj.transform.DOScale(1f, 0.1f);
		obj.transform.SetParent(base.transform);
	}

	private void OnDragLife(GameObject obj, PointerEventData eventData)
	{
		UnityEngine.Debug.Log("OnDragLife");
		if (obj != this.bloodList[0])
		{
			return;
		}
		if (this.m_isPuase)
		{
			return;
		}
		this.StopFingerAni();
		Vector2 a;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(base.transform.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out a);
		obj.transform.localPosition = a + new Vector2(0f, 100f);
	}

	private void OnEndDragLife(GameObject obj, PointerEventData eventData)
	{
		if (obj != this.bloodList[0])
		{
			return;
		}
		Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, obj.transform.position);
		if (RectTransformUtility.RectangleContainsScreenPoint(this.gameBox.GetComponent<RectTransform>(), screenPoint, eventData.pressEventCamera))
		{
			Vector2 vector = this.gameBox.GetComponent<RectTransform>().InverseTransformPoint(obj.transform.position);
			Vector2 expr_79 = this.gameBox.GetComponent<RectTransform>().sizeDelta;
			float num = expr_79.x / 2f + vector.x;
			double arg_B3_0 = (double)(expr_79.y / 2f + vector.y);
			int num2 = (int)Math.Floor((double)(num / 160));
			int num3 = (int)Math.Floor(arg_B3_0 / (double)160f);
			if (num2 < 0 || num2 >= 5)
			{
				return;
			}
			if (num3 < 0 || num3 >= 5)
			{
				return;
			}
			switch (this.m_step)
			{
			case 1:
				if (num2 == 1)
				{
					this.OnClick();
					return;
				}
				this.BackToInitPosition(obj, 1);
				return;
			case 2:
			case 5:
			case 7:
			case 9:
				break;
			case 3:
				if (num2 == 0)
				{
					this.OnClick();
					return;
				}
				this.BackToInitPosition(obj, 0);
				return;
			case 4:
				if (num2 == 2)
				{
					this.OnClick();
					return;
				}
				this.BackToInitPosition(obj, 2);
				return;
			case 6:
				if (num2 == 1)
				{
					this.OnClick();
					return;
				}
				this.BackToInitPosition(obj, 1);
				return;
			case 8:
				if (num2 == 1)
				{
					this.OnClick();
					return;
				}
				this.BackToInitPosition(obj, 1);
				return;
			case 10:
				if (num2 == 1)
				{
					this.OnClick();
					return;
				}
				this.BackToInitPosition(obj, 1);
				return;
			default:
				return;
			}
		}
		else
		{
			switch (this.m_step)
			{
			case 1:
				this.BackToInitPosition(obj, 1);
				return;
			case 2:
			case 5:
			case 7:
			case 9:
				break;
			case 3:
				this.BackToInitPosition(obj, 0);
				return;
			case 4:
				this.BackToInitPosition(obj, 2);
				return;
			case 6:
				this.BackToInitPosition(obj, 1);
				return;
			case 8:
				this.BackToInitPosition(obj, 1);
				return;
			case 10:
				this.BackToInitPosition(obj, 1);
				break;
			default:
				return;
			}
		}
	}

	private void BackToInitPosition(GameObject obj, int idx)
	{
		obj.GetComponent< G1BlockEvent>().DisableDrag = true;
		obj.transform.DOKill(false);
		obj.transform.SetParent(this.bloodBox.transform);
		obj.transform.DOLocalMove(this.bloodPosList[0].transform.localPosition, 0.1f, false);
		obj.transform.DOScale(this.bloodPosList[0].transform.localScale, 0.1f).OnComplete(delegate
		{
			obj.GetComponent<G1BlockEvent>().DisableDrag = false;
			this.PlayFingerMoveAni(idx);
		});
	}

	private void ToMask(List<G1BoardManager.TransformControl> list, string txt, bool isOut, Vector3 tipsPos)
	{
		this.m_mask.transform.Find("txt").GetComponent<LanguageComponent>().SetText(txt);
	}

	private void SetTipsText(string txt)
	{
        Debug.Log("Tip : "+txt);
		this.m_tips01.GetComponent<LanguageComponent>().SetText(txt);
	}

	private void PlayFingerMoveAni(int endIdx)
	{
		DOTween.Kill(this.m_img_finger, false);
		Sequence _sequence = DOTween.Sequence();
        _sequence.AppendCallback(delegate
		{
			this.m_img_finger.transform.localPosition = this.m_mask.transform.InverseTransformPoint(this.bloodList[0].transform.position) + this.m_fingerRect2;
			this.m_img_finger.gameObject.SetActive(true);
		});
        _sequence.Append(this.m_img_finger.transform.DOLocalMove(this.m_mask.transform.InverseTransformPoint(this.m_blocks[endIdx].transform.position) + this.m_fingerRect2, 1f, false));
        _sequence.AppendInterval(0.5f);
        _sequence.SetLoops(-1);
        _sequence.SetTarget(this.m_img_finger);
	}

	private void StopFingerAni()
	{
		DOTween.Kill(this.m_img_finger, false);
		this.m_img_finger.gameObject.SetActive(false);
	}

	public G1BoardManager()
	{
		int[][] _arr1 = new int[6][];
		int num1 = 0;
		int[] _arr3 = new int[3];
        _arr3[1] = 1;
        _arr1[num1] = _arr3;
        _arr1[1] = new int[]
		{
			1,
			0,
			1
		};
		int num2 = 2;
		int[] _arr2 = new int[3];
        _arr2[0] = 3;
        _arr2[1] = 1;
        _arr1[num2] = _arr2;
        _arr1[3] = new int[]
		{
			3,
			0,
			3
		};
        _arr1[4] = new int[]
		{
			2,
			0,
			2
		};
        _arr1[5] = new int[]
		{
			2,
			0,
			2
		};
		this.m_nextMap = _arr1;
        this.m_isPuase = true;
		this.m_saveFingerPos = Vector3.zero;
		this.m_transformList = new List<G1BoardManager.TransformControl>();
		this.m_fingerRect = new Vector3(70f, -70f);
		this.m_fingerRect2 = new Vector3(100f, -70f);
		
	}
}
