using Assets.Scripts.Configs;
using Assets.Scripts.GameManager;
using Assets.Scripts.Utils;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Game3 manager.
/// </summary>
public class Game3Manager : MonoBehaviour
{
	public GameObject ScrollView;

	public GameObject ScrollViewLv;

	public GameObject[] gameboxs;

    public GameObject gamebox_LV;

	private G3MapData m_model;

	private List<G3Category> m_lvs = new List<G3Category>();

	private List<G3LevelItem> m_checkPoints = new List<G3LevelItem>();

    public GameObject[] dots;

    public Sprite dot;

    public Sprite none_dot;

    public G3MapData Model
	{
		get
		{
			return this.m_model;
		}
		set
		{
			this.m_model = value;
		}
	}

	public List<G3Category> Lvs
	{
		get
		{
			return this.m_lvs;
		}
		set
		{
			this.m_lvs = value;
		}
	}

	public List<G3LevelItem> CheckPoints
	{
		get
		{
			return this.m_checkPoints;
		}
		set
		{
			this.m_checkPoints = value;
		}
	}

	private void Start()
	{
		this.Model = G3MapData.GetInstance();
		this.InitUI();
		this.InitEvent();
	}

	private void Update()
	{
        Utils.BackListener(base.gameObject, delegate
        {
            this.OnClickReturn();
        });
    }

	public void InitUI()
	{

		this.InitLv();
		this.UpdateCheckPoint(301);
		this.ScrollView.SetActive(false);
		this.ScrollViewLv.SetActive(true);
	}

	public void InitLv()
	{
		if (this.Model.GetLvCount() > 0)
		{
			this.gamebox_LV.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, (float)(this.Model.GetLvBoxHeight() + 240));
			foreach (KeyValuePair<string, G3CateroryModel> current in this.Model.modelConfig)
			{
				GameObject expr_6C = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/G00305") as GameObject);
                G3Category component = expr_6C.GetComponent<G3Category>();
                G3CateroryModel value = current.Value;
				component.Init(value.ID, value.Star, 0, value.Count, value.Lang);
				expr_6C.SetActive(true);
				expr_6C.transform.SetParent(this.gamebox_LV.transform, false);
				this.Lvs.Add(component);
			}
		}
		this.UpdateLv();
	}

	public void UpdateLv()
	{
		foreach (G3Category current in this.Lvs)
		{
            int num = this.Model.Lv_score[current.Id.ToString()];

            current.SetLabel(0);
			current.SetProgress(0f);
			List<string> list = this.Model.GetcheckPointByLv(current.Id);
			int num2 = 0;
			foreach (string arg_8B_0 in list)
			{
				num2++;
				if (arg_8B_0 == num.ToString())
				{
					current.SetLabel(num2);
					current.SetProgress((float)num2 / (float)list.Count);
					break;
				}

            }
            current.SetIsUnLock(num != -1, num2);

        }
    }

    int content = 0;

    public void UpdateCheckPoint(int lv = 301)
	{
		this.Model.LoadScore();
		using (List<G3LevelItem>.Enumerator enumerator = this.CheckPoints.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				enumerator.Current.gameObject.SetActive(false);
			}
		}
		List<string> list = this.Model.GetcheckPointByLv(lv);
		if (list.Count > 0)
		{
			int num = 0;

            int dot_val = list.Count / 20;

            for (int k = 0; k < dots.Length; k++)
            {
                dots[k].SetActive(dot_val >= k);
            }
			foreach (string current in list)
			{
                LevelModel tG = this.Model.levelConfig[current];
				if (this.CheckPoints.Count - 1 < num)
				{

					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/G00300") as GameObject);
					gameObject.SetActive(true);
					gameObject.transform.SetParent(this.gameboxs[num/20].transform, false);
					this.CheckPoints.Add(gameObject.GetComponent<G3LevelItem>());
				}
				this.CheckPoints[num].gameObject.SetActive(true);
				this.CheckPoints[num].Init(num + 1, num, current);
				this.CheckPoints[num].SetCheckpointStatus(tG.ID <= this.Model.Lv_score[lv.ToString()] + 1);
                this.CheckPoints[num].Setnow(tG.ID == this.Model.Lv_score[lv.ToString()] + 1);

                num++;

            }

            for (int k = 0; k < gameboxs.Length; k++)
            {
                int ten = this.Model.Lv_score[lv.ToString()] % 100 / 10;
                int one = this.Model.Lv_score[lv.ToString()] % 10;

                gameboxs[k].SetActive((ten * 10 + one) / 20 == k);
                content = (ten * 10 + one) / 20;
            }

            Check_Dot();


            if (this.Model.Lv_score[lv.ToString()] == 0)
			{
				this.CheckPoints[0].SetCheckpointStatus(true);
                this.CheckPoints[0].Setnow(true);

            }
        }
		this.UpdateLv();
	}


    public void prev()
    {
        if (content - 1 >= 0)
        {
            gameboxs[content].SetActive(false);
            gameboxs[content - 1].SetActive(true);

            content -= 1;

            Check_Dot();
        }

    }


    public void Next()
    {
        int val = -1;

        for (int k = 0; k < dots.Length; k++)
        {
            if (dots[k].activeSelf)
                val += 1;
        }

        if (content + 1 <= val)
        {
            gameboxs[content].SetActive(false);
            gameboxs[content + 1].SetActive(true);

            content += 1;

            Check_Dot();
        }
    }


    public void Check_Dot()
    {
        for (int k = 0; k < dots.Length; k++)
        {
            dots[k].GetComponent<Image>().sprite = content == k ? dot : none_dot;
        }

    }

	public void InitEvent()
	{
        G3MapData _mapdata = this.Model;
        _mapdata.DoClickLvHandler = (Action<G3Category>)Delegate.Combine(_mapdata.DoClickLvHandler, new Action<G3Category>(this.DoClickLv));
        G3MapData expr_2D = this.Model;
		expr_2D.DoClickCheckPointHandler = (Action<G3LevelItem>)Delegate.Combine(expr_2D.DoClickCheckPointHandler, new Action<G3LevelItem>(this.DoClickCheckPoint));
		GlobalEventHandle.DoRefreshCheckPoint += new Action<int>(this.UpdateCheckPoint);
	}

	public void DoClickLv(G3Category btn)
	{
		this.ScrollView.SetActive(true);
		this.ScrollViewLv.SetActive(false);
		this.UpdateCheckPoint(btn.Id);
	}

	public void DoClickCheckPoint(G3LevelItem btn)
	{
		this.Model.StartNewGame(btn.Key);
		base.gameObject.SetActive(false);
	}

	public void OnClickAds()
	{
		//AdsManager.GetInstance().Play(AdsManager.AdType.Stimulate, null, null, 5, null);
	}

	public void OnClickReturn()
	{

		if (this.ScrollViewLv.activeSelf)
        {
            FindObjectOfType<MainScene>().Open_Icon();

            GlobalEventHandle.EmitClickPageButtonHandle("main", 0);
		}

		if (this.ScrollView.activeSelf)
		{
			this.ScrollView.SetActive(false);
			this.ScrollViewLv.SetActive(true);
		}
	}
    
}
