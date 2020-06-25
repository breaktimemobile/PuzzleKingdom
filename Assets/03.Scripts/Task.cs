using Assets.Scripts.GameManager;
using Assets.Scripts.Utils;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Task : MonoBehaviour
{

	[Serializable]
	private sealed class CommonClass
	{
		public static readonly Task.CommonClass _common = new Task.CommonClass();

		public static TweenCallback _tween1;

		public static TweenCallback _tween2;

		internal void EntranceAni1()
		{
		}

		internal void EntranceAni2()    
		{
		}
	}

	[SerializeField]
	public List<TaskItem> items = new List<TaskItem>();

    public Text txt_gold;

    private void Start()
	{
		GlobalTimer.GetInstance().RefreshHandle += new Action(this.Refresh);

        txt_gold.GetComponent<OverlayNumber>().SetStartNumber(DataManager.Instance.state_Player.LocalData_Diamond);

    }

    private void Update()
	{
		//Utils.BackListener(base.gameObject, delegate
		//{
		//	this.OnClickReturn();
		//});
	}

	private void OnDestroy()
	{
		GlobalTimer.GetInstance().RefreshHandle -= new Action(this.Refresh);
	}

	public void OnClickReturn()
	{
        DialogManager.GetInstance().Close(null);
    }

    public void Refresh()
	{

        foreach (var item in items)
        {
            item.BindDataToUI();
        }

	}

}
