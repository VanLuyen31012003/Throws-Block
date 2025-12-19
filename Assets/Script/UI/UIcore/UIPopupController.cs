using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopupController : MonoBehaviour
{
	#region Serilize Field
	/// <summary>
	/// popup thắng
	/// </summary>
	[SerializeField]
	private UIPopupWin UIPopupWin;

	/// <summary>
	/// popup thua
	/// </summary>
	[SerializeField]
	private UIPopupLose UIPopupLose;

	/// <summary>
	/// popup setting
	/// </summary>
	[SerializeField]
	private BasePopup UIPopupSetting;
	#endregion

	public void ShowWinPopup()
	{
		this.UIPopupWin.ShowPopup();
    }
	/// <summary>
	/// show popup thua
	/// </summary>
	public void ShowLosePopup()
	{
		this.UIPopupLose.ShowPopup();
    }	

}
