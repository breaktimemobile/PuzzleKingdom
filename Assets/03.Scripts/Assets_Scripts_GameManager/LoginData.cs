using System;
using UnityEngine;

namespace Assets.Scripts.GameManager
{
	internal class LoginData
	{
		private static LoginData m_instance;

		public static LoginData GetInstance()
		{
			if (LoginData.m_instance == null)
			{
				LoginData.m_instance = new LoginData();
			}
			return LoginData.m_instance;
		}

		public static void Initialize()
		{
			//if (GM.GetInstance().IsNewUser())
			//{
			//	return;
			//}
			LoginData.GetInstance().RunSerialDay();
		}

		public void RunSerialDay()
		{
			int serialStatus = this.GetSerialStatus();
			int num = this.GetSerialLoginCount();
			switch (serialStatus)
			{
			case -2:
				this.ResetSignData();
				num = 1;
				this.Save(num, false);
				this.SetSignInData(num, 0);
				break;
			case -1:
				this.ResetSignData();
				num = 1;
				this.Save(num, false);
				this.SetSignInData(num, 1);
				return;
			case 0:
				break;
			case 1:
			{
				bool flag = false;
				int[] signInData = this.GetSignInData();
				for (int i = 0; i < signInData.Length; i++)
				{
					if (signInData[i] == 1)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					num++;
					if (num > 7)
					{
						this.ResetSignData();
						num = 1;
					}
					this.Save(num, false);
					this.SetSignInData(num, 1);
					return;
				}
				break;
			}
			default:
				return;
			}
		}

		public void SetSignInData(int id, int value)
		{
			int[] signInData = this.GetSignInData();
			if (id > signInData.Length)
			{
				return;
			}
			signInData[id - 1] = value;
            DataManager.Instance.state_Player.LocalData_SignData = this.ConvertArrayToString(signInData);
            DataManager.Instance.Save_Player_Data();
        }

		public int GetSerialLoginCount()
		{
			return DataManager.Instance.state_Player.LocalData_SerialLogin;
		}

		public int[] GetSignInData()
		{
			int[] array = new int[7];
			string @string = DataManager.Instance.state_Player.LocalData_SignData;
			if (@string.Equals("-1"))
			{
				return array;
			}
			string[] array2 = @string.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Convert.ToInt32(array2[i]);
			}
			return array;
		}

		public void Save(int count, bool newUser = false)
		{
			this.SetSerialLoginCount(count);
			this.SaveLastLoginTime(newUser);
		}

		private int GetSerialStatus()
		{
			bool arg_0F_0 = this.GetSerialLoginCount() != 0;
			DateTime lastLoginTime = this.GetLastLoginTime();
			if (!arg_0F_0)
			{
				return 1;
			}
			int days = (DateTime.Now - lastLoginTime).Days;
			int result;
			if (days != 0)
			{
				if (days != 1)
				{
					result = -1;
				}
				else
				{
					result = 1;
				}
			}
			else
			{
				result = 0;
			}
			return result;
		}

		private DateTime GetLastLoginTime()
		{
			string @string = DataManager.Instance.state_Player.LocalData_LastLogin;
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

		public void SaveLastLoginTime(bool newUser = false)
		{
			if (!newUser)
			{
                DataManager.Instance.state_Player.LocalData_LastLogin = DateTime.Now.ToString("yyyy-MM-dd");
                DataManager.Instance.Save_Player_Data();
				return;
			}
            DataManager.Instance.state_Player.LocalData_LastLogin = "1970-1-1";
            DataManager.Instance.Save_Player_Data();
		}

		private void SetSerialLoginCount(int value)
		{
            DataManager.Instance.state_Player.LocalData_SerialLogin = value;
            DataManager.Instance.Save_Player_Data();
        }

		private void ResetSignData()
		{
			int[] array = new int[7];
            DataManager.Instance.state_Player.LocalData_SignData = this.ConvertArrayToString(array);
            DataManager.Instance.Save_Player_Data();
        }

		private string ConvertArrayToString(int[] array)
		{
			string text = "";
			for (int i = 0; i < array.Length; i++)
			{
				if (i < array.Length - 1)
				{
					text = text + array[i] + ",";
				}
				else
				{
					text += array[i];
				}
			}
			UnityEngine.Debug.Log(text);
			return text;
		}
	}
}
