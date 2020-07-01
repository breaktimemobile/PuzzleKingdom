using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum tween_type
{
    scale,
    rotate
}

public class ScaleObject : MonoBehaviour
{
    public tween_type tween_Type = tween_type.scale;
    private float scale = 0.95f;
    public float speed = 1;
    public float rotate_speed = 5;

    float width = 0;
    float hight = 0;

    float base_width = 0;
    float base_hight =0;

    // Start is called before the first frame update
    void Start()
    {
        switch (tween_Type)
        {
            case tween_type.scale:

                base_width = base_width != 0 ? base_width : GetComponent<RectTransform>().sizeDelta.x;
                base_hight = base_hight != 0 ? base_hight : GetComponent<RectTransform>().sizeDelta.y;

                width = GetComponent<RectTransform>().sizeDelta.x;
                hight = GetComponent<RectTransform>().sizeDelta.y;

                DOTween.To(() => width, x => width = x, width * scale, speed)
                   .SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

                DOTween.To(() => hight, x => hight = x, hight * scale, speed)
                 .SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

                break;
            case tween_type.rotate:

                width = GetComponent<RectTransform>().sizeDelta.x;
                hight = GetComponent<RectTransform>().sizeDelta.y;

                transform.DORotate(new Vector3(0, 0, -180), rotate_speed).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);

                break;
            default:
                break;
        }

    }

    private void Update()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(width, hight);
    }

    private void OnEnable()
    {
      
    }

    private void OnDisable()
    {
        //transform.DOKill();
    }
}
