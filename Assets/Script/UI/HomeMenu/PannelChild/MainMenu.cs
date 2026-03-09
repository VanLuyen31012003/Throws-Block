using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button Btn_PlayGame;

    public Action cbHide;
    private void Awake()
    {
        UIHelper.AddButtonClickNormal(Btn_PlayGame, OnClickBtnPlayGame);
    }
    private void OnClickBtnPlayGame()
    {
        cbHide?.Invoke();
        SenceController.Instance.LoadSencePlay(StaticControl.KEY_SENCE_PLAY);
    }
}
