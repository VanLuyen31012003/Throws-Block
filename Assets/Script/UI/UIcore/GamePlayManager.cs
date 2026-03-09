using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
	#region singleten
	private static GamePlayManager _instance;
    public static GamePlayManager Instance=>_instance;
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
