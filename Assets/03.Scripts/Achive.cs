using Assets.Scripts.GameManager;
using Assets.Scripts.Utils;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Achive : MonoBehaviour
{
    /*
	private sealed class __c__DisplayClass8_0
	{
		public AchiveItem obj;

		internal void _EntranceAni_b__2()
		{
			this.obj.gameObject.SetActive(true);
		}
	}
    */

	[Serializable]
	private sealed class AnimClass
	{
		public static readonly Achive.AnimClass animFunc = new Achive.AnimClass();

		public static TweenCallback _animCallback1;

		public static TweenCallback _animCallback2;

		internal void _EntranceAni1()
		{
		}

		internal void _EntranceAni2()
		{
		}
	}

    private void OnEnable()
    {
        Debug.Log("¾÷Àû...");

        Debug.Log(GetComponentInChildren<ScrollRect>().name);
        GetComponentInChildren<ScrollRect>().content.localPosition = Vector3.zero;

        txt_gold.GetComponent<OverlayNumber>().SetStartNumber(DataManager.Instance.state_Player.LocalData_Diamond);

    }

    [SerializeField]
	public List<AchiveItem> m_items;

    public Text txt_gold;

    private void Update()
	{
		//Utils.BackListener(base.gameObject, delegate
		//{
		//	this.OnClickReturn();
		//});
	}

	public void OnClickAds()
	{
		//AdsManager.GetInstance().Play(AdsManager.AdType.Stimulate, null, null, 5, null);
	}

	public void OnClickReturn()
	{
        DialogManager.GetInstance().Close(null);
    }
    
    public void Set_BindDataToUI()
    {
        foreach (var item in m_items)
        {
            item.BindDataToUI();
        }
    }
}
