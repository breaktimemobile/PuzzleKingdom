using Assets.Scripts.GameManager;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * this class will attached to category object, you will chose difficult of levels from this object
 */
public class G3Category : MonoBehaviour
{
	public static int STAR_WIDTH = 35;

	public Image Img_bg;

    public Image Img_bg_1;

    public Image Progress;

	public Text Desc;

	public Text Label;

    public Text txt_star;

    public Text txt_title;

    public Button btn_start;

    public GameObject obj_new;

    public List<Sprite> top = new List<Sprite>();

    public List<Color> colors = new List<Color>();

	private int m_id;

    private int m_fullNum;

	private bool m_isActivity = true;

	public int Id
	{
		get
		{
			return this.m_id;
		}
		set
		{
			this.m_id = value;
		}
	}

	public int FullNum
	{
		get
		{
			return this.m_fullNum;
		}
		set
		{
			this.m_fullNum = value;
		}
	}

	public bool IsActivity
	{
		get
		{
			return this.m_isActivity;
		}
		set
		{
			this.m_isActivity = value;
		}
	}

    public void Init(int id = 0, float lv = 0, int num = 0, int maxnum = 0, string desc = "")
	{
		this.Id = id;
        this.SetMaxNum(maxnum);
		this.SetImgStar(lv);
		this.SetLabel(num);
		this.SetDesc(desc);
		this.SetProgress((float)num / (float)maxnum);
	}

	public void OnClick()
	{
		if (this.IsActivity)
		{
            G3MapData.GetInstance().DoClickLvHandler(this);
		}
		else
		{
            Debug.Log("ÀÌ°ÅÂ¡");
			ToastManager.Show("TXT_NO_50055", true);
		}
		AudioManager.GetInstance().PlayEffect("sound_eff_button");
	}

	public void SetIsUnLock(bool isUnlock, int num)
	{
		this.IsActivity = isUnlock;
        Img_bg.color = new Color(Img_bg.color.r, Img_bg.color.g, Img_bg.color.b, isUnlock ? 1 : 0.8f);
        btn_start.interactable = isUnlock;

        btn_start.transform.DOKill();

        if (isUnlock)
        btn_start.transform.DOScale(1.05f, 1.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

        obj_new.SetActive(num == 0 && isUnlock);
    }

    public void SetMaxNum(int max)
	{
		this.FullNum = max;
	}

	public void SetImgStar(float star)
	{
		if (star < 0 || star > 5)
		{
			return;
		}

        txt_star.text = "x" + star;
        Img_bg_1.sprite = top[(int)star-1];

        foreach (var item in txt_star.GetComponents<Outline>())
        {
            item.effectColor = colors[(int)star - 1];
        }

        foreach (var item in txt_title.GetComponents<Outline>())
        {
            item.effectColor = colors[(int)star - 1];
        }

    }

	public void SetProgress(float perc)
	{
		this.Progress.fillAmount = perc;


	}

	public void SetDesc(string lang)
	{
		this.Desc.GetComponent<LanguageComponent>().SetText(lang);
	}

	public void SetLabel(int num)
	{
		this.Label.text = string.Format("{0}/{1}", num, this.FullNum);

    }
}
