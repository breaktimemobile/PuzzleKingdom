using Assets.Scripts.Configs;
using Assets.Scripts.GameManager;
using System;
using System.Collections.Generic;
using UnityEngine;

public class G3MapData : MonoBehaviour
{
    /// <summary>
    /// this class will process category model parameter and create data for game
    /// </summary>
    /// total lines that connect block(check point row)
	public const int CHECK_POINT_ROW = 4;
    //height of cell(block)
	public const int CHECK_POINT_CELL_HEIGHT = 150;
    //width of cell(blocl)
	public const int CHECK_POINT_CELL_WIDTH = 150;

	public const int LV_CELL_HEIGHT = 150;
    //action if you click to level item
	public Action<G3LevelItem> DoClickCheckPointHandler;
    //action if you click to category
	public Action<G3Category> DoClickLvHandler;

	private Dictionary<string, int> m_lv_score = new Dictionary<string, int>();

	private int m_lv;

	private int m_checkPoint;

	private int m_topCheckPoint;

	private Dictionary<string, G3CateroryModel> mConfig;

	private Dictionary<string, LevelModel> m_config_G00301;

	private int m_gamebox_height = 113;

	private static G3MapData m_instance;

	internal Dictionary<string, G3CateroryModel> modelConfig
	{
		get
		{
			return this.mConfig;
		}
		set
		{
			this.mConfig = value;
		}
	}

	internal Dictionary<string, LevelModel> levelConfig
	{
		get
		{
			return this.m_config_G00301;
		}
		set
		{
			this.m_config_G00301 = value;
		}
	}

	public Dictionary<string, int> Lv_score
	{
		get
		{
			return this.m_lv_score;
		}
		set
		{
			this.m_lv_score = value;
		}
	}

	public int Lv
	{
		get
		{
			return this.m_lv;
		}
		set
		{
			this.m_lv = value;
		}
	}

	public int Gamebox_height
	{
		get
		{
			return this.m_gamebox_height;
		}
		set
		{
			this.m_gamebox_height = value;
		}
	}

	public int CheckPoint
	{
		get
		{
			return this.m_checkPoint;
		}
		set
		{
			this.m_checkPoint = value;
		}
	}

	public int TopCheckPoint
	{
		get
		{
			return this.m_topCheckPoint;
		}
		set
		{
			this.m_topCheckPoint = value;
		}
	}

	public static G3MapData GetInstance()
	{
		return G3MapData.m_instance;
	}

	private void Awake()
	{
        G3MapData.m_instance = this;
	}

	private void Start()
	{
		GM.GetInstance().SaveG003ScoreRecord(301, 0);
		this.LoadConfig();
		this.LoadScore();
	}

	public void LoadConfig()
	{
		this.modelConfig = Configs._G3CateroryModel;
		this.levelConfig = Configs.TG00301;
	}

	public void LoadScore()
	{
		this.Lv_score.Clear();
		this.TopCheckPoint = GM.GetInstance().GetScoreRecord(3);
		this.TopCheckPoint = ((this.TopCheckPoint == 0) ? 30000 : this.TopCheckPoint);
		foreach (KeyValuePair<string, G3CateroryModel> current in this.modelConfig)
		{
			this.Lv_score.Add(current.Value.ID.ToString(), GM.GetInstance().GetG003ScoreRecord(current.Value.ID));
		}
	}

	public List<string> GetcheckPointByLv(int lv)
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, LevelModel> current in this.levelConfig)
		{
			if (current.Value.G003 == lv)
			{
				list.Add(current.Key);
			}
		}
		return list;
	}

	public void StartNewGame(string idx)
	{
		GlobalEventHandle.EmitClickPageButtonHandle("Game03", this.levelConfig[idx].ID);
	}

	public void UnlockLv(int lv)
	{
		GM.GetInstance().SaveG003ScoreRecord(lv, 0);
	}

	public int GetRow(int index)
	{
		return index / 4;
	}

	public int GetCol(int index)
	{
		return index % 4;
	}

	public int GetCheckPointCount(int lv = 0)
	{
		if (lv != 0)
		{
			return this.GetcheckPointByLv(lv).Count;
		}
		return this.levelConfig.Count;
	}

	public int GetLvCount()
	{
		return this.modelConfig.Count;
	}

	public int GetGameBoxHeight(int lv = 0)
	{
		return this.GetCheckPointCount(lv) / 4 * 150;
	}

	public int GetGameBoxHeightEx(int lv = 0)
	{
		return (this.GetCheckPointCount(lv) / 4 + ((this.GetCheckPointCount(0) % 4 != 0) ? 2 : 1)) * 150;
	}

	public int GetLvBoxHeight()
	{
		return this.GetLvCount() * 150;
	}
}
