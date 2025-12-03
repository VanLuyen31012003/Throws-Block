using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDataConfig
{
	public int row;
	public int column;
	public List<SquareBoxDataConfig> squareBoxDataConfigs;
}
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
