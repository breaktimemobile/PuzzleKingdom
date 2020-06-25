using Assets.Scripts.GameManager;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * this class will attach to level block or level line for line connect games
 */
public class G3Line : MonoBehaviour
{
    //image of block (show line or show level block)
	public Image img_block;
    //star when level block are connected
	public Image img_star;
    //if this object is line, it has list of sprite for all direction
    //list color for this object
    [SerializeField]
	public List<Sprite> block_sp = new List<Sprite>();
    //index of this object
	private int index;
    //current color of this object
	private int color;
    //type of object (line : SEGMENT or level block : TARGET)
	private G3BoardGenerator.Node_type type;
    //direction if this object is line
	private G3BoardGenerator.Direction direction;
    //width of object
	private float width;
    //heigh of object
	private float height;


    public int Index
	{
		get
		{
			return this.index;
		}
		set
		{
			this.index = value;
		}
	}

	public int Color
	{
		get
		{
			return this.color;
		}
		set
		{
			this.color = value;
		}
	}

	public G3BoardGenerator.Node_type Type
	{
		get
		{
			return this.type;
		}
		set
		{
			this.type = value;
		}
	}

	public G3BoardGenerator.Direction Direction
	{
		get
		{
			return this.direction;
		}
		set
		{
			this.direction = value;
		}
	}

	public float Width
	{
		get
		{
			return this.width;
		}
		set
		{
			this.width = value;
		}
	}

	public float Height
	{
		get
		{
			return this.height;
		}
		set
		{
			this.height = value;
		}
	}


    private void Start()
    {
    }

    private void Update()
    {
    }

    public void Init(int idx, int color, G3BoardGenerator.Node_type type)
    {

        this.Index = idx;
        this.Color = color;
        this.Type = type;
        this.SetPosition(idx);
        this.SetColor(color);
        this.SetType(type);
        this.img_star.gameObject.SetActive(false);

        if(type == G3BoardGenerator.Node_type.TARGET)
        img_block.color =  new Color(1, 1, 1, 1f);

    }

    public void SetContentSize(float x, float y)
    {
        this.SetContentSize(new Vector2(x, y));
    }

    public void SetContentSize(Vector2 v)
    {
        this.Width = v.x;
        this.Height = v.y;
        this.img_block.GetComponent<RectTransform>().sizeDelta = v;
    }

    public void SetPosition(int index)
    {
        this.SetPosition(G3BoardGenerator.GetInstance().GetRow(index), G3BoardGenerator.GetInstance().GetCol(index));
    }

    public void SetPosition(int row, int col)
    {
        base.transform.localPosition = new Vector3((float)col * G3BoardGenerator.GetInstance().Cell_height + G3BoardGenerator.GetInstance().Cell_height / 2f - 400f,
         400f - (float)row * G3BoardGenerator.GetInstance().Cell_width - G3BoardGenerator.GetInstance().Cell_width / 2f, 0f);
    }

    public void SetType(G3BoardGenerator.Node_type idx)
    {

        //if (this.sprites[(int)idx] != null)
        //{
        //    this.Type = idx;
        //    this.img_block.sprite = this.sprites[(int)idx];
        //}
        this.ReloadBlock();
    }

    public void SetColor(int idx)
    {
        this.Color = idx;

        img_block.color = new Color(1, 1, 1, 0.5f);

        Debug.Log("컬러 세팅");
        if (idx == 0)
        {
            img_block.color = new Color(0, 0, 0, 0);
        }

        this.img_block.sprite = this.block_sp[idx];
    }

    public void SetDirection(G3BoardGenerator.Direction diec)
    {
        this.Direction = diec;
        this.ReloadBlock();
    }
    //show or hide star if block are connected
    public void ShowStar()
    {
        this.img_star.gameObject.SetActive(true);

    }

    public void HideStar()
    {
        this.img_star.gameObject.SetActive(false);
    }
    //set direction for line
    public void ReloadBlock()
    {
        if (this.Type == G3BoardGenerator.Node_type.SEGMENT)
        {
            Transform transform = this.img_block.transform;
            //switch (this.Direction)
            //{
            //    case G3BoardGenerator.Direction.UP:
            //        //this.img_block.sprite = this.sprites[3];
            //        //transform.localPosition = new Vector3(0f, -this.Height, -1f);
            //        return;
            //    case G3BoardGenerator.Direction.RIGHT:
            //        //this.img_block.sprite = this.sprites[2];
            //        //transform.localPosition = new Vector3(-this.Width, 0f, -1f);
            //        return;
            //    case G3BoardGenerator.Direction.DOWN:
            //        //this.img_block.sprite = this.sprites[3];
            //        //transform.localPosition = new Vector3(0f, this.Height, -1f);
            //        return;
            //    case G3BoardGenerator.Direction.LEFT:
            //        //this.img_block.sprite = this.sprites[2];
            //        //transform.localPosition = new Vector3(this.Width, 0f, -1f);
            //        return;
            //    default:
            //        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(this.width, this.height);
            //        transform.localPosition = new Vector3(0f, 0f, -1f);
            //        break;
            //}
        }
    }

    public Vector3 GetPosition()
    {
        return base.transform.localPosition;
    }
}
