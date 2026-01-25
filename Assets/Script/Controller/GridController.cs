using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Collections;
using Unity.Mathematics;
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
	private CellBackGround ImgBackground;

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
	//private Cell CurrentCellLast;

    /// <summary>
    /// dic này để lưu dấu các cell cuối cùng đã merge trong 1 lượt của từng loại block
    /// </summary>
    private Dictionary<ETypeBlock, Cell> dicCurrentLast= new Dictionary<ETypeBlock, Cell>();

    /// <summary>
    /// Số loại type merge trong 1 lượt
    /// </summary>
    private int totalTypeMerge;
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
		RectTransform rect = ImgBackground.GetComponent<RectTransform>();
		this._width = rect.rect.width;
		this._height = rect.rect.height;
		this.BuildGridLayout();
	}

	/// <summary>
	/// xây giao diện grid
	/// </summary>
	private void BuildGridLayout()
	{
		int countIndex = 0;

		float totalWidth = _col * _width;
		float totalHeight = _row * _height;

		float startX = -totalWidth / 2f + _width / 2f;
		float startY = totalHeight / 2f - _height / 2f;
		int k = 0;
		for (int i = 0; i < this._row; i++)
		{
			k = i;
			for (int j = 0; j < this._col; j++)
			{
				CellBackGround imgBg = Instantiate<CellBackGround>(this.ImgBackground, this.transform);

				float posX = startX + j * _width;
				float posY = startY - i * _height;

				imgBg.transform.localPosition = new Vector2(posX, posY);
				imgBg.SetData(k);
				countIndex++;
				k++;
			}
		}
		countIndex = 0;
		k = 0;
		for (int i = 0; i < this._row; i++)
		{
			k = i;
			for (int j = 0; j < this._col; j++)
			{
			//	CellBackGround imgBg = Instantiate<CellBackGround>(this.ImgBackground, this.transform);
				GameObject gridItem = Instantiate(SquarePrefap, this.transform);

				float posX = startX + j * _width;
				float posY = startY - i * _height;

				gridItem.transform.localPosition = new Vector2(posX, posY);
			//	imgBg.transform.localPosition = new Vector2(posX, posY);
				//imgBg.SetData(k);

				CellGrid[i, j] = gridItem.GetComponent<Cell>();
				CellGrid[i, j].x = i;
				CellGrid[i, j].y = j;

				if (this.levelConfig.cellDataConfigs.Count > countIndex &&
					this.levelConfig.cellDataConfigs[countIndex].squareBoxDataConfigs.Count >= 1)
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
				k++;
			}
		}
		this.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 50);
		this.SetPlusForCellInGrid();
	}

	///  hàm này sẽ để merge ô bắn từ slide lên
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <param name="cellMerge"></param>
	public void MergeToNoneBlock(int i, int j, Cell cellMerge)
	{
		Sequence seq = DOTween.Sequence();
        //reset lại dữ liệu cờ
        this.dicCurrentLast.Clear();
		this.totalTypeMerge = 0;
        /// bắt đầu merge
        foreach (var square in cellMerge.lstBlock)
		{
			CellGrid[i, j].AddSquare(square);
		}
		seq.AppendInterval(0.2f);
		// xóa thằng cell bắn này đi
		Destroy(cellMerge.gameObject);

		seq.OnComplete(() =>
		{
			// lấy ra list type có trong thằng merge đầu này
			List<ETypeBlock> stackTypes = CellGrid[i, j].lstBlock.Select(b => b.GetComponent<Square>().typeBlock)
			.Distinct()
			.ToList();
			//reverse để lấy cái từ trên đầu trước
			stackTypes.Reverse();
			// dic lưu dữ liệu cell và đường đi
			Dictionary<ETypeBlock, List<Cell>> dicTypeAndPath = new Dictionary<ETypeBlock, List<Cell>>();
			foreach (var type in stackTypes)
			{
				List<Cell> path = BfsPath.FindBestMergePathBFS(CellGrid[i, j], type);
				if (path.Count >= 2)
				{
					this.dicCurrentLast.TryAdd(type, path[path.Count - 1]);
					dicTypeAndPath.TryAdd(type, path);
				}
				else
				{
					break;
				}
			}
			// nếu có cái cần merge
			if (dicTypeAndPath.Count > 0)
			{
				this.totalTypeMerge = dicTypeAndPath.Count;
				foreach (var element in dicTypeAndPath)
				{
					MergeByPathAnim(element.Value, 0, element.Key);
				}
			}
			else
			{
				this.DetermineCheckWinOrTranslate(i, j);
			}
		});
	}
	/// <summary>
	///  Merge từ cell này sang cel kia theo path cho trước
	/// </summary>
	/// <param name="path"></param>
	/// <param name="type"></param>
	private void MergeByPathAnim(List<Cell> path, int index, ETypeBlock type)
    {
        // Điều kiện dừng
        if (index >= path.Count - 1)
        {
			this.dicCurrentLast[type] = path[index];
            AfterAllMergeDone(path[0].x, path[0].y, type);
            return;
        }

        Cell from = path[index];
        Cell to = path[index + 1];

        Sequence seq = DOTween.Sequence();

        var listSquareSameTypeTop = from.GetListSameTypeFirst(type);

        float startTime = 0f;
        int i = 1;
		int count =listSquareSameTypeTop.Count;
		from.SetVisibleTextNumberTotalSameType(false);
		//	listSquareSameTypeTop.Reverse();
		foreach (var sq in listSquareSameTypeTop)
        {
            var square = sq;

            // Vị trí bay tới
            Vector3 pos = to.transform.position
                        + Vector3.up * ((to.lstBlock.Count + i) * StaticControl.VALUE_DEVIDE);

            // Insert animation gối đầu
            seq.Insert(
                startTime,
                square.transform
                    .DOMove(pos, StaticControl.TIME_DOTWEEN_DURATION_ANIM)
                    .SetEase(Ease.OutQuad)
            );
			// cái này để tạo hiệu ứng cái sau cao hơn cái trước
			seq.InsertCallback(startTime + StaticControl.TIME_DOTWEEN_CONTINUOUS_ANIM, 
				() => { square.transform.SetParent(to.transform); }		  
		   ); 
			// AddSquare sau khi anim của square đó xong
			seq.InsertCallback(startTime + StaticControl.TIME_DOTWEEN_DURATION_ANIM, () =>
            {
                to.AddSquare(square);
            });
            i++;
            startTime += StaticControl.TIME_DOTWEEN_CONTINUOUS_ANIM;
        }

        // Sau khi toàn bộ anim gối đầu xong → merge tiếp cell kế
        seq.OnComplete(() =>
		{
			from.SetVisibleTextNumberTotalSameType(true);
			MergeByPathAnim(path, index + 1, type);
        });
    }

    /// <summary>
    /// truyền thêm type để biết được thằng nào merge xong rồi
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <param name="typeBlock"></param>
    private void AfterAllMergeDone(int i, int j,ETypeBlock typeKey)
	{
		AddPoint(() => {
			this.totalTypeMerge--;
            // nếu mà tất cả các loại merge đã xong thì mới check win dịch ô
			if (this.totalTypeMerge <= 0)
                DetermineCheckWinOrTranslate(i, j);

        },typeKey);	
	}
	private void DetermineCheckWinOrTranslate(int i, int j)
	{
		if (ScoreManager.Instance.CheckWin())
		{
			PlayerController.Instance.FrameShoot.SpawnBulletSquare();
			return;
		}
		// dịch ô nếu cần
		if (CellGrid[i, j].lstBlock.Count > 0 && i == _row - 1)
		{
			TranslateCell(i, j, () =>
			{
				this.EnTurnPlay();
			});
		}
		else
		{
			this.EnTurnPlay();
		}
	}	
	public void EnTurnPlay()
	{
		// hết lượt
		if (!ScoreManager.Instance.IsHaveTurn())
		{
			ScoreManager.Instance.ShowLose();
		}
		this.SetPlusForCellInGrid();
		PlayerController.Instance.FrameShoot.SpawnBulletSquare();
		PlayerController.Instance.IsEndTurn = true;
	}

	public void TranslateCell(int rowIndex, int col, Action actionEndturn)
	{
		int checkIndex = this.GetRowCellEmpty(col);
		Sequence sequence = DOTween.Sequence();
		//Nếu không có ô nào trong cột rỗng thì dịch cả cột
		if(checkIndex<0)
		{
			// clear dữ liệu trong list cái này đi
			CellGrid[0, col].ClearAndDestroyListGameObj();
			// bắt đầu dịch dữ liệu
			for (int row = 1; row <= rowIndex; row++)
			{
				// add dữ liệu block của thằng này sang thằng trước
				//CellGrid[row - 1, col] = CellGrid[row, col]; 
				int countList = CellGrid[row - 1, col].lstBlock.Count;
				int indexMove = 0;
				foreach (var square in CellGrid[row, col].lstBlock)
				{
					float y = (float)(countList+indexMove) * StaticControl.VALUE_DEVIDE;
					indexMove++;
					CellGrid[row - 1, col].AddSquare(square,true);
					sequence.Join(square.GetComponent<RectTransform>().DOAnchorPos(new Vector3(0,y,0),StaticControl.TIME_DOTWEEN_DURATION_ANIM));
				}
				//sequence.AppendCallback(() => { CellGrid[row, col].SetTextNumberTotalSameType();}); 
				// sau đó sẽ clear cái list đi 
				CellGrid[row, col].ClearListGameObj();
			}
			sequence.OnComplete(() =>
			{
				for (int row = 0; row <= rowIndex; row++)
				{
					CellGrid[row, col].SetTextNumberTotalSameType();
				}
				this.SetPlusForCellInGrid();
				actionEndturn?.Invoke();
			});
		}
		else
		{
			// bắt đầu dịch dữ liệu
			for (int row = checkIndex; row <= rowIndex; row++)
			{
				// add dữ liệu block của thằng này sang thằng trước
				//CellGrid[row - 1, col] = CellGrid[row, col]; 
				int countList = CellGrid[row - 1, col].lstBlock.Count;
				int indexMove = 0;
				foreach (var square in CellGrid[row, col].lstBlock)
				{
					float y = (float)(countList + indexMove) * StaticControl.VALUE_DEVIDE;
					indexMove++;
					CellGrid[row - 1, col].AddSquare(square,true);
					sequence.Join(square.GetComponent<RectTransform>().DOAnchorPos(new Vector3(0, y, 0), StaticControl.TIME_DOTWEEN_DURATION_ANIM));
				}
				// sau đó sẽ clear cái list đi 
				CellGrid[row, col].ClearListGameObj();

			}
			sequence.OnComplete(() =>
			{
				for (int row = checkIndex; row <= rowIndex; row++)
				{
					CellGrid[row - 1, col].SetTextNumberTotalSameType();
				}
				this.SetPlusForCellInGrid();
				actionEndturn?.Invoke();
			});
		}
	
	}
	private int GetRowCellEmpty(int j)
	{
		for(int i = this._row-1;i>=0;i--)
		{
			if (CellGrid[i,j].lstBlock.Count==0 )
			{
				return i;
			}	
		}	
		return -1;
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
		if(this.GetCell(i-1,j)==null)
		{
			return true;
		}	
		bool x = this.GetCell(i -1, j).lstBlock.Count > 0;
		//Debug.Log("giá trị của checknone là:"+i);
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
	private void AddPoint(Action actionCb,ETypeBlock typeKey)
	{
		int point = ScoreManager.Instance.AddPoint(this.dicCurrentLast[typeKey].GetTotalSuareSameTypeOntop(), typeKey, out int totalRemainPoint);
		// kiểm tra xe ô cuối lớn hơn không thì tức là nó đủ đk xóa rồi
		if (point > 0)
		{
			Sequence sq = DOTween.Sequence();
			this.dicCurrentLast[typeKey].SetVisibleTextNumberTotalSameType(false);
			Vector3 positionPlayAnim= UIManager.Instance.uICoreHub.GetPositionSquareMission(typeKey);
            // thực thi hiệu ứng scale xuống
            float startTime = 0f;
            if (positionPlayAnim==Vector3.zero)
			{         
                foreach (var square in this.dicCurrentLast[typeKey].GetListSameTypeFirst(typeKey, true))
                {
                    sq.Insert(
                        startTime,
                        square.transform
                            .DOScale(0f, StaticControl.TIME_DOTWEEN_DURATION_ANIM)
                            .SetEase(Ease.OutQuad)
                    );

                    startTime += StaticControl.TIME_DOTWEEN_CONTINUOUS_ANIM;
                }
            }
			else
			{
				// thực thi hiệu ứng scale xuống và move về thanh nhiệm vụ
				foreach (var square in this.dicCurrentLast[typeKey].GetListSameTypeFirst(typeKey, true))
				{
					sq.Insert(startTime,square.transform
                    .DOMove(positionPlayAnim, 0.1f)
                    .SetEase(Ease.OutQuad));
                    // vừa move vừa scale xuống
                    sq.Insert(
                       startTime,
                       square.transform
                           .DOScale(0f, StaticControl.TIME_DOTWEEN_DURATION_ANIM)
                           .SetEase(Ease.OutQuad)
					 );
                    startTime += StaticControl.TIME_DOTWEEN_CONTINUOUS_ANIM;
                    //sq.Append(square.transform.DOMove(positionPlayAnim, 0.1f).SetEase(Ease.OutQuad));
                    //sq.Join(square.transform.DOScale(0f, 0.1f).SetEase(Ease.OutQuad));
                }
            }		
			sq.OnComplete(() => {
                this.dicCurrentLast[typeKey].ClearListSquareHaveSameTypeOnTop();
                // cập nhật lại gjá trị nhiệm vụ
                UIManager.Instance.uICoreHub.SetTargetItem(typeKey, totalRemainPoint);
				// lấy ra 1 ô trong grid đang có cùng type và số lượng nhiều nhất
				Cell cellCache = this.GetCellAddMoreInGrid(typeKey, this.dicCurrentLast[typeKey].x, this.dicCurrentLast[typeKey].y);
				// nếu mà không còn ô nào cùng loại thì clear đi
				if (cellCache == null)
				{
					Debug.Log("Vào logic add point xóa tất cả vì k tìm thấy thằng nào ");
					this.dicCurrentLast[typeKey].ClearListSquareHaveSameTypeOnTop();
					this.dicCurrentLast[typeKey].SetTextNumberTotalSameType();
					actionCb?.Invoke();
					return;
				}
				Sequence sequenceMove =DOTween.Sequence();
				Cell cellPosAdd = this.SpawnGameObj(typeKey, point, this.dicCurrentLast[typeKey]);
                List<GameObject> lstGameObt = new List<GameObject>();
                lstGameObt=cellPosAdd.lstBlock;
				lstGameObt.Reverse();
				int k = 0;
                foreach (var square in lstGameObt)
				{
						
					Vector2 posCache = square.transform.position;
					posCache.y=posCache.y+StaticControl.VALUE_TRANSLATETOP;
					sequenceMove.Join(square.transform.DOMove(posCache, 0.3f));
                    if (k == 0)
                    {
						sequenceMove.Join((cellPosAdd.TotalNumberSquareTopSameType.transform.DOMove(posCache, 0.3f)));
                    }
					k++;

                }
                int m = 1;
				sequenceMove.AppendCallback(() => {
					Debug.Log("Vào logic add point chuyển sang thằng khác để add ");
                    Destroy(cellPosAdd.TotalNumberSquareTopSameType.gameObject);

                    cellCache.SetVisibleTextNumberTotalSameType(false);
				});

                foreach (var sq in lstGameObt)
                {
                    var square = sq;
                    sequenceMove.Append(
                        square.transform
                            .DOMove(cellCache.transform.position, 0.15f)
                            .SetEase(Ease.OutQuad)
                    );
                    sequenceMove.AppendCallback(() =>
                    {
                        cellCache.AddSquare(square);
                    });
                    m++;
                }
				sequenceMove.OnComplete(() =>
				{
					Destroy(cellPosAdd.gameObject);
                    this.dicCurrentLast[typeKey].SetTextNumberTotalSameType();
                    /// set lại thằng CurrentCellLast = chính thằng vừa add rồi đệ quy lại addpoint này để xem có ăn không
                    this.dicCurrentLast[typeKey] = cellCache;
                    this.AddPoint(actionCb,typeKey);
                });	
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
			this.CellGrid = null;
			this._row = 0;
			this._col = 0;
			this._width = 0;
			this._height = 0;
			this.dicCurrentLast.Clear();
			this.totalTypeMerge = 0;
            this.levelConfig = null;
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
    /// <summary>
    /// hàm dùng để sinh ra 1 cell mới có các block bên trong dùng cho khi cộng điểm
    /// </summary>
    /// <param name="typeBlock"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public Cell SpawnGameObj(ETypeBlock typeBlock, int count,Cell cellAdd )
    {
        GameObject gameObj  = Instantiate(this.SquarePrefap,cellAdd.transform);
		Cell cell = gameObj.GetComponent<Cell>();
        cell.transform.position = cellAdd.transform.position;
        cell.gameObject.SetActive(true);
        cell.lstBlock = new List<GameObject>();
        List<GameObject> listSquare = new List<GameObject>();
		cell.SetImageVisible(false);
		cell.SpawnBlockAnim(typeBlock,count, cellAdd.lstBlock.Count);
        return cell;
    }
	/// <summary>
	/// hàm này để 
	/// </summary>
	public void SetPlusForCellInGrid()
	{
		foreach (var cell in this.CellGrid)
		{
			cell.SetImageVisible(false);
		}	
	//	string s = "Giá trị của cell được set text là:";
		for(int i=0;i<this._col;i++)
		{
			this.GetCellLastEmpty(i).SetTextNumberTotalSameType(true);
		}
	}
	public Cell GetCellLastEmpty(int col)
	{
		for (int i = this._row - 1; i >= 0; i--)
		{
			Cell current = this.GetCell(i, col);

			if (current.lstBlock.Count == 0)
			{
				if (i == 0)
				{
					return current;
				}
				Cell above = this.GetCell(i - 1, col);
				if (above.lstBlock.Count != 0)
				{
					return current;
				}
				if (above.lstBlock.Count == 0)
				{
					current.SetTextNumberTotalSameType();
				}
			}
		}
		return null;
	}
	public (float min,float max,float widthCell, int col) GetMinMaxWidth()
	{
		float witdh= this._width*this._col;
		float min = -witdh/2 + this._width/2;
		float max = witdh/2- this._width/2;
		return (min,max,this._width,this._col);
	}
	#endregion

	#region Function Feat sp
	/// <summary>
	///  thực thi hiệu ứng hỗ trợ tên lửa
	/// </summary>
	public void DoSpROCKET(int i, int j)
	{
		Sequence sequence = DOTween.Sequence();
		Dictionary<ETypeBlock, int> dic = new Dictionary<ETypeBlock, int>();
		for (int index = 0; index < this._row; index++)
		{
			this.MergeDic(dic, CellGrid[index, j].GetNumberSquarePerType());
			sequence.Join(CellGrid[index, j].transform.DOScale(0, StaticControl.TIME_DOTWEEN_SCALE_ANIM).SetEase(Ease.OutQuad));
		}
		sequence.OnComplete(() =>
		{
			for (int index = 0; index < this._row; index++)
			{
				CellGrid[index, j].DestroyListGameObj();
				CellGrid[index, j].transform.localScale = new Vector3(1, 1, 1);

				//CellGrid[index, j].transform.DOScale(1,0.01f);
			}
			foreach (var e in dic)
			{
				ScoreManager.Instance.AddPointBySp(e.Value, e.Key);

			}
			this.SetPlusForCellInGrid();
			ScoreManager.Instance.CheckWin();
			SupportController.Instance.IsUsingSP = false;
		});
	}
	public void DoSpBOWLING(int i, int j)
	{
		Sequence sequence = DOTween.Sequence();
		// dic này sẽ lưu tất cả giá trị số square ăn được từ việc dùng sp
		Dictionary<ETypeBlock,int> dic = new Dictionary<ETypeBlock,int>();
		for (int dx = -1; dx <= 1; dx++)
		{
			for (int dy = -1; dy <= 1; dy++)
			{
				int nx = i + dx;
				int ny = j + dy;

				if (GetCell(nx, ny) == null)
					continue;
				this.MergeDic(dic, CellGrid[nx, ny].GetNumberSquarePerType());
				sequence.Join(CellGrid[nx, ny].transform.DOScale(0, StaticControl.TIME_DOTWEEN_SCALE_ANIM).SetEase(Ease.OutQuad));
			}
		}
		sequence.OnComplete(() =>
		{
			for (int dx = -1; dx <= 1; dx++)
			{
				for (int dy = -1; dy <= 1; dy++)
				{

					int nx = i + dx;
					int ny = j + dy;

					if (GetCell(nx, ny) == null)
						continue;
					CellGrid[nx, ny].DestroyListGameObj();
					CellGrid[nx, ny].transform.localScale = new Vector3(1,1,1);

				//	CellGrid[nx, ny].transform.DOScale(1, 0.01f);

				}
			}
			foreach(var e in dic)
			{
				ScoreManager.Instance.AddPointBySp(e.Value, e.Key);

			}
			this.SetPlusForCellInGrid();
			ScoreManager.Instance.CheckWin();
			SupportController.Instance.IsUsingSP = false;

		});
	}
	public void DoSHUFFLE()
	{
		Sequence seq = DOTween.Sequence();

		List<Cell> cellsHaveBlock = new List<Cell>();
		// row -1 để không lấy hàng cuối
		for (int i = 0; i < this._row - 1; i++)
		{
			for (int j = 0; j < this._col; j++)
			{
				cellsHaveBlock.Add(CellGrid[i, j]);
			}
		}
		cellsHaveBlock = cellsHaveBlock.OrderBy(x => UnityEngine.Random.value).ToList();
		int index = 0;
		for (int i = 0; i < this._row - 1; i++)
		{
			for (int j = 0; j < this._col; j++)
			{
				Cell cellCache = this.GetCell(i, j);
				int indexSibling = cellCache.transform.GetSiblingIndex();
				if (cellCache == null)
				{
					Debug.Log("lỗi gì đó trong shuffle");
					break;
				}
				CellGrid[i, j] = cellsHaveBlock[index++];
				CellGrid[i, j].transform.SetSiblingIndex(index+this._row*this._col);
				CellGrid[i, j].x = i;
				CellGrid[i, j].y= j;
				seq.Join(CellGrid[i, j].GetComponent<RectTransform>().DOAnchorPos(cellCache.GetComponent<RectTransform>().anchoredPosition, StaticControl.TIME_DOTWEEN_SCALE_ANIM).SetEase(Ease.OutQuad));
			}
		}
		seq.onComplete = () => {
			this.SetPlusForCellInGrid();
			SupportController.Instance.IsUsingSP=false;
		};
	}
	void MergeDic(Dictionary<ETypeBlock, int> target,Dictionary<ETypeBlock, int> source)
	{
		foreach (var kv in source)
		{
			if (target.ContainsKey(kv.Key))
				target[kv.Key] += kv.Value;
			else
				target[kv.Key] = kv.Value;
		}
	}
	#endregion
}
