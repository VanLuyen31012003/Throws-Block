using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class PlayerController : Singleton<PlayerController>
{
	#region serilize field	
	/// <summary>
	/// chiều rộng của thanh slide
	/// </summary>
	[SerializeField]
	private float width;

	/// <summary>
	/// tốc độ của cell
	/// </summary>
	[SerializeField]
	private float speed = 0.01f;

	/// <summary>
	/// frame shoot quản lý di chuyển ô
	/// </summary>
	[SerializeField]
	private FrameShoot frameShoot;
	#endregion

	#region public field
	public FrameShoot FrameShoot=>this.frameShoot;

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
		var tupple = GameManager.Instance.GridManager.GetMinMaxWidth();
		this.maxX = tupple.max;
		this.minX = tupple.min;
		this.widthCell=tupple.widthCell;
		this.colGrid = tupple.col;
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
	}	

	/// <summary>
	/// Bắn các ô bay lên
	/// </summary>
	public void Shoot(int col)
	{
		if(ScoreManager.Instance.IsHaveTurn())
		{
			/// trừ điểm nó đi
			ScoreManager.Instance.MoveSub();
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
