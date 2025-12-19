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

    /// <summary>
    /// tổng số square sẽ được thêm vào trong lần merge này
    /// </summary>
    private int totalSquareWillAdd = 0;
	/// <summary>
	/// loại type được add trong lần này
	/// </summary>
	private ETypeBlock typeBlockCache;
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
				if (this.levelConfig.cellDataConfigs[countIndex].squareBoxDataConfigs.Count >= 1)
				{
					foreach (var dataInCell in this.levelConfig.cellDataConfigs[countIndex].squareBoxDataConfigs)
					{
						CellGrid[i, j].SpawnBlock((ETypeBlock)dataInCell.type, dataInCell.count);
					}
				}
				else
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
	public void SnapMerge(int i, int j, Cell cellMerge)
	{
		// nếu nó cùng loại thì merge hoặc ô này chưa có cái nào
		ETypeBlock eTypeCache = cellMerge.GetLastSquareType();
		if (CellGrid[i, j].GetLastSquareType() == eTypeCache || CellGrid[i, j].GetLastSquareType() == ETypeBlock.NONE)
		{
			/// bắt đầu merge
			foreach (var square in cellMerge.GetListSameTypeFirst(eTypeCache))
			{
				CellGrid[i, j].AddSquare(square);
			}
            this.totalSquareWillAdd = CellGrid[i, j].GetTotalSuareSameTypeOntop();
			this.typeBlockCache = eTypeCache;
            /// sau check 8 ô xung quanh
            this.CheckMergeAround(i, j, this.GetCell(i, j).GetLastSquareType());
		}
    }
	public bool CanMerge(int i, int j, Cell cellNeedCheck)
	{
		if (CellGrid[i, j].GetLastSquareType() == cellNeedCheck.GetLastSquareType())
		{
			return true;
		}
		return false;
	}
	/// <summary>
	///  hàm này sẽ để merge ô bắn từ slide lên
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <param name="cellMerge"></param>
	public void MergeToNoneBlock(int i, int j, Cell cellMerge)
	{
		/// bắt đầu merge
		foreach (var square in cellMerge.lstBlock)
		{
			CellGrid[i, j].AddSquare(square);
		}
		// xóa thằng cell bắn này đi
		Destroy(cellMerge.gameObject);
		// lúc này thì CellGrid[i, j] này đã có dữ liệu
		/// sau check 8 ô xung quanh để merge
		this.CheckMergeAround(i, j, this.GetCell(i, j).GetLastSquareType());
		/// add điểm xem được không
		ScoreManager.Instance.AddPoint(this.totalSquareWillAdd, typeBlockCache);
        if(ScoreManager.Instance.CheckWin())
		{
            return;
        }	
        // reset tổng số ô add và loại add 
        this.typeBlockCache = ETypeBlock.NONE;
        this.totalSquareWillAdd = 0;
        //cuối cùng check nếu nó vẫn còn thì mình sẽ đẩy ô lên
        if (CellGrid[i, j].lstBlock.Count > 0)
		{
			// chỉ translate khi mà đây là hàng cuối
			if (i == _row - 1)
			{
				this.TranslateCell(i, j, CellGrid[i, j]);
			}
		}
        // sau khi xong thì sẽ check xem thằng này hết lượt chưa 
		if(ScoreManager.Instance.IsHaveTurn()==false)
		{
            // hết rồi mà vẫn không thắng thì show popup lose luôn
            ScoreManager.Instance.ShowLose();
        }
    }
	public void TranslateCell(int rowIndex, int col, Cell cellNeedTranslate)
	{
		// clear dữ liệu trong list cái này đi
		 CellGrid[0, col].ClearAndDestroyListGameObj();
		// bắt đầu dịch dữ liệu
		for (int row = 1; row <= rowIndex; row++)
		{
			// add dữ liệu block của thằng này sang thằng trước
			//CellGrid[row - 1, col] = CellGrid[row, col]; 
			foreach (var square in CellGrid[row, col].lstBlock)
			{
				CellGrid[row - 1, col].AddSquare(square);
			}
			// sau đó sẽ clear cái list đi 
			CellGrid[row, col].ClearListGameObj();

		}
		//Cập nhật lại text cho ô cuối
		CellGrid[rowIndex, col].SetTextNumberTotalSameType();
	}

	/// <summary>
	/// Check 8 ô xung quanh xem có ô nào có phần tử đầu giống với cái hiện tại vừa add không
	/// </summary>
	public void CheckMergeAround(int i, int j, ETypeBlock typeBlock)
	{
		/// Hàng 1 ô đầu
		if (this.GetCell(i - 1, j - 1) != null && this.GetCell(i - 1, j - 1).CheckMergeCondition(typeBlock))
			this.PrepareMerge(i - 1, j - 1, i, j, typeBlock);
		/// Hàng 1 ô 2
		else if (this.GetCell(i - 1, j) != null && this.GetCell(i - 1, j).CheckMergeCondition(typeBlock))
			this.PrepareMerge(i - 1, j, i, j, typeBlock);
		/// Hàng 1 ô 3
		else if (this.GetCell(i - 1, j + 1) != null && this.GetCell(i - 1, j + 1).CheckMergeCondition(typeBlock))
			this.PrepareMerge(i - 1, j + 1, i, j, typeBlock);
		/// Hàng 2 ô đầu
		else if (this.GetCell(i, j - 1) != null && this.GetCell(i, j - 1).CheckMergeCondition(typeBlock))
			this.PrepareMerge(i, j - 1, i, j, typeBlock);
		/// Hàng 2 ô 3
		else if (this.GetCell(i, j + 1) != null && this.GetCell(i, j + 1).CheckMergeCondition(typeBlock))
			this.PrepareMerge(i, j + 1, i, j, typeBlock);
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
		DelayedMergeAction(iNew, jNew, iOld, jOld, type, 0.1f);
	}
	// hàm trả về cell trên ô hiện tại có đang không có phần tử nào không
	public bool CheckNone(int i, int j)
	{
		bool x = this.GetCell(i -1, j).lstBlock.Count > 0;
		Debug.Log("giá trị của checknone là:"+x);
		return this.GetCell(i-1, j).lstBlock.Count > 0;
	}
	#endregion

	#region funtion IEnumerator
	private void DelayedMergeAction(int iNew, int jNew, int iOld, int jOld, ETypeBlock type, float delay)
	{
		this.SnapMerge(iNew, jNew, this.GetCell(iOld, jOld));
	}
	private void DelayedTranslate(Cell cell, float delay)
	{
		//delay nó tí
		Vector2 cachePos = cell.transform.localPosition;
		cell.transform.localPosition = new Vector2(cachePos.x, cachePos.y + -_height);
		Destroy(cell.gameObject);
	}
	#endregion
}
