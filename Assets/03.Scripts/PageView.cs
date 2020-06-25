using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PageView : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler, IEndDragHandler
{
    public float m_scrollSpeed = 800f;

    public RectTransform m_content;

    private int pageIdx = 1;

    private bool m_isMove;

    private bool m_isDrag;

    private GridLayoutGroup m_grid;

    private Vector2 m_startPosition = Vector3.zero;

    private float m_edge;

    public Action OnDragHandle;

    public Action<int> OnClickHandle = null;

    public GameObject[] dots;

    public int PageIdx
    {
        get
        {
            return this.pageIdx;
        }
        set
        {
            this.pageIdx = value;
        }
    }

    private void Start()
    {
        if (this.m_content == null)
        {
            return;
        }
        this.m_grid = this.m_content.GetComponent<GridLayoutGroup>();
        this.m_edge = (this.m_content.sizeDelta.x - (float)this.m_grid.constraintCount * this.m_grid.cellSize.x) / 2f;
        Change_dot();
        this.ScrollToPage(1, null, false);

    }

    public void Update()
    {
        if (!this.m_isMove)
        {
            return;
        }
        this.DoDragAni();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (this.m_content == null)
        {
            return;
        }
        DOTween.Kill(this.m_content, false);
        this.m_startPosition = eventData.pressPosition;
        this.m_isMove = true;
        this.m_isDrag = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (this.m_content == null)
        {
            return;
        }
        int num = this.pageIdx;
        float num2 = eventData.position.x - this.m_startPosition.x;
        if (num2 >= 120f)
        {
            num--;
            num = ((num < 0) ? 0 : num);
        }
        else if (num2 <= -120f)
        {
            num++;
            num = ((num > 2) ? 2 : num);
        }
        else
        {
            num = this.ConvertToIdx(this.m_content.anchoredPosition);
        }
        this.ScrollToPage(num, null, true);
        this.m_isDrag = false;
    }

    public void ScrollToPage(int idx, Action callFunc = null, bool isAni = true)
    {
        if (this.m_content == null)
        {
            return;
        }
        this.m_isMove = true;
        GridLayoutGroup component = this.m_content.GetComponent<GridLayoutGroup>();
        float num = -1f * ((float)idx * component.cellSize.x + component.cellSize.x / 2f + this.m_edge);
        if (isAni)
        {
            this.m_content.DOKill(this.m_content);
            DOTween.To(() => this.m_content.anchoredPosition, delegate (Vector2 x)
            {
                this.m_content.anchoredPosition = x;
            }, new Vector2(num, this.m_content.anchoredPosition.y), Math.Abs((this.m_content.anchoredPosition.x - num) / this.m_scrollSpeed)).SetTarget(this.m_content).OnComplete(delegate
            {
                this.m_isMove = false;
                Action expr_12 = callFunc;
                if (expr_12 == null)
                {
                    return;
                }
                expr_12();
            });
        }
        else
        {
            this.m_content.anchoredPosition = new Vector2(num, this.m_content.anchoredPosition.y);
        }
        this.PageIdx = idx;
        Change_dot();

    }

    public void Change_dot()
    {
        dots[0].GetComponent<Image>().color = 0 == PageIdx ? new Color(255 / 255f, 213 / 255f, 53 / 255f) : Color.white;
        dots[1].GetComponent<Image>().color = 1 == PageIdx ? new Color(255 / 255f, 213 / 255f, 53 / 255f) : Color.white;
        dots[2].GetComponent<Image>().color = 2 == PageIdx ? new Color(255 / 255f, 213 / 255f, 53 / 255f) : Color.white;
    }

    private int ConvertToIdx(Vector2 position)
    {
        float num = 0f;
        float x = this.m_content.sizeDelta.x;
        GridLayoutGroup component = this.m_content.GetComponent<GridLayoutGroup>();
        int result;
        if (this.m_content.anchoredPosition.x >= num)
        {
            result = 0;
        }
        else if (this.m_content.anchoredPosition.x <= -1f * x)
        {
            result = component.constraintCount - 1;
        }
        else
        {
            result = (int)Math.Floor((double)(Math.Abs(this.m_content.anchoredPosition.x + this.m_edge) / component.cellSize.x));
        }
        return result;
    }

    private void DoDragAni()
    {
        float num = base.GetComponent<RectTransform>().sizeDelta.x / 2f;
        for (int i = 0; i < this.m_content.childCount; i++)
        {
            Transform child = this.m_content.GetChild(i);
            float num2 = Math.Abs(base.transform.InverseTransformPoint(child.position).x);
            num2 = ((num2 >= num) ? num : num2);
            float num3 = 1f - 0.2f * (num2 / num);
            child.localScale = new Vector3(num3, num3, num3);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (this.m_content == null)
        {
            return;
        }
        if (Vector3.Distance(eventData.pressPosition, eventData.position) > 30f)
        {
            this.m_isDrag = true;
        }
        float x = this.m_content.anchoredPosition.x + eventData.delta.x;
        if (this.isOutLeftEdge(x))
        {
            x = this.m_content.anchoredPosition.x + 2f;
        }
        else if (this.isOutRightEdge(x))
        {
            x = this.m_content.anchoredPosition.x - 2f;
        }
        float y = this.m_content.anchoredPosition.y;
        this.m_content.anchoredPosition = new Vector2(x, y);
        Action _action = this.OnDragHandle;
        if (_action == null)
        {
            return;
        }
        _action();
    }

    public void GameStart()
    {
        if (this.m_isDrag)
        {
            return;
        }
        Action<int> _action = this.OnClickHandle;
        if (_action == null)
        {
            return;
        }
        _action(0);
    }

    private bool isOutLeftEdge(float x)
    {
        return x > -1f * (this.m_grid.cellSize.x / 2f + this.m_edge);
    }

    private bool isOutRightEdge(float x)
    {
        return x < -1f * (this.m_grid.cellSize.x * (float)this.m_grid.constraintCount - this.m_grid.cellSize.x / 2f + this.m_edge);
    }
}
