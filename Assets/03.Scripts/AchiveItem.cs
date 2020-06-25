using Assets.Scripts.Configs;
using Assets.Scripts.GameManager;
using Assets.Scripts.Utils;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AchiveItem : MonoBehaviour
{
    public Image img_progress;

    public Text txt_src;

    public Text txt_desc;

    public Text txt_awards;

    public Text m_btn_txt;

    public Button btn_get;

    public Image img_icon;

    public GameObject obj_new;

    public int type;

    float base_width = 0;
    float base_hight = 0;

    float width = 0;
    float hight = 0;

    bool play_anim = false;

    private void OnEnable()
    {
        this.BindDataToUI();
    }

    private void Update()
    {
        if(play_anim)
        btn_get.GetComponent<RectTransform>().sizeDelta = new Vector2(width, hight);
    }

    public void BindDataToUI()
    {

        base_width = base_width != 0 ? base_width : btn_get.GetComponent<RectTransform>().sizeDelta.x;
        base_hight = base_hight != 0 ? base_hight : btn_get.GetComponent<RectTransform>().sizeDelta.y;

        width = btn_get.GetComponent<RectTransform>().sizeDelta.x;
        hight = btn_get.GetComponent<RectTransform>().sizeDelta.y;

        LocalData localData = AchiveData.GetInstance().Get(this.type);
        TAchive tAchive = Configs.TAchives[localData.key.ToString()];
        float num = (float)localData.value / (float)tAchive.Value;
        num = ((num >= 1f) ? 1f : num);
        this.img_progress.fillAmount = num;
        this.txt_desc.GetComponent<LanguageComponent>().SetText(tAchive.Desc);
        this.txt_src.text = localData.value.ToString() + "/" + tAchive.Value.ToString();
        this.txt_awards.text = string.Format("{0}", tAchive.Item);

        obj_new.SetActive(false);


        DOTween.Kill(btn_get.gameObject);

        btn_get.transform.localScale = Vector3.one;

        switch (localData.status)
        {
            case -3:

                Debug.Log("모든 업적 완료");
                btn_get.interactable = false;
                btn_get.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);

                play_anim = false;
                this.m_btn_txt.GetComponent<LanguageComponent>().SetText("TXT_NO_20004");

                btn_get.GetComponent<RectTransform>().sizeDelta = new Vector2(base_width, base_hight);

                return;
            case -2:

                Debug.Log("완료");
                btn_get.interactable = true;
                btn_get.GetComponent<Image>().color = new Color(1, 1, 1, 1);

                play_anim = true;
                Debug.Log(width * 0.9f);

                DOTween.To(() => width, x => width = x, width * 0.9f, 1)
                   .SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

                DOTween.To(() => hight, x => hight = x, hight * 0.9f, 1)
                 .SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

                this.m_btn_txt.GetComponent<LanguageComponent>().SetText("TXT_NO_20001");

                EventTrigger eventTrigger = btn_get.gameObject.AddComponent<EventTrigger>();

                EventTrigger.Entry entry_PointerDown = new EventTrigger.Entry();
                entry_PointerDown.eventID = EventTriggerType.PointerDown;
                entry_PointerDown.callback.AddListener((data) => { FindObjectOfType<MainScene>().Pointer_Down(btn_get.transform); });
                eventTrigger.triggers.Add(entry_PointerDown);

                EventTrigger.Entry entry_PointerUp = new EventTrigger.Entry();
                entry_PointerUp.eventID = EventTriggerType.PointerUp;
                entry_PointerUp.callback.AddListener((data) => { FindObjectOfType<MainScene>().Pointer_Up(btn_get.transform); });
                eventTrigger.triggers.Add(entry_PointerUp);


                return;
            case -1:

                Debug.Log("미완료");
                btn_get.interactable = false;
                btn_get.GetComponent<Image>().color = new Color(1,1,1,0.5f);
                play_anim = false;

                this.m_btn_txt.GetComponent<LanguageComponent>().SetText("TXT_NO_20001");

                btn_get.GetComponent<RectTransform>().sizeDelta = new Vector2(base_width, base_hight);

                return;
            default:
                return;
        }
    }



    public void OnClickButton()
    {
        int status = AchiveData.GetInstance().Get(this.type).status;
        if (status == -3)
        {
            Debug.Log("이건 완료");

            //ToastManager.Show("TXT_NO_50017", true);
            return;
        }

        if (status == -1)
        {
            Debug.Log("업적달성 못함");
            //ToastManager.Show("TXT_NO_50016", true);
            return;
        }


        GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/reward") as GameObject);
        obj.GetComponent<RewardPopup>().set_Achive(this.type);
        DialogManager.GetInstance().show(obj);


        //Utils.ShowVideoConfirm(tAchive.Item, "TXT_NO_50015", Confirm.ConfirmType.VIDEO2);
        AudioManager.GetInstance().PlayEffect("sound_eff_achive");

    }

}
