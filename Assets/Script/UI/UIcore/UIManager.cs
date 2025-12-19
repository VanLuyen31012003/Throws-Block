using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	#region singleten
	private static UIManager _instance;
    public static UIManager Instance=>_instance;
	#endregion

	#region Serilize Field
	/// <summary>
	/// quản lý popup
	/// </summary>
	public UIPopupController uIPopupController;

	/// <summary>
	/// quản lý corehub
	/// </summary>
	public UICoreHub uICoreHub;

	#endregion

	private void Awake()
    {
		_instance=this;
	}

}
