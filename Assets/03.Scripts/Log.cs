using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour
{
	private static Log m_instance;

	public GameObject m_assetText;

	private List<Text> m_texts = new List<Text>();

	private void Awake()
	{
		Log.m_instance = this;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public static Log GetInstance()
	{
		return Log.m_instance;
	}

	public void Add(string log)
	{
		GameObject _obj = UnityEngine.Object.Instantiate<GameObject>(this.m_assetText, base.transform, true);
		float y = 300f;
		if (this.m_texts.Count > 0)
		{
			y = this.m_texts[this.m_texts.Count - 1].transform.localPosition.y - this.m_texts[this.m_texts.Count - 1].preferredHeight - 10f;
		}
        _obj.transform.localPosition = new Vector3(-300f, y);
        _obj.transform.localScale = new Vector3(1f, 1f, 1f);
		Text component = _obj.GetComponent<Text>();
		component.text = log;
		this.m_texts.Add(component);
	}
}
