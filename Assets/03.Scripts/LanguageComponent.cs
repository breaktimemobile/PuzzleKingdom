using Assets.Scripts.Utils;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LanguageComponent : MonoBehaviour
{
	private Text m_text;

	public string m_id = "TXT_NO_10001";

	public string m_suffix = "";

	public float m_linespace_ru;

	public float m_linespace_en;

    private float m_linespace_default = 1;

	private void Start()
	{
		this.m_text = base.GetComponent<Text>();
		this.Set();
		Language.GetInstance().AddEvent(new Action(this.TransformLanuage));
	}

	private void OnDestroy()
	{
		Language.GetInstance().RemoveEvent(new Action(this.TransformLanuage));
	}

	public void SetText(string id)
	{
		this.m_id = id;
		this.Set();
	}

	private void Set()
	{
		if (this.m_text == null)
		{
			return;
		}

        //Debug.Log("langucoimp 언어 " + Application.systemLanguage +"  플레이어 언어 " + (SystemLanguage)DataManager.Instance.state_Player.LocalData_LanguageId);
        SystemLanguage @int = Application.systemLanguage;

        if (DataManager.Instance.state_Player.LocalData_LanguageId != -1)
        {
            @int = (SystemLanguage)DataManager.Instance.state_Player.LocalData_LanguageId;

        }

        this.m_text.text = Language.GetText(m_id, @int) + this.m_suffix;
        this.m_text.font = Language.GetInstance().GetFont();

        if (@int == SystemLanguage.Russian)
		{
			if (this.m_linespace_ru.CompareTo(0f) != 0)
			{
				this.m_text.lineSpacing = this.m_linespace_ru;
				return;
			}
		}
		else if (this.m_linespace_default.CompareTo(0f) != 0)
		{
			this.m_text.lineSpacing = this.m_linespace_default;
		}
	}

	private void TransformLanuage()
	{
		this.SetText(this.m_id);
	}
}
