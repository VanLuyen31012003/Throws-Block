using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    #region serilize field
    /// <summary>
    /// prefap của  square
    /// </summary>
    [SerializeField]
    private GameObject SquarePrefap;

    /// <summary>
    /// space giữa các item trong grid
    /// </summary>
    [SerializeField]
    private float space;
	#endregion

	#region private field
	/// <summary>
	/// thông tin của level hiện tại
	/// </summary>
	private LevelConfig levelConfig;
	/// <summary>
	/// grid quản lý các ô
	/// </summary>
	private Cell[,] CellGrid;
	/// <summary>
	/// số dòng
	/// </summary>
	private int _row;
	/// <summary>
	/// số cột
	/// </summary>
	private int _col;
	/// <summary>
	/// size width của prefap
	/// </summary>
	private float _width;
	/// <summary>
	/// size height của prefap
	/// </summary>
	private float _height;
	#endregion

	#region function logic
	/// <summary>
	/// set up chỉ số
	/// </summary>
	public void Initialize(LevelConfig levelConfig)
	{
		this._row = levelConfig.rows;
		this._col = levelConfig.cols;
		this.CellGrid = new Cell[_row, _col];
		this.levelConfig = levelConfig;
		SpriteRenderer spriteRenderer = SquarePrefap.GetComponent<SpriteRenderer>();
		this._width = spriteRenderer.sprite.bounds.size.x;
		this._height = spriteRenderer.sprite.bounds.size.y;
		Debug.Log("size của w:" + _width);
		Debug.Log("size của h:" + _height);
		this.BuildGridLayout();
    }
	/// <summary>
	/// xây giao diện grid
	/// </summary>
	private void BuildGridLayout()
	{
		int countIndex = 0;
		for (int i = 0; i < this._row; i++)
		{
			for (int j = 0; j < this._col; j++)
			{
				GameObject gridItem = Instantiate(SquarePrefap, this.transform);
				float posX = j * _width;
				float posy = i * -_height;
				gridItem.transform.localPosition = new Vector2(posX, posy);
				CellGrid[i, j] = gridItem.GetComponent<Cell>();
				foreach(var dataInCell  in this.levelConfig.cellDataConfigs[countIndex].squareBoxDataConfigs)
				{
					CellGrid[i, j].SpawnBlock((ETypeBlock)dataInCell.type, dataInCell.count);
				}				
				countIndex++;
			}
		}
		this.transform.position = new Vector2(-1.9f, 3);
	}
	#endregion
}
