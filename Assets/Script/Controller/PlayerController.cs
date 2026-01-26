using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : Singleton<PlayerController>
{
	#region serilize field	
	/// <summary>
	/// chiều rộng của thanh slide
	/// </summary>
	[SerializeField]
	private float width;

	/// <summary>
	/// frame shoot quản lý di chuyển ô
	/// </summary>
	[SerializeField]
	private FrameShoot frameShoot;
	#endregion

	#region public field
	public FrameShoot FrameShoot=>this.frameShoot;

	private bool _isEndTurn = true;
	public bool IsEndTurn
	{
		get => _isEndTurn;
		set
		{
			Debug.Log("IsEndTurn bị set = " + value + "\n" + Environment.StackTrace);
			_isEndTurn = value;
		}
	}


	public bool isDragging;
	#endregion

	#region function monobehaviour
	private RectTransform parentRect;
	private RectTransform frameRect;
	private Vector2 dragOffset; // Lưu khoảng cách giữa chuột và tâm vật thể khi bắt đầu kéo
	private float maxX;
	private float minX;
	private float widthCell;
	private int colGrid;

	private void Start()
	{
		this.Initialize();

		frameRect = frameShoot.GetComponent<RectTransform>();
		parentRect = frameRect.parent.GetComponent<RectTransform>();
	}

	private void Update()
	{
		if (!isDragging) return;
		Vector2 localPoint;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(
			parentRect,
			Input.mousePosition,
			null,
			out localPoint
		);
		float clampedX = Mathf.Clamp(localPoint.x+ dragOffset.x, this.minX, this.maxX);
		frameRect.anchoredPosition = new Vector2(clampedX, frameRect.anchoredPosition.y);
	}
	#endregion

	#region Function logic
	/// <summary>
	/// Khởi tạo 
	/// </summary>
	private void Initialize()
	{
		this.width = this.GetComponent<RectTransform>().rect.width/2;
		this.frameShoot.SpawnBulletSquare();
		this.Refresh();
	}	
	public void Refresh()
	{
		var tupple = GameManager.Instance.GridManager.GetMinMaxWidth();
		this.maxX = tupple.max;
		this.minX = tupple.min;
		this.widthCell = tupple.widthCell;
		this.colGrid = tupple.col;
	}

	/// <summary>
	/// Bắn các ô bay lên
	/// </summary>
	public void Shoot(int col)
	{
		//nó phải còn lượt và đang k có lượt nào và thằng này đang k có đang sử dụng buff sp
		if(ScoreManager.Instance.IsHaveTurn()&&this.IsEndTurn&&!SupportController.Instance.IsUsingSP)
		{
			/// trừ điểm nó đi
			ScoreManager.Instance.MoveSub();
			this.IsEndTurn=false;
			this.frameShoot.currentCell.gameObject.transform.SetParent(GameManager.Instance.GridManager.transform);
			this.frameShoot.currentCell.SetFxVisible(false);
			Cell cellNeedMove = GameManager.Instance.GridManager.GetCellLastEmpty(col);
			Vector2 rectCellNeedMove = cellNeedMove.GetComponent<RectTransform>().anchoredPosition;
			this.frameShoot.currentCell.GetComponent<RectTransform>().
				DOAnchorPos(new Vector2(rectCellNeedMove.x, rectCellNeedMove.y), 0.5f).SetEase(Ease.OutCubic)
				.OnComplete(() =>
				{
					GameManager.Instance.GridManager.MergeToNoneBlock(cellNeedMove.x, cellNeedMove.y, this.frameShoot.currentCell);
					this.frameShoot.currentCell = null;
				});
		}
	}
	public void OnPointerDown()
	{
		SupportController.Instance.ResetItemSP();
		RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, Input.mousePosition, null, out Vector2 startMousePos);
		dragOffset = frameRect.anchoredPosition - startMousePos;
		this.isDragging= true;
	}
	public void OnPointerUp()
	{
		this.isDragging = false;
		this.SnapToColNearest();
	}
	private void SnapToColNearest()
	{
		float currentX= this.frameRect.anchoredPosition.x;
		int columnIndex = Mathf.RoundToInt((currentX - this.minX) / this.widthCell);
		columnIndex = Mathf.Clamp(columnIndex, 0, colGrid - 1);
		float snapX = minX + columnIndex * this.widthCell;
		StartCoroutine(SmoothSnap(snapX,columnIndex));
	}
	IEnumerator SmoothSnap(float targetX,int col)
	{
		float startX = frameRect.anchoredPosition.x;
		float t = 0f;

		while (t < 1f)
		{
			t += Time.deltaTime * 10f;
			float x = Mathf.Lerp(startX, targetX, t);
			frameRect.anchoredPosition = new Vector2(x, frameRect.anchoredPosition.y);
			yield return null;
		}
		frameRect.anchoredPosition = new Vector2(targetX, frameRect.anchoredPosition.y);
		this.Shoot(col);
	}
	#endregion
}
