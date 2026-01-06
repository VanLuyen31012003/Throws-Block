using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupWin : BasePopup
{
	#region serilize field
	/// <summary>
	/// Button chơi tiếp
	/// </summary>
	[SerializeField]	
	private Button UIButton_PlayNextLevel;

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
		this.UIButton_PlayNextLevel.onClick.AddListener(this.UIButton_PlayNextLevel_Clicked);
		this.UIButton_PlayLevelAgain.onClick.AddListener(this.UIButton_PlayLevelAgain_Clicked);

	}
	/// <summary>
	/// Hàm xử lý cho việc chơi tiếp
	/// </summary>
	private void UIButton_PlayNextLevel_Clicked()
	{
		GameManager.Instance.SetUpPlayGame();
		this.HidePopup();
	}
	/// <summary>
	/// Hàm xử lý cho việc chơi lại
	/// </summary>
	private void UIButton_PlayLevelAgain_Clicked()
	{
		LevelManager.Instance.BackLevel();
		GameManager.Instance.SetUpPlayGame();
		this.HidePopup();
	}
	#endregion
}
