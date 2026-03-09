using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TabItem : MonoBehaviour
{
    [SerializeField]
    private Button btnTab;
    [SerializeField]
    private GameObject goTab, goSelect, goDeselect;

    private int _index;
    private Action<int> _onSelect;

    private void OnEnable()
    {
        UIHelper.AddButtonClickNormal(btnTab, () =>
        {
            _onSelect?.Invoke(_index);
        });
    }

    private void OnDisable()
    {
        btnTab.onClick.RemoveAllListeners();
    }

    public void Initialize(int index, Action<int> onSelect)
    {
        _index = index;
        _onSelect = onSelect;
    }

    public void SetSelected(bool isSelected = false)
    {
        if (goTab) goTab.SetActive(isSelected);
        goSelect.SetActive(isSelected);
        goDeselect.SetActive(!isSelected);
    }
}
