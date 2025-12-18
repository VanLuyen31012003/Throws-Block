using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	#region singleten
	private static ScoreManager _instance;
    public static ScoreManager Instance=>_instance;
	#endregion

	#region private field
	/// <summary>
	/// Dữ liệu thông tin điểm số
	/// </summary>
	private LevelConfig _levelConfig;
	/// <summary>
	/// Yêu cầu của bàn chơi
	/// </summary>
	private List<Target> targets;
	#endregion

	#region public field
	/// <summary>
	/// Dữ liệu thông tin điểm số
	/// </summary>
	public LevelConfig levelConfig {
        get { return this._levelConfig; }
        set { this._levelConfig = value; }
    }
	#endregion

	#region function logic
	public void Awake()
	{
        // gán singleten
        _instance = this;
	}

	/// <summary>
	/// set dữ liệu cho manager này
	/// </summary>
	/// <param name="data"></param>
	public void SetData(LevelConfig data)
	{
		this._levelConfig = data;
		this.targets = data.targets;
	}
	/// <summary>
	/// add điểm
	/// </summary>
	public void AddPoint()
	{

	}
	public bool IsEnoughToMove()
	{
		return false;
	}	
	#endregion

}
