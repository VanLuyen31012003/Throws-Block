using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UIHelper
{
    public static void AddButtonClickNormal(Button button, Action actionClick)
    {
        button.onClick.AddListener(() =>
        {
            if(SoundManager.Instance!=null)
            {
                SoundManager.Instance.PlaySoundButtonClick();
            }    
            actionClick?.Invoke();
        });
    }
}
