using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TabController : MonoBehaviour
{
    [SerializeField] private List<TabItem> tabItems;

    private int _selectedIndex = -1;
    private Action<int> _onSelectTab;

    public void Initialize(int selectedIndex = 0, Action<int> onSelectTab = null)
    {
        _selectedIndex = -1;
        _onSelectTab = onSelectTab;
        for (var i = 0; i < tabItems.Count; i++)
        {
            tabItems[i].Initialize(i, SelectTab);
            tabItems[i].SetSelected();
        }

        SelectTab(selectedIndex);
    }

    public void SetEnableTab(int index, bool isEnable)
    {
        tabItems[index].gameObject.SetActive(isEnable);
    }

    private void SelectTab(int index)
    {
        if (_selectedIndex == index) return;
        if (_selectedIndex >= 0)
        {
            tabItems[_selectedIndex].SetSelected();
        }

        _selectedIndex = index;
        _onSelectTab?.Invoke(index);
        tabItems[_selectedIndex].SetSelected(true);
    }
}

