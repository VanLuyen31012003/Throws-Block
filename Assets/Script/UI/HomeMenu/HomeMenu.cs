using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeMenu : BasePopup
{
    [SerializeField]
    private TabController TabController;

    [SerializeField]
    private MainMenu mainMenu;

    private void Start()
    {
        mainMenu.cbHide =()=> { this.HidePopup(); };    
        TabController.Initialize(1);    
    }
}
