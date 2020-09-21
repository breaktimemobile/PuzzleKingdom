using Assets.Scripts.GameManager;
using CloudOnce;
using System;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_IOS
using UnityEngine.iOS;
#endif
public class Setting : MonoBehaviour
{
	private AudioManager audioManager;

	public Sprite m_asset_switch_off;

	public Sprite m_asset_switch_on;

	public Image m_img_music_switch;

	public Image m_img_effect_switch;

    public Text m_Ver;

    public Button btn_save;

    public Button btn_load;

    public GameObject btn_terms;

    public GameObject btn_google;
    public GameObject btn_ios;

    private void Start()
	{
		this.Init();
	}

	public void Init()
	{
		this.audioManager = AudioManager.GetInstance();
		this.InitUI();

        m_Ver.text = "VER. " + Application.version;


    }

    public void InitUI()
	{
		this.m_img_music_switch.sprite = ((this.audioManager.Switch_bg == 1) ? this.m_asset_switch_on : this.m_asset_switch_off);
		this.m_img_effect_switch.sprite = ((this.audioManager.Switch_eff == 1) ? this.m_asset_switch_on : this.m_asset_switch_off);
        btn_terms.SetActive(Application.systemLanguage == SystemLanguage.Korean);

#if UNITY_ANDROID

        btn_google.SetActive(true);
        btn_ios.SetActive(false);

#elif UNITY_IOS
        btn_google.SetActive(false);
        btn_ios.SetActive(true);
#endif


    }

    public void OnClickMusicBtn()
	{
		if (this.audioManager.Switch_bg == 1)
		{
			this.audioManager.SetMusicSwitch(0);
			this.audioManager.StopBgMusic();
		}
		else
		{
			this.audioManager.SetMusicSwitch(1);
			this.audioManager.PlayBgMusic();
		}
		this.InitUI();
	}

	public void OnClickEffectBtn()
	{
		if (this.audioManager.Switch_eff == 1)
		{
			this.audioManager.SetEffectSwitch(0);
		}
		else
		{
			this.audioManager.SetEffectSwitch(1);
		}
		this.InitUI();
	}

	public void OnClickLanguage()
	{
        FireBaseManager.Instance.LogEvent("Setting_Langauge");

        GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/setting_language") as GameObject);
		DialogManager.GetInstance().show(obj, false);
	}

    public void OnClickClose()
	{
		DialogManager.GetInstance().Close(null);
	}


    public void OnClickFacebook()
    {
        FireBaseManager.Instance.LogEvent("Setting_Review");


#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.block.puzzle.puzzlego.number.puzzledom.Kingdom");
#elif UNITY_IOS
         Device.RequestStoreReview();
#endif

    }

    public void OnClickprivacy()
    {
        if(Application.systemLanguage == SystemLanguage.Korean)
        {
            Application.OpenURL("https://sites.google.com/site/breaktieme/privacy-policy_kr");

        }
        else
        {
            Application.OpenURL("https://sites.google.com/site/breaktieme/privacy-policy");

        }

    }

    public void OnClickterms()
    {

        if (Application.systemLanguage == SystemLanguage.Korean)
        {
            Application.OpenURL("https://sites.google.com/site/breaktieme/terms-of-service");
        }
    }

    public void OnClickMail()
    {
        FireBaseManager.Instance.LogEvent("Setting_Langauge");

        string str = "help.breaktime@gmail.com";
        string str2 = "Puzzle kingdom";
        Application.OpenURL("mailto:" + str + "?subject=" + str2);
    }

    public void OnClickGoogle()
    {
        FireBaseManager.Instance.LogEvent("Setting_Login");

        if (!Cloud.IsSignedIn)
        {
            GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/Google_Login") as GameObject);
            obj.GetComponent<DataPopup>().Set_Google();
            DialogManager.GetInstance().show(obj, false);

            
        }
        else
        {
#if UNITY_ANDROID
            GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/Google_Logout") as GameObject);
            DialogManager.GetInstance().show(obj, false);
#elif UNITY_IOS
            CloudOnceManager.Instance.Show_Achievements();
#endif
        }

    }

    public void OnClickSave()
    {
        if (!Social.localUser.authenticated)
        {
            CloudOnceManager.Instance.OnClickGoogle();
        }
        else
        {
            GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/data_save") as GameObject);
            DialogManager.GetInstance().show(obj, false);
        }

    }

    public void OnClickLoad()
    {
        if (!Social.localUser.authenticated)
        {
            CloudOnceManager.Instance.OnClickGoogle();
        }
        else
        {
            GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/data_load") as GameObject);
            DialogManager.GetInstance().show(obj, false);
        }
    }
}
