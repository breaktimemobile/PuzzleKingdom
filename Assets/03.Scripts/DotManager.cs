using Assets.Scripts.GameManager;
using System;
using UnityEngine;

public class DotManager : MonoBehaviour
{
    public GameObject m_adsDot;

    public GameObject m_shopDot;

    public GameObject m_taskDot;

    public GameObject m_achievDot;

    public GameObject m_rank;



    private static DotManager m_instance;

	private void Awake()
	{
		DotManager.m_instance = this;
	}

	private void Update()
	{
	}

	public static DotManager GetInstance()
	{
		return DotManager.m_instance;
	}

	public void Check()
	{
		this.CheckAchiev();
		this.CheckTask();
        this.CheckAds();
        m_shopDot.SetActive(false);
        m_rank.SetActive(false);

    }

    public void CheckAchiev()
	{
		bool active = false;
		for (int i = 1; i <= 6; i++)
		{
			if (AchiveData.GetInstance().Get(i).status == -2)
			{
				active = true;
				break;
			}
		}
		this.m_achievDot.SetActive(active);
	}

	public void CheckTask()
	{
		bool active = false;
		int[] array = new int[]
		{
			100101,
			100102,
			100103,
			100104,
			100105
		};
		for (int i = 0; i < array.Length; i++)
		{
			int type = array[i];
			if (TaskData.GetInstance().Get(type).status == -2)
			{
				active = true;
				break;
			}
		}
		this.m_taskDot.SetActive(active);
	}

    public void CheckAds()
    {
        Debug.Log(name);

        Debug.Log(AdsManager.GetInstance().IsWatch);
        this.m_adsDot.SetActive(AdsManager.GetInstance().IsWatch);
    }
}
