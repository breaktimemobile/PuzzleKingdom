using Assets.Scripts.Utils;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Assets.Scripts.GameManager
{
	internal class GoodsManager
	{
		private static GoodsManager m_intance;

		private int m_ads;


		public Action<Product, bool> ProcessNonConsumableHandle;

		public Action<Product, bool> ProcessConsumableHanle;



		public event Action<int, int> ShowSubscriptionHanle;

		public int Ads
		{
			get
			{
				return this.m_ads;
			}
			set
			{
				this.m_ads = value;
			}
		}

		public static void Initialize()
		{
			GoodsManager.m_intance = new GoodsManager();
			
		}

		public static GoodsManager GetInstance()
		{
			if (GoodsManager.m_intance == null)
			{
				GoodsManager.m_intance = new GoodsManager();
			}
			return GoodsManager.m_intance;
		}

		public bool isPuchasedAds()
		{
			return this.m_ads == 1;
		}


		public void Reset()
		{
            DataManager.Instance.state_Player.LocalData_NowStatus = 0;
            DataManager.Instance.Save_Player_Data();

        }

		private void OnProcessConsumable(Product product, bool isInit)
		{
			GM.GetInstance().AddDiamond((int)product.definition.payout.quantity, false);
			Action<Product, bool> expr_23 = this.ProcessConsumableHanle;
			if (expr_23 == null)
			{
				return;
			}
			expr_23(product, isInit);
		}

		private void OnProcessNonConsumable(Product product, bool isInit)
		{
			this.m_ads = 1;
			Action<Product, bool> expr_0D = this.ProcessNonConsumableHandle;
			if (expr_0D == null)
			{
				return;
			}
			expr_0D(product, isInit);
		}
        

		private void SaveTime()
		{
            DataManager.Instance.state_Player.LocalData_PreSubscriptionTime = DateTime.Now.ToString("yyyy-MM-dd");
            DataManager.Instance.Save_Player_Data();
        }

		private DateTime GetPreTime()
		{
			string @string = DataManager.Instance.state_Player.LocalData_PreSubscriptionTime;
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

		private bool isGeted()
		{
			return DataManager.Instance.state_Player.LocalData_NowStatus == 1;
		}

		private void Mark()
		{
			this.SaveTime();
            DataManager.Instance.state_Player.LocalData_NowStatus = 1;
            DataManager.Instance.Save_Player_Data();
        }
	}
}
