using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
	/// <summary>
	/// dict parse dữ liệu từ json ra thông tin của level vào 1 dic, dic này có key là level 
	/// </summary>
	Dictionary<int,LevelConfig> levelConfigs = new Dictionary<int,LevelConfig>();
	protected override void Awake()
	{
		base.Awake();
		LoadInfoLevelConfigs();
	}

	/// hàm đọc dữ liệu từ config parse vào levelConfigs
	/// </summary>
	private void LoadInfoLevelConfigs()
	{
		TextAsset jsonFile = Resources.Load<TextAsset>("DataGame/LevelConfigJson/AllLevels");

		if (jsonFile == null)
		{
			Debug.LogError("Không tìm thấy AllLevels.json!");
			return;
		}
		LevelConfigList wrapper = JsonUtility.FromJson<LevelConfigList>(jsonFile.text);

		foreach (LevelConfig config in wrapper.levels)
		{
			levelConfigs.Add(config.level,config);
		}

		Debug.Log($"Đã load thành công {levelConfigs.Count} level!");
	}
	//Set Level của game hiện tại
	public void UpdateLevel()
	{
		int levelPlayerNow = PlayerPrefs.GetInt(StaticControl.KEY_LEVEL, -1);
		if (levelPlayerNow < 0)
		{
			Debug.Log("Set level Mặc định là 1");
			PlayerPrefs.SetInt(StaticControl.KEY_LEVEL,1);
			return;
		}
		PlayerPrefs.SetInt(StaticControl.KEY_LEVEL, levelPlayerNow+1);
		Debug.Log($"Update từ level {levelPlayerNow} lên level {levelPlayerNow + 1}");
	}
	// Lấy ra Level hiện tại của người chơi
	public LevelConfig GetLevel()
	{
		int levelPlayerNow = PlayerPrefs.GetInt(StaticControl.KEY_LEVEL, -1);
		// nếu đ có thì cho thằng này là level1
		if (levelPlayerNow < 0)
		{
			// set level mặc định là 1
			PlayerPrefs.SetInt(StaticControl.KEY_LEVEL, 1);
			return levelConfigs[1];
		}
		return levelConfigs[levelPlayerNow].Clone();
	}
}
