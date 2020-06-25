using Assets.Scripts.Configs;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameManager
{
	internal class GM
	{
		private static GM g_intance;

		private int diamond;

		private int lv = 1;

		private int exp;

		private int m_consumeCount;

		private int m_gameId;

		private int m_skinID = 1;

        private int m_boardSize = 0;

        private bool m_isPlayVedioAds;

		private System.Random m_random = new System.Random();

		public int Exp
		{
			get
			{
				return this.exp;
			}
			set
			{
				this.exp = value;
			}
		}

		public int Diamond
		{
			get
			{
				return this.diamond;
			}
			set
			{
				this.diamond = value;
			}
		}

		public int Lv
		{
			get
			{
				return this.lv;
			}
			set
			{
				this.lv = value;
			}
		}

		public int ConsumeCount
		{
			get
			{
				return this.m_consumeCount;
			}
			set
			{
				this.m_consumeCount = value;
			}
		}

		public int GameId
		{
			get
			{
				return this.m_gameId;
			}
			set
			{
				this.m_gameId = value;
			}
		}

		public int SkinID
		{
			get
			{
				return this.m_skinID;
			}
			set
			{
				this.m_skinID = value;
			}
		}

		public bool IsPlayVedioAds
		{
			get
			{
				return this.m_isPlayVedioAds;
			}
			set
			{
				this.m_isPlayVedioAds = value;
			}
		}

        public int BoardSize { get => m_boardSize; set => m_boardSize = value; }

        public static GM GetInstance()
		{
			if (GM.g_intance == null)
			{
				GM.g_intance = new GM();
			}
			return GM.g_intance;
		}

		public bool Init()
		{
			this.SaveInstallTime();
			this.LoadLocalData();
			this.InitEvent();
			return true;
		}

		public int AddDiamond(int value, bool isPlayAni = true)
		{
			this.Diamond += value;
            DataManager.Instance.state_Player.LocalData_Diamond = this.Diamond;
            DataManager.Instance.Save_Player_Data();
			GlobalEventHandle.EmitGetDiamondHandle(value, isPlayAni);
			return this.Diamond;
		}

		public int AddDiamondBase(int value)
		{
			this.Diamond += value;
            DataManager.Instance.state_Player.LocalData_Diamond = this.Diamond;
            DataManager.Instance.Save_Player_Data();
            return this.Diamond;
		}

		public void ConsumeGEM(int value)
		{
			this.Diamond -= value;
            DataManager.Instance.state_Player.LocalData_Diamond = this.Diamond;
            DataManager.Instance.Save_Player_Data();
            GlobalEventHandle.EmitConsumeDiamondHandle(value);
		}

		public bool isFullGEM(int value)
		{
			return this.Diamond >= value;
		}

		public bool AddExp(int value)
		{
			this.Exp += value;
			if (this.Lv >= 120)
			{
				this.Lv = 120;
				this.Exp = ((Configs.Configs.TPlayers[120.ToString()].Exp < this.Exp) ? Configs.Configs.TPlayers[120.ToString()].Exp : this.Exp);
			}
            DataManager.Instance.state_Player.LocalData_Exp = this.Exp;
            DataManager.Instance.Save_Player_Data();

            GlobalEventHandle.EmitAddExpHandle(false);
			TPlayer tPlayer = null;
			if (Configs.Configs.TPlayers.ContainsKey(this.Lv.ToString()))
			{
				tPlayer = Configs.Configs.TPlayers[this.Lv.ToString()];
			}
			if (tPlayer == null)
			{
				return false;
			}
			if (this.Exp <= tPlayer.Exp)
			{
				return false;
			}
			while (this.exp > tPlayer.Exp)
			{
				this.Exp -= tPlayer.Exp;
				this.Lv++;
				AchiveData.GetInstance().Add(1, 1, true);
				if (Configs.Configs.TPlayers.ContainsKey(this.Lv.ToString()))
				{
					tPlayer = Configs.Configs.TPlayers[this.Lv.ToString()];
				}
				if (tPlayer == null)
				{
					break;
				}
			}
            DataManager.Instance.state_Player.LocalData_Exp = this.Exp;
            DataManager.Instance.state_Player.LocalData_Lv = this.Lv;

            DataManager.Instance.Save_Player_Data();

			GlobalEventHandle.EmitAddExpHandle(true);
			//AppsflyerUtils.TrackLevel(this.lv);
			return true;
		}

		public void ResetToNewGame()
		{
		}

		public void ResetConsumeCount()
		{
			this.ConsumeCount = 0;
		}

		public void AddConsumeCount()
		{
			int consumeCount = this.ConsumeCount + 1;
			this.ConsumeCount = consumeCount;
		}

		public bool isSavedGame()
		{
			return this.GetSavedGameID() != 0;
		}

		public int GetSavedGameID()
		{
			return DataManager.Instance.state_Player.LocalData_GameId;
		}

		public void SetSavedGameID(int value)
		{
			this.GameId = value;
            DataManager.Instance.state_Player.LocalData_GameId = value;
            DataManager.Instance.Save_Player_Data();
		}

		public string GetSavedGameMap()
		{
			return DataManager.Instance.state_Player.LocalData_OldGame;
		}

		public string GetSavedLife()
		{
			return DataManager.Instance.state_Player.LocalData_OldLife;
		}

		public Vector2 GetSavedPos()
		{
			return new Vector2(DataManager.Instance.state_Player.LocalData_OldPosX, DataManager.Instance.state_Player.LocalData_OldPosY);
		}

		public void SaveGame(string value, string life, float x = -9999999f, float y = -9999999f)
		{
			if (!value.Equals(""))
			{
                DataManager.Instance.state_Player.LocalData_OldGame = value;
            }
			if (!life.Equals(""))
            {
                DataManager.Instance.state_Player.LocalData_OldLife = life;
			}
			if (x != -9999999f)
            {
                DataManager.Instance.state_Player.LocalData_OldPosX = x;
			}
			if (y != -9999999f)
            {
                DataManager.Instance.state_Player.LocalData_OldPosY = y;
			}

            DataManager.Instance.Save_Player_Data();

        }

        public void SaveScore(int gameID, int score)
		{
			string[] array = DataManager.Instance.state_Player.LocalData_OldScore.Split(new char[]
			{
				','
			});
			if (gameID > array.Length)
			{
				return;
			}
			array[gameID - 1] = score.ToString();
            DataManager.Instance.state_Player.LocalData_OldScore = string.Format("{0},{1},{2}", array[0], array[1], array[2]);
            DataManager.Instance.Save_Player_Data();
        }

		public int GetScore(int gameID)
		{
			string[] arg_25_0 = DataManager.Instance.state_Player.LocalData_OldScore.Split(new char[]
			{
				','
			});
			List<int> list = new List<int>();
			string[] array = arg_25_0;
			for (int i = 0; i < array.Length; i++)
			{
				string value = array[i];
				list.Add(Convert.ToInt32(value));
			}
			if (gameID > list.Count)
			{
				return 0;
			}
			return list[gameID - 1];
		}

		public void SaveScoreRecord(int gameID, int score)
		{
			string[] array = DataManager.Instance.state_Player.LocalData_Record_Score.Split(new char[]
			{
				','
			});
			if (gameID > array.Length)
			{
				return;
			}
			if (score > Convert.ToInt32(array[gameID - 1]))
			{
				array[gameID - 1] = score.ToString();
				if (gameID == 3)
				{
					///AppsflyerUtils.TrackNewLevel(3, score);
				}
                DataManager.Instance.state_Player.LocalData_Record_Score = string.Format("{0},{1},{2}", array[0], array[1], array[2]);
                DataManager.Instance.Save_Player_Data();
                GlobalEventHandle.EmitRefreshMaxScoreHandle(array);
			}
		}

		public int GetScoreRecord(int gameID)
		{
			string[] arg_25_0 = DataManager.Instance.state_Player.LocalData_Record_Score.Split(new char[]
			{
				','
			});
			List<int> list = new List<int>();
			string[] array = arg_25_0;
			for (int i = 0; i < array.Length; i++)
			{
				string value = array[i];
				list.Add(Convert.ToInt32(value));
			}
			if (gameID > list.Count)
			{
				return 0;
			}
			return list[gameID - 1];
		}

        public LocalData_G003_Record_Score Find_G003(string id)
        {
            LocalData_G003_Record_Score G003_Record_Scores =
                DataManager.Instance.state_Player.localData_G003_Record_Scores
                .Find(x => x._Id.Equals(id));

            if (G003_Record_Scores == null)
            {
                G003_Record_Scores = new LocalData_G003_Record_Score
                {
                    _Id = id,
                    val = "-1"
                };

                DataManager.Instance.state_Player.localData_G003_Record_Scores.Add(G003_Record_Scores);

                return DataManager.Instance.state_Player.localData_G003_Record_Scores
                .Find(x => x._Id.Equals(id));

            }

            return G003_Record_Scores;
        }

        public void SaveG003ScoreRecord(int LvID, int CheckPoint)
		{

            LocalData_G003_Record_Score G003_Record_Scores = Find_G003("LocalData_G003_Record_Score_" + LvID.ToString());

            string @string = G003_Record_Scores.val;
			if (CheckPoint > Convert.ToInt32(@string))
			{
                G003_Record_Scores.val = CheckPoint.ToString();
                DataManager.Instance.Save_Player_Data();

                ///AppsflyerUtils.TrackNewLevel(3, CheckPoint);
				//PlayerPrefs.SetString("LocalData_G003_Record_Score_" + LvID.ToString(), CheckPoint.ToString());
			}
		}

		public int GetG003ScoreRecord(int LvID)
        {
            LocalData_G003_Record_Score G003_Record_Scores = Find_G003("LocalData_G003_Record_Score_" + LvID.ToString());

            return Convert.ToInt32(G003_Record_Scores.val);
		}

		public bool isFirstGame()
		{
			return DataManager.Instance.state_Player.LocalData_FirstGame == 0;
		}

		public void SetFristGame()
		{
            DataManager.Instance.state_Player.LocalData_FirstGame = 1;
            DataManager.Instance.Save_Player_Data();
        }

		public bool IsFirstFinishGame()
		{
			return DataManager.Instance.state_Player.LocalData_IsFirstFinish < 2;
		}

		public void SetFirstFinishGame()
		{
			int @int = DataManager.Instance.state_Player.LocalData_IsFirstFinish;
			if (@int < 10)
			{
                DataManager.Instance.state_Player.LocalData_IsFirstFinish = @int + 1;
                DataManager.Instance.Save_Player_Data();
            }
		}

		public int GetLocalSkinID()
		{
			return DataManager.Instance.state_Player.LocalData_SkinID;
		}

		public void SetLocalSkinID(int id)
		{
			this.SkinID = id;
            DataManager.Instance.state_Player.LocalData_SkinID = id;
            DataManager.Instance.Save_Player_Data();
            Action expr_17 = GlobalEventHandle.DoTransiformSkin;
			if (expr_17 == null)
			{
				return;
			}
			expr_17();
		}

		public void SaveInstallTime()
		{
			if (DataManager.Instance.state_Player.LocalData_InitTime.Equals("-1"))
			{
                Debug.Log("³Ê°¡¸ÕÀú´×?");
                DataManager.Instance.state_Player.LocalData_InitTime = DateTime.Now.ToString("yyyy-MM-dd");
                DataManager.Instance.Save_Player_Data();
			}
		}

		private DateTime GetInstallTime()
		{
			string @string = DataManager.Instance.state_Player.LocalData_InitTime;
            Debug.Log(@string);
			if (@string.Equals("-1"))
			{
				return DateTime.Now;
			}
			string[] array = @string.Split(new char[]
			{
				'-'
			});
			return new DateTime(int.Parse(array[0]), int.Parse(array[1]), int.Parse(array[2]));
		}

		public bool IsNewUser()
		{
			return (DateTime.Now - this.GetInstallTime()).Days == 0;
		}

		public List<int> GetSkinData()
		{
			List<int> list = new List<int>();
			string[] array = DataManager.Instance.state_Player.LocalData_SkinData.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string value = array[i];
				list.Add(Convert.ToInt32(value));
			}
			return list;
		}

		public void SetSkinData(int skinID, int status)
		{
			string[] array = DataManager.Instance.state_Player.LocalData_SkinData.Split(new char[]
			{
				','
			});
			if (skinID > array.Length)
			{
				return;
			}
			array[skinID - 1] = status.ToString();
            DataManager.Instance.state_Player.LocalData_SkinData = string.Format("{0},{1}", array[0], array[1]);
            DataManager.Instance.Save_Player_Data();
        }

		public DateTime GetSkinFreeTime(int skinID)
		{
			List<DateTime> list = new List<DateTime>();
			string[] array = DataManager.Instance.state_Player.LocalData_SkinFreeTime.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				if (text.Equals("-1"))
				{
					list.Add(DateTime.Now);
				}
				else
				{
					string[] array2 = text.Split(new char[]
					{
						'-'
					});
					list.Add(new DateTime(int.Parse(array2[0]), int.Parse(array2[1]), int.Parse(array2[2])));
				}
			}
			return list[skinID - 1];
		}

		public void SetSkinFreeTime(int skinID, DateTime time)
		{
			List<DateTime> list = new List<DateTime>();
			string[] array = DataManager.Instance.state_Player.LocalData_SkinFreeTime.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				if (text.Equals("-1"))
				{
					list.Add(DateTime.Now);
				}
				else
				{
					string[] array2 = text.Split(new char[]
					{
						'-'
					});
					list.Add(new DateTime(int.Parse(array2[0]), int.Parse(array2[1]), int.Parse(array2[2])));
				}
			}
			if (skinID > list.Count)
			{
				return;
			}
			list[skinID - 1] = time;
			string text2 = "";
			for (int j = 0; j < list.Count; j++)
			{
				text2 += list[j].ToString("yyyy-MM-dd");
				if (j < list.Count - 1)
				{
					text2 += ",";
				}
			}
            DataManager.Instance.state_Player.LocalData_SkinFreeTime = text2;
            DataManager.Instance.Save_Player_Data();
        }

		public void ResetSkinFreeTime()
		{
			List<int> skinData = this.GetSkinData();
			for (int i = 0; i < skinData.Count; i++)
			{
				if (skinData[i] == 2)
				{
					DateTime skinFreeTime = this.GetSkinFreeTime(i + 1);
					if ((DateTime.Now - skinFreeTime.AddDays(3.0)).Days > 0)
					{
						this.SetSkinData(i + 1, 1);
						if (this.SkinID == i + 1)
						{
							this.SetLocalSkinID(1);
						}
					}
				}
			}
		}

		public bool isFirstShare()
		{
            return DataManager.Instance.state_Player.LocalData_FirstShare == 0;
		}

		public void ResetFirstShare(int value = 0)
		{
            DataManager.Instance.state_Player.LocalData_FirstShare = value;
            DataManager.Instance.Save_Player_Data();
        }

		public bool IsRandomStatus(int value)
		{
			return this.m_random.Next(1, 100) < value;
		}

		private void LoadLocalData()
		{
			this.Diamond = DataManager.Instance.state_Player.LocalData_Diamond;
			this.Lv = DataManager.Instance.state_Player.LocalData_Lv;
			this.exp = DataManager.Instance.state_Player.LocalData_Exp;
			this.GameId = this.GetSavedGameID();
			this.SkinID = this.GetLocalSkinID();
		}

		private void InitEvent()
		{
		}
	}
}
