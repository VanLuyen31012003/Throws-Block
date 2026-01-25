using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDragAndClick : MonoBehaviour
{
	#region Private Fields
	[SerializeField]
	ETypeItemClickDrag TypeItem;

	[SerializeField]
	private Button UIButton_Select;

	[SerializeField]
	private TextMeshProUGUI UIText_Times;
	#endregion

	#region Public Fields
	public ETypeItemClickDrag TypeItemClickDrag => TypeItem;
	#endregion

	#region function monobehaviour
	private void Start()
	{
		Initialize();
	}
	#endregion

	#region function logic
	private void Initialize()
	{
		this.UIButton_Select.onClick.AddListener(this.UIButton_Select_Clicked);
	}	
	private void UIButton_Select_Clicked()
	{
		SupportController.Instance.SetItemSpClick(this);
	}
	public void SetData(int count)
	{
        if (count<=0)
        {
			UIText_Times.text = "+";
		}
		else
			UIText_Times.text = count.ToString();
	}
	#endregion
}
