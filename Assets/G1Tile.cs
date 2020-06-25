using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G1Tile : MonoBehaviour
{
    //image of block
    public Image img_block;
    //normal color of block
    [SerializeField]
    public List<Color> colors = new List<Color>();

    //index of block
    private int index;
    //row index of block
    private int row;
    //col index of block
    private int col;
    //color index of block
    private int color;
    //width of block (pixel)
    private float width;
    //heigh of block(pixle)
    private float height;

    /*
     * get and set for parameter
     */

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

    public int Row
    {
        get
        {
            return this.row;
        }
        set
        {
            this.row = value;
        }
    }

    public int Col
    {
        get
        {
            return this.col;
        }
        set
        {
            this.col = value;
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

    public G1Tile(int idx, int color)
    {
        this.index = idx;
        this.color = color;
    }

    private void Start()
    {
    }

    private void Update()
    {
    }

    public Sprite[] Round;

    int BoardSize = 0;

    private void OnEnable()
    {
        BoardSize = PlayerPrefs.GetInt("BoardSize", 5);

    }

    public void Init(int idx, int int_color)
    {

        this.index = idx;
        this.color = int_color;
        this.SetPosition(idx);

        img_block.sprite = null;


        if (row % 2 == 0)
        {
            if (col % 2 == 0)
                img_block.color = colors[0];
            else
                img_block.color = colors[1];
        }
        else
        {
            if (col % 2 == 0)
                img_block.color = colors[1];
            else
                img_block.color = colors[0];
        }

        if (idx == 0)
        {
            img_block.sprite = Round[0];


        }
        else if (idx == BoardSize - 1)
        {
            img_block.sprite = Round[1];

        }
        else if (idx == (BoardSize * BoardSize) - BoardSize)
        {
            img_block.sprite = Round[2];

        }
        else if (idx == BoardSize * BoardSize - 1)
        {
            img_block.sprite = Round[3];

        }

    }

    public void SetContentSize(float x, float y)
    {
        this.SetContentSize(new Vector2(x, y));
    }
    /*
     * set size for this block  
     */
    public void SetContentSize(Vector2 v)
    {
        this.Width = v.x;
        this.Height = v.y;
        this.img_block.GetComponent<RectTransform>().sizeDelta = v;
    }
    /*
     * set position for this block in the board
     */
    public void SetPosition(int index)
    {
        Debug.Log("index " + index);
        this.index = index;
        this.SetPosition(GetRow(index), GetCol(index));
    }

    public void SetPosition(int row, int col)
    {
        Debug.Log(row + "  " + col);
        float m_cell_width = 800f / (float)this.BoardSize;

        this.row = row;
        this.col = col;
        base.transform.localPosition = new Vector3((float)col * m_cell_width + m_cell_width / 2f - 400f
        , 400f - (float)row * m_cell_width - m_cell_width / 2f, 0f);
    }

    public int GetRow(int index)
    {
        return index / this.BoardSize;
    }

    public int GetCol(int index)
    {
        return index % this.BoardSize;
    }
}
