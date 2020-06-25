using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/*
 * This scripts will be attached to bottom blocks (blood blocks) that be drag to the board to get score
 */
public class G1BlockEvent : MonoBehaviour
{
   //number of block
	private int number = 1;
    //index block in list
	private int index = -1;

    //list color of block depend on value of number
	[SerializeField]
	public List<Sprite> base_sp = new List<Sprite>();
    //image background of block
	public Image img_block;
    //number text of block
	public Text txt_number;

	public GameObject prefabScore;

	private bool m_disableDrag = true;
    //handle for drag action
	public Action<GameObject, PointerEventData> OnDragHandle;

	public Action<GameObject, PointerEventData> OnBegainDragHandle;

	public Action<GameObject, PointerEventData> OnEndDragHandle;

	public Action<GameObject, PointerEventData> OnDownHandle;

	public Action<GameObject, PointerEventData> OnUpHandle;

	public int Number
	{
		get
		{
			return this.number;
		}
		set
		{
			this.number = value;
		}
	}

	public int Index
	{
		get
		{
			return this.index;
		}
		set
		{
			this.index = value;
		}
	}

	public bool DisableDrag
	{
		get
		{
			return this.m_disableDrag;
		}
		set
		{
			this.m_disableDrag = value;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Init(int number, int idx)
	{
		this.index = idx;
		this.setNum(number);
		this.SetPosition(idx);
		transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
		Color color = this.img_block.color;
		color.a = 1f;
		this.img_block.color = color;
		this.img_block.gameObject.SetActive(true);
		color = this.txt_number.color;
		color.a = 1f;
		this.txt_number.color = color;
		this.txt_number.gameObject.SetActive(true);

    }

    public void setNum(int number)
	{
		this.Number = number;
		this.txt_number.gameObject.SetActive(true);
		this.img_block.sprite = (this.Number > 0) ? base_sp[0] : base_sp[1];
        string arg = (this.Number > 0) ? "+" : "-";
		this.txt_number.text = arg + Math.Abs(this.Number);
	}

	public Color GetCurrentColor()
	{
		return this.img_block.color;
	}

	public void ShowSymbol()
	{
		string arg = (this.Number > 0) ? "+" : "-";
		this.txt_number.text = arg + Math.Abs(this.Number);
	}

	public void SetPosition(int index)
	{
		int row = index / 5;
		int col = index % 5;
		this.SetPosition(row, col);

    }

    public void SetPosition(int row, int col)
	{
		base.transform.localPosition = new Vector3((float)(col * 120 + 60 - 300), (float)(row * 120 + 60 - 300), 0f);
	}

	public Tween Move(int index)
	{
		return base.transform.DOLocalMove(this.GetToPosition(index), 0.3f, false);
	}

	public Tween DelayMove(int index, float time)
	{
		return base.transform.DOLocalMove(this.GetToPosition(index), 0.1f, false).SetDelay(time).OnComplete(delegate
		{
            Game1DataLoader.GetInstance().FreeBlock(base.gameObject);
		});
	}

	public Sequence DoDeleteAni()
	{
		Sequence arg_50_0 = DOTween.Sequence();
		Tween t = base.transform.DORotate(new Vector3(0f, 0f, -360f), 0.5f, RotateMode.FastBeyond360);
		Tween t2 = base.transform.DOScale(new Vector3(0f, 0f, 0.5f), 0.5f);
		arg_50_0.Insert(0f, t);
		arg_50_0.Insert(0f, t2);
		return arg_50_0;
	}

	public Sequence FadeOut(Color color)
	{
		this.img_block.color = color;
		this.txt_number.gameObject.SetActive(false);
		Sequence expr_22 = DOTween.Sequence();
		expr_22.Insert(0f, this.img_block.DOFade(0f, 0.8f));
		expr_22.Insert(0f, this.txt_number.DOFade(0f, 0.8f));
		expr_22.SetTarget(this);
		return expr_22;
	}

	public void StopFade()
	{
		DOTween.Kill(this, false);
	}

	public void ShowScore()
	{
        
		GameObject obj = UnityEngine.Object.Instantiate<GameObject>(this.prefabScore);
		obj.transform.SetParent(base.transform.parent.parent, false);
		obj.transform.position = base.transform.position;
		obj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
		Text component = obj.transform.Find("txt").GetComponent<Text>();
		component.text = "+" + (this.Number * 10).ToString();
		Sequence expr_B8 = DOTween.Sequence();
		expr_B8.Append(obj.transform.DOScale(1.2f, 0.2f));
		expr_B8.Append(obj.transform.DOScale(1f, 0.1f));
		expr_B8.Append(obj.transform.DOBlendableLocalMoveBy(new Vector3(0f, 30f, 0f), 1f, false).SetDelay(0.3f).SetEase(Ease.OutBack).OnComplete(delegate
		{
			UnityEngine.Object.Destroy(obj);
		}));
		expr_B8.Insert(0.6f, component.DOFade(0f, 1f));
	}

	public void OnClick()
	{
	}

	private Vector3 GetToPosition(int row, int col)
	{
		return new Vector3((float)(col * 120 + 60 - 300), (float)(row * 120 + 60 - 300), 0f);
	}

	private Vector3 GetToPosition(int index)
	{
        transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);

        int row = Game1DataLoader.GetInstance().GetRow(index);
		int col = Game1DataLoader.GetInstance().GetCol(index);
		return this.GetToPosition(row, col);
	}

	public void OnDrag(BaseEventData eventData)
	{
		if (this.m_disableDrag)
		{
			return;
		}
		Action<GameObject, PointerEventData> expr_0F = this.OnDragHandle;
		if (expr_0F == null)
		{
			return;
		}
		expr_0F(base.gameObject, (PointerEventData)eventData);
	}

	public void OnEndDrag(BaseEventData eventData)
	{
		if (this.m_disableDrag)
		{
			return;
		}
		Action<GameObject, PointerEventData> expr_0F = this.OnEndDragHandle;
		if (expr_0F == null)
		{
			return;
		}
		expr_0F(base.gameObject, (PointerEventData)eventData);
	}

	public void OnBeginDrag(BaseEventData eventData)
	{
		if (this.m_disableDrag)
		{
			return;
		}
		Action<GameObject, PointerEventData> expr_0F = this.OnBegainDragHandle;
		if (expr_0F == null)
		{
			return;
		}
		expr_0F(base.gameObject, (PointerEventData)eventData);
	}

	public void OnPointerDown(BaseEventData eventData)
	{
		this.m_disableDrag = false;
		Action<GameObject, PointerEventData> expr_0D = this.OnDownHandle;
		if (expr_0D == null)
		{
			return;
		}
		expr_0D(base.gameObject, (PointerEventData)eventData);
	}

	public void OnPointerUp(BaseEventData eventData)
	{
		if (this.m_disableDrag)
		{
			return;
		}
		Action<GameObject, PointerEventData> expr_0F = this.OnUpHandle;
		if (expr_0F == null)
		{
			return;
		}
		expr_0F(base.gameObject, (PointerEventData)eventData);
	}
}
