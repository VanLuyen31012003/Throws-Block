using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private Transform FrameContent;

    [SerializeField]
    private UILoading UILoading;

    [SerializeField]
    private Image imgBlack;

    private Dictionary<string,GameObject> dicPopup = new Dictionary<string, GameObject>();
    protected void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        this.PlayLoadSlide();
        this.InitializedOnLoad();
    }   
    public void InitializedOnLoad()
    {
        this.OpenPopupByName(UIConstant.UIHomeMenu);
    }

    public void OpenPopupByName(string name, Action actionCb =null)
    {
        if(dicPopup.ContainsKey(name))
        {
            if (!dicPopup[name].activeSelf)
            {
                dicPopup[name].SetActive(true);
                actionCb?.Invoke();
            }
            return;
        }    
        GameObject prefab = Resources.Load<GameObject>("Prefabs/" + name);
        if (prefab != null)
        {
            GameObject gameobj= Instantiate(prefab, FrameContent, false);
            dicPopup.Add(name, gameobj);
            actionCb?.Invoke();
        }
        else
        {
            Debug.LogError("Không tìm thấy prefab ở đường dẫn:Prefabs/" + name);
        }
    }
    public void PlayLoadSlide(Action actionCb=null)
    {
       UILoading.PlayLoadSlide(actionCb);
    }
    public void HideAllPopup()
    {
        foreach (var child in dicPopup.Values)
        {
            child.gameObject.SetActive(false);
        }
    }
    public void ShowBlackScreen(bool value)
    {
        this.imgBlack.gameObject.SetActive(value);
    }
}
