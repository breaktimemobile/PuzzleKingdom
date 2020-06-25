using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ToastManager : MonoBehaviour
{
	

	private static ToastManager m_instance;

	public GameObject ToastPrefab;

	private List<Toast> ToastList = new List<Toast>();

	public Transform m_parent;

	private void Awake()
	{
		ToastManager.m_instance = this;
	}

	public static ToastManager GetInstance()
	{
		return ToastManager.m_instance;
	}

	public static void Show(string msg, bool isID = true)
	{
		if (isID)
		{
            SystemLanguage Id = (SystemLanguage)DataManager.Instance.state_Player.LocalData_LanguageId;

            msg = Language.GetText(msg, Id);
            Debug.Log(msg);
		}
		ToastManager.m_instance.CreatToast(msg);
	}

	private void CreatToast(string str)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ToastPrefab, this.m_parent, false);
		Toast comp = gameObject.GetComponent<Toast>();
		this.ToastList.Insert(0, comp);
		comp.InitToast(str, delegate
		{
			this.ToastList.Remove(comp);
		});
		this.ToastMove(0.2f);
	}

	private void ToastMove(float speed)
	{
		for (int i = 0; i < this.ToastList.Count; i++)
		{
			this.ToastList[i].Move(speed, i + 1);
		}
	}
}
