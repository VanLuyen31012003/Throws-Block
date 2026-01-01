using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Collections;
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
	/// cell cuối cùng lúc merge
	/// </summary>
	private Cell CurrentCellLast;
    #endregion

    #region function logic
    /// <summary>
    /// set up chỉ số
    /// </summary>
    public void Initialize(LevelConfig levelConfig)
	{
		this.ClearGrid();
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
				if (this.levelConfig.cellDataConfigs.Count> countIndex && this.levelConfig.cellDataConfigs[countIndex].squareBoxDataConfigs.Count >= 1)
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
	///  hàm này sẽ để merge ô bắn từ slide lên
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <param name="cellMerge"></param>
	public void MergeToNoneBlock(int i, int j, Cell cellMerge)
	{
		Sequence seq = DOTween.Sequence();

		List<GameObject> squaresToMove = new List<GameObject>(cellMerge.lstBlock);
		cellMerge.lstBlock.Clear();
		// lật ngược lại để phục vụ cho hiệu ứng
		squaresToMove.Reverse();
		int m = 1;
		foreach (var sq in squaresToMove)
		{
			var square = sq; 
			square.transform.SetParent(null);
			square.GetComponent<SpriteRenderer>().sortingOrder = 20-m;
			seq.Append(
				square.transform
					.DOMove(CellGrid[i, j].transform.position, 0.15f)
					.SetEase(Ease.OutQuad)
			);

			//seq.Join(
			//	square.transform
			//		.DOScale(1.3f, 0.1f)
			//		.SetEase(Ease.OutBack)
			//);

			//seq.Append(
			//	square.transform
			//		.DOScale(1f, 0.1f)
			//);

			seq.AppendCallback(() =>
			{
				CellGrid[i, j].AddSquare(square);
			});
			m++;
		}

		// destroy cell bắn lên (chỉ còn animation)
		Destroy(cellMerge.gameObject);

		seq.OnComplete(() =>
		{
			List<Cell> path = BfsPath.FindBestMergePathBFS(CellGrid[i, j]);

			if (path.Count >= 2)
			{
				MergeByPathAnim(path, 0, CellGrid[i, j].GetLastSquareType());
			}
			else
			{
				this.CurrentCellLast = CellGrid[i,j];
				AfterAllMergeDone(i, j);
			}
		});
	}
	/// <summary>
	///  Merge từ cell này sang cel kia theo path cho trước
	/// </summary>
	/// <param name="path"></param>
	/// <param name="type"></param>
	private void MergeByPathAnim(List<Cell> path,int index, ETypeBlock type)
	{
		if (index >= path.Count-1)
		{
			this.CurrentCellLast = path[index];
			AfterAllMergeDone(path[0].x, path[0].y);
			return;
		}
		Cell from = path[index];
		Cell to = path[index + 1];
		Sequence seq = DOTween.Sequence();
		var listSquareSameTypeTop = from.GetListSameTypeFirst(type);
		int i = 1;
		//foreach(var sq1 in listSquareSameTypeTop)
		//{
		//	sq1.GetComponent<SpriteRenderer>().sortingOrder = to.GetSlotSortingLayer(i);
		//	i++;
		//}
		//listSquareSameTypeTop.Reverse();
		int baseCount = to.lstBlock.Count;
		foreach (var sq in listSquareSameTypeTop)
		{
			var square = sq;
			//square.transform.SetParent(null);
			square.GetComponent<SpriteRenderer>().sortingOrder = 20-i;
			Vector3 pos = to.transform.position + Vector3.up * ((to.lstBlock.Count + i) / 15f);
			seq.Append(
				square.transform
					.DOMove(pos, 0.1f)
					.SetEase(Ease.OutQuad)
			);
			seq.Join(
				square.transform
					.DOScale(0.6f, 0.05f)
					.SetEase(Ease.OutBack)
			);
			seq.Append(
				square.transform
					.DOScale(1f, 0.05f)
			);
			seq.AppendCallback(() =>
			{
				to.AddSquare(square);// add vào square
			});
			i++;
		}
		// đệ quy lại
		seq.OnComplete(() => {
			MergeByPathAnim(path,index+1,type);
		});

	}
	private void AfterAllMergeDone(int i, int j)
	{
		AddPoint(() => {
			if (ScoreManager.Instance.CheckWin())
				return;

			CurrentCellLast = null;

			// dịch ô nếu cần
			if (CellGrid[i, j].lstBlock.Count > 0 && i == _row - 1)
			{
				TranslateCell(i, j, CellGrid[i, j]);
			}

			// hết lượt
			if (!ScoreManager.Instance.IsHaveTurn())
			{
				ScoreManager.Instance.ShowLose();
			}
		});	
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
	// hàm trả về cell trên ô hiện tại có đang không có phần tử nào không
	public bool CheckNone(int i, int j)
	{
		bool x = this.GetCell(i -1, j).lstBlock.Count > 0;
		Debug.Log("giá trị của checknone là:"+x);
		return this.GetCell(i-1, j).lstBlock.Count > 0;
	}
	/// <summary>
	/// Hàm này trả về ô trong grid có cùng loại ô ở đầu để add thêm block
	/// </summary>
	/// <returns></returns>
	public Cell GetCellAddMoreInGrid(ETypeBlock type, int row, int col)
	{
		Cell cellEnoughCondition=null;
		for (int i = 0; i < this._row; i++)
			for (int j = 0; j < this._col; j++)
			{
				/// có cùng loại nhưng không phải cái ý là ok
				if ((i != row || j != col) && CellGrid[i,j].GetLastSquareType()==type)
				{
					if(cellEnoughCondition==null)
						cellEnoughCondition=this.GetCell(i, j);
					else
					{
						if (CellGrid[i, j].GetTotalSuareSameTypeOntop() > cellEnoughCondition.GetTotalSuareSameTypeOntop())
							cellEnoughCondition = this.GetCell(i, j);
					}	
				}	
			}	
		return cellEnoughCondition;
	}
	/// <summary>
	/// Hàm này sẽ thực hiện việc gọi cộng điểm liên kết với score manager
	/// </summary>
	private void AddPoint(Action actionCb)
	{
		ETypeBlock typeTop = this.CurrentCellLast.GetLastSquareType();
		int point = ScoreManager.Instance.AddPoint(this.CurrentCellLast.GetTotalSuareSameTypeOntop(), typeTop,out int totalRemainPoint);
		// kiểm tra xe ô cuối lớn hơn không thì tức là nó đủ đk xóa rồi
		if (point > 0)
		{
			Sequence sq = DOTween.Sequence();
			this.CurrentCellLast.SetVisibleTextNumberTotalSameType(false);
			Vector3 positionPlayAnim= UIManager.Instance.uICoreHub.GetPositionSquareMission(typeTop);
			if(positionPlayAnim==Vector3.zero)
			{
				// thực thi hiệu ứng scale xuống
				foreach (var square in this.CurrentCellLast.GetListSameTypeFirst(typeTop, true))
				{
					sq.Append(square.transform.DOScale(0f, 0.2f).SetEase(Ease.OutQuad));
				}
			}
			else
			{
				// thực thi hiệu ứng scale xuống và move về thanh nhiệm vụ
				foreach (var square in this.CurrentCellLast.GetListSameTypeFirst(typeTop, true))
				{
					// vừa move vừa scale xuống
					sq.Append(square.transform.DOMove(positionPlayAnim, 0.2f).SetEase(Ease.OutQuad));
					sq.Join(square.transform.DOScale(0f, 0.2f).SetEase(Ease.OutQuad));
				}
			}		
			sq.OnComplete(() => {
				// cập nhật lại gjá trị nhiệm vụ
				UIManager.Instance.uICoreHub.SetTargetItem(typeTop, totalRemainPoint);
				// lấy ra 1 ô trong grid đang có cùng type và số lượng nhiều nhất
				Cell cellCache = this.GetCellAddMoreInGrid(this.CurrentCellLast.GetLastSquareType(), this.CurrentCellLast.x, this.CurrentCellLast.y);
				// nếu mà không còn ô nào cùng loại thì clear đi
				if (cellCache == null)
				{
					Debug.Log("Vào logic add point xóa tất cả vì k tìm thấy thằng nào ");
					this.CurrentCellLast.ClearListSquareHaveSameTypeOnTop();
					this.CurrentCellLast.SetTextNumberTotalSameType();
					return;
				}
				/// bắt đầu merge
				int iCheck = 0;
				foreach (var square in CurrentCellLast.GetListSameTypeFirst(CurrentCellLast.GetLastSquareType(), true))
				{
					iCheck++;
					// add đủ thì xong 
					if (iCheck > point)
						break;
					cellCache.AddSquare(square);
					// để tạm như này sau có thời gian sẽ  viết tối ưu lại
					CurrentCellLast.lstBlock.Remove(square);
				}
				this.CurrentCellLast.ClearListSquareHaveSameTypeOnTop();
				this.CurrentCellLast.SetTextNumberTotalSameType();
				/// set lại thằng CurrentCellLast = chính thằng vừa add rồi đệ quy lại addpoint này để xem có ăn không
				this.CurrentCellLast = cellCache;
				this.AddPoint(actionCb);
			});	
		}
		/// nếu không còn add được gì nữa thì sẽ gọi cb check  lại
		else
		{
			actionCb?.Invoke();
		}

	}
	/// <summary>
	/// Clear toàn bộ dữ liệu grid để chuẩn bị build level mới
	/// </summary>
	public void ClearGrid()
	{
		if (CellGrid != null)
		{
			for (int i = 0; i < _row; i++)
			{
				for (int j = 0; j < _col; j++)
				{
					if (CellGrid[i, j] != null)
					{
						// clear toàn bộ block bên trong cell
						CellGrid[i, j].ClearAndDestroyListGameObj();

						// destroy cell object
						Destroy(CellGrid[i, j].gameObject);
						CellGrid[i, j] = null;
					}
				}
			}
			// clear background + cell còn sót trong hierarchy
			for (int i = this.transform.childCount - 1; i >= 0; i--)
			{
				Destroy(transform.GetChild(i).gameObject);
			}
			// reset dữ liệu
			CellGrid = null;
			_row = 0;
			_col = 0;
			_width = 0;
			_height = 0;
			CurrentCellLast = null;
			levelConfig = null;
		}
	}
	/// <summary>
	/// Lấy danh sách các cell xung quanh (8 hướng) có thể merge được
	/// </summary>
	public List<Cell> GetMergeableAroundCells(Cell center, ETypeBlock type)
	{
		List<Cell> result = new List<Cell>();
		int cx = center.x;
		int cy = center.y;
		for (int dx = -1; dx <= 1; dx++)
		{
			for (int dy = -1; dy <= 1; dy++)
			{
				// bỏ qua chính nó
				if (dx == 0 && dy == 0)
					continue;

				Cell neighbor = GetCell(cx + dx, cy + dy);

				if (neighbor == null)
					continue;

				// điều kiện merge
				if (neighbor.CheckMergeCondition(type))
				{
					result.Add(neighbor);
				}
			}
		}

		return result;
	}
	#endregion
}
