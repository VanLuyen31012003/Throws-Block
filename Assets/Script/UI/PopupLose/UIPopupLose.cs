using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupLose : BasePopup
{
	#region serilize field
	/// <summary>
	/// Button về trang chủ
	/// </summary>
	[SerializeField]
	private Button UIButton_BackHome;

	/// <summary>
	/// Button chơi lại
	/// </summary>
	[SerializeField]
	private Button UIButton_PlayLevelAgain;
	#endregion

	#region Function logic
	private void Start()
	{
		this.Initialize();
	}

	private void Initialize()
	{
		this.UIButton_BackHome.onClick.AddListener(this.UIButton_BackHome_Clicked);
		this.UIButton_PlayLevelAgain.onClick.AddListener(this.UIButton_PlayLevelAgain_Clicked);

	}
	/// <summary>
	/// Hàm xử lý cho việc chơi tiếp
	/// </summary>
	private void UIButton_BackHome_Clicked()
	{
		this.HidePopup();
	}
	/// <summary>
	/// Hàm xử lý cho việc chơi lại
	/// </summary>
	private void UIButton_PlayLevelAgain_Clicked()
	{
		GameManager.Instance.SetUpPlayGame();
		this.HidePopup();
	}
	#endregion
}
