using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingLanguage : MonoBehaviour
{
	[SerializeField]
	public List<Toggle> m_toggles = new List<Toggle>();

    [SerializeField]
    public List<GameObject> m_uses = new List<GameObject>();

    private Dictionary<int, SystemLanguage> m_dicts = new Dictionary<int, SystemLanguage>();

    public ScrollRect scrollRect;

    public GameObject content;

    private void Start()
	{
		this.m_dicts.Add(0, SystemLanguage.Korean);
		this.m_dicts.Add(1, SystemLanguage.English);
        this.m_dicts.Add(2, SystemLanguage.Japanese);
        this.m_dicts.Add(3, SystemLanguage.ChineseSimplified);
		this.m_dicts.Add(4, SystemLanguage.ChineseTraditional);
		this.m_dicts.Add(5, SystemLanguage.German);
		this.m_dicts.Add(6, SystemLanguage.Italian);
		this.m_dicts.Add(7, SystemLanguage.Spanish);
        this.m_dicts.Add(8, SystemLanguage.French);
        this.m_dicts.Add(9, SystemLanguage.Indonesian);
        this.m_dicts.Add(10, SystemLanguage.Portuguese);
        this.m_dicts.Add(11, SystemLanguage.Vietnamese);
        this.m_dicts.Add(12, SystemLanguage.Turkish);
        this.m_dicts.Add(13, SystemLanguage.Thai);
        this.m_dicts.Add(14, SystemLanguage.Russian);
        this.m_dicts.Add(15, SystemLanguage.Unknown);
        this.m_dicts.Add(16, SystemLanguage.Arabic);

        this.SelectLanguage();
	}

	private void Update()
	{
	}

	public void OnClickToggle(bool isOn)
	{
		for (int i = 0; i < this.m_toggles.Count; i++)
		{
            m_uses[i].SetActive(false);

            if (this.m_toggles[i].isOn)
			{
                Debug.Log(this.m_dicts[i]);
				Language.GetInstance().Set(this.m_dicts[i]);
                m_uses[i].SetActive(true);

            }
        }
	}

	public void OnClickClose()
	{
		DialogManager.GetInstance().Close(null);
	}

	private void SelectLanguage()
	{
		SystemLanguage id = (SystemLanguage)DataManager.Instance.state_Player.LocalData_LanguageId;

        foreach (KeyValuePair<int, SystemLanguage> pair in m_dicts)
        {
            Console.WriteLine("{0}, {1}",
                pair.Key,
                pair.Value);

            if(pair.Value == id)
            {
                m_uses[pair.Key].SetActive(true);

                m_toggles[pair.Key].isOn = true;
                scrollRect.content.localPosition = new Vector3(0, pair.Key * 100, 0);
                return;
            }

        }


       
	}

	private void Hide()
	{

        foreach (var item in m_uses)
        {
            item.SetActive(false);
        }
	}
}
