using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Singleten
    private static ScoreManager _instance;
    public static ScoreManager Instance=>_instance;

    /// <summary>
    /// Dữ liệu thông tin điểm số
    /// </summary>
	private LevelConfig _levelConfig;

	/// <summary>
	/// Dữ liệu thông tin điểm số
	/// </summary>
	public LevelConfig levelConfig {
        get { return this._levelConfig; }
        set { this._levelConfig = value; }
    }

	public void Awake()
	{
        // gán singleten
        _instance = this;
	}
	public void SetData(LevelConfig data)
	{
		this._levelConfig = data;
	}


}
