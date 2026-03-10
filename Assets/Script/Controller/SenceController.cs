using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SenceController : Singleton<SenceController>
{
    protected override void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void LoadSence(string nameSence)
    {
        AsyncOperation taskLoad = SceneManager.LoadSceneAsync(nameSence);
     //   taskLoad.allowSceneActivation = false;
        UIManager.Instance.PlayLoadSlide(() => {
            //taskLoad.allowSceneActivation = true;
        });
    } 
    public void LoadSenceHome(string nameSence)
    {
        UIManager.Instance.ShowBlackScreen(true);
        UIManager.Instance.InitializedOnLoad();
        LoadSence(nameSence);
    }
    public void LoadSencePlay(string nameSence)
    {
        UIManager.Instance.HideAllPopup();
        UIManager.Instance.ShowBlackScreen(false);
        LoadSence(nameSence);
    }
}
