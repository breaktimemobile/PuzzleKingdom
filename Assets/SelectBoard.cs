using Assets.Scripts.GameManager;
using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBoard : MonoBehaviour
{
    private void Update()
    {
        Utils.BackListener(base.gameObject, delegate
        {
            this.OnClickReturn();
        });
    }

    public void OnClickReturn()
    {

        GlobalEventHandle.EmitClickPageButtonHandle("main", 0);

    }

    public void Select(int board)
    {
        Debug.Log("board " + board);
        //GM.GetInstance().BoardSize = board;
        PlayerPrefs.SetInt("BoardSize",board);
        if (DataManager.Instance.state_Player.LocalData_guide_game01 == 0)
        {
            GlobalEventHandle.EmitClickPageButtonHandle("G00103", 0);
            return;
        }

        gameObject.SetActive(false);

        FindObjectOfType<MainScene>().Game1Play();

    }
}
