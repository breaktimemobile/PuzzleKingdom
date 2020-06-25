using Assets.Scripts.GameManager;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    private struct DialogData
    {
        public GameObject parent;

        public GameObject child;

        public bool isIgnoreBack;

        public DialogData(GameObject parent, GameObject child, bool isIgnoreBack = false)
        {
            this.parent = parent;
            this.child = child;
            this.isIgnoreBack = isIgnoreBack;
        }
    }

    private static DialogManager Instance;

    private Stack<DialogManager.DialogData> m_childs = new Stack<DialogManager.DialogData>();

    private GameObject m_mask;

    public GameObject m_prefab_mask;

    public Transform m_parent;

    private void Awake()
    {
        DialogManager.Instance = this;
    }

    private void Start()
    {
        this.Init();
    }

    private void Update()
    {

    }

    public static DialogManager GetInstance()
    {
        return DialogManager.Instance;
    }

    public void show(GameObject obj, bool isIgnoreBack = false)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_prefab_mask, this.m_parent);
        gameObject.transform.localPosition = Vector3.zero;
        obj.transform.localPosition = new Vector3(0f, 2000f, 0f);
        obj.transform.SetParent(gameObject.transform, false);
        obj.transform.localScale = Vector3.zero;

        //Tween t = obj.transform.DOLocalMove(new Vector3(0,-100,0), 0.3f);
        obj.transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutBack);

        obj.transform.DOScale(1f, 0.1f);

        //Sequence _sequence = DOTween.Sequence();
        //_sequence.Append(t);
        //_sequence.Append(t1);
        //_sequence.SetTarget(gameObject);

        this.m_childs.Push(new DialogManager.DialogData(gameObject, obj, isIgnoreBack));
        Debug.Log(m_childs.Count);

        FindObjectOfType<MainScene>().Close_Icon();

        AudioManager.GetInstance().PlayEffect("sound_eff_popup");
    }

    public void Close(Action callfunc = null)
    {
        AudioManager.GetInstance().PlayEffect("sound_eff_popup_close");

        switch (PlayerPrefs.GetInt("MyGame", 0))
        {
            case 1:

                if (Game1DataLoader.GetInstance() != null)
                {
                    if (!Game1DataLoader.GetInstance().IsPlaying)
                    {
                        switch (Game1DataLoader.GetInstance().CurPropId)
                        {
                            case 1:
                                Debug.Log("아이템 1");
                                Game1DataLoader.GetInstance().CurPropId = 0;
                                Game1Manager.GetInstance().ControlPropsPannel(true);
                                return;

                            case 2:
                                Debug.Log("아이템 2");
                                Game1DataLoader.GetInstance().CurPropId = 0;
                                Game1Manager.GetInstance().ControlPropsPannel(true);
                                return;

                            case 3:
                                Debug.Log("아이템 3");
                                Game1DataLoader.GetInstance().CurPropId = 0;
                                Game1Manager.GetInstance().ControlPropsPannel(true);
                                return;
                        }
                    }
                }

                break;
            case 0:

                if (G2BoardGenerator.GetInstance() != null)
                {
                    G2BoardGenerator.GetInstance().CurPropId = 0;
                    G2BoardGenerator.GetInstance().IsPuase = false;
                    Game2Manager.GetInstance().ControlPropsPannel(true);
                }


                break;
            case 2:

                break;
        }

        if (this.m_childs.Count <= 0)
        {
            return;
        }

        Debug.Log(m_childs.Count);

        Debug.Log(m_childs.Peek().child.name);

       
        DialogManager.DialogData _dialogData = this.m_childs.Pop();

        _dialogData.child.transform.DOLocalMove(new Vector3(0, -2000, 0), 0.3f).SetEase(Ease.InBack)
            .OnComplete(
            delegate
            {

            });

        FindObjectOfType<MainScene>().Open_Icon();

        UnityEngine.Object.Destroy(_dialogData.parent, 0.3f);



        if (callfunc != null)
        {
            callfunc();
        }
    }

    public bool IsShow()
    {
        return this.m_childs.Count > 0;
    }

    public bool isIgnoreBack()
    {
        return this.m_childs.Peek().isIgnoreBack;
    }

    private void Init()
    {
        this.m_mask = UnityEngine.Object.Instantiate<GameObject>(this.m_prefab_mask, this.m_parent);
        this.m_mask.SetActive(false);
        this.m_mask.transform.localPosition = new Vector3(0f, 0f, 0f);
    }
}
