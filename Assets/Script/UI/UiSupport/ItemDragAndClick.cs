using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDragAndClick : MonoBehaviour
{
	#region Private Fields
	[SerializeField]
	ETypeItemClickDrag TypeItem;

	[SerializeField]
	private Button UIButton_Select;
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
	#endregion
}
