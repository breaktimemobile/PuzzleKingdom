using Assets.Scripts.GameManager;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class G2Block : MonoBehaviour
{
   /*
    * This class will be attached to block with number
    */  
	private int number = 1;

	private int index = -1;

    //list sprite used for block (in fact, we don't use too much sprite for block)
    //[SerializeField]
    //public List<Sprite> sprites = new List<Sprite>();


    //list color used for block, it depend on number of block
	[SerializeField]
	public List<Sprite> friut = new List<Sprite>();
    //background image of block
	public Image img_block;
    public GameObject img_panel;

    //the number of block
    public Text txt_number;
    //score object appear if you get points
	public GameObject prefabScore;
    //font size used for number text

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

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Init(int number, int idx)
	{
        Debug.Log("���ۺ��");
		this.index = idx;
		this.setNum(number);
		this.SetPosition(idx);
		base.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	public void setNum(int number)
	{
        Debug.Log("�� �ѹ� " + number);
		this.Number = number;
		this.txt_number.text = number.ToString();
		int num = 1;
		int num2 = number % 2;
		int num3 = 2;
		while (number != 0 && num2 == 0 && number != num3)
		{
			num++;
			num3 = (int)Math.Pow(2.0, (double)num);
		}
		num = ((num < this.friut.Count) ? num : (this.friut.Count - 1));
		this.img_block.sprite = friut[num % this.friut.Count];
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
        Debug.Log("row " + row +"  col " + col);
		base.transform.localPosition = new Vector3((float)col * 160f + 55f - 375f, (float)row * 160f + 55f - 440f, 0f);
       // base.transform.localPosition = new Vector3((float)col * 160f + 55f - 275f, (float)row * 160f + 55f - 385f, 0f);

    }

    public Color GetCurrentColor()
	{
		return this.img_block.color;
	}

	public Tween Move(int index)
	{
		return base.transform.DOLocalMove(this.GetToPosition(index), 0.3f, false);
	}

	public Tween DelayMove(int index, float time)
	{
		return base.transform.DOLocalMove(this.GetToPosition(index), 0.1f, false).SetDelay(time).OnComplete(delegate
		{
            G2BoardGenerator.GetInstance().FreeBlock(base.gameObject);
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

	public void ShowScore()
	{
		GameObject obj = UnityEngine.Object.Instantiate<GameObject>(this.prefabScore);
		obj.transform.SetParent(base.transform.parent.parent, false);
		obj.transform.position = base.transform.position;
		obj.transform.Find("txt").GetComponent<Text>().text = "+" + (Math.Log((double)this.Number, 2.0) * 10.0).ToString();
		obj.transform.DOBlendableLocalMoveBy(new Vector3(0f, 30f, 0f), 1f, false).SetEase(Ease.OutBack).OnComplete(delegate
		{
			UnityEngine.Object.Destroy(obj);
		});
	}

	public Sequence FadeOut()
	{
		this.txt_number.gameObject.SetActive(false);
		Sequence expr_16 = DOTween.Sequence();
		expr_16.Insert(0f, this.img_block.DOFade(0f, 0.8f));
		expr_16.Insert(0f, this.txt_number.DOFade(0f, 0.8f));
		return expr_16;
	}

	public Sequence FadeIn()
	{
		this.txt_number.gameObject.SetActive(true);
		Sequence expr_16 = DOTween.Sequence();
		expr_16.Insert(0f, this.img_block.DOFade(1f, 0f));
		expr_16.Insert(0f, this.txt_number.DOFade(1f, 0f));
		return expr_16;
	}

	public void HideNumber()
	{
		this.txt_number.gameObject.SetActive(false);
	}

	public void OnClick()
	{
		if (G2BoardGenerator.GetInstance().IsPlaying)
		{
			return;
		}
		if (!G2BoardGenerator.GetInstance().IsPuase)
		{
			return;
		}
		AudioManager.GetInstance().PlayEffect("sound_eff_click_1");
        G2BoardGenerator.GetInstance().DoClickBlock(this);
	}

	public void RemoveClick()
	{
		if (base.GetComponent<Button>() == null)
		{
			return;
		}
		UnityEngine.Object.Destroy(base.GetComponent<Button>());
	}

	public void AddClickListener()
	{
		if (base.GetComponent<Button>() == null)
		{
			base.gameObject.AddComponent<Button>();
		}
		base.GetComponent<Button>().transition = Selectable.Transition.None;
		base.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnClick));
	}

	private Vector3 GetToPosition(int row, int col)
	{
		return new Vector3((float)col * 160f + 55f - 375f, (float)row * 160f + 55f - 440f, 0f);
	}

	private Vector3 GetToPosition(int index)
	{
		int row = G2BoardGenerator.GetInstance().GetRow(index);
		int col = G2BoardGenerator.GetInstance().GetCol(index);
		return this.GetToPosition(row, col);
	}
}
