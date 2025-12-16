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
	public void SnapMerge(int i, int j, Cell cellMerge)
	{
		// nếu nó cùng loại thì merge hoặc ô này chưa có cái nào
		ETypeBlock eTypeCache = cellMerge.GetLastSquareType();
		if (CellGrid[i, j].GetLastSquareType()== eTypeCache)
		{
			/// bắt đầu merge
			foreach (var square in cellMerge.GetListSameTypeFirst(eTypeCache))
			{
				CellGrid[i, j].AddBlock(square);
			}
			/// sau check 8 ô xung quanh
			this.CheckMergeAround(i, j, this.GetCell(i, j).GetLastSquareType());
			// sau khi xong sẽ destroy thằng cell bắn đi
			// check nếu thằng này k có giá trị x,y tức là nó k phải cell nằm trong grid
			if (cellMerge.x < 0 && cellMerge.y < 0)
			{
				Destroy(cellMerge.gameObject);
			}
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
	public void TranslateCell(int col, Cell cellNeedTranslate)
	{
		// 1️⃣ Destroy cell ở đầu cột (row = 0)
		Cell topCell = CellGrid[0, col];
		if (topCell != null)
		{
			StartCoroutine(DelayedTranslate(topCell, 3f));
		}

		// 2️⃣ Dịch các cell còn lại lên trên
		for (int row = 1; row < _row-2; row++)
		{
			CellGrid[row - 1, col] = CellGrid[row, col];

			if (CellGrid[row - 1, col] != null)
			{
				CellGrid[row - 1, col].x = row - 1;
				CellGrid[row - 1, col].y = col;

				// cập nhật vị trí
				float posX = col * _width;
				float posY = (row - 1) * -_height;
				CellGrid[row - 1, col].transform.localPosition = new Vector2(posX, posY);
			}
		}
		// 3️⃣ Đưa cell bắn vào vị trí cuối cột
		int lastRowMerge = _row - 1;
		CellGrid[lastRowMerge, col] = cellNeedTranslate;

		cellNeedTranslate.transform.SetParent(this.transform);
		cellNeedTranslate.x = lastRowMerge;
		cellNeedTranslate.y = col;

		float finalX = col * _width;
		float finalY = (lastRowMerge) * -_height;
		cellNeedTranslate.transform.localPosition = new Vector2(finalX, finalY);
		cellNeedTranslate.gameObject.layer = LayerMask.NameToLayer("Cell");
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
		this.SnapMerge(iNew, jNew, this.GetCell(iOld, jOld));
	}
	IEnumerator DelayedTranslate(Cell cell, float delay)
	{
		//delay nó tí
		Vector2 cachePos= cell.transform.localPosition;
		cell.transform.localPosition = new Vector2(cachePos.x,cachePos.y + -_height);
		yield return new WaitForSeconds(delay);
		Destroy(cell.gameObject);
	}
	#endregion
}
