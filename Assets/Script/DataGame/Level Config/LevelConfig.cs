using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class LevelConfigList
{
	public List<LevelConfig> levels;
}
[Serializable]
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
	/// Số lượt 1 round
	/// </summary>
	public int numberInARound;
	/// <summary>
	///  Yêu cầu target 1 route
	/// </summary>
	public List<Target> targets;
	/// <summary>
	/// thông tin phân phối các ô
	/// </summary>
	public List<CellDataConfig> cellDataConfigs;
	// Cái này để tránh chung reference 
	public LevelConfig Clone()
	{
		return (LevelConfig)this.MemberwiseClone();
	}

}
[Serializable]
public class CellDataConfig
{
	public int row;
	public int column;
	public List<SquareBoxDataConfig> squareBoxDataConfigs;
}
[Serializable]
public class SquareBoxDataConfig
{
	/// <summary>
	/// ô vuông loại gì(xanh, đỏ , tím, vàng,..)
	/// sẽ có 1 enum riêng quy định xem type nào là màu nào
	/// </summary>
	public int type;

	/// <summary>
	/// số lượng của ô vuông màu này
	/// </summary>
	public int count;
}
[Serializable]
public class Target
{
	#region thông
	/// <summary>
	/// ô vuông loại gì(xanh, đỏ , tím, vàng,..)
	/// sẽ có 1 enum riêng quy định xem type nào là màu nào
	/// </summary>
	public int type;

	/// <summary>
	/// số lượng của ô vuông màu này
	/// </summary>
	public int countNeed;
	#endregion
}