using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
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
	/// Img background
	/// </summary>
	[SerializeField]
	private GameObject ImgBackground;

	/// <summary>
	/// space giữa các item trong grid
	/// </summary>
	[SerializeField]
    private float space;
	#endregion

	#region private and public field
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

	public int totalSquareWillAdd = 0;
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
				GameObject imgBg = Instantiate(this.ImgBackground, this.transform);
				GameObject gridItem = Instantiate(SquarePrefap, this.transform);
				float posX = j * _width;
				float posy = i * -_height;
				gridItem.transform.localPosition = new Vector2(posX, posy);
				imgBg.transform.localPosition = new Vector2(posX, posy);
				CellGrid[i, j] = gridItem.GetComponent<Cell>();
				CellGrid[i, j].x = i;
				CellGrid[i, j].y = j;
				// nếu nó có dữ liệu
				if(this.levelConfig.cellDataConfigs[countIndex].squareBoxDataConfigs.Count>=1)
				{
					foreach (var dataInCell in this.levelConfig.cellDataConfigs[countIndex].squareBoxDataConfigs)
					{
						CellGrid[i, j].SpawnBlock((ETypeBlock)dataInCell.type, dataInCell.count);
					}
				} else
				{
					CellGrid[i, j].SpawnBlock(ETypeBlock.NONE, 0);
				}

				countIndex++;
			}
		}
		this.transform.position = new Vector2(-1.9f, 3);
	}

	/// <summary>
	/// Xử lý merge ô bắn từ slide lên
	/// </summary>
	public void SnapMerge(int i, int j, GameObject gameObj)
	{	
		// nếu nó cùng loại thì merge hoặc ô này chưa có cái nào
		if (CellGrid[i, j].GetLastSquareType() == ETypeBlock.NONE||CellGrid[i, j].GetLastSquareType()== gameObj.GetComponent<Square>().typeBlock)
		{
			/// add thằng này vào
			CellGrid[i, j].AddBlock(gameObj);
			this.totalSquareWillAdd -= 1;
			if(this.totalSquareWillAdd <=0)
			{
				this.CheckMergeAround(i, j, this.GetCell(i, j).GetLastSquareType());
			}				
		}
	}
	public bool CanMerge(int i, int j, GameObject gameObj)
	{
		if (CellGrid[i, j].GetLastSquareType() == ETypeBlock.NONE || CellGrid[i, j].GetLastSquareType() == gameObj.GetComponent<Square>().typeBlock)
		{
			return true;
		}
		return false;
	}
	public bool TranslateCell(int i, int j, GameObject gameObj)
	{
		for (int e =0;e<=_row;e++)
		{
			CellGrid[e, j].AddBlock(gameObj);
		}
		return false;
	}
	/// <summary>
	/// Check 8 ô xung quanh xem có ô nào có phần tử đầu giống với cái hiện tại vừa add không
	/// </summary>
	public void CheckMergeAround(int i, int j,ETypeBlock typeBlock)
	{
		/// Hàng 1 ô đầu
		if (this.GetCell(i - 1, j - 1) != null && this.GetCell(i - 1, j - 1).CheckMergeCondition(typeBlock))
			this.PrepareMerge(i - 1, j - 1, i, j, typeBlock);
		/// Hàng 1 ô 2
		else if (this.GetCell(i - 1, j) != null && this.GetCell(i - 1, j).CheckMergeCondition(typeBlock))
			this.PrepareMerge(i -1 , j, i, j, typeBlock);
		/// Hàng 1 ô 3
		else if (this.GetCell(i - 1, j + 1) != null && this.GetCell(i - 1, j + 1).CheckMergeCondition(typeBlock))
			this.PrepareMerge(i - 1, j + 1, i, j, typeBlock);
		/// Hàng 2 ô đầu
		else if (this.GetCell(i, j - 1) != null && this.GetCell(i, j - 1).CheckMergeCondition(typeBlock))
			this.PrepareMerge(i, j - 1, i, j, typeBlock);
		/// Hàng 2 ô 3
		else if (this.GetCell(i, j + 1) != null && this.GetCell(i, j + 1).CheckMergeCondition(typeBlock))
			this.PrepareMerge(i, j + 1,i,j, typeBlock);
		/// Hàng 3 ô đầu
		else if (this.GetCell(i + 1, j - 1) != null && this.GetCell(i + 1, j - 1).CheckMergeCondition(typeBlock))
			this.PrepareMerge(i + 1, j - 1, i, j, typeBlock);
		/// Hàng 3 ô 2
		else if (this.GetCell(i + 1, j) != null && this.GetCell(i + 1, j).CheckMergeCondition(typeBlock))
			this.PrepareMerge(i + 1, j, i, j, typeBlock);
		/// Hàng 3 ô 3
		else if (this.GetCell(i + 1, j + 1) != null && this.GetCell(i + 1, j + 1).CheckMergeCondition(typeBlock))
			this.PrepareMerge(i + 1, j + 1, i, j, typeBlock);

	}

	/// <summary>
	/// Hàm lấy ra ô hiện tại mà k gây lỗi 
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <returns></returns>
	private Cell GetCell(int i, int j)
	{
		if (i < 0 || i >= _row || j < 0 || j >= _col)
		{
			return null;
		}	
		return this.CellGrid[i, j];
	}
	/// <summary>
	/// Hàm này tách đoạn merge to snap ra 1 hàm riêng để CheckMergeAround đỡ dài
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	private void PrepareMerge(int iNew, int jNew, int iOld, int jOld, ETypeBlock type)
	{
		// Bắt đầu Coroutine, Coroutine này sẽ chờ 0.5 giây trước khi chạy logic merge để tạo cảm giác từ từ
		StartCoroutine(DelayedMergeAction(iNew, jNew, iOld, jOld, type, 0.1f));
	}

	IEnumerator DelayedMergeAction(int iNew, int jNew, int iOld, int jOld, ETypeBlock type, float delay)
	{
		//delay nó tí
		yield return new WaitForSeconds(delay);
		this.totalSquareWillAdd = this.GetCell(iOld, jOld).GetListSameTypeFirst(type, true).Count;

		foreach (var square in this.GetCell(iOld, jOld).GetListSameTypeFirst(type))
		{
			// khả năng đoạn này cũng vần
			this.SnapMerge(iNew, jNew, square);
		}
	}
	#endregion
}
