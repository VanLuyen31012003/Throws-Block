using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Utils 
{
    public static void SetActiveButton(Button button, bool isActive=true)
    {
        button.interactable = isActive;
        Image[] images = button.GetComponent<Button>().GetComponentsInChildren<Image>();

        foreach (Image img in images)
        {
            if(isActive)
                img.color = Color.white;
            else
                img.color = Color.gray;
        }
    }
}
