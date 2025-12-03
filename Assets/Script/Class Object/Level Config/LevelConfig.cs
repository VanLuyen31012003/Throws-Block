using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConfig
{
	/// <summary>
	/// level của màn
	/// </summary>
	public int level;
	/// <summary>
	/// số hàng của grid
	/// </summary>
	public int rows;
	/// <summary>
	/// số cột của grid
	/// </summary>
	public int cols;
	/// <summary>
	/// thông tin phân phối các ô
	/// </summary>
	public List<CellDataConfig> cellDataConfigs;
	
}
