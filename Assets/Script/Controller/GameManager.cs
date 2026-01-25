using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	#region singleten
	private static GameManager _instance;
	public static GameManager Instance => _instance;
	#endregion

	#region serilize field
	/// <summary>
	/// layer mask của cell để nhập
	/// </summary>
	[SerializeField]
	private LayerMask layerMaskCell;

	/// <summary>
	/// đối tượng quản lý lưới 
	/// </summary>
	[SerializeField]
	private GridManager gridManager;
	#endregion

	#region private and public field


	/// <summary>
	///   GridManager
	/// </summary>
	public GridManager GridManager {
		get { return gridManager; }
	}

	/// <summary>
	/// Quanr lý cell index của layerMaskkCell
	/// </summary>
	public LayerMask LayerMaskCell
	{
		get { return layerMaskCell; }
	}
	#endregion

	#region function monobehaviour
	private void Awake()
    {
        this.Initialize();
		// clear đi đẻ chơi từ đầu 
		PlayerPrefs.DeleteAll();
	}

	private void Start()
    {
		this.SetUpPlayGame();
	}
	#endregion

	#region function logic
	/// <summary>
	/// Hàm khởi tạo
	/// </summary>
	private void Initialize()
    {
		_instance = this;
	}
	public void SetUpPlayGame()
	{
		LevelConfig levelConfigNow = LevelManager.Instance.GetLevel();
		/// Build giao diện grid
		gridManager.Initialize(levelConfigNow);
		ScoreManager.Instance.SetData(levelConfigNow);
		//set up điểm yêu cầu cho setData
		UIManager.Instance.uICoreHub.SetData(levelConfigNow);
		PlayerController.Instance.Refresh();
		SupportController.Instance.Refresh();
	}

	#endregion
}
