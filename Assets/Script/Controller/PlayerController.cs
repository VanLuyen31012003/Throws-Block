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

		float halfParentWidth = parentRect.rect.width / 2f;
		float halfFrameWidth = frameRect.rect.width / 2f;

		float minX = -halfParentWidth + halfFrameWidth;
		float maxX = halfParentWidth - halfFrameWidth;

		float clampedX = Mathf.Clamp(localPoint.x, minX, maxX);

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
		this.frameShoot.maxWidth = this.width;
		this.frameShoot.speed = this.speed;
		this.frameShoot.SpawnBulletSquare();
	}	

	/// <summary>
	/// Bắn các ô bay lên
	/// </summary>
	public void Shoot()
	{
		if(ScoreManager.Instance.IsHaveTurn())
		{
			/// trừ điểm nó đi
			ScoreManager.Instance.MoveSub();
			this.frameShoot.currentCell.gameObject.transform.SetParent(null);
			this.frameShoot.currentCell.SetFxVisible(false);
			this.frameShoot.currentCell=null;		
		}
	}
	public void OnPointerDown()
	{
		this.isDragging= true;
	}
	public void OnPointerUp()
	{
		this.isDragging = false;
	}
	#endregion
}
